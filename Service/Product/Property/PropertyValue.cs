using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Property
{
    /// <summary>
    /// 属性值
    /// </summary>
    [Serializable]
    public class PropertyValue
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public virtual int PropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 属性值ID
        /// </summary>
        public virtual int PropertyValueId
        {
            get;
            set;
        }

        /// <summary>
        /// 组ID
        /// </summary>
        public virtual int PropertyValueGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// 属性值名称
        /// </summary>
        public virtual string PropertyValueName
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 属性值Code（ERP code）
        /// </summary>
        public virtual string PropertyValueCode
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


    }
}
