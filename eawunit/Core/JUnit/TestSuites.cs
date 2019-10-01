using System.Collections.Generic;
using System.Xml.Serialization;

namespace EaWUnit.Core.JUnit
{
    [XmlRoot(ElementName = "testsuites")]
    public class TestSuites
    {
        [XmlElement(ElementName = "testsuite")]
        public List<TestSuite> TestSuite { get; set; }

        [XmlAttribute(AttributeName = "disabled")]
        public string Disabled { get; set; }

        [XmlAttribute(AttributeName = "errors")]
        public string Errors { get; set; }

        [XmlAttribute(AttributeName = "failures")]
        public string Failures { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "tests")]
        public string Tests { get; set; }

        [XmlAttribute(AttributeName = "time")]
        public string Time { get; set; }
    }
}