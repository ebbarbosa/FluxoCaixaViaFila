using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class FluxoCaixaService : QueueHostedService
    {
        public IFluxoCaixaRepository FluxoCaixaRepository { get; private set; }
        protected IModel Channel;

        public FluxoCaixaService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void GetRequiredServices(IServiceProvider serviceProvider)
        {
            Channel = RabbitMqConnectionHelper.GetModel();
            FluxoCaixaRepository = serviceProvider.GetRequiredService<IFluxoCaixaRepository>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumirFluxoCaixa();
                await Task.Delay(CheckUpdateTime, stoppingToken);
            }
        }
        private void ConsumirFluxoCaixa()
        {
            var queueName = "FluxoCaixa";
            Channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    var fluxoDiario = JsonConvert.DeserializeObject<FluxoCaixaDiario>(message);
                    FluxoCaixaRepository.Add(fluxoDiario);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
            Channel.BasicConsume(queue: queueName,
                autoAck: false,
                consumer: consumer);
        }
    }
}


