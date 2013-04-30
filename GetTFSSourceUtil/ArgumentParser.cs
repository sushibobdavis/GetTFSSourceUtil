using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSSourceLib = GetTFSSourceUtil.Library;
namespace GetTFSSourceUtil
{
    public class ArgumentParser
    {
        private string[] arguments = null;
        private Dictionary<TFSSourceLib.Enums.TFSSourceArgTypes, string> sourceArgs;
        private TFSSourceLib.Enums.SourceType argumentSourceType;

        public ArgumentParser(string[] args)
        {
            arguments = args;
            Initialize();
        }

        public void Initialize()
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("/{1}(?<myparmtype>[a-zA-Z]+):{1}(?<myparmvalue>[a-zA-Z0-9._:\\W\\s\\\\/]+)");
            sourceArgs = new Dictionary<TFSSourceLib.Enums.TFSSourceArgTypes, string>();

            foreach (string argument in arguments)
            {
                System.Text.RegularExpressions.Match match = regex.Match(argument);

                if (match != null)
                {
                    string parmtype = match.Groups["myparmtype"].Value;
                    string parmvalue = match.Groups["myparmvalue"].Value;

                    TFSSourceLib.Enums.TFSSourceArgTypes argType = (TFSSourceLib.Enums.TFSSourceArgTypes)Enum.Parse(typeof(TFSSourceLib.Enums.TFSSourceArgTypes), parmtype, true);

                    sourceArgs.Add(argType, parmvalue);

                    Console.WriteLine(string.Format("parmtype = {0}", parmtype));
                    Console.WriteLine(string.Format("parmvalue = {0}", parmvalue));
                }
            }

            argumentSourceType = (TFSSourceLib.Enums.SourceType)Enum.Parse(typeof(TFSSourceLib.Enums.SourceType), sourceArgs[TFSSourceLib.Enums.TFSSourceArgTypes.Type], true);
        }

        public Dictionary<TFSSourceLib.Enums.TFSSourceArgTypes, string> SourceArguments
        {
            get
            {
                return sourceArgs;
            }
        }

        public TFSSourceLib.Enums.SourceType ArgumentSourceType
        {
            get
            {
                return argumentSourceType;
            }
        }

    }
}
