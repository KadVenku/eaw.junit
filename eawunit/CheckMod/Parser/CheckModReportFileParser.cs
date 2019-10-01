using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using EaWUnit.Core.Application;
using EaWUnit.Core.JUnit;

[assembly: InternalsVisibleTo("eawunit.test")]

namespace EaWUnit.CheckMod.Parser
{
    internal class CheckModReportFileParser
    {
        private readonly string DUPLICATE_ERROR_LINE1_REGEX =
            "(?<fileName>.*):(?<lineNumber>[\\d]+): duplicate (?<type>.*) found: \"(?<name>.*)\"";

        private readonly string DUPLICATE_ERROR_LINE2_REGEX =
            "(?<fileName>.*):(?<lineNumber>[\\d]+): previous declaration was here";

        private readonly string UNKNOWN_MTD_TEXTURE_REFERENCE_REGEX =
            "(?<fileName>.*):(?<lineNumber>[\\d]+): unknown MTD texture \"(?<name>.*)\"";

        private readonly string UNKNOWN_OBJECT_REFERENCE_REGEX = "(?<fileName>.*): unknown (?<type>.*) \"(?<name>.*)\"";

        private readonly string UNKNOWN_STRING_REFERENCE_REGEX =
            "(?<fileName>.*):(?<lineNumber>[\\d]+): unknown string \"(?<name>.*)\"";

        private readonly string UNKNOWN_XML_OBJECT_REFERENCE_REGEX =
            "(?<fileName>.*):(?<lineNumber>[\\d]+): unknown (?<type>.*) \"(?<name>.*)\"";

