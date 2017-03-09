using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nurse.Common.helper
{
    public class XmlHelper
    {
        /// <summary>
        /// 实体到xml
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sourceObj"></param>
        /// <param name="type"></param>
        /// <param name="xmlRootName"></param>
        public static void Enity2Xml(string filePath, object sourceObj, Type type = null, string xmlRootName = null)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
            {
                type = type != null ? type : sourceObj.GetType();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = string.IsNullOrWhiteSpace(xmlRootName) ?
                        new System.Xml.Serialization.XmlSerializer(type) :
                        new System.Xml.Serialization.XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                    xmlSerializer.Serialize(writer, sourceObj);
                }
            }
        }

        /// <summary>
        /// xml到实体
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Xml2Entity(string filePath, Type type)
        {
            object result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                    result = xmlSerializer.Deserialize(reader);
                }
            }

            return result;
        }
    }
}
