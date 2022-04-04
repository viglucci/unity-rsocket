using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class ExtFrame : AbstractFrame
        {
            public override FrameType Type => FrameType.EXT;

            public override List<byte> Serialize()
            {
                throw new NotImplementedException();
            }

            public ExtFrame(int streamId) : base(streamId)
            {
            }
        }
    }
}