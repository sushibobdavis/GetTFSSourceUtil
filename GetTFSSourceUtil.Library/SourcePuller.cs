using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSClient = Microsoft.TeamFoundation.VersionControl.Client;

namespace GetTFSSourceUtil.Library
{
    public abstract class SourcePuller : ISourcePuller
    {
        private TFSSourceContext context = null;
        private Dictionary<Enums.TFSSourceArgTypes, Action<string>> contextStrategy = null;

        public SourcePuller()
        {
            contextStrategy = new Dictionary<Enums.TFSSourceArgTypes, Action<string>>();
            contextStrategy.Add(Enums.TFSSourceArgTypes.CsId, delegate(string s) { SetChangesetId(s); });
            contextStrategy.Add(Enums.TFSSourceArgTypes.FromDate, delegate(string s) { SetFromDate(s); });
            contextStrategy.Add(Enums.TFSSourceArgTypes.Label, delegate(string s) { SetLabel(s); });
            contextStrategy.Add(Enums.TFSSourceArgTypes.Path, delegate(string s) { SetPath(s); });
            contextStrategy.Add(Enums.TFSSourceArgTypes.ToDate, delegate(string s) { SetToDate(s); });
            contextStrategy.Add(Enums.TFSSourceArgTypes.WorkingDir, delegate(string s) { SetWorkingDirectory(s); });
            contextStrategy.Add(Enums.TFSSourceArgTypes.Type, delegate(string s) { return; });
        }

        public Dictionary<Enums.TFSSourceArgTypes, string> Arguments { get; set; }
        public Dictionary<Enums.TFSSourceArgTypes, string> TFSSourceArguments { get; set; }
        public TFSClient.VersionControlServer VersionControl { get; set; }
        public string DefaultProjectPath { get; set; }
        public string TfsTeamProjectCollectionPath { get; set; }
        public string WorkspaceName { get; set; }

        #region ISourcePuller Members

        public TFSSourceContext Context
        {
            get
            {
                if (context == null)
                {
                    PopulateContext();
                }

                return context;
            }
        }


        public abstract List<TFSClient.Changeset> GetChangesets();
        #endregion

        private void PopulateContext()
        {
            context = new TFSSourceContext();

            context.SourceControlPath = this.DefaultProjectPath;

            foreach (KeyValuePair<Enums.TFSSourceArgTypes, string> kvp in TFSSourceArguments)
            {
                contextStrategy[kvp.Key].Invoke(kvp.Value);
            }
        }

        private void SetFromDate(string value)
        {
            context.FromDate = DateTime.Parse(value);
        }

        private void SetToDate(string value)
        {
            context.ToDate = DateTime.Parse(value);
        }

        private void SetChangesetId(string value)
        {
            context.ChangesetId = int.Parse(value);
        }

        private void SetLabel(string value)
        {
            context.Label = value;
        }

        private void SetPath(string value)
        {
            context.SourceControlPath = value;

            if (!context.SourceControlPath.StartsWith("$"))
                context.SourceControlPath = string.Format("{0}/{1}", DefaultProjectPath, value); 
        }

        private void SetWorkingDirectory(string value)
        {
            context.WorkingDirectory = value;
        }
    }
}