using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class ErrorFrame : AbstractFrame
        {
            public int Code { get; }
            public string Message { get; }

            public override FrameType Type => FrameType.ERROR;

            public override List<byte> Serialize()
            {
                throw new NotImplementedException();
            }

            public ErrorFrame(int streamId, int codeValue, string message) : base(streamId)
            {
                Code = codeValue;
                Message = message;
            }
        }
    }
}