// ZipStorer, by Jaime Olivares
// Website: http://github.com/jaime-olivares/zipstorer

namespace System.IO.Compression
{
    /// <summary>
    /// Represents an entry in Zip file directory
    /// </summary>
    public class ZipFileEntry
    {
        /// <summary>Compression method</summary>
        public ZipStorer.Compression Method { get; set; }
        /// <summary>Full path and filename as stored in Zip</summary>
        public string FilenameInZip { get; set; }
        /// <summary>Original file size</summary>
        public long FileSize { get; set; }
        /// <summary>Compressed file size</summary>
        public long CompressedSize { get; set; }
        /// <summary>Offset of header information inside Zip storage</summary>
        public long HeaderOffset { get; set; }
        /// <summary>Offset of file inside Zip storage</summary>
        public long FileOffset { get; set; }
        /// <summary>Size of header information</summary>
        public uint HeaderSize { get; set; }
        /// <summary>32-bit checksum of entire file</summary>
        public uint Crc32 { get; set; }
        /// <summary>Last modification time of file</summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>Creation time of file</summary>
        public DateTime CreationTime { get; set; }
        /// <summary>Last access time of file</summary>
        public DateTime AccessTime { get; set; }
        /// <summary>User comment for file</summary>
        public string Comment { get; set; }
        /// <summary>True if UTF8 encoding for filename and comments, false if default (CP 437)</summary>
        public bool EncodeUTF8 { get; set; }

        /// <summary>Overriden method</summary>
        /// <returns>Filename in Zip</returns>
        public override string ToString()
        {
            return this.FilenameInZip;
        }

        public override bool Equals(object obj)
        {
            ZipFileEntry o = obj as ZipFileEntry;
            
            if (o is null) 
                return false;
            
            return this.HeaderOffset == o.HeaderOffset;
        }

        public override int GetHashCode()
        {
            return this.FilenameInZip.GetHashCode() + this.FileSize.GetHashCode();
        }

        public bool IsZip64ExtNeeded(byte mask)
        {
            bool zip64 = false;
            
            if ((mask & 1) != 0) 
                zip64 = this.FileSize >= 0xFFFFFFFF || this.CompressedSize >= 0xFFFFFFFF;
            
            if (!zip64 && (mask & 2) != 0) 
                zip64 = this.HeaderOffset >= 0xFFFFFFFF;
            
            return zip64;
        }

        public byte[] CreateExtraInfo(bool localHeader)
        {
            var zip64FileSize = this.FileSize >= 0xFFFFFFFF || localHeader && this.FileSize == 0;
            var zip64CompSize = this.CompressedSize >= 0xFFFFFFFF || localHeader && (this.FileSize == 0 || this.FileSize >= 0xFFFFFFFF);
            var zip64Offset = !localHeader && this.HeaderOffset >= 0xFFFFFFFF;
            int offset = (zip64FileSize ? 8 : 0) + (zip64CompSize ? 8 : 0) + (zip64Offset ? 8 : 0);
            
            if (offset != 0) 
                offset += 4;

            byte[] buffer = new byte[offset + 36];

            if (offset > 0)
            {
                BitConverter.GetBytes((ushort)0x0001).CopyTo(buffer, 0); // ZIP64 Information
                BitConverter.GetBytes((ushort)(offset - 4)).CopyTo(buffer, 2); // Length
                BitConverter.GetBytes(zip64FileSize ? this.FileSize : zip64CompSize ? this.CompressedSize : this.HeaderOffset).CopyTo(buffer, 4);
                
                if (zip64CompSize || zip64Offset) 
                    BitConverter.GetBytes(zip64CompSize ? this.CompressedSize : this.HeaderOffset).CopyTo(buffer, 12);
                
                if (zip64Offset) 
                    BitConverter.GetBytes(this.HeaderOffset).CopyTo(buffer, 20);
            }

            BitConverter.GetBytes((ushort)0x000A).CopyTo(buffer, offset); // NTFS FileTime
            BitConverter.GetBytes((ushort)32).CopyTo(buffer, offset + 2); // Length
            BitConverter.GetBytes((uint)0).CopyTo(buffer, offset + 4); // Reserved
            BitConverter.GetBytes((ushort)0x0001).CopyTo(buffer, offset + 8); // Tag 1
            BitConverter.GetBytes((ushort)24).CopyTo(buffer, offset + 10); // Size 1
            BitConverter.GetBytes(this.ModifyTime.ToFileTime()).CopyTo(buffer, offset + 12); // MTime
            BitConverter.GetBytes(this.AccessTime.ToFileTime()).CopyTo(buffer, offset + 20); // ATime
            BitConverter.GetBytes(this.CreationTime.ToFileTime()).CopyTo(buffer, offset + 28); // CTime

            return buffer;
        }

        public void ReadExtraInfo(byte[] buffer, int offset, int extraSize)
        {
            if (buffer.Length < 4)
                return;

            int start = offset;
            int pos = offset;
            uint tag, size;

            while (pos < buffer.Length - 4 && pos - start < extraSize)
            {
                uint extraId = BitConverter.ToUInt16(buffer, pos);
                uint length = BitConverter.ToUInt16(buffer, pos + 2);

                if (extraId == 0x0001) // ZIP64 Information
                {
                    int fieldOffset = 0;
                    while (fieldOffset < Math.Min(length, 24))
                    {
                        var data = BitConverter.ToInt64(buffer, pos + 4 + fieldOffset);
                        if (this.FileSize == 0xFFFFFFFF)
                            this.FileSize = data;
                        else if (this.CompressedSize == 0xFFFFFFFF)
                            this.CompressedSize = data;
                        else if (this.HeaderOffset == 0xFFFFFFFF)
                            this.HeaderOffset = data;
                        fieldOffset += 8;
                    }
                }

                if (extraId == 0x000A) // NTFS FileTime
                {
                    tag = BitConverter.ToUInt16(buffer, pos + 8);
                    size = BitConverter.ToUInt16(buffer, pos + 10);

                    if (tag == 1 && size == 24)
                    {
                        this.ModifyTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 12));
                        this.AccessTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 20));
                        this.CreationTime = DateTime.FromFileTime(BitConverter.ToInt64(buffer, pos + 28));
                    }
                }

                pos += (int)length + 4;
            }
        }
    }
}
