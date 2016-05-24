using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Property
{
    /// <summary>
    /// 属性值组
    /// </summary>
    [Serializable]
    public class PropertyValueGroup
    {
        /// <summary>
        /// 属性值组ID
        /// </summary>
        public virtual int GroupId
        {
            get;
            set;
        }

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
        /// 是否有效
        /// </summary>
        public virtual bool IsValid
        {
            get;
            set;
        }

        /// <summary>
        /// 属性值组中文名称
        /// </summary>
        public virtual string PropertyValueGroupName
        {
            get;
            set;
        }


        /// <summary>
        /// 属性值组Code
        /// </summary>
        public virtual string PropertyValueGroupCode
        {
            get;
            set;
        }

    }
}
