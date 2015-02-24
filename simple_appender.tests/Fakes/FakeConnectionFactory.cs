﻿using RabbitMQ.Client;

namespace simple_appender.tests.Fakes
{
    public class FakeConnectionFactory:ConnectionFactory
    {
        public IConnection Connection { get; private set; }

        public FakeConnectionFactory WithConnection(IConnection connection)
        {
            Connection = connection;
            return this;
        }

        public FakeConnection UnderlyingConnection
        {
            get { return (FakeConnection) Connection; }
        }

        public FakeModel UnderlyingModel
        {
            get { return (FakeModel) UnderlyingConnection.Model; }
        }

        public override IConnection CreateConnection()
        {
            if(Connection == null)
                Connection = new FakeConnection();

            return Connection;
        }
    }
}