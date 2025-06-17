using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public class ProgressReportInfo
    {
        public int id { get; set; }
        public int operation { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public bool is_completed { get; set; }
        public bool has_error { get; set; }
        public bool is_cancelled { get; set; }
        public string filename { get; set; }
        public long total_tables { get; set; }
        public long total_rows { get; set; }
        public long total_rows_current_table { get; set; }
        public string current_table { get; set; }
        public long current_table_index { get; set; }
        public long current_row { get; set; }
        public long current_row_in_current_table { get; set; }
        public long total_bytes { get; set; }
        public long current_bytes { get; set; }
        public long percent_complete { get; set; }
        public string remarks { get; set; }
        public int dbfile_id { get; set; }
        public DateTime last_update_time { get; set; }
        public bool client_request_cancel_task { get; set; }
    }
}