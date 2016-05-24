using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace  Com.Panduo.Web.Common.Config.Hander
{
    /// <summary>
    /// 自定义配置文件管理类
    /// </summary>
    public class CustomConfigManager
    {
        private readonly XmlNode section; 

        public CustomConfigManager(XmlNode section)
        {
            this.section = section;
             
        } 
    }
}
