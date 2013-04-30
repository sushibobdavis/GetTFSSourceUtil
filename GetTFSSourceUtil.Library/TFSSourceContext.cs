using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetTFSSourceUtil.Library
{
    public class TFSSourceContext
    {
        public Enums.SourceType Type { get; set; }
        public string SourceControlPath { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Label { get; set; }
        public string WorkingDirectory { get; set; }
        public int ChangesetId { get; set; }
    }   
}
