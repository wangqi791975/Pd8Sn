//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：PackageDetail.cs
//创 建 人：罗海明
//创建时间：2014/12/20 14:49:50 
//功能说明：包裹明细Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order
{
   public class PackageDetail
    {
        /// <summary>
        /// 包裹id
        /// </summary>
        public virtual int PackageId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品id
        /// </summary>
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductModel
        {
            get;
            set;
        }
        /// <summary>
        /// 购买数量
        /// </summary>
        public virtual int ProductQty
        {
            get;
            set;
        }
        /// <summary>
        /// 已发货数量
        /// </summary>
        public virtual int TotalShipped
        {
            get;
            set;
        }
        /// <summary>
        /// 发货数量
        /// </summary>
        public virtual int ShippedQty
        {
            get;
            set;
        }

        public virtual int OrderId
        {
            get;
            set;
        }
    }
}
