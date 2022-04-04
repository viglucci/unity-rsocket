using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Viglucci.UnityRSocket.Metadata;
using Viglucci.UnityRSocket.Scheduling;
using Viglucci.UnityRSocket.Transport;

namespace Viglucci.UnityRSocket.Example
{
    public class ClientManager : MonoBehaviour
    {
        private static ClientManager _instance;

        /**
     * How often to send KeepAlive frames.
     */
        public int keepAliveInterval = 10_000;

        /**
     * The max delay between a keep alive frame and a server ACK. Client will disconnect if server does not
     * respond to a KeepAlive frame within this time period.
     */
        public int keepAliveTimeout = 60_000;

        private IRSocket _rSocket;
        private IClientTransport _transport;

        private void Awake()
        {
            Debug.Log("Awake");

            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Debug.LogWarning(
                    $"Instance of {GetType().Name} already exists. Newly created instance will be destroyed.");
                Destroy(this);
            }
        }

        private void Start()
        {
            Debug.Log("Start");

            _transport = new TcpClientTransport("localhost", 9090);
    
            SetupOptions setupOptions = new SetupOptions(
                keepAliveInterval,
                keepAliveTimeout,
                data: new List<byte>(),
                metadata: new List<byte>(),
                dataMimeType:
                Metadata.Metadata.WellKnownMimeTypeToString(
                    WellKnownMimeType.APPLICATION_JSON),
                metadataMimeType:
                Metadata.Metadata.WellKnownMimeTypeToString(
                    WellKnownMimeType.MESSAGE_RSOCKET_COMPOSITE_METADATA)
            );

            RSocketConnector connector = new RSocketConnector(
                _transport,
                setupOptions,
                new MonoBehaviorScheduler());

            IRSocket rSocket = connector.Bind();
        
            try
            {
                Debug.Log("Binding connector");
                _rSocket = connector.Bind();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }

            OnRSocketConnected();
        }

        private void Update()
        {
            _transport.ProcessMessages();
        }

        private void OnRSocketConnected()
        {
            _rSocket.OnClose((ex) =>
            {
                Debug.Log("RSocket connection closed.");
                if (ex != null)
                    Debug.LogError(ex);
            });
        
            List<byte> data = new List<byte>(Encoding.UTF8.GetBytes("{ \"key\": \"value\"}"));
            List<byte> metadata = new List<byte>();

            ICancellable cancellable = _rSocket.RequestResponse(new RSocketPayload
                {
                    Data = data,
                    Metadata = metadata
                },
                new Subscriber(
                    (payload, isComplete) =>
                    {
                        string decodedData = Encoding.UTF8.GetString(payload.Data.ToArray());
                        string decodedMetadata = Encoding.UTF8.GetString(payload.Metadata.ToArray());

                        Debug.Log($"data: {decodedData}");
                        Debug.Log($"metadata: {decodedMetadata}");
                        Debug.Log($"isComplete: {isComplete}");

                        if (isComplete)
                        {
                            Debug.Log("RequestResponse done");
                        }
                    },
                    () => Debug.Log("RequestResponse done"),
                    Debug.LogError
                ));
        }
    }
}