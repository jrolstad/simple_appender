using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using log4net.Appender;
using log4net.Core;
using RabbitMQ.Client;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace simple_appender
{
    public class AsyncAmqpAppender:AppenderSkeleton
    {
        public string ExchangeName { get; set; }

        public string RoutingKey { get; set; }

        public string ServerUri { get; set; }

        private IModel _channel;
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;

        private readonly BlockingCollection<LoggingEvent> _loggingEvents = new BlockingCollection<LoggingEvent>(); 

        public AsyncAmqpAppender():this(new ConnectionFactory())
        {
            
        }

        public AsyncAmqpAppender(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

        }

        protected override bool RequiresLayout
        {
            get { return true; }
        }

        public override void ActivateOptions()
        {
            _loggingEvents
                .GetConsumingEnumerable()
                .ToObservable(NewThreadScheduler.Default)
                .Subscribe(e => { SendLoggingEventAsMessage(e); });
            base.ActivateOptions();
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
           _loggingEvents.Add(loggingEvent);
        }

        private void SendLoggingEventAsMessage(LoggingEvent loggingEvent)
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
            Layout.Format(stringWriter, loggingEvent);

            var renderedMessage = builder.ToString();
            var messageBody = Encoding.UTF8.GetBytes(renderedMessage);

            var routingKey = RoutingKey ?? string.Empty;
            _channel.BasicPublish(exchange: ExchangeName, routingKey: routingKey, basicProperties: null, body: messageBody);
        }
    }
}