using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EXECommander
{
    /// <summary>
    /// 1. 선언할때 XML 파일 경로셋팅
    /// 2. Public한 SetData, GetData 만 사용할것
    /// </summary>
    internal class Serializer
    {
        public string DirectoryPath { get; set; }
        
        public Serializer(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        public void SetDataToXMLFile<T>(T toSerialize)
        {
            string xmlContent = this.SerializeObjectToXml(toSerialize);
            
            File.WriteAllText(this.DirectoryPath + @"\" + this.GetTypeName<T>(), xmlContent);
        }

        public T GetDataFromXMLFile<T>()
        {
            if (File.Exists(this.DirectoryPath + @"\" + this.GetTypeName<T>()))
            {
                string xmlContent = File.ReadAllText(this.DirectoryPath + @"\" + this.GetTypeName<T>());

                return this.DeserializeXmlToObject<T>(xmlContent);
            }
            else
            {
                throw new FileNotFoundException($"The file {this.DirectoryPath + @"\" + this.GetTypeName<T>()} does not exist.");
            }
        }

        private string SerializeObjectToXml<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        private T DeserializeXmlToObject<T>(string xmlText)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StringReader textReader = new StringReader(xmlText))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        private string GetTypeName<T>()
        {
            Type type = typeof(T);
            if (type.IsGenericType)
            {
                string genericTypeName = type.GetGenericTypeDefinition().Name;
                // Remove the generic parameter count (`1, `2, etc.) from the type name
                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));

                var genericArgs = type.GetGenericArguments();
                string[] genericArgNames = genericArgs.Select(t => t.Name).ToArray();

                // Construct the full generic type name including generic argument names
                return $"{genericTypeName}Of{string.Join("And", genericArgNames)}";
            }
            else
            {
                return type.Name; // Non-generic types just use the direct name
            }
        }
    }
}
