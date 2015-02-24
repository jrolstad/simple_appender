using System.Collections.Generic;
using System.Net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace simple_appender.tests.Fakes
{
    public class FakeConnection : IConnection
    {
        public IModel Model { get; private set; }
        
        public FakeConnection WithModel(IModel model)
        {
            Model = model;

            return this;
        }


        public EndPoint LocalEndPoint { get; set; }

        public EndPoint RemoteEndPoint { get; set; }

        public int LocalPort { get; set; }

        public int RemotePort { get; set; }

        public void Dispose()
        {
            
        }

        public IModel CreateModel()
        {
            if(Model == null)
                Model = new FakeModel();
            return Model;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Close(ushort reasonCode, string reasonText)
        {
            IsOpen = false;
        }

        public void Close(int timeout)
        {
            IsOpen = false;
        }

        public void Close(ushort reasonCode, string reasonText, int timeout)
        {
            IsOpen = false;
        }

        public void Abort()
        {
            
        }

        public void Abort(ushort reasonCode, string reasonText)
        {
           
        }

        public void Abort(int timeout)
        {
            
        }

        public void Abort(ushort reasonCode, string reasonText, int timeout)
        {
            
        }

        public void HandleConnectionBlocked(string reason)
        {
            
        }

        public void HandleConnectionUnblocked()
        {
            
        }

        public AmqpTcpEndpoint Endpoint { get; set; }

        public IProtocol Protocol { get; set; }

        public ushort ChannelMax { get; set; }

        public uint FrameMax { get; set; }

        public ushort Heartbeat { get; set; }

        public IDictionary<string, object> ClientProperties { get; set; }

        public IDictionary<string, object> ServerProperties { get; set; }

        public AmqpTcpEndpoint[] KnownHosts { get; set; }

        public ShutdownEventArgs CloseReason { get; set; }

        public bool IsOpen { get; set; }

        public bool AutoClose { get; set; }

        public IList<ShutdownReportEntry> ShutdownReport { get; set; }

        public event ConnectionShutdownEventHandler ConnectionShutdown;
        public event CallbackExceptionEventHandler CallbackException;
        public event ConnectionBlockedEventHandler ConnectionBlocked;
        public event ConnectionUnblockedEventHandler ConnectionUnblocked;
    }
}