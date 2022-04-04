using System;
using System.Collections.Generic;
using System.Text;

namespace Viglucci.UnityRSocket.Metadata
{
    public static class Routing
    {
        public static byte[] EncodeRoute(string route)
        {
            byte[] routeBytes = Encoding.ASCII.GetBytes(route);
            if (routeBytes.Length > 255)
            {
                throw new Exception(
                    $"Route length must fit within unsigned byte length. Given route length is {routeBytes.Length}");
            }

            List<byte> bytes = new List<byte>();
            BufferUtils.WriteInt8(bytes, (byte)routeBytes.Length);
            bytes.AddRange(routeBytes);
            return bytes.ToArray();
        }
    }
}