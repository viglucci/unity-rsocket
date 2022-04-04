using System.Collections.Generic;
using Viglucci.UnityRSocket.KeepAlive;
using Viglucci.UnityRSocket.Scheduling;

namespace Viglucci.UnityRSocket
{
    public class SetupOptions
    {
        private List<byte> _data;
        private List<byte> _metadata;
        private string _dataMimeType;
        private string _metadataMimeType;
        private int _keepAlive;
        private int _lifetime;

        public SetupOptions(int keepAlive, int lifetime,
            List<byte> metadata,
            List<byte> data,
            string metadataMimeType = "application/octet-stream",
            string dataMimeType = "application/octet-stream")
        {
            _lifetime = lifetime;
            _keepAlive = keepAlive;
            _metadataMimeType = metadataMimeType;
            _dataMimeType = dataMimeType;
            _metadata = metadata;
            _data = data;
        }

        public List<byte> Data => _data;

        public List<byte> Metadata => _metadata;

        public string DataMimeType => _dataMimeType;

        public string MetadataMimeType => _metadataMimeType;

        public int KeepAlive => _keepAlive;

        public int Lifetime => _lifetime;
    }

    public class RSocketConnector
    {
        private readonly IClientTransport _clientTransport;
        private readonly IScheduler _scheduler;
        private readonly Frame.RSocketFrame.SetupFrame _setupAbstractFrame;

        public RSocketConnector(
            IClientTransport clientTransport,
            SetupOptions setupOptions,
            IScheduler scheduler)
        {
            _clientTransport = clientTransport;
            _scheduler = scheduler;

            ushort metaDataFlag = (ushort)(setupOptions.Metadata != null
                ? RSocketFlagType.METADATA
                : RSocketFlagType.NONE);

            _setupAbstractFrame = new Frame.RSocketFrame.SetupFrame(0)
            {
                Data = setupOptions.Data,
                DataMimeType = setupOptions.DataMimeType,
                Metadata = setupOptions.Metadata,
                MetadataMimeType = setupOptions.MetadataMimeType,
                KeepAlive = setupOptions.KeepAlive,
                LifeTime = setupOptions.Lifetime,
                MajorVersion = 1,
                MinorVersion = 0,
                Flags = metaDataFlag
            };
        }

        public RSocketRequester Bind()
        {
            IDuplexConnection connection = _clientTransport.Connect();

            KeepAliveSender keepAliveSender = new KeepAliveSender(
                connection.ConnectionOutbound,
                _setupAbstractFrame.KeepAlive,
                _scheduler);

            KeepAliveHandler keepAliveHandler = new KeepAliveHandler(
                    connection,
                    _setupAbstractFrame.LifeTime,
                    _scheduler);
            
            IConnectionFrameHandler connectionFrameHandler
                = new DefaultConnectionFrameHandler(keepAliveHandler);

            connection.OnClose((error) =>
            {
                keepAliveSender.Close();
                keepAliveHandler.Close(); 
            });
            
            connection.ConnectionInBound(connectionFrameHandler);
            
            connection.ConnectionOutbound.Send(_setupAbstractFrame);
            
            keepAliveSender.Start();
            keepAliveHandler.Start();

            return new RSocketRequester(connection);
        }
    }
}