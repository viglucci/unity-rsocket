using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class RequestChannelFrame : AbstractFrame
        {
            public override FrameType Type => FrameType.REQUEST_CHANNEL;

            public override List<byte> Serialize()
            {
                throw new NotImplementedException();
            }

            public RequestChannelFrame(int streamId) : base(streamId)
            {
            }
        }
    }
}