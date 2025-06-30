// ZipStorer, by Jaime Olivares
// Website: http://github.com/jaime-olivares/zipstorer

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Compression
{
    /// <summary>
    /// Unique class for compression/decompression file. Represents a Zip file.
    /// </summary>
    public class ZipStorer : IDisposable
    {
        private const uint LocalFileHeaderSignature = 0x04034b50;
        private const uint CentralDirHeaderSignature = 0x02014b50;
        private const uint EndOfCentralDirSignature = 0x06054b50;
        private const uint Zip64EndOfCentralDirSignature = 0x06064b50;
        private const uint Zip64EndOfCentralDirLocatorSignature = 0x07064b50;

        /// <summary>
        /// Compression method enumeration
        /// </summary>
        public enum Compression : ushort
        {
            /// <summary>Uncompressed storage</summary> 
            Store = 0,
            /// <summary>Deflate compression method</summary>
            Deflate = 8
        }

        #region Public properties
        /// <summary>True if UTF8 encoding for filename and comments, false if default (CP 437)</summary>
        public bool EncodeUTF8 { get; set; } = false;
        /// <summary>Force deflate algotithm even if it inflates the stored file. Off by default.</summary>
        public bool ForceDeflating { get; set; } = false;
        #endregion

        #region Private fields
        // List of files to store
        private readonly List<ZipFileEntry> Files = new List<ZipFileEntry>();
        // List of files in Central Directory
        private readonly List<ZipFileEntry> CentralDirectoryFiles = new List<ZipFileEntry>();
        // Filename of storage file
        private string FileName;
        // Stream object of storage file
        private Stream ZipFileStream;
        // General comment
        private string Comment = string.Empty;
        // Central dir image
        private byte[] CentralDirImage = null;
        // Existing files in zip
        private long ExistingFiles = 0;
        // File access for Open method
        private FileAccess Access;
        // Dispose control
        private bool IsDisposed = false;
        // Default filename encoder
        private static Encoding DefaultEncoding;
        // leave the stream open after the ZipStorer object is disposed
        private bool LeaveOpen;
        #endregion

        #region Public static methods
        static ZipStorer()
        {
            // Configure CP 437 encoding
#if NET5_0_OR_GREATER
            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            DefaultEncoding = Encoding.GetEncoding(437);
        }

        /// <summary>
        /// Method to create a new storage file
        /// </summary>
        /// <param name="filename">Full path of Zip file to create</param>
        /// <param name="comment">General comment for Zip file</param>
        /// <returns>A valid ZipStorer object</returns>
        public static ZipStorer Create(string filename, string comment = null)
        {
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);

            ZipStorer zip = Create(stream, comment);
            zip.Comment = comment ?? string.Empty;
            zip.FileName = filename;

            return zip;
        }

        /// <summary>
        /// Method to create a new zip storage in a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="comment"></param>
        /// <param name="leaveOpen">true to leave the stream open after the ZipStorer object is disposed; otherwise, false (default).</param>
        /// <returns>A valid ZipStorer object</returns>
        public static ZipStorer Create(Stream stream, string comment = null, bool leaveOpen = false)
        {
            ZipStorer zip = new ZipStorer()
            {
                Comment = comment ?? string.Empty,
                ZipFileStream = stream,
                Access = FileAccess.Write,
                LeaveOpen = leaveOpen,
                CentralDirImage = Array.Empty<byte>()
            };

            return zip;
        }

        /// <summary>
        /// Method to open an existing storage file
        /// </summary>
        /// <param name="filename">Full path of Zip file to open</param>
        /// <param name="access">File access mode as used in FileStream constructor</param>
        /// <returns>A valid ZipStorer object</returns>
        public static ZipStorer Open(string filename, FileAccess access)
        {
            Stream stream = null;
            ZipStorer zip = null;

            try
            {
                stream = new FileStream(filename, FileMode.Open, access == FileAccess.Read ? FileAccess.Read : FileAccess.ReadWrite);

                zip = Open(stream, access);
                zip.FileName = filename;
            }
            catch (Exception)
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }

                if (zip != null)
                {
                    zip.Dispose();
                    zip = null;
                }

                throw;
            }

            return zip;
        }

        /// <summary>
        /// Method to open an existing storage from stream
        /// </summary>
        /// <param name="stream">Already opened stream with zip contents</param>
        /// <param name="access">File access mode for stream operations</param>
        /// <param name="leaveOpen">true to leave the stream open after the ZipStorer object is disposed; otherwise, false (default).</param>
        /// <returns>A valid ZipStorer object</returns>
        public static ZipStorer Open(Stream stream, FileAccess access, bool leaveOpen = false)
        {
            if (!stream.CanSeek && access != FileAccess.Read)
                throw new InvalidOperationException("Stream cannot seek");

            ZipStorer zip = new ZipStorer()
            {
                ZipFileStream = stream,
                Access = access,
                LeaveOpen = leaveOpen
            };

            if (zip.readFileInfo())
            {
                zip.ReadCentralDir(true);
                return zip;
            }

            if (!leaveOpen)
                zip.Close();

            throw new System.IO.InvalidDataException();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Add full contents of a file into the Zip storage
        /// </summary>
        /// <param name="method">Compression method</param>
        /// <param name="pathname">Full path of file to add to Zip storage</param>
        /// <param name="filenameInZip">Filename and path as desired in Zip directory</param>
        /// <param name="comment">Comment for stored file</param>        
        public ZipFileEntry AddFile(Compression method, string pathname, string filenameInZip, string comment = null)
        {
            if (Access == FileAccess.Read)
                throw new InvalidOperationException("Writing is not allowed");

            using (var stream = new FileStream(pathname, FileMode.Open, FileAccess.Read))
            {
                return this.AddStream(method, filenameInZip, stream, File.GetLastWriteTime(pathname), comment);
            }
        }

        /// <summary>
        /// Add full contents of a stream into the Zip storage
        /// </summary>
        /// <remarks>Same parameters and return value as AddStreamAsync()</remarks>
        public ZipFileEntry AddStream(Compression method, string filenameInZip, Stream source, DateTime modTime, string comment = null)
        {
            // return this.AddStreamAsync(method, filenameInZip, source, modTime, comment);
            return Task.Run(() => this.AddStreamAsync(method, filenameInZip, source, modTime, comment)).Result;
        }

        /// <summary>
        /// Add full contents of a stream into the Zip storage
        /// </summary>
        /// <param name="method">Compression method</param>
        /// <param name="filenameInZip">Filename and path as desired in Zip directory</param>
        /// <param name="source">Stream object containing the data to store in Zip</param>
        /// <param name="modTime">Modification time of the data to store</param>
        /// <param name="comment">Comment for stored file</param>
        public async Task<ZipFileEntry> AddStreamAsync(Compression method, string filenameInZip, Stream source, DateTime modTime, string comment = null)
        {
            if (Access == FileAccess.Read)
                throw new InvalidOperationException("Writing is not allowed");

            // Prepare the fileinfo
            ZipFileEntry zfe = new ZipFileEntry()
            {
                Method = method,
                EncodeUTF8 = this.EncodeUTF8,
                FilenameInZip = normalizedFilename(filenameInZip),
                FileSize = !source.CanSeek || ForceDeflating ? 0 : source.Length,
                Comment = comment ?? string.Empty,
                Crc32 = 0,  // to be updated later
                HeaderOffset = this.ZipFileStream.Position,  // offset within file of the start of this local record
                CreationTime = modTime,
                ModifyTime = modTime,
                AccessTime = modTime
            };

            // Write local header
            this.writeLocalHeader(zfe);
            zfe.FileOffset = this.ZipFileStream.Position;

            // Write file to zip (store)
            await storeAsync(zfe, source);

            source.Close();
            this.updateCrcAndSizes(zfe);
            Files.Add(zfe);

            return zfe;
        }

        /// <summary>
        /// Add full contents of a directory into the Zip storage
        /// </summary>
        /// <param name="method">Compression method</param>
        /// <param name="pathname">Full path of directory to add to Zip storage</param>
        /// <param name="pathnameInZip">Path name as desired in Zip directory</param>
        /// <param name="comment">Comment for stored directory</param>
        public void AddDirectory(Compression method, string pathname, string pathnameInZip, string comment = null)
        {
            if (Access == FileAccess.Read)
                throw new InvalidOperationException("Writing is not allowed");

            string foldername;
            int pos = pathname.LastIndexOf(Path.DirectorySeparatorChar);
            string separator = Path.DirectorySeparatorChar.ToString();

            if (pos >= 0)
                foldername = pathname.Remove(0, pos + 1);
            else
                foldername = pathname;

            if (!string.IsNullOrEmpty(pathnameInZip))
                foldername = pathnameInZip + foldername;

            if (!foldername.EndsWith(separator, StringComparison.CurrentCulture))
                foldername = foldername + separator;

            // this.AddStream(method, foldername, null, File.GetLastWriteTime(pathname), comment);

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(pathname);

            foreach (string fileName in fileEntries)
                this.AddFile(method, fileName, foldername + Path.GetFileName(fileName), "");

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(pathname);

            foreach (string subdirectory in subdirectoryEntries)
                this.AddDirectory(method, subdirectory, foldername, "");
        }

        /// <summary>
        /// Updates central directory (if pertinent) and close the Zip storage
        /// </summary>
        /// <remarks>This is a required step, unless automatic dispose is used</remarks>
        public void Close()
        {
            if (this.Access != FileAccess.Read && Files.Count > 0)
            {
                long centralOffset = this.ZipFileStream.Position;
                long centralSize = 0;

                if (this.CentralDirImage != null)
                    this.ZipFileStream.Write(CentralDirImage, 0, CentralDirImage.Length);

                for (int i = 0; i < Files.Count; i++)
                {
                    long pos = this.ZipFileStream.Position;
                    this.writeCentralDirRecord(Files[i]);
                    centralSize += this.ZipFileStream.Position - pos;
                }

                if (this.CentralDirImage != null)
                    this.writeEndRecord(centralSize + (uint)CentralDirImage.Length, centralOffset);
                else
                    this.writeEndRecord(centralSize, centralOffset);
            }

            if (this.ZipFileStream != null && !this.LeaveOpen)
            {
                this.ZipFileStream.Flush();
                this.ZipFileStream.Dispose();
                this.ZipFileStream = null;
            }
        }

        /// <summary>
        /// Read all the file records in the central directory 
        /// </summary>
        /// <param name="skipFileOffsetCalculation">Delay file offset calculation</param>
        /// <returns>List of all entries in directory</returns>
        /// <remarks>The calculation of the file offsets can be delayed until files are actually extracted. 
        /// This results in better performance, if only a few files are extracted.</remarks>
        public List<ZipFileEntry> ReadCentralDir(bool skipFileOffsetCalculation = false)
        {
            var lastPos = this.ZipFileStream.Position;

            if (this.CentralDirImage == null)
                throw new InvalidOperationException("Central directory currently does not exist");

            CentralDirectoryFiles.Clear();

            for (int pointer = 0; pointer < this.CentralDirImage.Length;)
            {
                uint signature = BitConverter.ToUInt32(CentralDirImage, pointer);

                if (signature != CentralDirHeaderSignature)
                    break;

                bool encodeUTF8 = (BitConverter.ToUInt16(CentralDirImage, pointer + 8) & 0x0800) != 0;
                ushort method = BitConverter.ToUInt16(CentralDirImage, pointer + 10);
                uint modifyTime = BitConverter.ToUInt32(CentralDirImage, pointer + 12);
                uint crc32 = BitConverter.ToUInt32(CentralDirImage, pointer + 16);
                long comprSize = BitConverter.ToUInt32(CentralDirImage, pointer + 20);
                long fileSize = BitConverter.ToUInt32(CentralDirImage, pointer + 24);
                ushort filenameSize = BitConverter.ToUInt16(CentralDirImage, pointer + 28);
                ushort extraSize = BitConverter.ToUInt16(CentralDirImage, pointer + 30);
                ushort commentSize = BitConverter.ToUInt16(CentralDirImage, pointer + 32);
                uint headerOffset = BitConverter.ToUInt32(CentralDirImage, pointer + 42);
                uint headerSize = (uint)(46 + filenameSize + extraSize + commentSize);
                DateTime modifyTimeDT = ZipDate.DosTimeToDateTime(modifyTime) ?? DateTime.Now;

                EncodeUTF8 |= encodeUTF8;
                Encoding encoder = encodeUTF8 ? Encoding.UTF8 : DefaultEncoding;

                ZipFileEntry zfe = new ZipFileEntry()
                {
                    Method = (Compression)method,
                    EncodeUTF8 = encodeUTF8,
                    Comment = string.Empty,
                    FilenameInZip = encoder.GetString(CentralDirImage, pointer + 46, filenameSize),
                    FileOffset = skipFileOffsetCalculation ? 0 : headerOffset == 0xFFFFFFFF ? 0 : this.getFileOffset(headerOffset),
                    FileSize = fileSize,
                    CompressedSize = comprSize,
                    HeaderOffset = headerOffset,
                    HeaderSize = headerSize,
                    Crc32 = crc32,
                    ModifyTime = modifyTimeDT,
                    CreationTime = modifyTimeDT,
                    AccessTime = DateTime.Now,
                };

                if (commentSize > 0)
                    zfe.Comment = encoder.GetString(CentralDirImage, pointer + 46 + filenameSize + extraSize, commentSize);

                if (extraSize > 0)
                {
                    zfe.ReadExtraInfo(CentralDirImage, pointer + 46 + filenameSize, extraSize);
                    
                    if (!skipFileOffsetCalculation && headerOffset == 0xFFFFFFFF) 
                        zfe.FileOffset = this.getFileOffset(zfe.HeaderOffset);
                }

                CentralDirectoryFiles.Add(zfe);
                pointer += 46 + filenameSize + extraSize + commentSize;
            }

            this.ZipFileStream.Position = lastPos;

            return CentralDirectoryFiles;
        }

        /// <summary>
        /// Copy the contents of a stored file into a physical file
        /// </summary>
        /// <param name="zfe">Entry information of file to extract</param>
        /// <param name="filename">Name of file to store uncompressed data</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique compression methods are Store and Deflate</remarks>
        public bool ExtractFile(ZipFileEntry zfe, string filename)
        {
            // Make sure the parent directory exist
            string path = Path.GetDirectoryName(filename);

            if (!string.IsNullOrWhiteSpace(path) && !Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Check if it is a directory. If so, do nothing.
            if (Directory.Exists(filename))
                return true;

            bool result;

            using (var output = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                result = this.ExtractFile(zfe, output);
            }

            if (result)
            {
                File.SetCreationTime(filename, zfe.CreationTime);
                File.SetLastWriteTime(filename, zfe.ModifyTime);
                File.SetLastAccessTime(filename, zfe.AccessTime);
            }

            return result;
        }

        /// <summary>
        /// Copy the contents of a stored file into an opened stream
        /// </summary>
        /// <remarks>Same parameters and return value as ExtractFileAsync</remarks>
        public bool ExtractFile(ZipFileEntry zfe, Stream stream)
        {
            return Task.Run(() => ExtractFileAsync(zfe, stream)).Result;
        }

        /// <summary>
        /// Copy the contents of a stored file into an opened stream
        /// </summary>
        /// <param name="zfe">Entry information of file to extract</param>
        /// <param name="stream">Stream to store the uncompressed data</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique compression methods are Store and Deflate</remarks>
        public async Task<bool> ExtractFileAsync(ZipFileEntry zfe, Stream stream)
        {
            if (!stream.CanWrite)
                throw new InvalidOperationException("Stream cannot be written");

            // check signature
            byte[] signature = new byte[4];
            this.ZipFileStream.Seek(zfe.HeaderOffset, SeekOrigin.Begin);
            await this.ZipFileStream.ReadAsync(signature, 0, 4);

            if (BitConverter.ToUInt32(signature, 0) != LocalFileHeaderSignature)
                return false;

            if (zfe.FileOffset == 0)
                zfe.FileOffset = this.getFileOffset(zfe.HeaderOffset);

            // Buffered copy
            byte[] buffer = new byte[65535];
            this.ZipFileStream.Seek(zfe.FileOffset, SeekOrigin.Begin);
            long bytesPending = zfe.FileSize;

            if (zfe.Method == Compression.Store)
            {
                while (bytesPending > 0)
                {
                    int bytesRead = await this.ZipFileStream.ReadAsync(buffer, 0, (int)Math.Min(bytesPending, buffer.Length));
                    await stream.WriteAsync(buffer, 0, bytesRead);
                    bytesPending -= bytesRead;
                }
            }
            else if (zfe.Method == Compression.Deflate)
            {
                // Use 'using' to ensure DeflateStream is disposed even if an exception occurs
                using (var inStream = new DeflateStream(this.ZipFileStream, CompressionMode.Decompress, true))
                {
                    while (bytesPending > 0)
                    {
                        int bytesRead = await inStream.ReadAsync(buffer, 0, (int)Math.Min(bytesPending, buffer.Length));
                        await stream.WriteAsync(buffer, 0, bytesRead);
                        bytesPending -= bytesRead;
                    }
                }
            }
            else
            {
                return false;
            }

            await stream.FlushAsync();
            return true;
        }

        /// <summary>
        /// Copy the contents of a stored file into a byte array
        /// </summary>
        /// <param name="zfe">Entry information of file to extract</param>
        /// <param name="file">Byte array with uncompressed data</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique compression methods are Store and Deflate</remarks>
        public bool ExtractFile(ZipFileEntry zfe, out byte[] file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (ExtractFile(zfe, ms))
                {
                    file = ms.ToArray();
                    return true;
                }
                else
                {
                    file = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Removes one of many files in storage. It creates a new Zip file.
        /// </summary>
        /// <param name="zip">Reference to the current Zip object</param>
        /// <param name="zfes">List of Entries to remove from storage</param>
        /// <returns>True if success, false if not</returns>
        /// <remarks>This method only works for storage of type FileStream</remarks>
        public static bool RemoveEntries(ref ZipStorer zip, List<ZipFileEntry> zfes)
        {
            if (!(zip.ZipFileStream is FileStream))
                throw new InvalidOperationException("RemoveEntries is allowed just over streams of type FileStream");

            // Just remove affected entries from the newly added files list
            zip.Files.RemoveAll(x => zfes.Contains(x));

            // either here if needed or in Create/Open for (file) streams
            zip.FileName = zip.FileName ?? ((FileStream)zip.ZipFileStream).Name;

            // In order to delete we need to create a copy of the zip file excluding the selected items
            var tempZipName = zip.FileName + ".tmp";

            try
            {
                var tempZip = Create(tempZipName, zip.Comment);
                tempZip.EncodeUTF8 = zip.EncodeUTF8;

                var br = new BinaryReader(zip.ZipFileStream);
                long offset = 0;

                for (int listIndex = 0; listIndex <= 1; listIndex++)
                {
                    var list = listIndex == 0 ? zip.CentralDirectoryFiles : zip.Files;

                    for (int index = 0; index < list.Count; index++)
                    {
                        var zfe = list[index];
                        
                        if (zfe.FileOffset == 0) 
                            zfe.FileOffset = zip.getFileOffset(zfe.HeaderOffset);
                        
                        if (!zfes.Contains(zfe))
                        {
                            // Copy local header and consider filename and extra field lengths
                            zip.ZipFileStream.Position = zfe.HeaderOffset;
                            var bytes = br.ReadBytes(26);
                            tempZip.ZipFileStream.Write(bytes, 0, bytes.Length);
                            var filenameLength = br.ReadInt16();
                            var extraFieldLength = br.ReadInt16();
                            tempZip.ZipFileStream.Write(BitConverter.GetBytes((ushort)filenameLength), 0, 2);
                            tempZip.ZipFileStream.Write(BitConverter.GetBytes((ushort)extraFieldLength), 0, 2);
                            bytes = br.ReadBytes(filenameLength + extraFieldLength);
                            tempZip.ZipFileStream.Write(bytes, 0, bytes.Length);

                            // Buffered copy
                            byte[] buffer = new byte[65535];
                            long bytesPending = zfe.CompressedSize;

                            while (bytesPending > 0)
                            {
                                int bytesRead = zip.ZipFileStream.Read(buffer, 0, (int)Math.Min(bytesPending, buffer.Length));
                                tempZip.ZipFileStream.Write(buffer, 0, bytesRead);

                                bytesPending -= bytesRead;
                            }

                            tempZip.ZipFileStream.Flush();

                            // Adjust offsets
                            zfe.HeaderOffset += offset;
                            zfe.FileOffset += offset;

                            // Add to new files list
                            tempZip.Files.Add(zfe);
                        }
                        else
                        {
                            // offset correction
                            offset -= (zfe.FileOffset - zfe.HeaderOffset) + zfe.CompressedSize;
                        }
                    }
                }

                zip.Close();
                tempZip.Close();

                File.Delete(zip.FileName);
                File.Move(tempZipName, zip.FileName);

                zip = Open(zip.FileName, zip.Access);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (File.Exists(tempZipName))
                    File.Delete(tempZipName);
            }
            return true;
        }

        /// <summary>
        /// Returns a list of all file entries.
        /// </summary>
        /// <returns>List of all entries (central directory and newly added)</returns>
        public List<ZipFileEntry> GetEntries()
        {
            var list = new List<ZipFileEntry>(CentralDirectoryFiles);
            list.AddRange(Files);
            return list;
        }

        /// <summary>
        /// Returns the entry with the specified name.
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <param name="comparisonType">StringComparison enum value</param>
        /// <returns>ZipFileEntry found or null</returns>
        /// <remarks>Searching with IgnoreCase in a Linux zip archive returns only the first entry found.</remarks>
        public ZipFileEntry GetEntry(string name, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase)
        {
            var entry = CentralDirectoryFiles.Where(x => x.FilenameInZip.Equals(name, comparisonType)).FirstOrDefault();
            if (entry is null) entry = Files.Where(x => x.FilenameInZip.Equals(name, comparisonType)).FirstOrDefault();
            return entry;
        }
        #endregion

        #region Private methods
        // Calculate the file offset by reading the corresponding local header
        private long getFileOffset(long headerOffset)
        {
            byte[] buffer = new byte[2];

            this.ZipFileStream.Seek(headerOffset + 26, SeekOrigin.Begin);
            this.ZipFileStream.Read(buffer, 0, 2);
            ushort filenameSize = BitConverter.ToUInt16(buffer, 0);
            this.ZipFileStream.Read(buffer, 0, 2);
            ushort extraSize = BitConverter.ToUInt16(buffer, 0);

            return 30 + filenameSize + extraSize + headerOffset;
        }

        /* Local file header:
            local file header signature     4 bytes  (0x04034b50)
            version needed to extract       2 bytes
            general purpose bit flag        2 bytes
            compression method              2 bytes
            last mod file time              2 bytes
            last mod file date              2 bytes
            crc-32                          4 bytes
            compressed size                 4 bytes
            uncompressed size               4 bytes
            filename length                 2 bytes
            extra field length              2 bytes

            filename (variable size)
            extra field (variable size)
        */
        private void writeLocalHeader(ZipFileEntry zfe)
        {
            long pos = this.ZipFileStream.Position;
            Encoding encoder = zfe.EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
            byte[] encodedFilename = encoder.GetBytes(zfe.FilenameInZip);
            byte[] extraInfo = zfe.CreateExtraInfo(true);

            this.ZipFileStream.Write(BitConverter.GetBytes(LocalFileHeaderSignature), 0, 4);  // header signature
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)(zfe.IsZip64ExtNeeded(3) ? 45 : zfe.Method == Compression.Deflate ? 20 : 10)), 0, 2); // version needed to extract
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)(zfe.EncodeUTF8 ? 0x0800 : 0)), 0, 2); // filename and comment encoding 
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)zfe.Method), 0, 2);  // zipping method
            this.ZipFileStream.Write(BitConverter.GetBytes(ZipDate.DateTimeToDosTime(zfe.ModifyTime)), 0, 4); // zipping date and time
            this.ZipFileStream.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0, 12); // unused CRC, un/compressed size, updated later
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)encodedFilename.Length), 0, 2); // filename length
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)extraInfo.Length), 0, 2); // extra length

            this.ZipFileStream.Write(encodedFilename, 0, encodedFilename.Length);
            this.ZipFileStream.Write(extraInfo, 0, extraInfo.Length);
            zfe.HeaderSize = (uint)(this.ZipFileStream.Position - pos);
        }

        /* Central directory's File header:
            central file header signature   4 bytes  (0x02014b50)
            version made by                 2 bytes
            version needed to extract       2 bytes
            general purpose bit flag        2 bytes
            compression method              2 bytes
            last mod file time              2 bytes
            last mod file date              2 bytes
            crc-32                          4 bytes
            compressed size                 4 bytes
            uncompressed size               4 bytes
            filename length                 2 bytes
            extra field length              2 bytes
            file comment length             2 bytes
            disk number start               2 bytes
            internal file attributes        2 bytes
            external file attributes        4 bytes
            relative offset of local header 4 bytes

            filename (variable size)
            extra field (variable size)
            file comment (variable size)
        */
        private void writeCentralDirRecord(ZipFileEntry zfe)
        {
            Encoding encoder = zfe.EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
            byte[] encodedFilename = encoder.GetBytes(zfe.FilenameInZip);
            byte[] encodedComment = encoder.GetBytes(zfe.Comment);
            byte[] extraInfo = zfe.CreateExtraInfo(false);
            var isZip64 = zfe.IsZip64ExtNeeded(3);

            this.ZipFileStream.Write(BitConverter.GetBytes(CentralDirHeaderSignature), 0, 4);  // header signature
            this.ZipFileStream.Write(new byte[] { (byte)(isZip64 ? 45 : 23), 0x0, (byte)(isZip64 ? 45 : zfe.Method == Compression.Deflate ? 20 : 10), 0 }, 0, 4);  // version made by / needed to extract
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)(zfe.EncodeUTF8 ? 0x0800 : 0)), 0, 2); // filename and comment encoding 
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)zfe.Method), 0, 2);  // zipping method
            this.ZipFileStream.Write(BitConverter.GetBytes(ZipDate.DateTimeToDosTime(zfe.ModifyTime)), 0, 4);  // zipping date and time
            this.ZipFileStream.Write(BitConverter.GetBytes(zfe.Crc32), 0, 4); // file CRC
            this.ZipFileStream.Write(BitConverter.GetBytes(get32bitSize(zfe.CompressedSize)), 0, 4); // compressed file size
            this.ZipFileStream.Write(BitConverter.GetBytes(get32bitSize(zfe.FileSize)), 0, 4); // uncompressed file size
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)encodedFilename.Length), 0, 2); // Filename in zip
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)extraInfo.Length), 0, 2); // extra length
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)encodedComment.Length), 0, 2);

            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)0), 0, 2); // disk=0
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)0), 0, 2); // Internal file attributes
            this.ZipFileStream.Write(BitConverter.GetBytes((uint)0), 0, 4); // External file attributes (normal/readable)
            this.ZipFileStream.Write(BitConverter.GetBytes(get32bitSize(zfe.HeaderOffset)), 0, 4);  // Offset of header

            this.ZipFileStream.Write(encodedFilename, 0, encodedFilename.Length);
            this.ZipFileStream.Write(extraInfo, 0, extraInfo.Length);
            this.ZipFileStream.Write(encodedComment, 0, encodedComment.Length);
        }

        /* 
        Zip64 end of central directory record
            zip64 end of central dir 
            signature                       4 bytes  (0x06064b50)
            size of zip64 end of central
            directory record                8 bytes
            version made by                 2 bytes
            version needed to extract       2 bytes
            number of this disk             4 bytes
            number of the disk with the 
            start of the central directory  4 bytes
            total number of entries in the
            central directory on this disk  8 bytes
            total number of entries in the
            central directory               8 bytes
            size of the central directory   8 bytes
            offset of start of central
            directory with respect to
            the starting disk number        8 bytes
            zip64 extensible data sector    (variable size)        
        
        Zip64 end of central directory locator

            zip64 end of central dir locator 
            signature                       4 bytes  (0x07064b50)
            number of the disk with the
            start of the zip64 end of 
            central directory               4 bytes
            relative offset of the zip64
            end of central directory record 8 bytes
            total number of disks           4 bytes

        End of central dir record:
            end of central dir signature    4 bytes  (0x06054b50)
            number of this disk             2 bytes
            number of the disk with the
            start of the central directory  2 bytes
            total number of entries in
            the central dir on this disk    2 bytes
            total number of entries in
            the central dir                 2 bytes
            size of the central directory   4 bytes
            offset of start of central
            directory with respect to
            the starting disk number        4 bytes
            zipfile comment length          2 bytes
            zipfile comment (variable size)
        */
        private void writeEndRecord(long size, long offset)
        {
            long dirOffset = ZipFileStream.Length;

            // Zip64 end of central directory record
            this.ZipFileStream.Position = dirOffset;
            this.ZipFileStream.Write(new byte[] { 80, 75, 6, 6 }, 0, 4);
            this.ZipFileStream.Write(BitConverter.GetBytes((Int64)44), 0, 8); // size of zip64 end of central directory
            this.ZipFileStream.Write(BitConverter.GetBytes((UInt16)45), 0, 2); // version made by
            this.ZipFileStream.Write(BitConverter.GetBytes((UInt16)45), 0, 2); // version needed to extract 
            this.ZipFileStream.Write(BitConverter.GetBytes((UInt32)0), 0, 4); // current disk
            this.ZipFileStream.Write(BitConverter.GetBytes((UInt32)0), 0, 4); // start of central directory 
            this.ZipFileStream.Write(BitConverter.GetBytes((Int64)Files.Count + ExistingFiles), 0, 8); // total number of entries in the central directory in disk
            this.ZipFileStream.Write(BitConverter.GetBytes((Int64)Files.Count + ExistingFiles), 0, 8); // total number of entries in the central directory
            this.ZipFileStream.Write(BitConverter.GetBytes(size), 0, 8); // size of the central directory
            this.ZipFileStream.Write(BitConverter.GetBytes(offset), 0, 8); // offset of start of central directory with respect to the starting disk number

            // Zip64 end of central directory locator
            this.ZipFileStream.Write(new byte[] { 80, 75, 6, 7 }, 0, 4);
            this.ZipFileStream.Write(BitConverter.GetBytes((UInt32)0), 0, 4); // number of the disk 
            this.ZipFileStream.Write(BitConverter.GetBytes(dirOffset), 0, 8); // relative offset of the zip64 end of central directory record
            this.ZipFileStream.Write(BitConverter.GetBytes((UInt32)1), 0, 4); // total number of disks 

            Encoding encoder = this.EncodeUTF8 ? Encoding.UTF8 : DefaultEncoding;
            byte[] encodedComment = encoder.GetBytes(this.Comment);

            this.ZipFileStream.Write(new byte[] { 80, 75, 5, 6, 0, 0, 0, 0 }, 0, 8);
            this.ZipFileStream.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 0, 12);
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)encodedComment.Length), 0, 2);
            this.ZipFileStream.Write(encodedComment, 0, encodedComment.Length);
        }

        // Copies all the source file into the zip storage
        private async Task<Compression> storeAsync(ZipFileEntry zfe, Stream source)
        {
            if (source.Length == 0)
            {
                zfe.FileSize = 0;
                zfe.CompressedSize = 0;
                zfe.Crc32 = 0;
                zfe.Method = Compression.Store;
                return zfe.Method;
            }

            byte[] buffer = new byte[16384];
            int bytesRead;
            ulong totalRead = 0;
            Stream outStream;

            long posStart = this.ZipFileStream.Position;
            long sourceStart = source.CanSeek ? source.Position : 0;

            if (zfe.Method == Compression.Store)
                outStream = this.ZipFileStream;
            else
                outStream = new DeflateStream(this.ZipFileStream, CompressionMode.Compress, true);

            zfe.Crc32 = 0 ^ 0xffffffff;

            do
            {
                bytesRead = await source.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                    await outStream.WriteAsync(buffer, 0, bytesRead);

                zfe.Crc32 = ZipCrc32.UpdateCRC(zfe.Crc32, buffer, bytesRead);

                totalRead += (uint)bytesRead;
            } while (bytesRead > 0);

            outStream.Flush();

            if (zfe.Method == Compression.Deflate)
                outStream.Dispose();

            zfe.Crc32 ^= 0xFFFFFFFF;
            zfe.FileSize = (long)totalRead;
            zfe.CompressedSize = this.ZipFileStream.Position - posStart;

            // Verify for real compression
            if (zfe.Method == Compression.Deflate && !this.ForceDeflating && source.CanSeek && zfe.CompressedSize > zfe.FileSize)
            {
                // Start operation again with Store algorithm
                zfe.Method = Compression.Store;
                this.ZipFileStream.Position = posStart;
                this.ZipFileStream.SetLength(posStart);
                source.Position = sourceStart;

                return await this.storeAsync(zfe, source);
            }

            return zfe.Method;
        }

        private void updateCrcAndSizes(ZipFileEntry zfe)
        {
            long lastPos = this.ZipFileStream.Position;  // remember position

            this.ZipFileStream.Position = zfe.HeaderOffset + 4;
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)(zfe.IsZip64ExtNeeded(3) ? 45 : zfe.Method == Compression.Deflate ? 20 : 10)), 0, 2);  // version needed

            this.ZipFileStream.Position = zfe.HeaderOffset + 8;
            this.ZipFileStream.Write(BitConverter.GetBytes((ushort)zfe.Method), 0, 2);  // zipping method

            var zip64Sizes = updateLocalHeaderExtraFields(zfe);

            this.ZipFileStream.Position = zfe.HeaderOffset + 14;

            this.ZipFileStream.Write(BitConverter.GetBytes(zfe.Crc32), 0, 4);  // Update CRC
            this.ZipFileStream.Write(BitConverter.GetBytes(zip64Sizes ? 0xFFFFFFFF : zfe.CompressedSize), 0, 4);  // Compressed size
            this.ZipFileStream.Write(BitConverter.GetBytes(zip64Sizes ? 0xFFFFFFFF : zfe.FileSize), 0, 4);  // Uncompressed size

            this.ZipFileStream.Position = lastPos;  // restore position
        }

        private bool updateLocalHeaderExtraFields(ZipFileEntry zfe)
        {
            this.ZipFileStream.Position = zfe.HeaderOffset + 26;

            bool zip64Sizes = false;
            var buffer = new byte[4];
            this.ZipFileStream.Read(buffer, 0, 4);
            var fileNameLength = BitConverter.ToUInt16(buffer, 0);
            var extraFieldLength = BitConverter.ToUInt16(buffer, 2);

            if (extraFieldLength > 0)
            {
                this.ZipFileStream.Seek(fileNameLength, SeekOrigin.Current);
                var extraFieldsBuffer = new byte[extraFieldLength];
                this.ZipFileStream.Read(extraFieldsBuffer, 0, extraFieldLength);

                int pos = 0;
                while (pos < extraFieldsBuffer.Length - 4)
                {
                    uint extraId = BitConverter.ToUInt16(extraFieldsBuffer, pos);
                    uint length = BitConverter.ToUInt16(extraFieldsBuffer, pos + 2);

                    if (extraId == 0x0001) // ZIP64 Information
                    {
                        zip64Sizes = true;
                        this.ZipFileStream.Position = zfe.HeaderOffset + 30 + fileNameLength + 4 + pos;
                        this.ZipFileStream.Write(BitConverter.GetBytes((ulong)zfe.FileSize), 0, 8);
                        this.ZipFileStream.Write(BitConverter.GetBytes((ulong)zfe.CompressedSize), 0, 8);
                    }
                    pos += (int)length + 4;
                }
            }
            return zip64Sizes;
        }

        // Reads the end-of-central-directory record
        private bool readFileInfo()
        {
            if (this.ZipFileStream.Length < 22)
                return false;

            this.ZipFileStream.Seek(0, SeekOrigin.Begin);

            var br = new BinaryReader(this.ZipFileStream);
            UInt32 headerSig = br.ReadUInt32();

            if (headerSig != LocalFileHeaderSignature && headerSig != EndOfCentralDirSignature)
            {
                // not PK.. signature header and not 'end of central directory record' (empty ZIP files)
                return false;
            }

            var end = this.ZipFileStream.Seek(-17, SeekOrigin.End); // Will start seeking from -22

            try
            {
                do
                {
                    this.ZipFileStream.Seek(-5, SeekOrigin.Current);
                    UInt32 sig = br.ReadUInt32();

                    if (sig == EndOfCentralDirSignature) // It is central dir
                    {
                        long dirPosition = ZipFileStream.Position - 4;

                        this.ZipFileStream.Seek(6, SeekOrigin.Current);

                        long entries = br.ReadUInt16();
                        long centralSize = br.ReadInt32();
                        long centralDirOffset = br.ReadUInt32();
                        UInt16 commentSize = br.ReadUInt16();

                        var commentPosition = ZipFileStream.Position;

                        if (centralDirOffset == 0xffffffff) // It is a Zip64 file
                        {
                            this.ZipFileStream.Position = dirPosition - 20;

                            sig = br.ReadUInt32();

                            if (sig != Zip64EndOfCentralDirLocatorSignature) // Not a Zip64 central dir locator
                                return false;

                            this.ZipFileStream.Seek(4, SeekOrigin.Current);

                            long dir64Position = br.ReadInt64();
                            this.ZipFileStream.Position = dir64Position;

                            sig = br.ReadUInt32();

                            if (sig != Zip64EndOfCentralDirSignature) // Not a Zip64 central dir record
                                return false;

                            this.ZipFileStream.Seek(28, SeekOrigin.Current);
                            entries = br.ReadInt64();
                            centralSize = br.ReadInt64();
                            centralDirOffset = br.ReadInt64();
                        }

                        // check if comment field is the very last data in file
                        if (commentPosition + commentSize != this.ZipFileStream.Length)
                            return false;

                        // Copy entire central directory to a memory buffer
                        this.ExistingFiles = entries;
                        this.CentralDirImage = new byte[centralSize];
                        this.ZipFileStream.Seek(centralDirOffset, SeekOrigin.Begin);
                        this.ZipFileStream.Read(this.CentralDirImage, 0, (int)centralSize);

                        // Leave the pointer at the begining of central dir, to append new files
                        this.ZipFileStream.Seek(centralDirOffset, SeekOrigin.Begin);
                        return true;
                    }
                } while (this.ZipFileStream.Position > end - 65535);
            }
            catch { }

            return false;
        }

        // Replaces backslashes with slashes to store in zip header
        private static string normalizedFilename(string filename)
        {
            string filename1 = filename.Replace('\\', '/');

            int pos = filename1.IndexOf(':');
            
            if (pos >= 0)
                filename1 = filename1.Remove(0, pos + 1);

            return filename1.Trim('/');
        }

        // Returns filesize or 0xFFFFFFFF, if greater
        private static uint get32bitSize(long size)
        {
            return size >= 0xFFFFFFFF ? 0xFFFFFFFF : (uint)size;
        }
        #endregion

        #region IDisposable implementation
        /// <summary>
        /// Closes the Zip file stream
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                    this.Close();

                IsDisposed = true;
            }
        }
        #endregion
    }
}