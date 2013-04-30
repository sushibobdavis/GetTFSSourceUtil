using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSClient = Microsoft.TeamFoundation.VersionControl.Client;

namespace GetTFSSourceUtil.Library
{
    public class LabelSinceSourcePuller : SourcePuller, ISourcePuller
    {
        private LabelSourcePuller labelSourcePuller = null;
        private DateRangeSourcePuller dateRangeSourcePuller = null;

        public LabelSinceSourcePuller()
        {
        }

        private DateRangeSourcePuller DateRangeSourcePuller
        {
            get
            {
                if (dateRangeSourcePuller == null)
                {
                    dateRangeSourcePuller = new DateRangeSourcePuller();
                    dateRangeSourcePuller.VersionControl = this.VersionControl;
                    dateRangeSourcePuller.TFSSourceArguments = this.TFSSourceArguments;
                }

                return dateRangeSourcePuller;
            }
        }

        private LabelSourcePuller LabelSourcePuller
        {
            get
            {
                if (labelSourcePuller == null)
                {
                    labelSourcePuller = new LabelSourcePuller();
                    labelSourcePuller.VersionControl = this.VersionControl;
                    labelSourcePuller.TFSSourceArguments = this.TFSSourceArguments;
                }

                return labelSourcePuller;
            }
        }


        public override List<TFSClient.Changeset> GetChangesets()
        {
            List<TFSClient.Changeset> changesets = new List<TFSClient.Changeset>();
            List<TFSClient.Changeset> labelChangesets = LabelSourcePuller.GetChangesets();

            var creationDates = (from TFSClient.Changeset cs in labelChangesets
                                 orderby cs.CreationDate descending
                                 select cs.CreationDate);

            DateTime fromDate = creationDates.First();

            changesets = DateRangeSourcePuller.GetChangesets(fromDate, DateTime.Now);

            return changesets;
        }
    }
}
