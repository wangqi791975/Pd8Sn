//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderPriceDao.cs
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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.ShoppingCart;
using Com.Panduo.Service.Order.ShoppingCart;

namespace Com.Panduo.ServiceImpl.Order.ShoppingCart.Dao
{
    public class VShoppingCartDao : BaseDao<VShoppingCartPo, int>, IVShoppingCartDao
    {
    }
}
