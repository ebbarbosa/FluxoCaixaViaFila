using RabbitMQ.Client;
using Stone.FluxoCaixaViaFila.Domain;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public class FluxoCaixaConsumer : MqBase, IFluxoCaixaConsumer
    {
        private readonly IFluxoCaixaRepository fluxoCaixaRepository;
        private IConnection connection;
        private IModel channel;

        public FluxoCaixaConsumer(IFluxoCaixaRepository fluxoCaixaRepository)
        {
            this.fluxoCaixaRepository = fluxoCaixaRepository;
        }


        public void Execute()
        {
            var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = System.Text.Encoding.UTF8.GetString(body);

                fluxoCaixaRepository.Add(message);
            };

            channel.BasicConsume(queue: "fluxocaixa",
                                 autoAck: false,
                                 consumer: consumer);


        }
    }
}