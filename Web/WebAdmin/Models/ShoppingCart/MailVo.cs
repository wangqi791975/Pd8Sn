//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MailVo.cs
//创 建 人：罗海明
//创建时间：2015/05/13 16:16:40 
//功能说明：后台购物车14天未更新邮件Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

namespace Com.Panduo.Web.Models.ShoppingCart
{
    public class MailVo
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public virtual string MailContent { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public virtual string To{ get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public virtual string From { get; set; }

        /// <summary>
        /// 语种ID
        /// </summary>
        public virtual int LanguageId { get; set; }
    }
}