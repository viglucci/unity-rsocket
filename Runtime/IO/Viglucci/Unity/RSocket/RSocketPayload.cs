using System.Collections.Generic;

namespace IO.Viglucci.Unity.RSocket
{
    public class RSocketPayload : IPayload
    {
        public List<byte> Data { get; set; }
        public List<byte> Metadata { get; set; }
    }
}