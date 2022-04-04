namespace Viglucci.UnityRSocket.Frame
{
    public static class FrameUtils
    {
        public static bool IsConnectionFrame(RSocketFrame.AbstractFrame abstractFrame)
        {
            return abstractFrame.StreamId == 0;
        }

        public static bool IsRequestFrame(RSocketFrame.AbstractFrame abstractFrame)
        {
            return FrameType.REQUEST_RESPONSE <= abstractFrame.Type
                   && abstractFrame.Type <= FrameType.REQUEST_CHANNEL;
        }

        public static bool HasMetadataFlag(ushort flags)
        {
            return (flags & (int)RSocketFlagType.METADATA) == (int)RSocketFlagType.METADATA;
        }
    }
}