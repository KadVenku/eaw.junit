using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("eawunit.test")]

namespace EaWUnit.Core.Application
{
    internal static class ApplicationData
    {
        internal static TestMode TestMode { get; set; }
        internal static string ModCheckReportFilePath { get; set; }
        internal static string EaWUnitReportOutputPath { get; set; }
        internal static int ExitCode { get; set; } = 0;
    }
}
