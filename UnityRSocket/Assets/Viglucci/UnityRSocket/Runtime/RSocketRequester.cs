using System;
using System.Collections.Generic;
using Viglucci.UnityRSocket.Frame;

namespace Viglucci.UnityRSocket
{
    public class RSocketRequester : Deferred, IRSocket
    {
        private readonly IDuplexConnection _connection;

        public RSocketRequester(IDuplexConnection connection)
        {
            _connection = connection;
            _connection.OnClose(Close);
        }

        public ICancellable FireAndForget(IPayload payload, ISubscriber responderStream)
        {
            RequestFnFRequesterHandler handler
                = new RequestFnFRequesterHandler(payload, responderStream);

            _connection.CreateRequestStream(handler);

            return new CancellableWrapper(handler);
        }

        public CancellableWrapper RequestResponse(IPayload payload, ISubscriber responderStream)
        {
            RequestResponseRequesterStream handler
                = new RequestResponseRequesterStream(payload, responderStream);

            _connection.CreateRequestStream(handler);

            return new CancellableWrapper(handler);
        }

        public ICancellableRequestable RequestStream(IPayload payload,
            ISubscriber responderStream, int initialRequestN)
        {

            RequestStreamRequesterStream stream
                = new RequestStreamRequesterStream(payload, responderStream, initialRequestN);

            _connection.CreateRequestStream(stream);

            return new CancellableRequestableWrapper(stream);
        }

        public ISubscriberExtensionSubscriberUnionWithSubscription RequestChannel(IPayload payload, int initialRequestN,
            bool isComplete, ISubscriber responderStream)
        {
            throw new NotImplementedException();
        }
    }

    public class RequestResponseRequesterStream : IExtensionSubscriberWithCancellation, IStreamFrameStreamLifecyleHandler
    {
        private bool _done;

        private readonly IPayload _payload;
        private readonly ISubscriber _receiver;
        private IStream _stream;

        public FrameType StreamType { get; } = FrameType.REQUEST_RESPONSE;
        public int StreamId { get; private set; }

        public RequestResponseRequesterStream(IPayload payload, ISubscriber responderStream)
        {
            _payload = payload;
            _receiver = responderStream;
        }

        public bool HandleReady(int streamId, IStream stream)
        {
            if (_done)
            {
                return false;
            }

            StreamId = streamId;
            _stream = stream;

            ushort metaDataFlag = (ushort)(_payload.Metadata != null
                ? RSocketFlagType.METADATA
                : RSocketFlagType.NONE);
            
            RSocketFrame.RequestResponseFrame frame = new RSocketFrame.RequestResponseFrame(streamId)
            {
                Data = _payload.Data,
                Metadata = _payload.Metadata,
                Flags = metaDataFlag
            };

            stream.Send(frame);

            return true;
        }

        public void HandleReject(Exception exception)
        {
            if (_done)
            {
                return;
            }

            _done = true;
            
            _receiver.OnError(new RSocketError(RSocketErrorCodes.REJECTED, exception.Message));
        }

        public void Handle(RSocketFrame.AbstractFrame abstractFrame)
        {
            switch (abstractFrame.Type)
            {
                case FrameType.PAYLOAD:
                {
                    HandlePayloadFrame((RSocketFrame.AbstractRequestFrame) abstractFrame);
                    return;
                }
                case FrameType.ERROR:
                {
                    HandleErrorFrame((RSocketFrame.ErrorFrame) abstractFrame);
                    return;
                }
                case FrameType.EXT:
                {
                    throw new NotImplementedException();
                }
                default:
                {
                    Close(new RSocketError(RSocketErrorCodes.CANCELED, "Received unexpected frame"));

                    // TODO: send cancel frame

                    return;
                }
            }
        }

        private void HandleErrorFrame(RSocketFrame.ErrorFrame abstractFrame)
        {
            _done = true;

            // TODO: get actual error code and message from frame
            _receiver.OnError(new RSocketError(RSocketErrorCodes.REJECTED, "An unexpected error occurred"));
        }

        private void HandlePayloadFrame(RSocketFrame.AbstractRequestFrame abstractFrame)
        {
            bool hasComplete = RSocketFlagUtils.HasComplete(abstractFrame.Flags);
            bool hasPayload = RSocketFlagUtils.HasNext(abstractFrame.Flags);
            bool hasFollows = RSocketFlagUtils.HasFollows(abstractFrame.Flags);

            if (hasComplete || !hasFollows)
            {
                _done = true;

                if (!hasPayload)
                {
                    // TODO: add validation no frame in reassembly
                    _receiver.OnComplete();
                    return;
                }
            }

            RSocketPayload payload = new RSocketPayload()
            {
                Data = abstractFrame.Data,
                Metadata = abstractFrame.Metadata
            };

            _receiver.OnNext(payload, true);
        }

