//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：Channel.cs
//创 建 人：罗海明
//创建时间：2015/01/30 11:59:50 
//功能说明：渠道商Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Customer
{
    public class Channel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客户邮箱
        /// </summary>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDateTime { get; set; }

        /// <summary>
        /// 最后下单时间
        /// </summary>
        public DateTime? LastOrderDateTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string AdminUser { get; set; }


    }
}
