using System;
using UnityEngine;
using Viglucci.UnityRSocket.Frame;
using Viglucci.UnityRSocket.Scheduling;

namespace Viglucci.UnityRSocket.KeepAlive
{
    public class KeepAliveHandler : Deferred, IFrameHandler
    {
        private readonly IDuplexConnection _duplexConnection;
        private readonly int _keepAliveTimeoutDurationMillis;
        private readonly IScheduler _scheduler;
        private long _lastReceivedMillis;
        private int _currentTimeoutId;

        public KeepAliveHandler(IDuplexConnection connection, int keepAliveTimeoutDurationMillis, IScheduler scheduler)
        {
            _duplexConnection = connection;
            _keepAliveTimeoutDurationMillis = keepAliveTimeoutDurationMillis;
            _scheduler = scheduler;
        }

        public void Handle(RSocketFrame.AbstractFrame abstractFrame)
        {
            _lastReceivedMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (RSocketFlagUtils.HasRespond(abstractFrame.Flags))
            {
                _duplexConnection.ConnectionOutbound.Send(new RSocketFrame.KeepAliveFrame(0)
                {
                    Flags = (ushort)(abstractFrame.Flags ^ (ushort)RSocketFlagType.RESPOND),
                    LastReceivedPosition = 0
                    // TODO: include data from received frame
                });
            }
        }

        public void Start()
        {
            _lastReceivedMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _currentTimeoutId = _scheduler.RunTimeout(
                (float)_keepAliveTimeoutDurationMillis / 1000,
                TimeoutCheck);
            OnClose(_ => _scheduler.Clear(_currentTimeoutId));
        }

        private void TimeoutCheck()
        {
            if (Done) return;
            
            Debug.Log("TimeoutCheck");
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long noKeepAliveDurationMillis = now - _lastReceivedMillis;
            if (noKeepAliveDurationMillis >= _keepAliveTimeoutDurationMillis)
            {
                _duplexConnection.Close(new Exception($"No keep-alive acks for {noKeepAliveDurationMillis} millis"));
                return;
            }

            float nextCheckMillis = _keepAliveTimeoutDurationMillis - noKeepAliveDurationMillis;
            float nextCheckSeconds = nextCheckMillis / 1000;
            float nextCheckSecondsBound = Math.Max(1.0f, nextCheckSeconds);
            _currentTimeoutId = _scheduler.RunTimeout(
                nextCheckSecondsBound,
                TimeoutCheck);
        }
    }
}