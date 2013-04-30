using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using SourceUtilLib = GetTFSSourceUtil.Library;


namespace GetTFSSourceUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args.Length == 1) && (args[0] == "/?"))
            {
                DisplayHelp();
                return;
            }

            ArgumentParser argParser = new ArgumentParser(args);

            SourceUtilLib.ISourcePuller sourcePuller = SourceUtilLib.Settings.SourcePullerStrategies[argParser.ArgumentSourceType];
            sourcePuller.TFSSourceArguments = argParser.SourceArguments;
            sourcePuller.WorkspaceName = System.Configuration.ConfigurationManager.AppSettings.Get("WorkspaceName");
            sourcePuller.DefaultProjectPath = System.Configuration.ConfigurationManager.AppSettings.Get("DefaultProjectPath");
            sourcePuller.TfsTeamProjectCollectionPath = System.Configuration.ConfigurationManager.AppSettings.Get("TfsTeamProjectCollection.Path");

            SourceUtilLib.SourcePullerManager mgr = new SourceUtilLib.SourcePullerManager(sourcePuller);
            mgr.PullSources();
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("GetTFSSourceUtil.exe [arguments]");
            Console.WriteLine("");
            Console.WriteLine("Arguments:");
            Console.WriteLine("/type:\t\tRequired.  Possible values: daterange, changeset, label, sincelabel (i.e. /type:label)");
            Console.WriteLine("/fromdate:\tRequired for DateRange pulling.  Format mm/dd/yyyy hh:mm (i.e. /fromdate:\"01/01/2011 00:00\")");
            Console.WriteLine("/todate:\tRequired for DateRange pulling.  Format mm/dd/yyyy hh:mm (i.e. /todate:\"12/31/2011 23:59\")");
            Console.WriteLine("/csid:\t\tRequired for Changeset pulling.  Id of the Changeset (i.e. /csid:500)");
            Console.WriteLine("/label:\t\tRequired for Label pulling.  Name of the label (i.e. /label:\"QA Label\"");
            Console.WriteLine("/path:\t\tOptional.  Source Control Path omitting the TFS Project Name (i.e. /path:\"Branches/Database/1.0.140.5\")");
            Console.WriteLine("/workingdir:\tRequired.  Path to the local working directory (i.e. /workingdir:\"C:\\temp\")");
        }        
    }
}

