using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品多语种
    /// </summary>
    [Serializable]
    public class ProductLanguage
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// 产品描述
        /// </summary>
        public virtual string ProductDescription
        {
            get;
            set;
        }

        /// <summary>
        /// 营销标题
        /// </summary>
        public virtual string MarketingTitle
        {
            get;
            set;
        }

    }
}
