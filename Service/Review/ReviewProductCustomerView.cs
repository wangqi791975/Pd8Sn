using System;

namespace Com.Panduo.Service.Review
{
    public class ReviewProductCustomerView
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        public virtual int Id
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
        /// 产品名称
        /// </summary>
        public virtual string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductModel
        {
            get;
            set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 国家Id
        /// </summary>
        public virtual string CountryId
        {
            get;
            set;
        }

        /// <summary>
        /// 国家名称
        /// </summary>
        public virtual string CountryName
        {
            get;
            set;
        }

        /// <summary>
        /// 评分
        /// </summary>
        public virtual int Rating
        {
            get;
            set;
        }

        /// <summary>
        /// 评论时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// 语言Id
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// 语言中文名称
        /// </summary>
        public virtual string LanguageChName
        {
            get;
            set;
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        public virtual bool IsValid
        {
            get;
            set;
        }

        /// <summary>
        /// 评论内容
        /// </summary>
        public virtual string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 答复内容
        /// </summary>
        public virtual string RepContent
        {
            get;
            set;
        }
    }
}