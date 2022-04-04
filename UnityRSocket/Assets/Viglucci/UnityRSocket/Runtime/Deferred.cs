using System;
using System.Collections.Generic;
using UnityEngine;

namespace Viglucci.UnityRSocket
{
    public class Deferred : ICloseable
    {
        private readonly List<Action<Exception>> _onCloseCallbacks = new List<Action<Exception>>();
        private Exception _exception;
        protected bool Done { get; private set; }

        public void Close(Exception exception = null)
        {
            if (Done)
            {
                Debug.LogWarning("Cannot call close when already done.");
                return;
            }

            Done = true;
            _exception = exception;

            _onCloseCallbacks.ForEach(onCloseCallback => { onCloseCallback.Invoke(_exception); });
        }

        public void OnClose(Action<Exception> onCloseCallback)
        {
            if (Done)
            {
                onCloseCallback.Invoke(_exception);
                return;
            }

            _onCloseCallbacks.Add(onCloseCallback);
        }
    }
}