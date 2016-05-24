//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ProductAlsoboughtDao.cs
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

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public class ProductAlsoboughtDao:BaseDao<ProductAlsoboughtPo,int>, IProductAlsoboughtDao
    {
        public IList<ProductAlsoboughtPo> GetProductAlsobought(int productId, int topn)
        {
            return FindDataByHqlLimit("from ProductAlsoboughtPo   where  ProductId=?", topn,productId);
        }
    } 
}
   