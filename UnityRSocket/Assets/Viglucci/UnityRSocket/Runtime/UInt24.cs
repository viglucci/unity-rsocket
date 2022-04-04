using System.Collections.Generic;

namespace Viglucci.UnityRSocket
{
    public struct UInt24
    {
        private readonly byte _b0;
        private readonly byte _b1;
        private readonly byte _b2;
        
        public UInt24(uint value) {
            _b0 = (byte)(value & 0xFF);
            _b1 = (byte)(value >> 8); 
            _b2 = (byte)(value >> 16);
        }

        public IEnumerable<byte> BytesBigEndian => new List<byte>()
        {
            _b2,
            _b1,
            _b0
        };
        
        public IEnumerable<byte> Bytes => new List<byte>()
        {
            _b0,
            _b1,
            _b2
        };
    }
}