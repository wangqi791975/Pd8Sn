//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：OrderDao.cs
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
using Com.Panduo.Common;
using Com.Panduo.Entity.Order;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{
    public class OrderDao : BaseDao<OrderPo, int>, IOrderDao
    {
        public OrderPo GetOrderByOrderNo(string orderNo)
        {
            return GetOneObject("from OrderPo where OrderNumber = ?", orderNo);
        }

        public bool IsCustomerOrder(int customerId, int orderId)
        {
            return GetOneObject("from OrderPo where CustomerId=? and OrderId=?", new object[] {customerId, orderId}) !=
                   null;
        }

        public IDictionary<int, int> GetOrderStatusCountByCustomerId(int customerId)
        {
            var dic = new Dictionary<int, int>();
            var parms = new[]
            {
                new SqlParameter("@customerId", SqlDbType.Int) {Value = customerId},
            };
            using (
                var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure,
                    "up_order_status_qty_get", parms))
            {
                while (reader.Read())
                {
                    dic.Add(reader.IsDBNull(0) ? 0 : reader.GetInt32(0), reader.IsDBNull(1) ? 0 : reader.GetInt32(1));
                }
            }
            return dic;
        }

        public PageData<Service.Order.Order> FindOrders(int currentPage, int pageSize,
            IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<Service.Order.Order>();
            var dataList = new List<Service.Order.Order>();
            var rowCount = 0;

            //设置查询提交
            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@pageIndex", SqlDbType.Int) {Value = currentPage},
                new SqlParameter("@pageSize", SqlDbType.Int) {Value = pageSize},
                new SqlParameter("@customerId", SqlDbType.Int)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.CustomerId)
                },
                new SqlParameter("@ordernumber", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.OrderNo).ToSqlString()
                },
                new SqlParameter("@partno", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.PartNo).ToSqlString()
                },
                new SqlParameter("@orderstatus", SqlDbType.Int)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.OrderStatus)
                },
                new SqlParameter("@createDateFrom", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.OrderDateFrom)
                },
                new SqlParameter("@createDateTo", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.OrderDateTo)
                },
                new SqlParameter("@sortField", SqlDbType.VarChar, 100) {Value = string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.VarChar, 10) {Value = "ASC"}
            };

            //设置排序条件(暂时不需要提供)
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case OrderSorterCriteria.OrderTime:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "AddDate";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc
                                ? "ASC"
                                : "DESC";
                            break;
                    }
                }
            }

            Service.Order.Order order;
            using (
                var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure,
                    "up_order_search", parmsList.ToArray()))
            {
                //数据条数
                if (reader.Read())
                {
                    rowCount = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
                reader.NextResult();

                //分页数据 
                while (reader.Read())
                {
                    order = new Service.Order.Order
                    {
                        OrderId = reader["order_id"].ParseTo<int>(),
                        CustomerId = reader["customer_id"].ParseTo<int>(),
                        OrderNo = reader["order_number"].ParseTo<string>(),
                        ShippingId = reader["shipping_id"].ParseTo<int>(),
                        PaymentMethod = reader["payment_id"].ParseTo<int>(),
                        ExchangeRate = reader["exchange_rate"].ParseTo<decimal>(),
                        Currency = reader["currency_code"].ParseTo<string>(),
                        OrderStatus = reader["order_status"].ParseTo<int>().ToEnum<OrderStatusType>(),
                        PaidStatus = reader["pay_status"].ParseTo<int>().ToEnum<PaidStatusType>(),
                        IsUpgradeShipping = reader["is_upgrade_shipping_method"].ParseTo<bool>(),
                        //,[upgrade_shipping_method_id]
                        //,[upgrade_shipping_method_money]
                        ReportMoney = reader["report_product_money"].ParseTo<decimal>(),
                        //,[is_report_money_and_shipping]
                        CustomerTaxNumber = reader["tax_number"].ParseTo<string>(),
                        SoldWaitType = reader["out_of_stock_wait_type"].ParseTo<int>(),
                        OrderType = reader["order_type"].ParseTo<int>(),
                        Weight = reader["weight"].ParseTo<decimal>(),
                        ShippingWeight = reader["shipping_weight"].ParseTo<decimal>(),
                        OrderTime = reader["order_time"].ParseTo<DateTime>(),
                        PaidTime = reader["last_pay_time"].ParseTo<DateTime>(),
                        OrderSource = reader["order_source"].ParseTo<int>(),
                        OrderRemark = reader["order_remark"].ParseTo<string>(),
                        GiftLevel = reader["gift_level"].ParseTo<string>(),
                        IsReviewAll = reader["is_review_all"].ParseTo<bool>(),
                        CollectType = reader["collect_type"].ParseTo<int>(),
                        LanguageCode = reader["language_code"].ParseTo<string>(),
                        CustomerTaxType = reader["tax_type"].ParseTo<string>(),
                    };
                    dataList.Add(order);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }

        public PageData<Service.Order.Order> GetAdminOrdersList(int currentPage, int pageSize, IDictionary<OrderSearchCriteria, object> searchCriteria, IList<Sorter<OrderSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<Service.Order.Order>();
            var dataList = new List<Service.Order.Order>();
            var rowCount = 0;

            //设置查询提交
            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@pageIndex", SqlDbType.Int) {Value = currentPage},
                new SqlParameter("@pageSize", SqlDbType.Int) {Value = pageSize},
                new SqlParameter("@customer", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.Customer).ToSqlString()
                },
                new SqlParameter("@languageCode", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.LanguageCode).ToSqlString()
                },
                new SqlParameter("@ordersource", SqlDbType.Int)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.OrderSource)
                },
                new SqlParameter("@paymentId", SqlDbType.Int)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.PaymentMethod)
                },
                new SqlParameter("@shippingId", SqlDbType.Int)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.ShippingMethod)
                },
                new SqlParameter("@ordernumber", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.OrderNo).ToSqlString()
                },
                new SqlParameter("@orderstatus", SqlDbType.Int)
                {
                    Value = searchCriteria.TryGet(OrderSearchCriteria.OrderStatus)
                },
                new SqlParameter("@sortField", SqlDbType.VarChar, 100) {Value = string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.VarChar, 10) {Value = "ASC"}
            };

            //设置排序条件(暂时不需要提供)
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case OrderSorterCriteria.OrderTime:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "AddDate";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc
                                ? "ASC"
                                : "DESC";
                            break;
                    }
                }
            }

            Service.Order.Order order;
            using (
                var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure,
                    "up_admin_order_search", parmsList.ToArray()))
            {
                //数据条数
                if (reader.Read())
                {
                    rowCount = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
                reader.NextResult();

                //分页数据 
                while (reader.Read())
                {
                    order = new Service.Order.Order
                    {
                        OrderId = reader["order_id"].ParseTo<int>(),
                        CustomerId = reader["customer_id"].ParseTo<int>(),
                        OrderNo = reader["order_number"].ParseTo<string>(),
                        ShippingId = reader["shipping_id"].ParseTo<int>(),
                        PaymentMethod = reader["payment_id"].ParseTo<int>(),
                        ExchangeRate = reader["exchange_rate"].ParseTo<decimal>(),
                        Currency = reader["currency_code"].ParseTo<string>(),
                        OrderStatus = reader["order_status"].ParseTo<int>().ToEnum<OrderStatusType>(),
                        PaidStatus = reader["pay_status"].ParseTo<int>().ToEnum<PaidStatusType>(),
                        IsUpgradeShipping = reader["is_upgrade_shipping_method"].ParseTo<bool>(),
                        //,[upgrade_shipping_method_id]
                        //,[upgrade_shipping_method_money]
                        ReportMoney = reader["report_product_money"].ParseTo<decimal>(),
                        //,[is_report_money_and_shipping]
                        CustomerTaxNumber = reader["tax_number"].ParseTo<string>(),
                        SoldWaitType = reader["out_of_stock_wait_type"].ParseTo<int>(),
                        OrderType = reader["order_type"].ParseTo<int>(),
                        Weight = reader["weight"].ParseTo<decimal>(),
                        ShippingWeight = reader["shipping_weight"].ParseTo<decimal>(),
                        OrderTime = reader["order_time"].ParseTo<DateTime>(),
                        PaidTime = reader["last_pay_time"].ParseTo<DateTime>(),
                        OrderSource = reader["order_source"].ParseTo<int>(),
                        OrderRemark = reader["order_remark"].ParseTo<string>(),
                        GiftLevel = reader["gift_level"].ParseTo<string>(),
                        IsReviewAll = reader["is_review_all"].ParseTo<bool>(),
                        CollectType = reader["collect_type"].ParseTo<int>(),
                        LanguageCode = reader["language_code"].ParseTo<string>(),
                        CustomerTaxType = reader["tax_type"].ParseTo<string>(),
                    };
                    dataList.Add(order);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }

        public OrderPo GetCustomerLatestOrder(int customerId, Sorter<LatestOrderSorterCriteria> critria)
        {
            //设置查询提交
            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@customerId", SqlDbType.Int) {Value = customerId},
                new SqlParameter("@sortField", SqlDbType.VarChar, 100) {Value = string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.VarChar, 10) {Value = "ASC"}
            };
            //设置排序条件(暂时不需要提供)
            if (critria != null)
            {

                switch (critria.Key)
                {
                    case LatestOrderSorterCriteria.OrderTime:
                        parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "order_time";
                        parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = critria.IsAsc
                            ? "ASC"
                            : "DESC";
                        break;
                }
            }

            OrderPo order=new OrderPo();
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_last_order_get", parmsList.ToArray()))
            {
                if (reader.Read())
                {
                    order = new OrderPo
                    {
                        OrderId = reader["order_id"].ParseTo<int>(),
                        CustomerId = reader["customer_id"].ParseTo<int>(),
                        ShippingId = reader["shipping_id"].ParseTo<int>(),
                        ExchangeRate = reader["exchange_rate"].ParseTo<decimal>(),
                        TaxNumber = reader["tax_number"].ParseTo<string>(),
                        OrderType = reader["order_type"].ParseTo<int>(),
                        Weight = reader["weight"].ParseTo<decimal>(),
                        ShippingWeight = reader["shipping_weight"].ParseTo<decimal>(),
                        OrderTime = reader["order_time"].ParseTo<DateTime>(),
                        OrderSource = reader["order_source"].ParseTo<int>(),
                        OrderRemark = reader["order_remark"].ParseTo<string>(),
                        GiftLevel = reader["gift_level"].ParseTo<string>(),
                        IsReviewAll = reader["is_review_all"].ParseTo<bool>(),
                        CollectType = reader["collect_type"].ParseTo<int>(),
                        LanguageCode = reader["language_code"].ParseTo<string>()

                    };
                }

            }
            return order;
        }
    }
}
   