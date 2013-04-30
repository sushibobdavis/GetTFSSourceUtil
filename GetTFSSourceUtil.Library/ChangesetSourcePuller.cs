using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSClient = Microsoft.TeamFoundation.VersionControl.Client;

namespace GetTFSSourceUtil.Library
{
    public class ChangesetSourcePuller : SourcePuller, ISourcePuller
    {
        #region ISourcePuller Members

        public override List<TFSClient.Changeset> GetChangesets()
        {
            List<TFSClient.Changeset> changesets = new List<TFSClient.Changeset>();

            TFSClient.Changeset changeset = VersionControl.GetChangeset(Context.ChangesetId);

            if (changeset != null)
                changesets.Add(changeset);

            return changesets;
        }

        #endregion

    }
}