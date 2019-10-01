using System.Collections.Generic;
using System.Xml.Serialization;

namespace EaWUnit.Core.JUnit
{
    [XmlRoot(ElementName = "testsuite")]
    public class TestSuite
    {
        [XmlElement(ElementName = "testcase")]
        public List<TestCase> TestCase { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "tests")]
        public string Tests { get; set; }

        [XmlAttribute(AttributeName = "failures")]
        public string Failures { get; set; }

        [XmlAttribute(AttributeName = "hostname")]
        public string Hostname { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
}