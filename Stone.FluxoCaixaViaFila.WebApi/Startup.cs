using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stone.FluxoCaixaViaFila.Common;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json.Converters;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Stone.FluxoCaixaViaFila.Domain;
using Microsoft.Extensions.PlatformAbstractions;
using Stone.FluxoCaixaViaFila.Infra.MQ;
using Microsoft.Extensions.Hosting;
using Stone.FluxoCaixaViaFila.WebApi.Controllers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Stone.FluxoCaixaViaFila.WebApi
{
    public class Startup
    {
        Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions((opt) =>
                    {
                        opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                        opt.SerializerSettings.Converters.Insert(0, new CustomDateConverter());
                    });

            IntegrateSimpleInjector(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Fila de Lancamentos",
                        Version = "v1",
                        Description = "API REST para criacao de lancamentos e seu envio para filas rabbit mq, criada com o ASP.NET Core",
                        Contact = new Contact
                        {
                            Name = "Eduardo Brandao Barbosa",
                            Url = "https://github.com/ebbarbosa"
                        }
                    });

                string caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    System.IO.Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(caminhoXmlDoc);
            });
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Add application services. For instance:
            ContainerHelper.RegisterServices(container, InfraContainerHelper.RegisterServices);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);

            services.AddSingleton<IHostedService, FluxoCaixaService>().AddSingleton<IFluxoCaixaRepository, FluxoCaixaRepository>();
            services.AddSingleton<IHostedService, ConsumerPagamentosService>().AddSingleton<IFluxoCaixaDiarioMq, FluxoCaixaDiarioMq>();
            services.AddSingleton<IHostedService, ConsumerRecebimentosService>().AddSingleton<IFluxoCaixaDiarioMq, FluxoCaixaDiarioMq>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            container.Verify();

            app.UseMvc();

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Filas de Lancamentos");
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Allow Simple Injector to resolve services from ASP.NET Core.
            container.AutoCrossWireAspNetComponents(app);
        }
    }
}
