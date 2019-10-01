using System.Xml.Serialization;

namespace EaWUnit.Core.JUnit
{
    [XmlRoot(ElementName="failure")]
    public class Failure {
        [XmlAttribute(AttributeName="message")]
        public string Message { get; set; }
        [XmlAttribute(AttributeName="type")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }
}
