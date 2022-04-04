using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket
{
    public class StreamIdGenerator : IStreamIdGenerator
    {
        private int _currentId;

        public static StreamIdGenerator Create(int seed)
        {
            return new StreamIdGenerator(seed);
        }

        public StreamIdGenerator(int currentId)
        {
            _currentId = currentId;
        }

        public void Next(Func<int, bool> callback, List<int> streams)
        {
            int nextId = _currentId + 2;
            
            if (!callback(nextId))
            {
                return;
            }

            _currentId = nextId;
        }
    }
}