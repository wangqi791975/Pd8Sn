using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Com.Panduo.Common
{
    public static class Helper
    {
        /// <summary>
        /// 将XmlNode转换为XElement
        /// </summary>
        /// <param name="node">要转换的XmlNode</param>
        /// <returns>转换后的XElement</returns>
        public static XElement ToXElement(this XmlNode node)
        {
            var document = new XDocument();
            using (var xmlWriter = document.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }

            return document.Root;
        }

        /// <summary>
        /// 将XElement转换为XmlNode
        /// </summary>
        /// <param name="element">要转换的Xelement</param>
        /// <returns>转换后的XmlNode</returns>
        public static XmlNode ToXmlNode(this XElement element)
        {
            using (var xmlReader = element.CreateReader())
            {
                var document = new XmlDocument();
                document.Load(xmlReader);
                return document;
            }
        }
    }

}
