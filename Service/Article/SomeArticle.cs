//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：SomeArticle.cs
//创 建 人：罗海明
//创建时间：2015/04/07 16:01:00 
//功能说明：零散文章主表Vo
//-----------------------------------------------------------------
//修改记录：  
//修改人：   
//修改时间：  
//修改内容： 
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Article
{
   public class SomeArticle
    {
       /// <summary>
       /// 文章Id
       /// </summary>
       public virtual int ArticleId { get; set; }

       /// <summary>
       /// 中文标题
       /// </summary>
       public virtual string ChineseTitle { get; set; }

       /// <summary>
       /// 英文标题
       /// </summary>
       public virtual string EnglishTitle { get; set; }

       /// <summary>
       /// 涉及语种
       /// </summary>
       public virtual string RelatedLang { get; set; }

       /// <summary>
       /// 创建时间
       /// </summary>
       public virtual DateTime CreateTime { get; set; }

       /// <summary>
       /// 创建人
       /// </summary>
       public virtual string Creater { get; set; }

       /// <summary>
       /// 创建人Id
       /// </summary>
       public virtual int CreateId { get; set; }

    }
}
