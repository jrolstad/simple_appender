using System.IO;
using System.Text;
using log4net.Appender;
using log4net.Core;
using RabbitMQ.Client;

namespace simple_appender
{
    public class AmqpAppender:AppenderSkeleton
    {
        public string ExchangeName { get; set; }

        public string RoutingKey { get; set; }

        public string ServerUri { get; set; }

        private IModel _channel;
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;

        public AmqpAppender():this(new ConnectionFactory())
        {
            
        }

        public AmqpAppender(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected override bool RequiresLayout
        {
            get { return true; }
        }

        protected override void OnClose()
        {
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
                _channel.Dispose();
                _channel = null;
            }


            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {

            if (_channel == null ||
                _channel.IsClosed)
            {
                _connectionFactory.Uri = ServerUri;

                if (_connection == null ||
                    !_connection.IsOpen)
                {
                    _connection = _connectionFactory.CreateConnection();
                }

                _channel = _connection.CreateModel();
            }

            var builder = new StringBuilder();
            var stringWriter = new StringWriter(builder);
            Layout.Format(stringWriter,loggingEvent);

            var renderedMessage = builder.ToString();
            var messageBody = Encoding.UTF8.GetBytes(renderedMessage);

            var routingKey = RoutingKey ?? string.Empty;
            _channel.BasicPublish(exchange: ExchangeName, routingKey: routingKey, basicProperties: null, body: messageBody);
        }
    }
}