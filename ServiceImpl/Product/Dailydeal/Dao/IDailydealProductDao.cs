//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IDailydealProductDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
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
using Com.Panduo.Entity.Product.Dailydeal;

namespace Com.Panduo.ServiceImpl.Product.Dailydeal.Dao
{ 
    public interface IDailydealProductDao:IBaseDao<DailydealProductPo,int>
    {
        IList<DailydealProductPo> GetDailydealProductsByProductIds(string productIds);

        DailydealProductPo GetDailydealProduct(int productId);

        bool IsProductDailyDeal(int productId);
    } 
}
   