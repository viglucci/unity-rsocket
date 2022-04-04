using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Viglucci.UnityRSocket.Frame;

namespace Viglucci.UnityRSocket.Transport
{
    public class TcpDuplexConnection : ClientServerInputMultiplexerDemultiplexer, IDuplexConnection
    {
        public static int DataBufferSize = 4096;

        private readonly byte[] _receiveBuffer;
        private List<byte> _remainingBuffer = new List<byte>();
        private Thread _receiveThread;
        private readonly TcpClient _socket;

        public new IOutboundConnection ConnectionOutbound => this;

        public TcpDuplexConnection(TcpClient socket) : base(StreamIdGenerator.Create(-1))
        {
            _receiveBuffer = new byte[DataBufferSize];
            _socket = socket;
        }
        
        public void Listen()
        {
            _receiveThread = new Thread(ListenForData)
            {
                IsBackground = true
            };
            _receiveThread.Start();
        }
        
        public new void Close(Exception e)
        {
            _receiveThread.Abort();
            base.Close(e);
        }

        private void ListenForData()
        {
            while (true)
            {
                using NetworkStream stream = _socket.GetStream();
                int length; 									
                while ((length = stream.Read(_receiveBuffer, 0, DataBufferSize)) != 0) { 						
                    byte[] received = new byte[length]; 						
                    Array.Copy(_receiveBuffer, 0, received, 0, length); 						
                    HandleData(received);
                }
            }
        }

        private void HandleData(byte[] data)
        {
            List<byte> buffer = _remainingBuffer.Concat(data).ToList();
            int lastOffset = 0;

            IEnumerator<(RSocketFrame.AbstractFrame frame, int offset)> frames
                = FrameDeserializer.DeserializeFrames(buffer);

            while (frames.MoveNext())
            {
                lastOffset = frames.Current.offset;
                Handle(frames.Current.frame);
            }

            _remainingBuffer = new ArraySegment<byte>(data, lastOffset, data.Length - lastOffset).ToList();
        }

        public override void Send(ISerializableFrame<RSocketFrame.AbstractFrame> frame)
        {
            List<byte> bytes = frame.SerializeLengthPrefixed();
            Send(bytes);
        }

        private void Send(List<byte> bytes)
        {
            try
            {
                NetworkStream stream = _socket.GetStream(); 			
                if (stream.CanWrite) {
                    stream.Write(bytes.ToArray(), 0, bytes.Count);                        
                }
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
        }

        public void HandleRequestStream(RSocketStreamHandler handler)
        {
            throw new NotImplementedException();
        }

        private void HandleConnectionError(Exception exception)
        {
            Close(new Exception("TCP connection error: " + exception.Message));
        }
    }
}