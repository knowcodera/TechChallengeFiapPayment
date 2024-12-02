namespace PaymentApi.Services
{
    public interface IRabbitMQPublisher
    {
        void Publish(string queueName, string message);
    }
}
