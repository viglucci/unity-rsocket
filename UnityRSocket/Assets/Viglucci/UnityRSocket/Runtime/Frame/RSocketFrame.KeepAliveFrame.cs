using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class KeepAliveFrame : AbstractFrame, ISerializableFrame<AbstractFrame>
        {
            public override FrameType Type => FrameType.KEEPALIVE;

            public ulong LastReceivedPosition { get; set; }

            public override List<byte> Serialize()
            {
                List<byte> bytes = new List<byte>();

                // Stream ID
                BufferUtils.WriteUInt32BigEndian(bytes, StreamId);

                // Type and Flags
                int type = (int) Type << FrameTypeOffset;
                int flags = Flags & FlagsMask;
                Int16 typeAndFlags = (Int16) (type | flags);
                BufferUtils.WriteUInt16BigEndian(bytes, typeAndFlags);

                // Last received position
                BufferUtils.WriteUInt64BigEndian(bytes, 0);
                
                return bytes;
            }

            public KeepAliveFrame(int streamId) : base(streamId)
            {
            }
        }
    }
}