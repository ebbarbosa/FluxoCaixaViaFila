using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Stone.FluxoCaixaViaFila.Infra.MQ;

namespace Stone.FluxoCaixaViaFila.WebApi
{
    public static class AplicationBuilderExtensions
    {
        public static FluxoCaixaConsumer Listener { get; set; }

        public static IApplicationBuilder UseFluxoCaixaConsumer(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<FluxoCaixaConsumer>();

            var life = app.ApplicationServices.GetService<IApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);

            Listener.Execute();

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Register();
        }

        private static void OnStopping()
        {
            Listener.Deregister();
        }
    }
}
