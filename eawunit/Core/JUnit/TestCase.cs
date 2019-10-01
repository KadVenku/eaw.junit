using System.Collections.Generic;
using System.Xml.Serialization;

namespace EaWUnit.Core.JUnit
{
    [XmlRoot(ElementName = "testcase")]
    public class TestCase
    {
        [XmlElement(ElementName = "skipped")]
        public Skipped Skipped { get; set; }

        [XmlElement(ElementName = "failure")]
        public List<Failure> Failure { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "classname")]
        public string Classname { get; set; }

        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }

        [XmlAttribute(AttributeName = "time")]
        public string Time { get; set; }
    }
}