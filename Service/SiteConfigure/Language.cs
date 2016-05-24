using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 语种
    /// </summary>
   [Serializable]
    public class Language
    {
        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// 语种简称
        /// </summary>
        public virtual string LanguageCode
        {
            get;
            set;
        }

        /// <summary>
        /// 订单号前缀(如8seasons网站俄语站点为SR、德语站点为SD)
        /// </summary>
        public virtual string OrderPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// 语种名称
        /// </summary>
        public virtual string LanguageName
        {
            get;
            set;
        }

        /// <summary>
        /// 中文名称
        /// </summary>
        public virtual string ChineseName
        {
            get;
            set;
        }

       /// <summary>
        /// 长日期格式
       /// </summary>
        public virtual string DateFormatLong
        {
            get;
            set;
        }
        /// <summary>
        /// 短日期格式
        /// </summary>
        public virtual string DateFormatShort
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public virtual bool IsValid
        {
            get;
            set;
        }
       
         /// <summary>
        /// 语种站点 Host
        /// </summary>
        public virtual string Host { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 语种对应客户经理ID(t_customer_manager)
        /// </summary>
        public virtual int? CustomerManagerId
        {
            get;
            set;
        }

    }
}
