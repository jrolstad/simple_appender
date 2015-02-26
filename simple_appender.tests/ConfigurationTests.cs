using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using log4net.Config;
using NUnit.Framework;

namespace simple_appender.tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void HowToGetConfigurationData()
        {
            // Act
            var config = (NameValueCollection)ConfigurationManager.GetSection("PPASectionGroup/PPA.Data.Database.Cortex");

            // Assert
            var server = config["Server"];
            var database = config["database"];

            Assert.That(server.ToLower(),Is.StringContaining("cortexdb"));
            Assert.That(database.ToLower(),Is.StringContaining("cortex_"));
        }

        [Test]
        public void Get_TimeZones()
        {
            foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
                Console.WriteLine("{0}|{1}|{2}|{3}|{4}|", z.Id, z.DisplayName, z.StandardName, z.DaylightName, z.BaseUtcOffset);
        }

        [Test]
        public void Sandbox()
        {
            var collection = new BlockingCollection<int>();

            collection
                .GetConsumingEnumerable()
                .ToObservable(NewThreadScheduler.Default)
                .Subscribe(SendMessage);

            collection.Add(1);
            collection.Add(2);
            collection.Add(3);
            collection.Add(4);

            // Wait for everyting to be written out
            Thread.Sleep(1000);

        }

        private void SendMessage(int data)
        {
            Console.WriteLine("{0}|{1}",Thread.CurrentThread.ManagedThreadId,data);
        }
    }
}