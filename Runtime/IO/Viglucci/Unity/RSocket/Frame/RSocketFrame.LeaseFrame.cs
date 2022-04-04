using System;
using System.Collections.Generic;

namespace IO.Viglucci.Unity.RSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class LeaseFrame : AbstractFrame
        {
            public override FrameType Type => FrameType.LEASE;

            public override List<byte> Serialize()
            {
                throw new NotImplementedException();
            }

            public LeaseFrame(int streamId) : base(streamId)
            {
            }
        }
    }
}