using System;
using System.Collections.Generic;
using System.Text;

namespace Viglucci.UnityRSocket.Frame
{
    public static partial class RSocketFrame
    {
        public class SetupFrame : AbstractRequestFrame, ISerializableFrame<SetupFrame>
        {
            public override FrameType Type => FrameType.SETUP;

            public string DataMimeType;
            public string MetadataMimeType;
            public int KeepAlive;
            public int LifeTime;
            public ushort MajorVersion;
            public ushort MinorVersion;

            public SetupFrame(int streamId) : base(streamId)
            {
            }

            public override List<byte> Serialize()
            {
                List<byte> bytes = new List<byte>();

                // Stream ID
                BufferUtils.WriteUInt32BigEndian(bytes, StreamId);

                // Type and Flags
                int type = (int) Type << FrameTypeOffset;
                int flags = Flags & FlagsMask;
                Int16 typeAndFlags = (Int16) (type | flags);
                BufferUtils.WriteUInt16BigEndian(bytes, typeAndFlags);

                BufferUtils.WriteUInt16BigEndian(bytes, MajorVersion);
                BufferUtils.WriteUInt16BigEndian(bytes, MinorVersion);
                BufferUtils.WriteUInt32BigEndian(bytes, KeepAlive);
                BufferUtils.WriteUInt32BigEndian(bytes, LifeTime);

                // TODO: handle resume token
                // - (16 bits = max value 65,535) Unsigned 16-bit integer of Resume Identification Token Length in bytes. (Not present if R flag is not set)
                // - Token used for client resume identification (Not present if R flag is not set)
                // const ushort resumeTokenLength = 0;
                // WriteBytes(bytes, BitConverter.GetBytes(resumeTokenLength));

                // MetadataMimeType length (uint8)
                byte metaDataMimeTypeLength = (byte) (MetadataMimeType != null
                    ? Encoding.ASCII.GetByteCount(MetadataMimeType)
                    : 0);
                BufferUtils.WriteInt8(bytes, metaDataMimeTypeLength);
                if (MetadataMimeType != null)
                {
                    // Protocol spec dictates ASCII
                    bytes.AddRange(Encoding.ASCII.GetBytes(MetadataMimeType));
                }

                // DataMimeType length (uint8)
                byte dataMimeTypeLength = (byte) (DataMimeType != null
                    ? Encoding.UTF8.GetByteCount(DataMimeType)
                    : 0);
                BufferUtils.WriteInt8(bytes, dataMimeTypeLength);
                // WriteBytes(bytes, dataMimeTypeLengthBytes);
                if (DataMimeType != null)
                {
                    // Protocol spec dictates ASCII
                    bytes.AddRange(Encoding.ASCII.GetBytes(DataMimeType));
                }

                WritePayload(bytes);

                return bytes;
            }
        }
    }
}