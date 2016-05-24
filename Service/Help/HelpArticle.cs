
using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Help
{
    /// <summary>
    ///描述：网站帮助中心
    ///创建人: 万天文
    ///创建时间：04/07/2015 16:39:02
    /// </summary>
    [Serializable]
    public class HelpArticle
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public virtual int ArticleId
        {
            get;
            set;
        }
        /// <summary>
        /// 类别ID
        /// </summary>
        public virtual int HelpCategoryId
        {
            get;
            set;
        }
        /// <summary>
        /// 分类逐级路径
        /// </summary>
        public virtual string CategoryPath
        {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title
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
        public virtual string Content
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0:已废弃,1:启用)
        /// </summary>
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public virtual bool IsRecommend
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 有用次数
        /// </summary>
        public virtual int UsefulNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 无用次数
        /// </summary>
        public virtual int UnusefulNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public virtual int BrowseNumber
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

        #region 扩展

        public virtual List<HelpArticleDescription> Descriptions { get; set; }

        #endregion
    }
}

