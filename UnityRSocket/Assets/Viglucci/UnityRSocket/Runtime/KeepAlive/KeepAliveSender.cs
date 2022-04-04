using Viglucci.UnityRSocket.Frame;
using Viglucci.UnityRSocket.Scheduling;

namespace Viglucci.UnityRSocket.KeepAlive
{
    public class KeepAliveSender : Deferred
    {
        private readonly IOutboundConnection _outboundConnection;
        private readonly int _keepAlivePeriodDurationMillis;
        private readonly IScheduler _scheduler;
        private int _currentIntervalId;

        public KeepAliveSender(
            IOutboundConnection outboundConnection,
            int keepAlivePeriodDurationMillis,
            IScheduler scheduler)
        {
            _outboundConnection = outboundConnection;
            _keepAlivePeriodDurationMillis = keepAlivePeriodDurationMillis;
            _scheduler = scheduler;
        }

        public void Start()
        {
            int timeSeconds = _keepAlivePeriodDurationMillis / 1000;
            _currentIntervalId = _scheduler.StartInterval(timeSeconds, SendKeepAlive);
            OnClose(_ => _scheduler.Clear(_currentIntervalId));
        }

        private void SendKeepAlive()
        {
            if (Done) return;
            _outboundConnection.Send(new RSocketFrame.KeepAliveFrame(0)
            {
                Flags = (ushort)RSocketFlagType.RESPOND
            });
        }
    }
}