
using System;

namespace Com.Panduo.Service.Help
{
    /// <summary>
    ///描述：网站帮助中心
    ///创建人: 万天文
    ///创建时间：04/07/2015 16:39:02
    /// </summary>
    [Serializable]
    public class HelpArticleDescription
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public virtual int DescriptionId
        {
            get;
            set;
        }
        /// <summary>
        /// 文章ID
        /// </summary>
        public virtual int ArticleId
        {
            get;
            set;
        }
        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string ArticleName
        {
            get;
            set;
        }
        /// <summary>
        /// 关键词
        /// </summary>
        public virtual string Keywords
        {
            get;
            set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string ArticleContent
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime DateModified
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0：隐藏,1:显示)
        /// </summary>
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 是否显示在上一级
        /// </summary>
        public virtual bool IsShowParent
        {
            get;
            set;
        }
    }
}

