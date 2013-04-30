using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSClient = Microsoft.TeamFoundation.VersionControl.Client;

namespace GetTFSSourceUtil.Library
{
    public class LabelSourcePuller : SourcePuller, ISourcePuller
    {        

        #region ISourcePuller Members

        public override List<TFSClient.Changeset> GetChangesets()
        {
            List<int> changesetIds = new List<int>();
            List<TFSClient.Changeset> changesets = new List<TFSClient.Changeset>();

            TFSClient.LabelVersionSpec vLabel = new TFSClient.LabelVersionSpec(Context.Label);

            TFSClient.VersionControlLabel[] versionControlLabels = VersionControl.QueryLabels(Context.Label, Context.SourceControlPath, string.Empty, true, string.Empty, vLabel);

            foreach (TFSClient.VersionControlLabel label in versionControlLabels)
            {
                foreach (TFSClient.Item item in label.Items)
                {
                    if (!changesetIds.Contains(item.ChangesetId))
                        changesetIds.Add(item.ChangesetId);
                }
            }

            foreach (int counter in changesetIds)
            {
                changesets.Add(VersionControl.GetChangeset(counter));
            }

            return changesets;
        }

        #endregion
    }

}