        public void OnExtension(int extendedType, List<byte> content, bool canBeIgnored)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            if (_done)
            {
                return;
            }

            _done = true;
            
            _stream.Send(new RSocketFrame.CancelFrame(StreamId));
        }

        public void Close(Exception error)
        {
            throw new NotImplementedException();
        }
    }

    public class RequestFnFRequesterHandler : IStreamFrameStreamLifecyleHandler, ICancellable
    {
        private bool done;

        private readonly IPayload _payload;
        private readonly ISubscriber _receiver;

        public FrameType StreamType { get; } = FrameType.REQUEST_FNF;
        public int StreamId { get; private set; }

        public RequestFnFRequesterHandler(IPayload payload, ISubscriber receiver)
        {
            _payload = payload;
            _receiver = receiver;
        }

        public void Handle(RSocketFrame.AbstractFrame abstractFrame)
        {
            throw new NotImplementedException();
        }

        public bool HandleReady(int streamId, IStream stream)
        {
            if (done)
            {
                return false;
            }

            StreamId = streamId;

            stream.Send(new RSocketFrame.RequestFnfFrame(streamId)
            {
                Data = _payload.Data,
                Metadata = _payload.Metadata
            });

            done = true;

            _receiver.OnComplete();

            return true;
        }

        public void HandleReject(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Close(Exception error)
        {
            throw new NotImplementedException();
        }
    }

    public class RequestStreamRequesterStream : IStreamFrameStreamLifecyleHandler, ICancellableRequestable
    {
        private readonly IPayload _payload;
        private readonly ISubscriber _receiver;
        private readonly int _initialRequestN;
        private bool _done;
        private IStream _stream;

        public FrameType StreamType { get; } = FrameType.REQUEST_STREAM;
        public int StreamId { get; private set; }

        public RequestStreamRequesterStream(IPayload payload, ISubscriber receiver, int initialRequestN)
        {
            _payload = payload;
            _receiver = receiver;
            _initialRequestN = initialRequestN;
        }
        
        public void Handle(RSocketFrame.AbstractFrame abstractFrame)
        {
            switch (abstractFrame.Type)
            {
                case FrameType.PAYLOAD:
                {
                    HandlePayloadFrame((RSocketFrame.AbstractRequestFrame) abstractFrame);
                    return;
                }
                case FrameType.ERROR:
                {
                    HandleErrorFrame((RSocketFrame.ErrorFrame) abstractFrame);
                    return;
                }
                case FrameType.EXT:
                {
                    throw new NotImplementedException();
                }
                default:
                {
                    Close(new RSocketError(RSocketErrorCodes.CANCELED, "Received unexpected frame"));

                    // TODO: send cancel frame

                    return;
                }
            }
        }

        private void HandleErrorFrame(RSocketFrame.ErrorFrame abstractFrame)
        {
            _done = true;

            // TODO: get actual error code and message from frame
            _receiver.OnError(new RSocketError(abstractFrame.Code, abstractFrame.Message));
        }

        private void HandlePayloadFrame(RSocketFrame.AbstractRequestFrame abstractFrame)
        {
            bool hasComplete = RSocketFlagUtils.HasComplete(abstractFrame.Flags);
            bool hasPayload = RSocketFlagUtils.HasNext(abstractFrame.Flags);
            bool hasFollows = RSocketFlagUtils.HasFollows(abstractFrame.Flags);

            if (hasComplete || !hasFollows)
            {
                _done = true;

                if (!hasPayload)
                {
                    // TODO: add validation no frame in reassembly
                    _receiver.OnComplete();
                    return;
                }
            }

            RSocketPayload payload = new RSocketPayload()
            {
                Data = abstractFrame.Data,
                Metadata = abstractFrame.Metadata
            };

            _receiver.OnNext(payload, hasComplete);
        }

        public void Close(Exception error)
        {
            throw new NotImplementedException();
        }
        
        public bool HandleReady(int streamId, IStream stream)
        {
            if (_done)
            {
                return false;
            }

            StreamId = streamId;
            _stream = stream;
            
            stream.Send(new RSocketFrame.RequestStreamFrame(streamId)
            {
                Data = _payload.Data,
                Metadata = _payload.Metadata,
                RequestN = _initialRequestN
            });

            return true;
        }

        public void HandleReject(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Request(int requestN)
        {
            throw new NotImplementedException();
        }
    }
}