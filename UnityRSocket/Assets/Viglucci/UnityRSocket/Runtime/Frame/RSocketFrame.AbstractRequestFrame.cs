using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public abstract class AbstractRequestFrame : AbstractFrame
        {
            protected AbstractRequestFrame(int streamId) : base(streamId)
            {
            }

            public List<byte> Data;
            public List<byte> Metadata;

            protected void WritePayload(List<byte> bytes)
            {
                // Check if Metadata flag is set
                if (FrameUtils.HasMetadataFlag(Flags))
                {
                    // Write metadata with length prefix if we have metadata
                    if (Metadata != null)
                    {
                        BufferUtils.WriteUInt24BigEndian(bytes, Metadata.Count);
                        bytes.AddRange(Metadata);
                    }
                    else
                    {
                        // Write zero length if we set flag but didn't provide data
                        bytes.Add(0);
                    }
                }

                if (Data == null) return;

                // Remainder of the frame is assumed to be data, no need to length prefix
                bytes.AddRange(Data);
            }
        }
    }
}