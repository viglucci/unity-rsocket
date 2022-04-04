using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class RequestNFrame : AbstractRequestFrame, ISerializableFrame<RequestNFrame>
        {
            public override FrameType Type => FrameType.REQUEST_N;

            public RequestNFrame(int streamId) : base(streamId)
            {
            }
            
            public override List<byte> Serialize()
            {
                throw new NotImplementedException();
            }
        }
    }
}