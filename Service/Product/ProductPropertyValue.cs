using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品属性值
    /// </summary>
    [Serializable]
    public class ProductPropertyValue
    {
        /// <summary>
        /// 产品属性值Id
        /// </summary>
        public virtual int ProductPropertyValueId
        {
            get;
            set;
        }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// 属性Id
        /// </summary>
        public virtual int PropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 属性值Id
        /// </summary>
        public virtual int PropertyValueId
        {
            get;
            set;
        }

    }

}
