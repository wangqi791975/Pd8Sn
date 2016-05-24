//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderStatusDescriptionDao.cs
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
using Com.Panduo.Entity.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{
    public class OrderStatusDescriptionDao : BaseDao<OrderStatusDescriptionPo, OrderStatusDescriptionPk>, IOrderStatusDescriptionDao
    {

        public IList<OrderStatusDescriptionPo> GetAdminOrderStatusDesc(int languageId)
        {
            return FindDataByHql("from OrderStatusDescriptionPo where Id.LanguageId=?", languageId);
        }

        public IList<OrderStatusDescriptionPo> GetAllOrderStatusDesc()
        {
            IList<OrderStatusDescriptionPo> list=new List<OrderStatusDescriptionPo>();
            OrderStatusDescriptionPo po= null;
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_order_status_get"))
            {
                while (reader.Read())
                {
                    po = new OrderStatusDescriptionPo();
                    po.Id=new OrderStatusDescriptionPk();
                    po.Id.OrderStatus = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    po.OrdersStatusName= reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    po.Id.LanguageId  = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    list.Add(po);
                }
            }

            return list;
        }

        public IList<OrderStatusDescriptionPo> GetAllCustomerOrderStatusDesc(int languageId)
        {
            IList<OrderStatusDescriptionPo> list = new List<OrderStatusDescriptionPo>();
            OrderStatusDescriptionPo po = null;
            var parms = new[] {
                                new SqlParameter("@languageId", SqlDbType.Int){Value =  languageId}, 
                              };
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_order_status_get", parms))
            {
                while (reader.Read())
                {
                    po = new OrderStatusDescriptionPo();
                    po.Id = new OrderStatusDescriptionPk();
                    po.Id.OrderStatus = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    po.OrdersStatusName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    po.Id.LanguageId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    list.Add(po);
                }
            }

            return list;
        }
    } 
}
   