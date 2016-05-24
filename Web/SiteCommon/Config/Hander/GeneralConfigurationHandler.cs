using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Com.Panduo.Common;

namespace  Com.Panduo.Web.Common.Config.Hander
{

    /// <summary>
    /// 自定义配置根节点处理
    /// </summary>
    public class GeneralConfigurationHandler : IConfigurationSectionHandler
    {
        private static readonly string VALUE_TYPE = "type";

        #region IConfigurationSectionHandler 成员

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            object obj = null;

            if (section.Attributes !=null)
            {
                var handerType = Type.GetType(section.Attributes[VALUE_TYPE].Value);//CustomConfig.ConfigManager

                var parameters = new[] { section };


                try
                {
                    obj = Activator.CreateInstance(handerType, parameters);
                }
                catch (Exception ex)
                {
                    LoggerHelper.GetLogger(LoggerType.Exception).Error(ex.Message,ex);
                }
            }

            return obj;
        }

        #endregion
    }
}
