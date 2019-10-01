using System;
using System.IO;
using EaWUnit.CheckMod.Builder;
using EaWUnit.CheckMod.Parser;
using EaWUnit.CheckMod.Writer;
using EaWUnit.Core.Application;
using EaWUnit.Core.JUnit;

namespace EaWUnit
{
    internal class EawUnitTest
    {
        private static void Main(string[] args)
        {
            try
            {
                AppUtility.ParseArgs(args);
                TestResultBuilder builder = new TestResultBuilder();
                TestSuites testSuites = builder.Build(new TestResultBuilderArgument
                {
                    TestCases = new CheckModReportFileParser().ParseFile(ApplicationData.ModCheckReportFilePath,
                        ApplicationData.TestMode),
                    TestSuiteName = "Mod::Check()"
                });
                TestSuiteWriter writer = new TestSuiteWriter();
                string testName = "TEST-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".xml";
                writer.WriteToFile(testSuites, Path.Combine(ApplicationData.EaWUnitReportOutputPath, testName));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"An error occured {e}", e.Message);
                ApplicationData.ExitCode = 1;
                AppUtility.PrintUsage();
            }

            Environment.Exit(ApplicationData.ExitCode);
        }
    }
}
