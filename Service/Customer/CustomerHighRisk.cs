//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：Channel.cs
//创 建 人：万天文
//创建时间：2015/05/03 14:59:50 
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
    public class CustomerHighRisk
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 客户邮箱
        /// </summary>
        public virtual string CustomerEmail
        {
            get;
            set;
        }

        /// <summary>
        /// 管理员ID
        /// </summary>
        public virtual int AdminId
        {
            get;
            set;
        }

        /// <summary>
        /// 客户邮箱
        /// </summary>
        public virtual string AdminEmail
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


    }
}
