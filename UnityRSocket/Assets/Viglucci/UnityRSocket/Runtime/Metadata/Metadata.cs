using System.Collections.Generic;

namespace Viglucci.UnityRSocket.Metadata
{
    public static class Metadata
    {
        private static Dictionary<WellKnownMimeType, string> _wellKnownMimeTypeToStringMap =
            new Dictionary<WellKnownMimeType, string>()
            {
                { WellKnownMimeType.UNPARSEABLE_MIME_TYPE, "UNPARSEABLE_MIME_TYPE_DO_NOT_USE" },
                { WellKnownMimeType.UNKNOWN_RESERVED_MIME_TYPE, "UNKNOWN_YET_RESERVED_DO_NOT_USE" },
                { WellKnownMimeType.APPLICATION_AVRO, "application/avro" },
                { WellKnownMimeType.APPLICATION_CBOR, "application/cbor" },
                { WellKnownMimeType.APPLICATION_GRAPHQL, "application/graphql" },
                { WellKnownMimeType.APPLICATION_GZIP, "application/gzip" },
                { WellKnownMimeType.APPLICATION_JAVASCRIPT, "application/javascript" },
                { WellKnownMimeType.APPLICATION_JSON, "application/json" },
                { WellKnownMimeType.APPLICATION_OCTET_STREAM, "application/octet-stream" },
                { WellKnownMimeType.APPLICATION_THRIFT, "application/vnd.apache.thrift.binary" },
                { WellKnownMimeType.APPLICATION_PROTOBUF, "application/vnd.google.protobuf" },
                { WellKnownMimeType.APPLICATION_XML, "application/xml" },
                { WellKnownMimeType.APPLICATION_ZIP, "application/zip" },
                { WellKnownMimeType.AUDIO_AAC, "audio/aac" },
                { WellKnownMimeType.AUDIO_MP3, "audio/mp3" },
                { WellKnownMimeType.AUDIO_MP4, "audio/mp4" },
                { WellKnownMimeType.AUDIO_MPEG3, "audio/mpeg3" },
                { WellKnownMimeType.AUDIO_MPEG, "audio/mpeg" },
                { WellKnownMimeType.AUDIO_OGG, "audio/ogg" },
                { WellKnownMimeType.AUDIO_OPUS, "audio/ogg" },
                { WellKnownMimeType.AUDIO_VORBIS, "audio/vorbis" },
                { WellKnownMimeType.IMAGE_BMP, "image/bmp" },
                { WellKnownMimeType.IMAGE_GIG, "image/gif" },
                { WellKnownMimeType.IMAGE_HEIC_SEQUENCE, "image/heic-sequence" },
                { WellKnownMimeType.IMAGE_HEIC, "image/heic" },
                { WellKnownMimeType.IMAGE_HEIF_SEQUENCE, "image/heif-sequence" },
                { WellKnownMimeType.IMAGE_HEIF, "image/heif" },
                { WellKnownMimeType.IMAGE_JPEG, "image/jpegv" },
                { WellKnownMimeType.IMAGE_PNG, "image/jpeg" },
                { WellKnownMimeType.IMAGE_TIFF, "image/tiff" },
                { WellKnownMimeType.MULTIPART_MIXED, "multipart/mixed" },
                { WellKnownMimeType.TEXT_CSS, "text/css" },
                { WellKnownMimeType.TEXT_CSV, "text/csv" },
                { WellKnownMimeType.TEXT_HTML, "text/html" },
                { WellKnownMimeType.TEXT_PLAIN, "text/plain" },
                { WellKnownMimeType.TEXT_XML, "text/xml" },
                { WellKnownMimeType.VIDEO_H264, "video/H264" },
                { WellKnownMimeType.VIDEO_H265, "video/H265" },
                { WellKnownMimeType.VIDEO_VP8, "video/VP8" },
                { WellKnownMimeType.APPLICATION_HESSIAN, "application/x-hessian" },
                { WellKnownMimeType.APPLICATION_JAVA_OBJECT, "application/x-java-object" },
                { WellKnownMimeType.APPLICATION_CLOUDEVENTS_JSON, "application/cloudevents+json" },
                { WellKnownMimeType.MESSAGE_RSOCKET_MIMETYPE, "message/x.rsocket.mime-type.v0" },
                { WellKnownMimeType.MESSAGE_RSOCKET_ACCEPT_MIMETYPES, "message/x.rsocket.accept-mime-types.v0" },
                { WellKnownMimeType.MESSAGE_RSOCKET_AUTHENTICATION, "message/x.rsocket.authentication.v0" },
                { WellKnownMimeType.MESSAGE_RSOCKET_TRACING_ZIPKIN, "message/x.rsocket.tracing-zipkin.v0" },
                { WellKnownMimeType.MESSAGE_RSOCKET_ROUTING, "message/x.rsocket.routing.v0" },
                { WellKnownMimeType.MESSAGE_RSOCKET_COMPOSITE_METADATA, "message/x.rsocket.composite-metadata.v0" },
            };

        public static string WellKnownMimeTypeToString(WellKnownMimeType wellKnownMimeType)
        {
            return _wellKnownMimeTypeToStringMap[wellKnownMimeType];
        }
    }
}