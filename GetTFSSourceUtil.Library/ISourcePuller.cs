using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSClient = Microsoft.TeamFoundation.VersionControl.Client;

namespace GetTFSSourceUtil.Library
{
    public interface ISourcePuller
    {
        TFSClient.VersionControlServer VersionControl { get; set; }
        Dictionary<Enums.TFSSourceArgTypes, string> TFSSourceArguments { get; set; }
        TFSSourceContext Context { get; }
        string DefaultProjectPath { get; set; }
        string TfsTeamProjectCollectionPath { get; set; }
        string WorkspaceName { get; set; }

        List<TFSClient.Changeset> GetChangesets();
    }


}
