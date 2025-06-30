namespace System.IO.Compression
{
    public static class ZipDate
    {
        /* DOS Date and time:
            MS-DOS date. The date is a packed value with the following format. Bits Description 
                0-4 Day of the month (131) 
                5-8 Month (1 = January, 2 = February, and so on) 
                9-15 Year offset from 1980 (add 1980 to get actual year) 
            MS-DOS time. The time is a packed value with the following format. Bits Description 
                0-4 Second divided by 2 
                5-10 Minute (059) 
                11-15 Hour (023 on a 24-hour clock) 
        */
        public static uint DateTimeToDosTime(DateTime dt)
        {
            return (uint)(
                (dt.Second / 2) | (dt.Minute << 5) | (dt.Hour << 11) |
                (dt.Day << 16) | (dt.Month << 21) | ((dt.Year - 1980) << 25));
        }

        public static DateTime? DosTimeToDateTime(uint dt)
        {
            int year = (int)(dt >> 25) + 1980;
            int month = (int)(dt >> 21) & 15;
            int day = (int)(dt >> 16) & 31;
            int hours = (int)(dt >> 11) & 31;
            int minutes = (int)(dt >> 5) & 63;
            int seconds = (int)(dt & 31) * 2;

            if (month == 0 || day == 0 || year >= 2107)
                return DateTime.Now;

            return new DateTime(year, month, day, hours, minutes, seconds);
        }
    }
}
