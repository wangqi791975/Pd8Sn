using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.Property
{
    /// <summary>
    /// 属性值排序
    /// </summary>
    public enum PropertyValueSortType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义")]
        Undefined = 0,
        /// <summary>
        /// 名称从A到Z排序
        /// </summary>
        [Description("名称从A到Z排序")]
        NameAtoZ = 10,
        /// <summary>
        /// 排序值升序
        /// </summary>
        [Description("排序值升序")]
        SortOrderAscending = 20,
        /// <summary>
        /// 商品数量从多到少
        /// </summary>
        [Description("商品数量从多到少")]
        ProductNumberMoreToLess = 30

    }

    /// <summary>
    /// 属性
    /// </summary>
    [Serializable]
    public class Property
    {
        /// <summary>
        /// id
        /// </summary>
        public virtual int PropertyId
        {
            get;
            set;
        }
        /// <summary>
        /// 属性中文名称
        /// </summary>
        public virtual string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 属性code（ERP code）
        /// </summary>
        public virtual string PropertyCode
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
        /// 是否基本属性
        /// </summary>
        public virtual bool IsBasicProperty
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public virtual bool IsDisplay
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
        /// 属性值排序类型
        /// </summary>
        public virtual PropertyValueSortType SortType
        {
            get;
            set;
        }
    }
}
