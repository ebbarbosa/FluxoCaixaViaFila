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
    public abstract class ConsumerLancamentoService : QueueHostedService
    {
        private IFluxoCaixaDiarioMq _fluxoCaixaDiarioMq;
        private IModel _channel;

        protected ConsumerLancamentoService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void GetRequiredServices(IServiceProvider serviceProvider)
        {
            _fluxoCaixaDiarioMq = serviceProvider.GetRequiredService<IFluxoCaixaDiarioMq>();
            _channel = RabbitMqConnectionHelper.GetModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ConsolidarLancamentos();
                await Task.Delay(CheckUpdateTime, stoppingToken);
            }
        }

        private void ConsolidarLancamentos()
        {
            _channel.QueueDeclare(queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                var lancamento = JsonConvert.DeserializeObject<Lancamento>(message);

                if (lancamento == null) return;

                var fluxoDiario = new FluxoCaixaDiario();
                fluxoDiario.Add(lancamento);
                _fluxoCaixaDiarioMq.Put(fluxoDiario);

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: QueueName,
                autoAck: false,
                consumer: consumer);
        }

        public abstract string QueueName { get; }
    }
}