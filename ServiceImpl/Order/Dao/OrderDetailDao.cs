//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderDetailDao.cs
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
using Com.Panduo.Common;
using Com.Panduo.Entity.Order;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{ 
    public class OrderDetailDao:BaseDao<OrderDetailPo,int>, IOrderDetailDao
    {
        public bool IsBuyProduct(int customerId, int productId)
        {
            //todo 判断该客户购买状态必须为是否为“待评价状态”
            throw new NotImplementedException();
        }

        public PageData<OrderDetailPo> GetOrderDetsById(int orderId, int currentPage, int pageSize, IDictionary<OrderDetailSearchCriteria, object> searchCriteria)
        {
            var hqlHelper = new HqlHelper("Select D from OrderDetailPo D");
            hqlHelper.AddWhere("D.OrderId", HqlOperator.Eq, "OrderId", orderId);
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case OrderDetailSearchCriteria.IsReviewed:
                            hqlHelper.AddWhere("D.IsReviewed", HqlOperator.Eq, "IsReviewed", item.Value);
                            break;
                    }
                }
            }
            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }

        public OrderDetailPo GetOrderDetsById(int orderDetsId)
        {
            return GetOneObject("from OrderDetailPo where OrderProductId = ?", orderDetsId);
        }

        public IDictionary<int, int> GetOrderDetsCountByOrderId(int orderId)
        {
            var dic = new Dictionary<int, int>();
            var parms = new[] {
                                new SqlParameter("@orderId", SqlDbType.Int){Value =  orderId}, 
                              };
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING,CommandType.StoredProcedure, "up_orderdetail_status_qty_get", parms))
            {
                while (reader.Read())
                {
                    dic.Add(reader.IsDBNull(0) ? 0 : reader.GetInt32(0),reader.IsDBNull(1) ? 0 : reader.GetInt32(1));
                }
            }
            return dic;
        }
    } 
}
   