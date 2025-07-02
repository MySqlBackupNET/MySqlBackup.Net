using System;
using System.IO.Compression;
using System.IO;

namespace System
{
    public static class CompressorHelper
    {
        public static byte[] Compress(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(output, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            using (var input = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzipStream.CopyTo(output);
                return output.ToArray();
            }
        }
    }
}