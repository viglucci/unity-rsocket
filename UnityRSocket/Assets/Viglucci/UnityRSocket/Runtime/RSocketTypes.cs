using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Viglucci.UnityRSocket.Frame;

namespace Viglucci.UnityRSocket
{
    public interface IPayload
    {
        public List<byte> Data { get; }

        public List<byte> Metadata { get; }
    }

    public interface ICancellable
    {
        public void Cancel();
    }

    public interface ISubscription : ICancellable
    {
        public void Request(int requestN);
    }

    public interface ISubscriber
    {
        public void OnNext(IPayload payload, bool isComplete);
        
        public void OnComplete();

        public void OnError(RSocketError error);
    }

    public interface IExtensionSubscriber
    {
        public void OnExtension(int extendedType, List<byte> content, bool canBeIgnored);
    }

    public interface IExtensionSubscriberWithCancellation : ICancellable, IExtensionSubscriber
    {
    }

    public interface IRequestableCancellableOnExtensenionSubscriber
        : ISubscriber, IExtensionSubscriber, ICancellable, IRequestable
    {
        
    }

    public interface IRequestable
    {
        public void Request(int requestN);
    }

    public interface ISubscriberExtensionSubscriberUnionWithSubscription : ISubscriber, IExtensionSubscriber,
        ISubscription
    {
    }

    public interface IRSocket : ICloseable
    {
        public ICancellable FireAndForget(IPayload payload, [NotNull] ISubscriber responderStream);

        public CancellableWrapper RequestResponse(IPayload payload, ISubscriber responderStream);

        public ICancellableRequestable RequestStream(IPayload payload,
            ISubscriber responderStream, int initialRequestN);

        public ISubscriberExtensionSubscriberUnionWithSubscription
            RequestChannel(IPayload payload, int initialRequestN, bool isComplete,
                ISubscriber responderStream);
    }

    public interface IClientTransport
    {
        // public abstract Task<IDuplexConnection> Connect();
        public abstract IDuplexConnection Connect();

        public abstract void ProcessMessages();
    }

    public interface IDuplexConnection : ICloseable, IMultiplexer, IDemultiplexer
    {
    }

    public interface IMultiplexer
    {
        IOutboundConnection ConnectionOutbound { get; }

        public void CreateRequestStream(IStreamFrameStreamLifecyleHandler frameHandler);
    }

    public interface IDemultiplexer
    {
        public void ConnectionInBound(IConnectionFrameHandler handler);

        public abstract void HandleRequestStream(RSocketStreamHandler handler);
    }

    public interface IFrameHandler
    {
        public void Handle(RSocketFrame.AbstractFrame abstractFrame);
        
        public void Close(Exception error);
    }

    public interface IConnectionFrameHandler : IFrameHandler
    {
        public void Pause();

        public void Resume();
    }

    public interface IStreamFrameHandler : IFrameHandler
    {
        FrameType StreamType { get; }

        int StreamId { get; }
    }

    public interface IStreamLifecycleHandle
    {
        public bool HandleReady(int streamId, IStream stream);

        public void HandleReject(Exception exception);
    }

    public interface IStream : IOutboundConnection
    {
    }
    
    public interface IStreamFrameStreamLifecyleHandler
        : IStreamFrameHandler, IStreamLifecycleHandle {}
    
    public interface IRequestableStreamFrameStreamLifecyleHandler
        : IStreamFrameHandler, IStreamLifecycleHandle, IRequestable {}
    
    public interface ICancellableRequestable : ICancellable, IRequestable {}
    
    public interface IAvailabilityProvider
    {
    }

    public interface IStreamIdGenerator
    {
        public void Next(Func<int, bool> callback, List<int> streams);
    }

    public interface ICloseable
    {
        public void Close(Exception error = null);

        public void OnClose(Action<Exception> error);
    }

    public interface IInboundConnection
    {
        public void Handle();
    }

    public interface IOutboundConnection
    {
        public void Send(ISerializableFrame<RSocketFrame.AbstractFrame> frame);
    }
}