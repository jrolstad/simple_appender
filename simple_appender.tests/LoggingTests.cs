using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Config;
using NUnit.Framework;
using simple_appender.tests.Fakes;

namespace simple_appender.tests
{
    [TestFixture]
    public class LoggingTests
    {
        [Test]
        public void LogInfo_nativeType_SendsMessage()
        {
            // Arrange
            var fakeConnectionFactory = new FakeConnectionFactory();
            var appender = GetAppender(fakeConnectionFactory);

            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            logger.Info(1);
            Thread.Sleep(2000);

            // Assert
            var publishedMessages = fakeConnectionFactory.UnderlyingModel.PublishedMessagesOnExchange("logging.test");
            Assert.That(publishedMessages, Has.Count.EqualTo(1));

            var messageBody = Encoding.UTF8.GetString(publishedMessages.First().body);
            Assert.That(messageBody,Is.EqualTo("1"));
        }

        [Test]
        public void LogInfo_customType_SendsMessage()
        {
            // Arrange
            var fakeConnectionFactory = new FakeConnectionFactory();
            var appender = GetAppender(fakeConnectionFactory);

            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            var item = new SomeType {Id = 1, Name = "My Name", UsedAt = new DateTime(2015,2,10)};
            logger.Info(item);

            Thread.Sleep(2000);

            // Assert
            var publishedMessages = fakeConnectionFactory.UnderlyingModel.PublishedMessagesOnExchange("logging.test");
            Assert.That(publishedMessages, Has.Count.EqualTo(1));

            var messageBody = Encoding.UTF8.GetString(publishedMessages.First().body);
            Assert.That(messageBody, Is.EqualTo("{\"Id\":1,\"Name\":\"My Name\",\"UsedAt\":\"2015-02-10T00:00:00\"}"));
        }

        [Test]
        public void LogInfo_dynamic_SendsMessage()
        {
            // Arrange
            var fakeConnectionFactory = new FakeConnectionFactory();
            var appender = GetAppender(fakeConnectionFactory);

            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            dynamic item = new ExpandoObject();
            item.Id = 2;
            item.Name = "some name";
            item.LastAccessedAt = new DateTime(2015,2,10);

            logger.Info(item);
            Thread.Sleep(2000);

            // Assert
            var publishedMessages = fakeConnectionFactory.UnderlyingModel.PublishedMessagesOnExchange("logging.test");
            Assert.That(publishedMessages, Has.Count.EqualTo(1));

            var messageBody = Encoding.UTF8.GetString(publishedMessages.First().body);
            Assert.That(messageBody, Is.EqualTo("{\"Id\":2,\"Name\":\"some name\",\"LastAccessedAt\":\"2015-02-10T00:00:00\"}"));
        }


        private static IAppender GetAppender(FakeConnectionFactory fakeConnectionFactory)
        {
            var appender = new AsyncAmqpAppender(fakeConnectionFactory)
            {
                ExchangeName = "logging.test",
                ServerUri = "amqp://foo:bar@my-machine-name:5672",
                RoutingKey = null,
                Layout = new NewtonSoftJsonLayout()
            };

            appender.ActivateOptions();
            return appender;
        }

        public class SomeType
        {
            public int Id;

            public string Name;

            public DateTime UsedAt;
        }

        
    }

   
}