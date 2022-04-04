using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Metadata
{
    public class CompositeMetadataBuilder
    {
        private Dictionary<WellKnownMimeType, List<byte[]>> _wellKnownTypes =
            new Dictionary<WellKnownMimeType, List<byte[]>>();

        public CompositeMetadataBuilder WellKnown(WellKnownMimeType mimeType, byte[] value)
        {
            List<byte[]> metadata = BytesList(mimeType);
            metadata.Add(value);

            return this;
        }

        private List<byte[]> BytesList(WellKnownMimeType mimeType)
        {
            bool found = _wellKnownTypes.TryGetValue(mimeType, out List<byte[]> metadata);
            if (!found)
            {
                metadata = new List<byte[]>();
                _wellKnownTypes.Add(mimeType, metadata);
            }

            return metadata;
        }

        public List<byte> Build()
        {
            List<byte> bytes = new List<byte>();

            foreach (KeyValuePair<WellKnownMimeType, List<byte[]>> wellKnownType in _wellKnownTypes)
            {
                foreach (byte[] valueBytes in wellKnownType.Value)
                {
                    CompositeMetadata.EncodeAndAddWellKnownMetadata(bytes, wellKnownType.Key, valueBytes);
                }
            }

            return bytes;
        }
    }
}