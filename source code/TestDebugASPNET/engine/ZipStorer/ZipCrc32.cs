namespace System.IO.Compression
{
    /* CRC32 algorithm
        The 'magic number' for the CRC is 0xdebb20e3.  
        The proper CRC pre and post conditioning is used, meaning that the CRC register is
        pre-conditioned with all ones (a starting value of 0xffffffff) and the value is post-conditioned by
        taking the one's complement of the CRC residual.
        If bit 3 of the general purpose flag is set, this field is set to zero in the local header and the correct
        value is put in the data descriptor and in the central directory.
    */
    public static class ZipCrc32
    {
        private static UInt32[] CrcTable = null;

        static ZipCrc32()
        {
            // Generate CRC32 table
            CrcTable = new UInt32[256];
            for (int i = 0; i < CrcTable.Length; i++)
            {
                UInt32 c = (UInt32)i;
                for (int j = 0; j < 8; j++)
                {
                    if ((c & 1) != 0)
                        c = 3988292384 ^ (c >> 1);
                    else
                        c >>= 1;
                }
                CrcTable[i] = c;
            }
        }

        public static UInt32 UpdateCRC(UInt32 init, byte[] buffer, int count)
        {
            UInt32 crc = init;

            for (uint i = 0; i < count; i++)
            {
                crc = CrcTable[(crc ^ buffer[i]) & 0xFF] ^ (crc >> 8);
            }

            return crc;
        }
    }
}
