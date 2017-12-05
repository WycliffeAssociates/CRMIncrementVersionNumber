using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMIncrementVersionNumber
{
    public class Program
    {
        static void Main(string[] args)
        {
            CommandLineArugments arguments = new CommandLineArugments();
            if (!CommandLine.Parser.Default.ParseArguments(args, arguments))
            {
                Environment.Exit(1);
            }
            CrmServiceClient service = new CrmServiceClient(arguments.ConnectionString);
            if (!string.IsNullOrEmpty(service.LastCrmError))
            {
                Console.WriteLine("Error connecting to CRM " + service.LastCrmError);
                Environment.Exit(2);
            }
            QueryExpression query = new QueryExpression("solution");
            query.Criteria.AddCondition("uniquename", ConditionOperator.Equal, arguments.Solution);
            query.ColumnSet = new ColumnSet("version");

            var result = service.RetrieveMultiple(query);

            if (result.Entities.Count == 0)
            {
                Console.WriteLine("Can't find a solution with the unique name " + arguments.Solution);
                Environment.Exit(3);
            }

            Entity solution = result.Entities[0];

            string versionNumber = IncrementVersionNumber((string)solution["version"], arguments.VersionNumber);

            Console.WriteLine("Setting Version number to " + versionNumber);

            solution["version"] = versionNumber;
            service.Update(solution);
        }

        /// <summary>
        /// Increment a version number
        /// </summary>
        /// <param name="currentVersion"></param>
        /// <param name="incrementVersion"></param>
        /// <returns></returns>
        public static string IncrementVersionNumber(string currentVersion, string incrementVersion)
        {
            List<int> input = ParseVersion(currentVersion);
            List<int> increment = ParseVersion(incrementVersion);

            for (int i = 0; i < increment.Count; i++)
            {
                if (i >= input.Count)
                {
                    input.Add(increment[i] < 0 ? 0 : increment[i]);
                }
                else
                {
                    input[i] += increment[i];
                }
            }

            return string.Join(".", input);
        }

        /// <summary>
        /// Parse a version number from a string to a list of ints
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static List<int> ParseVersion(string version)
        {
            List<int> output = new List<int>();
            int tmp;
            foreach (string s in version.Split('.'))
            {
                if (!int.TryParse(s, out tmp))
                {
                    throw new Exception("Invalid version number component " + s);
                }
                output.Add(tmp);
            }
            return output;
        }
    }
}
