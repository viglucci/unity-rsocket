using System;
using IO.Viglucci.Unity.RSocket.Frame;
using IO.Viglucci.Unity.RSocket.KeepAlive;

namespace IO.Viglucci.Unity.RSocket
{
    public class DefaultConnectionFrameHandler : IConnectionFrameHandler
    {
        private readonly KeepAliveHandler _keepAliveHandler;

        public DefaultConnectionFrameHandler(KeepAliveHandler keepAliveHandler)
        {
            _keepAliveHandler = keepAliveHandler;
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Handle(RSocketFrame.AbstractFrame abstractFrame)
        {
            switch (abstractFrame.Type)
            {
                case FrameType.KEEPALIVE:
                {
                    _keepAliveHandler.Handle(abstractFrame);
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void Close(Exception error)
        {
            throw new NotImplementedException();
        }
    }
}