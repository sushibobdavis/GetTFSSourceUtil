using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSClient = Microsoft.TeamFoundation.VersionControl.Client;


namespace GetTFSSourceUtil.Library
{
    public class DateRangeSourcePuller : SourcePuller, ISourcePuller
    {
        #region ISourcePuller Members

        public override List<TFSClient.Changeset> GetChangesets()
        {
            return GetChangesets(Context.FromDate, Context.ToDate);
        }

        #endregion

        public List<TFSClient.Changeset> GetChangesets(DateTime fromDate, DateTime toDate)
        {
            TFSClient.VersionSpec vFrom = TFSClient.VersionSpec.ParseSingleSpec(string.Format("D{0}", fromDate), null);
            TFSClient.VersionSpec vTo = TFSClient.VersionSpec.ParseSingleSpec(string.Format("D{0}", toDate), null);

            List<TFSClient.Changeset> changesets = VersionControl.QueryHistory(Context.WorkingDirectory, TFSClient.VersionSpec.Latest, 0, TFSClient.RecursionType.Full, null, vFrom, vTo, int.MaxValue, true, false).Cast<TFSClient.Changeset>().ToList();

            return changesets;
        }
    }
}