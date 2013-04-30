using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetTFSSourceUtil.Library
{
    public class Enums
    {
        public enum TFSSourceArgTypes
        {
            Type,
            FromDate,
            ToDate,
            CsId,
            Label,
            Path,
            WorkingDir
        }

        public enum SourceType
        {
            DateRange = 1,
            Label = 2,
            Changeset = 3,
            SinceLabel = 4
        }
    }
}
