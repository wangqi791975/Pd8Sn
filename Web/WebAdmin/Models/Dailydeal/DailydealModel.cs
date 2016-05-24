using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Models.Dailydeal
{
    public class DailydealModel
    {

        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId { set; get; }

        /// <summary>
        /// 头部图片
        /// </summary>
        public virtual string HeaderImg { set; get; }

        /// <summary>
        /// 中间图片推荐区域 Middle HTML
        /// </summary>
        public virtual string MiddleAreaHtml { set; get; }

        /// <summary>
        /// 商品推荐区域 HTML
        /// </summary>
        public virtual string RecommendAreaHtml { set; get; }

        /// <summary>
        /// Daily Deals开关
        /// </summary>
        //public virtual bool IsValid { set; get; }
    }
}