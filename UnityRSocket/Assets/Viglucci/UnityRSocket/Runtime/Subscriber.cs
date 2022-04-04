using System;

namespace Viglucci.UnityRSocket
{
    public class Subscriber : ISubscriber
    {
        private readonly Action<IPayload, bool> _onNext;
        private readonly Action _onComplete;
        private readonly Action<RSocketError> _onError;

        public Subscriber(
            Action<IPayload, bool> onNext,
            Action onComplete,
            Action<RSocketError> onError)
        {
            _onNext = onNext;
            _onComplete = onComplete;
            _onError = onError;
        }

        public void OnNext(IPayload payload, bool isComplete)
        {
            _onNext.Invoke(payload, isComplete);
        }

        public void OnComplete()
        {
            _onComplete.Invoke();
        }

        public void OnError(RSocketError error)
        {
            _onError.Invoke(error);
        }
    }
}