//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：MailLogo.cs
//创 建 人：罗海明
//创建时间：2015/04/08 09:40:40 
//功能说明：MailLogoVo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;


namespace Com.Panduo.Service.SystemMail
{
     public class MailLogo
     {
         /// <summary>
         /// Id
         /// </summary>
         public virtual int LogoId { get; set; }

         /// <summary>
         /// 语种Id
         /// </summary>
         public virtual int LanguageId { get; set; }

         /// <summary>
         /// Logo图片
         /// </summary>
         public virtual string LogoImage { get; set; }

         /// <summary>
         /// Logo链接
         /// </summary>
         public virtual string LogoUrl { get; set; }

         /// <summary>
         /// 是否使用
         /// </summary>
         public virtual bool IsUse { get; set; }

         /// <summary>
         /// 创建时间
         /// </summary>
         public virtual DateTime CreateTime { get; set; }

         /// <summary>
         /// 创建人
         /// </summary>
         public virtual int Creator { get; set; }
     }
}
