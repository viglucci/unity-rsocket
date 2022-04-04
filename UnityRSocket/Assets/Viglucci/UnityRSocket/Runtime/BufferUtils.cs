using System;
using System.Collections.Generic;

namespace Viglucci.UnityRSocket
{
    public static class BufferUtils
    {
        public static void WriteUInt64BigEndian(List<byte> target, Int64 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            target.AddRange(bytes);
        }
        
        public static void WriteUInt32BigEndian(List<byte> target, Int32 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            target.AddRange(bytes);
        }

        public static void WriteUInt16BigEndian(List<byte> target, int value)
        {
            WriteUInt16BigEndian(target, (short)value);
        }

        public static void WriteUInt16BigEndian(List<byte> target, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            target.AddRange(bytes);
        }

        public static void WriteInt8(List<byte> target, byte value)
        {
            byte[] bytes = { value };
            target.AddRange(bytes);
        }

        public static void WriteUInt24BigEndian(List<byte> target, int value)
        {
            UInt24 length = new UInt24((uint)value);
            target.AddRange(length.BytesBigEndian);
        }

        public static (int value, int nextOffset) ReadUint16BigEndian(List<byte> target, int offset)
        {
            byte[] bytes = target.GetRange(offset, 2).ToArray();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            int value = BitConverter.ToInt16(bytes, 0);
            return (value, offset + 2);
        }

        public static (int value, int nextOffset) ReadUInt24BigEndian(List<byte> target, int offset)
        {
            int value1 = target[offset] << 16;
            int value2 = target[offset + 1] << 24;
            int value3 = target[offset + 2];
            return (value1 | value2 | value3, offset + 3);
        }

        public static (int value, int nextOffset) ReadUInt32BigEndian(List<byte> target, int offset)
        {
            byte[] bytes = target.GetRange(offset, 4).ToArray();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            int value = BitConverter.ToInt32(bytes, 0);
            return (value, offset + 4);
        }
        
        public static (ulong value, int nextOffset) ReadUInt64BigEndian(List<byte> target, int offset)
        {
            // (int low, int nextOffset) = ReadUInt32BigEndian(target, offset);
            // (int high, _) = ReadUInt32BigEndian(target, offset + 4);
            
            byte[] bytes = target.GetRange(offset, 8).ToArray();
            
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            ulong value = (ulong) BitConverter.ToInt64(bytes, 0);
            return (value, offset + 8);
        }
    }
}