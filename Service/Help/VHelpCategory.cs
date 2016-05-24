
using System;

namespace Com.Panduo.Service.Help
{
    /// <summary>
    ///描述：网站帮助中心
    ///创建人: lxf
    ///创建时间：05/06/2015 16:39:02
    /// </summary>
    [Serializable]
    public class VHelpCategory
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public virtual int HelpCategoryId
        {
            get;
            set;
        }
        /// <summary>
        /// 分类类型(0:Customer Care,10:FAQ)
        /// </summary>
        public virtual int CategoryType
        {
            get;
            set;
        }
        /// <summary>
        /// 类别父ID
        /// </summary>
        public virtual int ParentId
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
        /// 是否显示在上一级
        /// </summary>
        public virtual bool IsShowParent
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
        /// 排序
        /// </summary>
        public virtual int SortOrder
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
        public virtual string CategoryName
        {
            get;
            set;
        }
        public virtual string EnCategoryName
        {
            get;
            set;
        }
    }
}

