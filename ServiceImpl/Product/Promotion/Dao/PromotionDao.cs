//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：PromotionDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
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
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Promotion.Dao
{ 
    public class PromotionDao:BaseDao<PromotionPo,int>, IPromotionDao
    {

        public PromotionPo GetPromotionAreaById(int promotionId)
        {
            return GetOneObject("from PromotionPo where PromotionId = ?", promotionId);
        }
    } 
}
   