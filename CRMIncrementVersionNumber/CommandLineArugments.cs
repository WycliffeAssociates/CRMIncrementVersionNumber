using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace CRMIncrementVersionNumber
{
    public class CommandLineArugments
    {
        [Option('c', "connection", HelpText = "CRM Connection String", Required = true)]
        public string ConnectionString { get; set; }
        [Option('s', "solution", HelpText = "Unqiue name of solution", Required = true)]
        public string Solution { get; set; }
        [Option('i', "increment", HelpText = "Version number to increment by", Required = true)]
        public string VersionNumber { get; set; }

        [HelpOption]
        public string GetHelp()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
