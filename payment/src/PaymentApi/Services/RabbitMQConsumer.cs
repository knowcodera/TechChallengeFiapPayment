using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PaymentApi.Services
{
    public class RabbitMQConsumer
    {
        private readonly RabbitMQConnection _connection;

        public RabbitMQConsumer(RabbitMQConnection connection)
        {
            _connection = connection;
        }

        public void Consume(string queueName, Action<string> onMessageReceived)
        {
            using var channel = _connection.GetChannel();

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                onMessageReceived(message);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
