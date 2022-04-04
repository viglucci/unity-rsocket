using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Viglucci.UnityRSocket.Frame;

namespace Viglucci.UnityRSocket
{
    public abstract class ClientServerInputMultiplexerDemultiplexer : Deferred, IMultiplexer, IStream
    {
        private readonly IStreamIdGenerator _streamIdGenerator;
        private IConnectionFrameHandler _connectionFramesHandler;
        private RSocketStreamHandler _streamHandler;

        public IOutboundConnection ConnectionOutbound { get; }

        private readonly Dictionary<int, IStreamFrameHandler> _streamFrameHandlers =
            new Dictionary<int, IStreamFrameHandler>();

        protected ClientServerInputMultiplexerDemultiplexer(IStreamIdGenerator streamIdGenerator)
        {
            _streamIdGenerator = streamIdGenerator;
        }

        protected void Handle(RSocketFrame.AbstractFrame abstractFrame)
        {
            if (FrameUtils.IsConnectionFrame(abstractFrame))
            {
                if (abstractFrame.Type == FrameType.RESERVED)
                {
                    // TODO: throw
                    // TODO: understand why RESERVED frame is invalid here?
                    return;
                }

                _connectionFramesHandler.Handle(abstractFrame);
            }
            else if (FrameUtils.IsRequestFrame(abstractFrame))
            {
                // TODO: Request stream handling
                throw new NotImplementedException();
            }
            else
            {
                _streamFrameHandlers.TryGetValue(abstractFrame.StreamId, out IStreamFrameHandler handler);

                if (handler == null)
                {
                    Debug.LogWarning(
                        $"Failed to find handler for unknown stream {abstractFrame.StreamId} for frame type ${abstractFrame.Type}.");
                    return;
                }

                handler.Handle(abstractFrame);
            }
        }

        public void CreateRequestStream(IStreamFrameStreamLifecyleHandler streamHandler)
        {
            if (Done)
            {
                streamHandler.HandleReject(new Exception("Stream already closed."));
                return;
            }

            _streamIdGenerator.Next((streamId) =>
            {
                _streamFrameHandlers.Add(streamId, streamHandler);
                return streamHandler.HandleReady(streamId, this);
            }, _streamFrameHandlers.Keys.ToList());
        }

        public abstract void Send(ISerializableFrame<RSocketFrame.AbstractFrame> frame);

        public void ConnectionInBound(IConnectionFrameHandler handler)
        {
            _connectionFramesHandler = handler;
        }
    }
}