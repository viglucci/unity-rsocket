using System;
using System.Net.Sockets;

namespace Viglucci.UnityRSocket.Transport
{
    public class TcpClientTransport : IClientTransport
    {
        private readonly string _host;
        private readonly int _port;
        private TcpClient _socket;

        public TcpClientTransport(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public IDuplexConnection Connect()
        {
            _socket = new TcpClient(_host, _port)
            {
                ReceiveBufferSize = TcpDuplexConnection.DataBufferSize,
                SendBufferSize = TcpDuplexConnection.DataBufferSize
            };

            TcpDuplexConnection tcpDuplexConnection
                = new TcpDuplexConnection(_socket);

            tcpDuplexConnection.Listen();
            
            return tcpDuplexConnection;
        }

        public void ProcessMessages()
        {
            // noop
        }

        public IDuplexConnection ConnectSync()
        {
            throw new NotImplementedException();
        }
    }
}