
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Category
{
    /// <summary>
    ///描述：类别表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:37:51
    /// </summary>
    [Class(Table = "t_category", Lazy = false, NameType = typeof(CategoryPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CategoryPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "CategoryId", Column = "category_id")]
        [Generator(2, Class = "native")]

        public virtual int CategoryId
        {
            get;
            set;
        }
        /// <summary>
        /// 父类别id
        /// </summary>
        [Property(Column = "parent_id")]
        public virtual int ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// code
        /// </summary>
        [Property(Column = "code")]
        public virtual string Code
        {
            get;
            set;
        }
        /// <summary>
        /// 图片
        /// </summary>
        [Property(Column = "image")]
        public virtual string Image
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [Property(Column = "chinese_name")]
        public virtual string ChineseName
        {
            get;
            set;
        }
        /// <summary>
        /// 1有效，0无效
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 1显示，0隐藏
        /// </summary>
        [Property(Column = "is_show")]
        public virtual bool IsShow
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime? DateModified
        {
            get;
            set;
        }
        /// <summary>
        /// 用于首页显示小图标
        /// </summary>
        [Property(Column = "class_name")]
        public virtual string ClassName
        {
            get;
            set;
        }
    }
}

