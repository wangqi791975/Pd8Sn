using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Dailydeal
{
    /// <summary>
    ///描述：dailydeal配置表 ORM 映射类 
    /// </summary>
    [Class(Table = "t_dailydeal_desc", Lazy = false, NameType = typeof(DailydealDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class DailydealDescPo
    {
        /// <summary>
        /// 语言id
        /// </summary>
        [Id(Name = "LanguageId", Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// dailydeal前台顶部图片
        /// </summary>
        [Property(Column = "headerImg")]
        public virtual string HeaderImg
        {
            get;
            set;
        }
        /// <summary>
        /// dailydeal前台中间
        /// </summary>
        [Property(Column = "middleAreaHtml")]
        public virtual string MiddleAreaHtml
        {
            get;
            set;
        }
        /// <summary>
        /// dailydeal前台底部推荐
        /// </summary>
        [Property(Column = "recommendAreaHtml")]
        public virtual string RecommendAreaHtml
        {
            get;
            set;
        }
        /// <summary>
        /// 该语种dailydeal开关
        /// </summary>
        [Property(Column = "isValid")]
        public virtual bool IsValid
        {
            get;
            set;
        }
    }
}

