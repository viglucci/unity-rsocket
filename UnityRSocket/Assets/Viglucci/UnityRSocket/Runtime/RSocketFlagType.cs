namespace Viglucci.UnityRSocket
{
    public enum RSocketFlagType
    {
        NONE = 0,
        COMPLETE = 0x40, // PAYLOAD, REQUEST_CHANNEL: indicates stream completion, if set onComplete will be invoked on receiver.
        FOLLOWS = 0x80, // PAYLOAD, REQUEST_XXX: indicates that frame was fragmented and requires reassembly
        IGNORE = 0x200, // (all): Ignore frame if not understood.
        LEASE = 0x40, // SETUP: Will honor lease or not.
        METADATA = 0x100, // (all): must be set if metadata is present in the frame.
        NEXT = 0x20, // PAYLOAD: indicates data/metadata present, if set onNext will be invoked on receiver.
        RESPOND = 0x80, // KEEPALIVE: should KEEPALIVE be sent by peer on receipt.
        RESUME_ENABLE = 0x80, // SETUP: Client requests resume capability if possible. Resume Identification Token present.
    }
}