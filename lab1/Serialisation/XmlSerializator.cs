using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace lab1.Serialisation
{
    class XmlSerializator : ISerializator
    {
        string ISerializator.Serialize(object obj)
        {
            string xml;
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (StringWriter stringWriter = new StringWriter())
            {
                    xmlSerializer.Serialize(stringWriter, obj);
                    xml = stringWriter.ToString();
            }
            return xml;
        }
    }
}
