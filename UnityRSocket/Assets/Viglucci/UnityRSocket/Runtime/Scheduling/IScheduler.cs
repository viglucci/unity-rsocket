using System;

namespace Viglucci.UnityRSocket.Runtime.Scheduling
{
    public interface IScheduler
    {
        public int RunTimeout(float seconds, Action callback);

        public int StartInterval(float seconds, Action callback);

        public void Clear(int id);

        public void RunImmediate(Action callback);
    }
}