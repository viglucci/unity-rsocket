using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public interface ISerializableFrame<out T>
    {
        public List<byte> Serialize();

        List<byte> SerializeLengthPrefixed();
    }
}