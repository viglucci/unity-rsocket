using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class ResumeOkFrame : AbstractFrame
        {
            public override FrameType Type => FrameType.RESUME_OK;


            public override List<byte> Serialize()
            {
                throw new NotImplementedException();
            }

            public ResumeOkFrame(int streamId) : base(streamId)
            {
            }
        }
    }
}