using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace Tracer
{
    [XmlRoot("root")]
    public class TraceResult
    {
        [XmlElement(ElementName = "thread")]
        public List<Threads> Threads = new List<Threads>();
    }
    public class Item
    {
        [XmlAttribute("time")]
        public long Time;
        [XmlElement(ElementName = "method")]
        public List<Methods> Methods;
    }
    public class Threads : Item
    {
        [XmlAttribute("id")]
        public int id;
    }
    public class Methods : Item
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("class")]
        public string ClassName;
    }
}