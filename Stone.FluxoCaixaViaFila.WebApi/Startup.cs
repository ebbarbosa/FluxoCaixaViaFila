﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stone.FluxoCaixaViaFila.Common;
using SimpleInjector;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using Stone.FluxoCaixaViaFila.Domain;
using Newtonsoft.Json.Converters;

namespace Stone.FluxoCaixaViaFila.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var container = new Container();
            ContainerHelper.RegisterServices(container, c =>
            {
                InfraContainerHelper.RegisterServices(c);
            });
            container.Verify();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddJsonOptions((opt) =>
                    {
                        opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                        opt.SerializerSettings.Converters.Add(new CurrencyConverter());
                        opt.SerializerSettings.Converters.Add(new PosicaoConverter());
                        opt.SerializerSettings.Converters.Add(new CustomDateConverter("dd-MM-yyyy"));
                    });

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Filas de Lancamentos");
            });
        }
    }
}
