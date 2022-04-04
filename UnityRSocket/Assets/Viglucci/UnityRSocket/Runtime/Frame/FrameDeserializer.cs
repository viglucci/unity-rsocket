using System;
using System.Collections.Generic;
using System.Text;

namespace Viglucci.UnityRSocket.Frame
{
    public static class FrameDeserializer
    {
        private const int Uint24Size = 3;

        public static IEnumerator<(RSocketFrame.AbstractFrame frame, int offset)> DeserializeFrames(List<byte> bytes)
        {
            int offset = 0;
            while (offset + Uint24Size < bytes.Count)
            {
                (int frameLength, _) = BufferUtils.ReadUInt24BigEndian(bytes, offset);
                int frameStart = offset + Uint24Size;
                int frameEnd = frameStart + frameLength;
                if (frameEnd > bytes.Count)
                {
                    // not all bytes of next frame received
                    yield break;
                }

                List<byte> frameBuffer = bytes.GetRange(frameStart, frameLength);
                RSocketFrame.AbstractFrame abstractFrame = DeserializeFrame(frameBuffer);
                offset = frameEnd;
                yield return (abstractFrame, offset);
            }
        }

        public static RSocketFrame.AbstractFrame DeserializeFrame(List<byte> frameBuffer)
        {
            int offset = 0;
            
            (int value, int nextOffset) streamId = BufferUtils.ReadUInt32BigEndian(frameBuffer, offset);
            offset = streamId.nextOffset;
            
            (int value, int nextOffset) typeAndFlags = BufferUtils.ReadUint16BigEndian(frameBuffer, offset);
            
            // keep highest 6 bits
            int type = (int)((uint)typeAndFlags.value >> RSocketFlagUtils.FrameTypeOffset);
            
            // keep lowest 10 bits
            int flags = typeAndFlags.value & RSocketFlagUtils.FlagsMask;
            
            offset = typeAndFlags.nextOffset;
            
            return (FrameType)type switch
            {
                FrameType.PAYLOAD => DeserializePayloadFrame(frameBuffer, streamId.value, flags, offset),
                FrameType.ERROR => DeserializeErrorFrame(frameBuffer, streamId.value, flags, offset),
                FrameType.KEEPALIVE => DeserializeKeepAliveFrame(frameBuffer, streamId.value, flags, offset),
                _ => throw new NotImplementedException()
            };
        }

        private static RSocketFrame.AbstractFrame DeserializeKeepAliveFrame(
            List<byte> frameBuffer,
            int streamId,
            int _,
            int offset)
        {
            (ulong value, int nextOffset) lastReceivedPosition = BufferUtils.ReadUInt64BigEndian(frameBuffer, offset);

            // TODO: Per spec KeepAlive frame can contain data. Read data and set on Frame.

            return new RSocketFrame.KeepAliveFrame(streamId)
            {
                LastReceivedPosition = lastReceivedPosition.value
            };
        }

        private static RSocketFrame.AbstractFrame DeserializeErrorFrame(List<byte> frameBuffer, int streamId, int flags,
            int offset)
        {
            (int value, int nextOffset) code = BufferUtils.ReadUInt32BigEndian(frameBuffer, offset);
            offset = code.nextOffset;
            int messageLength = frameBuffer.Count - offset;
            string message = "";
            if (messageLength > 0)
            {
                byte[] messageBytes = frameBuffer.GetRange(offset, messageLength).ToArray();
                message = Encoding.UTF8.GetString(messageBytes);
            }

            return new RSocketFrame.ErrorFrame(streamId, code.value, message);
        }

        private static RSocketFrame.AbstractFrame DeserializePayloadFrame(
            List<byte> frameBuffer,
            int streamId,
            int flags,
            int offset)
        {
            RSocketFrame.PayloadFrame frame = new RSocketFrame.PayloadFrame(streamId)
            {
                Flags = (ushort)flags
            };
            
            ReadPayload(frameBuffer, frame, offset);
            
            return frame;
        }

        private static void ReadPayload(List<byte> frameBuffer, RSocketFrame.AbstractRequestFrame frame, int offset)
        {
            frame.Metadata = new List<byte>();
            frame.Data = new List<byte>();
            
            if (RSocketFlagUtils.HasMetadata(frame.Flags))
            {
                (int value, int nextOffset) metadataLength
                    = BufferUtils.ReadUInt24BigEndian(frameBuffer, offset);

                offset = metadataLength.nextOffset;

                if (metadataLength.value > 0)
                {
                    frame.Metadata.AddRange(frameBuffer.GetRange(
                        offset,
                        offset + metadataLength.value));
                    offset += metadataLength.value;
                }
            }

            if (offset < frameBuffer.Count)
            {
                List<byte> data = frameBuffer.GetRange(
                    offset,
                    frameBuffer.Count - offset);
                frame.Data.AddRange(data);
            }
        }
    }
}