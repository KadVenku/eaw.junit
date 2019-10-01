using System.Xml.Serialization;

namespace EaWUnit.Core.JUnit
{
    [XmlRoot(ElementName = "skipped")]
    public class Skipped
    {
        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }
    }
}
