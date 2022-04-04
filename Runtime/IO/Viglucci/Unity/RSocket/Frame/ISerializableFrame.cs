using System.Collections.Generic;

namespace IO.Viglucci.Unity.RSocket.Frame
{
    public interface ISerializableFrame<out T>
    {
        public List<byte> Serialize();

        List<byte> SerializeLengthPrefixed();
    }
}