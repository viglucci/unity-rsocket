using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Metadata
{
    public class CompositeMetadata
    {
        private static int StreamMetadataKnownMask { get; } = 0x80;
        private static int StreamMetadataLengthMask { get; } = 0x7f;

        public static void EncodeAndAddWellKnownMetadata(List<byte> bytes, WellKnownMimeType mimeType, byte[] value)
        {
            int mimeTypeId = (int)mimeType;
            List<byte> header = EncodeWellKnownMetadataHeader(mimeTypeId, value.Length);
            bytes.AddRange(header);
            bytes.AddRange(value);
        }

        private static List<byte> EncodeWellKnownMetadataHeader(int mimeTypeId, int bytesCount)
        {
            List<byte> bytes = new List<byte>();
            BufferUtils.WriteInt8(bytes, (byte)(mimeTypeId | StreamMetadataKnownMask));
            BufferUtils.WriteUInt24BigEndian(bytes, bytesCount);
            return bytes;
        }
    }
}