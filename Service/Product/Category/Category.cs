using System;

namespace Com.Panduo.Service.Product.Category
{
    /// <summary>
    /// 类别
    /// </summary>
    [Serializable]
    public class Category
    {
        /// <summary>
        /// 类别ID
        /// </summary>
        public virtual int CategoryId
        {
            get;
            set;
        }
        /// <summary>
        /// 类别图片
        /// </summary>
        public virtual string CategoryImage
        {
            get;
            set;
        }

        /// <summary>
        /// 父类别ID
        /// </summary>
        public virtual int ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get;
            set;
        }


        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? ModfiyTime
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
        /// 类别code
        /// </summary>
        public virtual string CategoryCode
        {
            get;
            set;
        }

        /// <summary>
        /// 类别中文名称
        /// </summary>
        public virtual string CategoryName
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
        /// 排序
        /// </summary>
        public virtual int DiplayOrder
        {
            get;
            set;
        }


        /// <summary>
        /// 样式名称
        /// </summary>
        public virtual string CssName
        {
            get;
            set;
        }


    }
}
