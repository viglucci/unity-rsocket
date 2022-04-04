using System.Collections.Generic;

namespace Viglucci.UnityRSocket
{
    public class RSocketPayload : IPayload
    {
        public List<byte> Data { get; set; }
        public List<byte> Metadata { get; set; }
    }
}