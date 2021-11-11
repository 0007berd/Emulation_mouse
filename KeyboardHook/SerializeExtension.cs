using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace KeyboardHook		

{
    public static class SerializeExtension
    {
        public static string SerializeToString(object obj)
        {
            var xmlSerializer = new XmlSerializer(obj.GetType());
            var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        public static T DeserializeString<T>(string sourceString)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringReader = new StringReader(sourceString);

            return (T)xmlSerializer.Deserialize(stringReader);
        }
    }
}