        internal List<TestCase> ParseFile(string filePath, TestMode mode)
        {
            List<TestCase> testCases = new List<TestCase>();
            switch (mode)
            {
                case TestMode.Error:
                    ParseFile_Error(testCases, filePath);
                    break;
                case TestMode.Warning:
                    ParseFile_Warning(testCases, filePath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            return testCases;
        }

        private void ParseFile_Error(ICollection<TestCase> testCases, string filePath)
        {
            using (StreamReader file = new StreamReader(filePath))
            {
                string lineToParse;
                Regex unknownStringReference = new Regex(UNKNOWN_STRING_REFERENCE_REGEX, RegexOptions.IgnoreCase);
                Regex unknownMtdTextureReference =
                    new Regex(UNKNOWN_MTD_TEXTURE_REFERENCE_REGEX, RegexOptions.IgnoreCase);

                Regex duplicateGameObjectLine1 = new Regex(DUPLICATE_ERROR_LINE1_REGEX, RegexOptions.IgnoreCase);
                Regex duplicateGameObjectLine2 = new Regex(DUPLICATE_ERROR_LINE2_REGEX, RegexOptions.IgnoreCase);

                Regex unknownXmlObjectReference =
                    new Regex(UNKNOWN_XML_OBJECT_REFERENCE_REGEX, RegexOptions.IgnoreCase);
                Regex unknownObjectReference = new Regex(UNKNOWN_OBJECT_REFERENCE_REGEX, RegexOptions.IgnoreCase);
                while ((lineToParse = file.ReadLine()) != null)
                {
                    Match unknownStringReferenceMatch = unknownStringReference.Match(lineToParse);
                    Match unknownMtdTextureReferenceMatch = unknownMtdTextureReference.Match(lineToParse);

                    if (unknownStringReferenceMatch.Success || unknownMtdTextureReferenceMatch.Success)
                    {
                        continue;
                    }

                    Match duplicateGameObjectLine1Match = duplicateGameObjectLine1.Match(lineToParse);
                    if (duplicateGameObjectLine1Match.Success)
                    {
                        string line2 = file.ReadLine();
                        Match duplicateGameObjectLine2Match;
                        if (line2 != null)
                        {
                            duplicateGameObjectLine2Match = duplicateGameObjectLine2.Match(line2);
                        }
                        else
                        {
                            continue;
                        }

                        TestCase testCase = new TestCase
                        {
                            Name = "Mod::CheckDuplicate()->" + "Duplicate " +
                                   duplicateGameObjectLine1Match.Groups["type"].Value + " " +
                                   duplicateGameObjectLine1Match.Groups["name"].Value,
                            Classname =
                                "Duplicate " + duplicateGameObjectLine1Match.Groups["type"].Value + " " +
                                duplicateGameObjectLine1Match.Groups["name"].Value,
                            Failure = new List<Failure>(),
                            Status = TestMode.Error.ToString().ToUpper()
                        };

                        testCase.Failure.Add(new Failure
                        {
                            Message =
                                "Duplicate " + duplicateGameObjectLine1Match.Groups["type"].Value + " " +
                                duplicateGameObjectLine1Match.Groups["name"].Value,
                            Type = TestMode.Error.ToString().ToUpper(),
                            Text = "Duplicate " + duplicateGameObjectLine1Match.Groups["type"].Value +
                                   " definition for \"" + duplicateGameObjectLine1Match.Groups["name"].Value + "\"." +
                                   " The duplicate was found in file " +
                                   duplicateGameObjectLine1Match.Groups["fileName"].Value.Replace("\\", "/") +
                                   " in line " + duplicateGameObjectLine1Match.Groups["lineNumber"].Value + "." +
                                   " The previous declaration was in file " +
                                   duplicateGameObjectLine2Match.Groups["fileName"].Value.Replace("\\", "/") +
                                   " in line " + duplicateGameObjectLine2Match.Groups["lineNumber"].Value + "."
                        });
                        testCases.Add(testCase);
                        continue;
                    }

                    Match unknownXmlObjectReferenceMatch = unknownXmlObjectReference.Match(lineToParse);
                    if (unknownXmlObjectReferenceMatch.Success)
                    {
                        TestCase testCase = new TestCase
                        {
                            Name = "Mod::CheckGameObjectReference()->" + "Missing " +
                                   unknownXmlObjectReferenceMatch.Groups["type"].Value + " " +
                                   unknownXmlObjectReferenceMatch.Groups["name"].Value,
                            Classname =
                                "Missing " + unknownXmlObjectReferenceMatch.Groups["type"].Value + " " +
                                unknownXmlObjectReferenceMatch.Groups["name"].Value,
                            Failure = new List<Failure>(),
                            Status = TestMode.Error.ToString().ToUpper()
                        };

                        testCase.Failure.Add(new Failure
                        {
                            Message =
                                "Missing " + unknownXmlObjectReferenceMatch.Groups["type"].Value + " " +
                                unknownXmlObjectReferenceMatch.Groups["name"].Value,
                            Type = TestMode.Error.ToString().ToUpper(),
                            Text = "Reference to missing " + unknownXmlObjectReferenceMatch.Groups["type"].Value +
                                   " \"" + unknownXmlObjectReferenceMatch.Groups["name"].Value + "\"" +
                                   " found in file " +
                                   unknownXmlObjectReferenceMatch.Groups["fileName"].Value.Replace("\\", "/") +
                                   " in line: " + unknownXmlObjectReferenceMatch.Groups["lineNumber"].Value + "."
                        });

                        testCases.Add(testCase);
                        continue;
                    }

                    Match unknownObjectReferenceMatch = unknownObjectReference.Match(lineToParse);
                    if (unknownObjectReferenceMatch.Success)
                    {
                        TestCase testCase = new TestCase
                        {
                            Name = "Mod::CheckAssetReference()->" + "Missing " +
                                   unknownObjectReferenceMatch.Groups["type"].Value + " " +
                                   unknownObjectReferenceMatch.Groups["name"].Value,
                            Classname =
                                "Missing " + unknownObjectReferenceMatch.Groups["type"].Value + " " +
                                unknownObjectReferenceMatch.Groups["name"].Value,
                            Failure = new List<Failure>(),
                            Status = TestMode.Error.ToString().ToUpper()
                        };

                        testCase.Failure.Add(new Failure
                        {
                            Message =
                                "Missing " + unknownObjectReferenceMatch.Groups["type"].Value + " " +
                                unknownObjectReferenceMatch.Groups["name"].Value,
                            Type = TestMode.Error.ToString().ToUpper(),
                            Text = "Reference to missing " + unknownObjectReferenceMatch.Groups["type"].Value +
                                   " \"" + unknownObjectReferenceMatch.Groups["name"].Value + "\"" +
                                   " found in file " + unknownObjectReferenceMatch.Groups["fileName"].Value
                                       .Replace("\\", "/") + "."
                        });

                        testCases.Add(testCase);
                    }
                }
            }
        }

        private void ParseFile_Warning(ICollection<TestCase> testCases, string filePath)
        {
            using (StreamReader file = new StreamReader(filePath))
            {
                string lineToParse;
                Regex unknownStringReference = new Regex(UNKNOWN_STRING_REFERENCE_REGEX, RegexOptions.IgnoreCase);
                Regex unknownMtdTextureReference =
                    new Regex(UNKNOWN_MTD_TEXTURE_REFERENCE_REGEX, RegexOptions.IgnoreCase);

                while ((lineToParse = file.ReadLine()) != null)
                {
                    Match unknownStringReferenceMatch = unknownStringReference.Match(lineToParse);
                    Match unknownMtdTextureReferenceMatch = unknownMtdTextureReference.Match(lineToParse);
                    if (unknownStringReferenceMatch.Success)
                    {
                        TestCase testCase = new TestCase
                        {
                            Name =
                                "Mod::CheckStringReference()->" + "Unknown string " +
                                unknownStringReferenceMatch.Groups["name"].Value,
                            Classname = "Unknown string " + unknownStringReferenceMatch.Groups["name"].Value,
                            Failure = new List<Failure>(),
                            Status = TestMode.Warning.ToString().ToUpper()
                        };
                        testCase.Failure.Add(new Failure
                        {
                            Message = "Unknown string " + unknownStringReferenceMatch.Groups["name"].Value,
                            Type = TestMode.Warning.ToString().ToUpper(),
                            Text = "Unknown string key reference \"" +
                                   unknownStringReferenceMatch.Groups["name"].Value + "\"" + " found in file " +
                                   unknownStringReferenceMatch.Groups["fileName"].Value.Replace("\\", "/") +
                                   " in line " + unknownStringReferenceMatch.Groups["lineNumber"].Value
                        });
                        testCases.Add(testCase);
                    }
                    else if (unknownMtdTextureReferenceMatch.Success)
                    {
                        TestCase testCase = new TestCase
                        {
                            Name =
                                "Mod::CheckMtdTextureReference()->" + "Unknown MTD Texture " +
                                unknownMtdTextureReferenceMatch.Groups["name"].Value,
                            Classname =
                                "Unknown MTD Texture " + unknownMtdTextureReferenceMatch.Groups["name"].Value,
                            Failure = new List<Failure>(),
                            Status = TestMode.Warning.ToString().ToUpper()
                        };
                        testCase.Failure.Add(new Failure
                        {
                            Message = "Unknown MTD Texture " + unknownMtdTextureReferenceMatch.Groups["name"].Value,
                            Type = TestMode.Warning.ToString().ToUpper(),
                            Text = "Unknown MTD Texture reference \"" +
                                   unknownMtdTextureReferenceMatch.Groups["name"].Value + "\"" + " found in file " +
                                   unknownMtdTextureReferenceMatch.Groups["fileName"].Value.Replace("\\", "/") +
                                   " in line " + unknownMtdTextureReferenceMatch.Groups["lineNumber"].Value
                        });
                        testCases.Add(testCase);
                    }
                }
            }
        }
    }
}
