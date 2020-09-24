using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace lab1.Serialization
{
    class myXmlSerializer : ISerializer
    {
        private XmlSerializerNamespaces emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        public string Serialize(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(stringWriter, obj, emptyNamespaces);
                    return stringWriter.ToString();
                }
            }
        }
    }
}