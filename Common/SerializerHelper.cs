using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml.Serialization;

namespace Com.Panduo.Common
{
    /// <summary>
    /// 序列化类型
    /// </summary>
    public enum SerializerType
    {
        /// <summary>
        /// 二进制
        /// </summary>
        Binary = 0,
        /// <summary>
        /// Soap
        /// </summary>
        Soap = 2,
        /// <summary>
        /// XML
        /// </summary>
        Xml = 4
    }

    public class SerializerHelper<T>
    {
        private static readonly IFormatter soapFormatter = null;
        private static readonly IFormatter binaryFormatter = null;
        private static readonly XmlSerializer xmlSerializer = null;

        static SerializerHelper()
        {
            soapFormatter = new SoapFormatter();
            binaryFormatter = new BinaryFormatter();
            xmlSerializer = new XmlSerializer(typeof(T));
        }



        public static byte[] Serialize(T value, SerializerType serializerType)
        {

            var memoryStream = new MemoryStream();

            byte[] returnValue = null;
            try
            {
                switch (serializerType)
                {
                    case SerializerType.Binary:
                        binaryFormatter.Serialize(memoryStream, value);
                        break;
                    case SerializerType.Soap:
                        soapFormatter.Serialize(memoryStream, value);
                        break;
                    case SerializerType.Xml:
                        xmlSerializer.Serialize(memoryStream, value);
                        break;
                    default:
                        binaryFormatter.Serialize(memoryStream, value);
                        break;
                }
                returnValue = memoryStream.GetBuffer();

                memoryStream.Close();
            }
            catch (Exception)
            {
                returnValue = null;
                //throw;
            }
            return returnValue;
        }
        public static byte[] BinarySerialize(T value)
        {
            return Serialize(value, SerializerType.Binary);
        }
        public static byte[] SoapSerialize(T value)
        {
            return Serialize(value, SerializerType.Soap);
        }
        public static byte[] XmlSerialize(T value)
        {
            return Serialize(value, SerializerType.Xml);
        }

        public static T Deserialize(byte[] value, SerializerType serializerType)
        {
            var memoryStream = new MemoryStream(value);
            T returnValue = default(T);
            try
            {
                switch (serializerType)
                {
                    case SerializerType.Binary:
                        returnValue = (T)binaryFormatter.Deserialize(memoryStream);
                        break;
                    case SerializerType.Soap:
                        returnValue = (T)soapFormatter.Deserialize(memoryStream);
                        break;
                    case SerializerType.Xml:
                        returnValue = (T)xmlSerializer.Deserialize(memoryStream);
                        break;
                    default:
                        returnValue = (T)binaryFormatter.Deserialize(memoryStream);
                        break;
                }
                memoryStream.Close();
            }
            catch 
            {
                returnValue = default(T);
            }
            return returnValue;
        }
        public static T SoapDeserialize(byte[] value)
        {
            return Deserialize(value, SerializerType.Soap);
        }
        public static T BinaryDeserialize(byte[] value)
        {
            return Deserialize(value, SerializerType.Binary);
        }
        public static T XmlDeserialize(byte[] value)
        {
            return Deserialize(value, SerializerType.Xml);
        }

        public static string XmlDeserializeString(byte[] value)
        {

            return Encoding.UTF8.GetString(value);
        }

        public static string DeserializeString(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }
    }
}
