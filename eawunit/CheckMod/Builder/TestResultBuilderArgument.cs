using System.Collections.Generic;
using EaWUnit.Core.Application;
using EaWUnit.Core.JUnit;

namespace EaWUnit.CheckMod.Builder
{
    internal class TestResultBuilderArgument
    {
        internal string TestSuiteName { get; set; }
        internal List<TestCase> TestCases { get; set; } = new List<TestCase>();
    }
}
