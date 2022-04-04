using System;
using System.Collections.Generic;

namespace IO.Viglucci.Unity.RSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class ResumeFrame : AbstractFrame
        {
            public override FrameType Type => FrameType.RESUME;


            public override List<byte> Serialize()
            {
                throw new NotImplementedException();
            }

            public ResumeFrame(int streamId) : base(streamId)
            {
            }
        }
    }
}