namespace Viglucci.UnityRSocket.Frame
{
    public enum FrameType
    {
        RESERVED = 0x00, // Reserved
        SETUP = 0x01, // Setup: Sent by client to initiate protocol processing.
        LEASE = 0x02, // Lease: Sent by Responder to grant the ability to send requests.
        KEEPALIVE = 0x03, // Keepalive: Connection keepalive.
        REQUEST_RESPONSE = 0x04, // Request Response: Request single response.
        REQUEST_FNF = 0x05, // Fire And Forget: A single one-way message.
        REQUEST_STREAM = 0x06, // Request Stream: Request a completable stream.
        REQUEST_CHANNEL = 0x07, // Request Channel: Request a completable stream in both directions.
        REQUEST_N = 0x08, // Request N: Request N more items with Reactive Streams semantics.
        CANCEL = 0x09, // Cancel Request: Cancel outstanding request.
        PAYLOAD = 0x0a, // Payload: Payload on a stream. For example, response to a request, or message on a channel.
        ERROR = 0x0b, // Error: Error at connection or application level.
        METADATA_PUSH = 0x0c, // Metadata: Asynchronous Metadata frame
        RESUME = 0x0d, // Resume: Replaces SETUP for Resuming Operation (optional)
        RESUME_OK = 0x0e, // Resume OK : Sent in response to a RESUME if resuming operation possible (optional)
        EXT = 0x3f, // Extension Header: Used To Extend more frame types as well as extensions.
    }
}