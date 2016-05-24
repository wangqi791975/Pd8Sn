//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderStatusHistoryDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 18:08:40 
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
using Com.Panduo.Entity.Order;
using Com.Panduo.Service.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{
    public class OrderStatusHistoryDao : BaseDao<OrderStatusHistoryPo, int>, IOrderStatusHistoryDao
    {
        public IList<OrderStatusHistory> GetOrderStatusHistoryByOrderId‎(int orderId, int languageId)
        {
            var list = new List<OrderStatusHistory>();

            OrderStatusHistory orderStatusHistory = null;
            var parms = new[] { 
                                new SqlParameter("@languageId", SqlDbType.Int){Value =  languageId}, 
                                new SqlParameter("@orderId", SqlDbType.Int){Value =  orderId}, 
                              };
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_order_status_history_get", parms))
            {
                while (reader.Read())
                {
                    orderStatusHistory = new OrderStatusHistory
                        {
                            Id = reader["Id"].ParseTo<int>(),
                            Comments = reader["comments"].ParseTo<string>(),
                            ChangeDate = reader["date_updated"].ParseTo<DateTime>(),
                            OrderId = orderId,
                            StatusName = reader["orders_status_name"].ParseTo<string>(),
                            Status = reader["status"].ParseTo<int>(),
                            NotifyCustomer = reader["notify_customer"].ParseTo<bool>()
                        };
                    list.Add(orderStatusHistory);
                }
            }
            return list;
        }


        public IList<OrderStatusHistory> GetOrderAdminStatusHistoryByOrderId‎(int orderId)
        {
            var list = new List<OrderStatusHistory>();

            OrderStatusHistory orderStatusHistory = null;
            var parms = new[] { 
                                new SqlParameter("@orderId", SqlDbType.Int){Value =  orderId}, 
                              };
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_admin_order_status_history_get", parms))
            {
                while (reader.Read())
                {
                    orderStatusHistory = new OrderStatusHistory
                    {
                        Id = reader["Id"].ParseTo<int>(),
                        Comments = reader["comments"].ParseTo<string>(),
                        ChangeDate = reader["date_updated"].ParseTo<DateTime>(),
                        OrderId = orderId,
                        StatusName = reader["status_name"].ParseTo<string>(),
                        Status = reader["status"].ParseTo<int>(),
                        NotifyCustomer = reader["notify_customer"].ParseTo<bool>()
                    };
                    list.Add(orderStatusHistory);
                }
            }
            return list;
        }
    }
}
