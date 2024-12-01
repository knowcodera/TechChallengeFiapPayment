using RabbitMQ.Client;

namespace PaymentApi.Services
{
    public class RabbitMQConnection
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;

        public RabbitMQConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IModel GetChannel()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:HostName"],
                    UserName = _configuration["RabbitMQ:UserName"],
                    Password = _configuration["RabbitMQ:Password"]
                };

                // Corrigido: CreateConnection (removendo erro de digitação)
                _connection = factory.CreateConnection();
            }

            // Retorna um canal (modelo)
            return _connection.CreateModel();
        }
    }
}
