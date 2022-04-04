using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class RequestStreamFrame : AbstractRequestFrame, ISerializableFrame<AbstractFrame>
        {
            public override FrameType Type => FrameType.REQUEST_STREAM;
            public int RequestN { get; set; }
            
            public RequestStreamFrame(int streamId) : base(streamId)
            {
            }
            
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
                
                // RequestN
                BufferUtils.WriteUInt32BigEndian(bytes, RequestN);
                
                WritePayload(bytes);
                
                return bytes;
            }
        }
    }
}