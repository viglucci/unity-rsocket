using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public abstract class AbstractFrame
        {
            public int StreamId { get; private set; }
            public ushort Flags { get; set; }

            protected AbstractFrame(int streamId)
            {
                StreamId = streamId;
            }

            public abstract FrameType Type { get; }

            public abstract List<byte> Serialize();

            public List<byte> SerializeLengthPrefixed()
            {
                // TODO: improve memory allocation
                List<byte> bytes = Serialize();
                List<byte> lengthPrefixed = new List<byte>();
                BufferUtils.WriteUInt24BigEndian(lengthPrefixed, bytes.Count);
                lengthPrefixed.AddRange(bytes);
                return lengthPrefixed;
            }
        }
    }
}