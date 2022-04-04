namespace Viglucci.UnityRSocket
{
    public class CancellableRequestableWrapper : ICancellableRequestable
    {
        private readonly ICancellableRequestable _cancellableRequestable;

        public CancellableRequestableWrapper(ICancellableRequestable cancellableRequestable)
        {
            _cancellableRequestable = cancellableRequestable;
        }

        public void Cancel()
        {
            _cancellableRequestable.Cancel();
        }

        public void Request(int requestN)
        {
            _cancellableRequestable.Request(requestN);
        }
    }
}