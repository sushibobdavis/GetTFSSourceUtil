using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GetTFSSourceUtil.Library
{
    public class SourcePullerManager
    {
        Workspace workspace = null;
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISourcePuller sourcePuller = null;

        public SourcePullerManager(ISourcePuller sourcePuller)
        {
            this.sourcePuller = sourcePuller;
        }

        public void PullSources()
        {
            string workingDirectory = sourcePuller.Context.WorkingDirectory;

            if (!Directory.Exists(workingDirectory))
                Directory.CreateDirectory(workingDirectory);

            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(sourcePuller.TfsTeamProjectCollectionPath));

            VersionControlServer versionControl = tfs.GetService<VersionControlServer>();
            sourcePuller.VersionControl = versionControl;

            CleanupFiles(workingDirectory);

            SetupWorkspace(workingDirectory, sourcePuller.Context.SourceControlPath, versionControl);

            List<Changeset> changesets = sourcePuller.GetChangesets();
            GetSourcesForChangesets(changesets);

            Console.WriteLine("Process Complete...");
            Console.In.ReadLine();
        }



        private void GetSourcesForChangesets(List<Changeset> changesets)
        {
            changesets = changesets.OrderBy(c => c.ChangesetId).Cast<Changeset>().ToList();
            List<string> filesToBeDeleted = new List<string>();

            foreach (Changeset changeset in changesets)
            {
                log.Debug(string.Format("Date: {0} ChangesetId: {1} containing {2} changes", changeset.CreationDate, changeset.ChangesetId, changeset.Changes.Length));

                foreach (Change change in changeset.Changes)
                {
                    string filename = workspace.TryGetLocalItemForServerItem(change.Item.ServerItem);

                    if (!string.IsNullOrEmpty(filename))
                    {
                        log.Debug(string.Format("           Type: {0}, ServerItem: {1}, Destroyed: {2}", change.ChangeType, filename, change.Item.IsContentDestroyed));

                        if ((change.ChangeType.HasFlag(ChangeType.Delete)))
                            filesToBeDeleted.Add(filename);

                        if ((change.ChangeType.HasFlag(ChangeType.Add)) && (filesToBeDeleted.Exists(file => file == filename)))
                            filesToBeDeleted.Remove(filename);

                        if (change.Item.ItemType == ItemType.File)
                            change.Item.DownloadFile(filename);
                    }
                }
            }

            foreach (string file in filesToBeDeleted)
            {
                log.DebugFormat("Deleting: {0}", file);

                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }
        }

        private void SetupWorkspace(string workingDirectory, string cpSourceBranch, VersionControlServer versionControl)
        {
            List<WorkingFolder> workingFolders = new List<WorkingFolder>();
            workingFolders.Add(new WorkingFolder(cpSourceBranch, workingDirectory));

            // Create a workspace.
            Workspace[] workspaces = versionControl.QueryWorkspaces(sourcePuller.WorkspaceName, versionControl.AuthorizedUser, Environment.MachineName);

            if (workspaces.Length > 0)
                versionControl.DeleteWorkspace(sourcePuller.WorkspaceName, versionControl.AuthorizedUser);

            workspace = versionControl.CreateWorkspace(sourcePuller.WorkspaceName, versionControl.AuthorizedUser, "Work for GetTFSSourceUtil tool", workingFolders.ToArray(), Environment.MachineName);
        }

        public void CleanupFiles(string directoryPath)
        {
            //if (Directory.Exists(workingDirectory + @"\" + cpSourceBranch))
            if (Directory.Exists(directoryPath))
            {
                string[] cpDirectoryEntries = Directory.GetDirectories(directoryPath);

                foreach (string cpDirectory in cpDirectoryEntries)
                {
                    CleanupFiles(cpDirectory);
                }

                //string[] cpFileEntries = Directory.GetFiles(workingDirectory + @"\" + cpSourceBranch);
                string[] cpFileEntries = Directory.GetFiles(directoryPath);

                foreach (string cpFile in cpFileEntries)
                {
                    File.SetAttributes(cpFile, FileAttributes.Normal);
                    File.Delete(cpFile);
                }

                Directory.Delete(directoryPath);
            }
        }
    }
}
