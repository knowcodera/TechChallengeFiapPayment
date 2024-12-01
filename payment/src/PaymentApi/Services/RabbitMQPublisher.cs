using RabbitMQ.Client;
using System.Text;

namespace PaymentApi.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQConnection _connection;

        public RabbitMQPublisher(RabbitMQConnection connection)
        {
            _connection = connection;
        }

        public void Publish(string queueName, string message)
        {
            using var channel = _connection.GetChannel();

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
    }
}
