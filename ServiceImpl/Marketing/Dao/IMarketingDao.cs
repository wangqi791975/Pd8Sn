//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IMarketingDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 17:59:50 
//功能说明：
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
using Com.Panduo.Entity.Marketing;
using Com.Panduo.Service.Marketing;
using Com.Panduo.Service.Marketing.Criteria;

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{ 
    public interface IMarketingDao:IBaseDao<MarketingPo,int>
    {
        /// <summary>
        /// 存储过程
        /// 购物车和下单页调用：方法里要计算当前金额对Club免运费、VIP折扣、订单折扣等活动的匹配度
        /// 具体为 凑单需满足的金额*60% ≤ 当前金额 ＜ 凑单需满足的金额 时， 才显示
        /// 1.club
        /// 2.订单折扣
        /// 3.VIP
        /// </summary>
        PiecingOrderResult GetPiecingOrderInfo(PiecingOrderCriteria criteria);

        /// <summary>
        /// 修改活动状态
        /// </summary>
        /// <param name="marketingId">活动Id</param>
        /// <param name="status">状态</param>
        void UpdateMarketingStatus(int marketingId, bool status);
    } 
}
   