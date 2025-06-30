using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.IO.Compression;

namespace System
{
    public class ZipHelper
    {
        public static void ZipFile(string sourceFilePath, string destinationFilePathZip)
        {
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException($"Source file not found: {sourceFilePath}");

            // Ensure destination directory exists
            string destinationDir = Path.GetDirectoryName(destinationFilePathZip);
            if (!string.IsNullOrEmpty(destinationDir))
                Directory.CreateDirectory(destinationDir);

            using (var zip = ZipStorer.Create(destinationFilePathZip, ""))
            {
                string entryName = Path.GetFileName(sourceFilePath);
                zip.AddFile(ZipStorer.Compression.Deflate, sourceFilePath, entryName, "");
            }
        }

        public static void ZipMemoryStream(string fileName, Stream streamSource, Stream streamDestination)
        {
            using (var zip = ZipStorer.Create(streamDestination, ""))
            {
                streamSource.Position = 0;
                zip.AddStream(ZipStorer.Compression.Deflate, fileName, streamSource, DateTime.Now, "");
            }
        }

        public static void ExtractFile(string sourceZipFilePath, string destinationFilePath)
        {
            if (!File.Exists(sourceZipFilePath))
                throw new FileNotFoundException($"Source zip file not found: {sourceZipFilePath}");

            // Ensure destination directory exists
            string destinationDir = Path.GetDirectoryName(destinationFilePath);
            if (!string.IsNullOrEmpty(destinationDir))
                Directory.CreateDirectory(destinationDir);

            using (ZipStorer zip = ZipStorer.Open(sourceZipFilePath, FileAccess.Read))
            {
                var files = zip.ReadCentralDir();

                // Extract the first file found, or you could make this more specific
                var firstFile = files.FirstOrDefault();
                if (firstFile != null)
                {
                    zip.ExtractFile(firstFile, destinationFilePath);
                }
                else
                {
                    throw new InvalidOperationException("No files found in the zip archive.");
                }
            }
        }

        public static void ExtractToStream(Stream sourceStream, Stream destinationStream)
        {
            sourceStream.Position = 0;

            using (ZipStorer zip = ZipStorer.Open(sourceStream, FileAccess.Read))
            {
                var files = zip.ReadCentralDir();

                // Extract the first file found to the destination stream
                var firstFile = files.FirstOrDefault();
                if (firstFile != null)
                {
                    zip.ExtractFile(firstFile, destinationStream);
                    destinationStream.Position = 0; // Reset position for reading
                }
                else
                {
                    throw new InvalidOperationException("No files found in the zip archive.");
                }
            }
        }
    }
}