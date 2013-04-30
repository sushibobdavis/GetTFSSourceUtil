using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SourceUtil = GetTFSSourceUtil.Library;

namespace GetTFSSourceUtil.Library
{
    public static class Settings
    {
        private static Dictionary<SourceUtil.Enums.SourceType, ISourcePuller> sourcePullerStrategies = null;
        public static Dictionary<SourceUtil.Enums.SourceType, ISourcePuller> SourcePullerStrategies
        {
            get
            {
                if (sourcePullerStrategies == null)
                {
                    sourcePullerStrategies = new Dictionary<SourceUtil.Enums.SourceType, ISourcePuller>();

                    sourcePullerStrategies.Add(SourceUtil.Enums.SourceType.Changeset, new ChangesetSourcePuller());
                    sourcePullerStrategies.Add(SourceUtil.Enums.SourceType.DateRange, new DateRangeSourcePuller());
                    sourcePullerStrategies.Add(SourceUtil.Enums.SourceType.SinceLabel, new LabelSinceSourcePuller());
                    sourcePullerStrategies.Add(SourceUtil.Enums.SourceType.Label, new LabelSourcePuller());
                }

                return sourcePullerStrategies;
            }
        }
    }
}

