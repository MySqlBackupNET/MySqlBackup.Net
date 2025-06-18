using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace System
{
    public class DatabaseFileRecord
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public string Filename { get; set; }
        public string OriginalFilename { get; set; }
        public string LogFilename { get; set; }
        public string Sha256 { get; set; }
        public long Filesize { get; set; }
        public string DatabaseName { get; set; }
        public DateTime DateCreated { get; set; }
        public int TaskId { get; set; }
        public string Remarks { get; set; }
    }
}