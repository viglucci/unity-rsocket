namespace Viglucci.UnityRSocket
{
    public class CancellableWrapper : ICancellable
    {
        private readonly ICancellable _cancellable;

        public CancellableWrapper(ICancellable cancellable)
        {
            _cancellable = cancellable;
        }

        public void Cancel()
        {
            _cancellable.Cancel();
        }
    }
}