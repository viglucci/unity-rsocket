using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class PayloadFrame : AbstractRequestFrame
        {
            public override FrameType Type => FrameType.PAYLOAD;

            public override List<byte> Serialize()
            {
                throw new NotImplementedException();
            }

            public PayloadFrame(int streamId) : base(streamId)
            {
            }
        }
    }
}