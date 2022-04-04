using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class CancelFrame : AbstractFrame, ISerializableFrame<AbstractFrame>
        {
            public override FrameType Type => FrameType.CANCEL;

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

                return bytes;
            }

            public CancelFrame(int streamId) : base(streamId)
            {
            }
        }
    }
}