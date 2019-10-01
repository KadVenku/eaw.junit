using System.IO;
using System.Xml.Serialization;
using EaWUnit.Core.JUnit;

namespace EaWUnit.CheckMod.Writer
{
    internal class TestSuiteWriter
    {
        internal void WriteToFile(TestSuites testSuites, string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TestSuites));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, testSuites);
            writer.Close();
        }
    }
}