namespace Viglucci.UnityRSocket
{
    public class RSocketFlagUtils
    {
        public static int FrameTypeOffset { get; } = 10;
        public static int FlagsMask { get; } = 1023;

        public static bool HasMetadata(int flags)
        {
            return (flags & (int) RSocketFlagType.METADATA) == (int) RSocketFlagType.METADATA;
        }
        
        public static bool HasComplete(int flags)
        {
            return (flags & (int) RSocketFlagType.COMPLETE) == (int) RSocketFlagType.COMPLETE;
        }
        
        public static bool HasNext(int flags)
        {
            return (flags & (int) RSocketFlagType.NEXT) == (int) RSocketFlagType.NEXT;
        }
        
        public static bool HasFollows(int flags)
        {
            return (flags & (int) RSocketFlagType.FOLLOWS) == (int) RSocketFlagType.FOLLOWS;
        }
        
        public static bool HasIgnore(int flags)
        {
            return (flags & (int) RSocketFlagType.IGNORE) == (int) RSocketFlagType.IGNORE;
        }
        
        public static bool HasRespond(int flags)
        {
            return (flags & (int) RSocketFlagType.RESPOND) == (int) RSocketFlagType.RESPOND;
        }
        
        public static bool HasLease(int flags)
        {
            return (flags & (int) RSocketFlagType.LEASE) == (int) RSocketFlagType.LEASE;
        }
    }
}