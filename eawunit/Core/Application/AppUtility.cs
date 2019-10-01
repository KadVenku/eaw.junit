using System;
using System.IO;

namespace EaWUnit.Core.Application
{
    internal static class AppUtility
    {
        internal static void ParseArgs(string[] args)
        {
            if (args == null || args.Length != 3)
            {
                throw new ArgumentException($"Unexpected arguments: {args}");
            }

            if (args[0].Equals(TestMode.Warning.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationData.TestMode = TestMode.Warning;
            }
            else if (args[0].Equals(TestMode.Error.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationData.TestMode = TestMode.Error;
            }
            else
            {
                throw new ArgumentException($"Unexpected argument {args[0]}");
            }

            if (!File.Exists(args[1]))
            {
                throw new ArgumentException($"File does not exist: {args[1]}");
            }

            ApplicationData.ModCheckReportFilePath = args[1];

            if (!Directory.Exists(args[2]))
            {
                throw new ArgumentException($"Directory does not exist: {args[2]}");
            }

            ApplicationData.EaWUnitReportOutputPath = args[2];
        }
    }
}
