using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Viglucci.UnityRSocket.Scheduling
{
    internal class MainThreadCoroutineHelpingMonoBehaviour : MonoBehaviour
    {
        private readonly Queue<Coroutine> _queue = new Queue<Coroutine>();
        private Thread _mainThread;

        private void Awake()
        {
            _mainThread = Thread.CurrentThread;
        }

        protected void Update()
        {
            while (_queue.Count != 0)
            {
                Coroutine coroutine = _queue.Dequeue();
                StopCoroutine(coroutine);
            }
        }

        public void StopCoroutineOnMainThread(Coroutine coroutine)
        {
            // if main thread, StopCoroutine, otherwise, add to queue to stop next update
            if (IsMainThread())
                StopCoroutine(coroutine);
            else
                _queue.Enqueue(coroutine);
        }

        private bool IsMainThread()
        {
            return Thread.CurrentThread == _mainThread;
        }
    }

    public class MonoBehaviorScheduler : IScheduler
    {
        private int _nextIntervalId;
        private readonly MainThreadCoroutineHelpingMonoBehaviour _monoBehaviour;

        private readonly Dictionary<int, Coroutine> _intervals
            = new Dictionary<int, Coroutine>();

        public MonoBehaviorScheduler()
        {
            GameObject go = new GameObject();
            MainThreadCoroutineHelpingMonoBehaviour mainThreadCoroutineHelpingMonoBehaviour
                = go.AddComponent<MainThreadCoroutineHelpingMonoBehaviour>();
            go.name = "RSocketMonoBehaviorScheduler";

            _monoBehaviour = mainThreadCoroutineHelpingMonoBehaviour;
        }

        public int RunTimeout(float seconds, Action callback)
        {
            int intervalId = _nextIntervalId;
            _intervals.Add(intervalId, DoAfterSeconds(seconds, callback));
            _nextIntervalId += 1;
            return intervalId;
        }

        private Coroutine DoAfterSeconds(float seconds, Action callback)
        {
            return _monoBehaviour.StartCoroutine(DoAfterSecondsCo(seconds, callback));
        }

        private IEnumerator DoAfterSecondsCo(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);

            callback.Invoke();
        }

        public int StartInterval(float seconds, Action callback)
        {
            int intervalId = _nextIntervalId;
            _intervals.Add(intervalId, RunInterval(intervalId, seconds, callback));
            _nextIntervalId += 1;
            return intervalId;
        }

        private Coroutine RunInterval(int intervalId, float seconds, Action callback)
        {
            return DoAfterSeconds(seconds, () =>
            {
                callback.Invoke();
                _intervals[intervalId] = RunInterval(intervalId, seconds, callback);
            });
        }

        public void Clear(int id)
        {
            if (!_intervals.ContainsKey(id))
            {
                return;
            }

            _intervals.TryGetValue(id, out Coroutine value);
            _monoBehaviour.StopCoroutineOnMainThread(value);
        }

        public void RunImmediate(Action callback)
        {
            callback.Invoke();
        }
    }
}