//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：SomeArticleLanguage.cs
//创 建 人：罗海明
//创建时间：2015/04/07 16:02:00 
//功能说明：零散文章多语种Vo
//-----------------------------------------------------------------
//修改记录：  
//修改人：   
//修改时间：  
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Panduo.Service.Article
{
   public class SomeArticleLanguage
    {
       /// <summary>
       /// 文章Id
       /// </summary>
       public virtual int ArticleId { get; set; }

       /// <summary>
       /// 语种Id
       /// </summary>
       public virtual int LanguageId { get; set; }

       /// <summary>
       /// 对应语种标题
       /// </summary>
       public virtual string Title { get; set; }

       /// <summary>
       /// 对应语种内容
       /// </summary>
       public virtual string Content { get; set; }

       /// <summary>
       /// 状态
       /// </summary>
       public virtual bool Status { get; set; }

       /// <summary>
       /// 创建时间
       /// </summary>
       public virtual DateTime CreateTime { get; set; }

    }
}
