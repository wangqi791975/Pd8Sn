using System;
using Com.Panduo.Entity.Product;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Review
{
    [Class(Table = "v_reviewwebsite_customer", Lazy = false, NameType = typeof(ReviewWebsiteCustomerViewPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ReviewWebsiteCustomerViewPo
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        [Id(1, Name = "Id", Column = "review_id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        [Property(Column = "type")]
        public virtual string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Property(Column = "customer_email")]
        public virtual string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 头像
        /// </summary>
        [Property(Column = "avatar")]
        public virtual string Avatar
        {
            get;
            set;
        }
        
        /// <summary>
        /// 国家名
        /// </summary>
        [Property(Column = "country_name")]
        public virtual string CountryName
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "full_name")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 评论时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 语言Id
        /// </summary>
        [Property(Column = "languages_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言中文名称
        /// </summary>
        [Property(Column = "chinese_name")]
        public virtual string LanguageChName
        {
            get;
            set;
        }
        /// <summary>
        /// 首页推荐
        /// </summary>
        [Property(Column = "recommend")]
        public virtual bool Recommend
        {
            get;
            set;
        }
        /// <summary>
        /// 显示状态
        /// </summary>
        [Property(Column = "is_valid")]
        public virtual bool IsValid
        {
            get;
            set;
        }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Property(Column = "review_content")]
        public virtual string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 答复内容
        /// </summary>
        [Property(Column = "reply_content")]
        public virtual string RepContent
        {
            get;
            set;
        }
    }
}