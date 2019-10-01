using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EaWUnit.Core.Application;
using EaWUnit.Core.JUnit;

[assembly: InternalsVisibleTo("eawunit.test")]
namespace EaWUnit.CheckMod.Builder
{
    internal class TestResultBuilder
    {
        internal TestSuites Build(TestResultBuilderArgument args)
        {
            if (args.TestCases.Any())
            {
                ApplicationData.ExitCode = 1;
            }

            TestSuites testSuites = new TestSuites
            {
                Disabled = "0",
                Errors = "0",
                Failures = args.TestCases.Count.ToString(),
                Name = args.TestSuiteName,
                TestSuite = new List<TestSuite>()
            };

            TestSuite testSuite = new TestSuite
            {
                Name = args.TestSuiteName,
                Id = "0",
                Tests = args.TestCases.Count.ToString(),
                Failures = args.TestCases.Count.ToString(),
                TestCase = args.TestCases
            };

            testSuites.TestSuite.Add(testSuite);
            return testSuites;
        }
    }
}
