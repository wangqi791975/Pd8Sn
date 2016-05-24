//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：BannerInfo.cs
//创 建 人：罗海明
//创建时间：2015/04/07 14:40:40 
//功能说明：横幅广告实体类
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;


namespace Com.Panduo.Service.Marketing.Banner
{
   public class BannerInfo
    {
       /// <summary>
       /// 语种Id
       /// </summary>
       public virtual int LanguageId { get; set; }

       /// <summary>
       /// Banner主体代码
       /// </summary>
       public virtual string Content { get; set; }


       /// <summary>
       /// Banner开始时间
       /// </summary>
       public virtual DateTime BannerStartTime { get; set; }

       /// <summary>
       /// Banner结束时间
       /// </summary>
       public virtual DateTime BannerEndTime { get; set; }


       /// <summary>
       /// 状态 是否有效
       /// </summary>
       public virtual bool IsValid { set; get; }

       /// <summary>
       /// 是否显示首页
       /// </summary>
       public virtual bool IsShowHome { set; get; }

       /// <summary>
       /// 是否倒计时
       /// </summary>
       public virtual bool IsCountdown { set; get; }
    }
}
