using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;
using log4net.Config;
using log4net.Core;
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
            var appender = new AmqpAppender(fakeConnectionFactory)
            {
                ExchangeName = "logging.test",
                ServerUri = "amqp://foo:bar@my-machine-name:5672",
                RoutingKey = null,
                Layout = new NewtonSoftJsonLayout()
            };

            appender.ActivateOptions();
          
            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            logger.Info(1);

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
            var appender = new AmqpAppender(fakeConnectionFactory)
            {
                ExchangeName = "logging.test",
                ServerUri = "amqp://foo:bar@my-machine-name:5672",
                RoutingKey = null,
                Layout = new NewtonSoftJsonLayout()
            };

            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            var item = new SomeType {Id = 1, Name = "My Name", UsedAt = new DateTime(2015,2,10)};
            logger.Info(item);

            // Assert
            var publishedMessages = fakeConnectionFactory.UnderlyingModel.PublishedMessagesOnExchange("logging.test");
            Assert.That(publishedMessages, Has.Count.EqualTo(1));

            var messageBody = Encoding.UTF8.GetString(publishedMessages.First().body);
            Assert.That(messageBody, Is.EqualTo("{\"Id\":1,\"Name\":\"My Name\",\"UsedAt\":\"\\/Date(1423555200000-0800)\\/\"}"));
        }

        [Test]
        public void LogInfo_dynamic_SendsMessage()
        {
            // Arrange
            var fakeConnectionFactory = new FakeConnectionFactory();
            var appender = new AmqpAppender(fakeConnectionFactory)
            {
                ExchangeName = "logging.test",
                ServerUri = "amqp://foo:bar@my-machine-name:5672",
                RoutingKey = null,
                Layout = new NewtonSoftJsonLayout()
            };

            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            dynamic item = new ExpandoObject();
            item.Id = 2;
            item.Name = "some name";
            item.LastAccessedAt = new DateTime(2015,2,10);

            logger.Info(item);

            // Assert
            var publishedMessages = fakeConnectionFactory.UnderlyingModel.PublishedMessagesOnExchange("logging.test");
            Assert.That(publishedMessages, Has.Count.EqualTo(1));

            var messageBody = Encoding.UTF8.GetString(publishedMessages.First().body);
            Assert.That(messageBody, Is.EqualTo("{\"Id\":2,\"Name\":\"some name\",\"LastAccessedAt\":\"\\/Date(1423555200000-0800)\\/\"}"));
        }

        [Test]
        public void Get_TimeZones()
        {
            foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
                Console.WriteLine("{0}|{1}|{2}|{3}|{4}|",z.Id,z.DisplayName,z.StandardName,z.DaylightName,z.BaseUtcOffset);
        }

        [Test]
        public void Sandbox()
        {
            var collection = new BlockingCollection<int>();

            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            collection
                .GetConsumingEnumerable()
                .ToObservable(NewThreadScheduler.Default)
                .Subscribe(SendMessage);

            collection.Add(1);
            collection.Add(2);
            collection.Add(3);
            collection.Add(4);

            Thread.Sleep(1000);

        }

        private void SendMessage(int data)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }

        public class SomeType
        {
            public int Id;

            public string Name;

            public DateTime UsedAt;
        }

        
    }

   
}