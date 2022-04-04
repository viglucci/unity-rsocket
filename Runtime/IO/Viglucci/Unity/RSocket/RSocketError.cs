using System;

namespace IO.Viglucci.Unity.RSocket
{
    public class RSocketError : Exception
    {
        public RSocketErrorCodes Code { get; }

        public RSocketError(RSocketErrorCodes code, string errorMessage)
            : base(errorMessage)
        {
            Code = code;
        }

        public RSocketError(int frameCode, string frameMessage)
            : this((RSocketErrorCodes) (uint) frameCode, frameMessage)
        {
        }
    }
}