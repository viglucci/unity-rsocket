# Unity RSocket

`unity-rsocket` is a Unity compatible implementation of [RSocket](https://rsocket.io).

## Feature Support

### RSocket Core Features Support

- 🟨 Request Interactions (see "Interaction Models" below)
- ✔️ TCP Client Transport
- ❌ WebSocket Client Transport
- ❌ Leasing
- ❌ Resumability
- ✔️ Keepalive
- 🟨 Composite Metadata (wip)
- ✔️ Routing

### Interaction Models

Learn more about ["Stream Sequences and Lifetimes"](https://rsocket.io/about/protocol#stream-sequences-and-lifetimes).

- ✔️ Fire and Forget
- ✔️ Request Response
- ✔️ Request Stream
- ❌ Request Channel
- ❌ Metadata Push

## Examples

### Create an RSocket connector

```c#
private void Start()
{
    string host = "tcp://localhost";
    int port = "9090";

    IClientTransport transport = new TcpClientTransport(host, port);
    
    SetupOptions setupOptions = new SetupOptions(
        keepAliveInterval, // 3 seconds
        keepAliveTimeout, // 30 seconds
        data: new List<byte>(),
        metadata: new List<byte>(),
        dataMimeType:
        Metadata.WellKnownMimeTypeToString(WellKnownMimeType.APPLICATION_JSON),
        metadataMimeType:
        Metadata.WellKnownMimeTypeToString(WellKnownMimeType.MESSAGE_RSOCKET_COMPOSITE_METADATA)
    );

    RSocketConnector connector = new RSocketConnector(
        _transport,
        setupOptions,
        new MonoBehaviorScheduler());

    IRSocket rSocket = null;

    try
    {
        Debug.Log("Binding connector");
        _rSocket = connector.Bind();
    }
    catch (Exception e)
    {
        Debug.LogError(e);
        return;
    }

    Debug.Log("RSocket requester bound");
}
```

### Request Response

Request Response provides a familiar request & response interaction model, where a single request will result in a single response.

```c#
ICancellable cancellable = rSocket.RequestResponse(new RSocketPayload
    {
        Data = new List<byte>(Encoding.ASCII.GetBytes("PING"))
    },
    new Subscriber(
        (payload, isComplete) =>
        {
            string decodedData = Encoding.UTF8.GetString(payload.Data.ToArray());
            string decodedMetadata = Encoding.UTF8.GetString(payload.Metadata.ToArray());

            Debug.Log($"[data: {decodedData}, " +
                        $"metadata: {decodedMetadata}, " +
                        $"isComplete: {isComplete}]");

            if (isComplete)
            {
                Debug.Log("RequestResponse done");
            }
        },
        () => Debug.Log("RequestResponse done"),
        Debug.LogError
    ));
```

### Request Stream

Request Stream provides a interaction mode where a single request can produce a stream of responses from a responder. Request stream can often be through of as "subscribing" to events in a familiar sub/pub model.

```c#
int initialRequestN = 100;

_rSocket.RequestStream(new RSocketPayload
    {
        Data = new List<byte>(Encoding.ASCII.GetBytes("PING"))
    },
    new Subscriber(
        (payload, isComplete) =>
        {
            string decodedData = Encoding.UTF8.GetString(payload.Data.ToArray());
            string decodedMetadata = Encoding.UTF8.GetString(payload.Metadata.ToArray());

            Debug.Log($"[data: {decodedData}, " +
                        $"metadata: {decodedMetadata}, " +
                        $"isComplete: {isComplete}]");

            if (isComplete)
            {
                Debug.Log("RequestStream done");
            }
        },
        () => Debug.Log("RequestStream done"),
        (error) => Debug.LogError($"[code: {error.Code}, message: {error.Message}]", this)
    ),
    initialRequestN);
```

## FAQ

### Why not use [rsocket-net](https://github.com/rsocket/rsocket-net)?

In our tests, RSocket.NET was incompatible with the Unity scripting environment and is not compatible with Unity WebGL builds.

## Known Issues

### Performance

Library has not been performance profiled. Hot paths may not be optimized.

### Memory Usage

Memory allocation has not been profiled for performance considerations. Frame serialization & deserialization utilized `List<byte>` which is likely not as performant as other alternatives.

### Interaction Models

Not all interaction models are implemented. See legend above for full list of interaction models.
