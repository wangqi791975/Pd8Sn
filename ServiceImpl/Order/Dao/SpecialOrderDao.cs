//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：SpecialOrderDao.cs
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
using Com.Panduo.Common;
using Com.Panduo.Entity.Order;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{
    public class SpecialOrderDao : BaseDao<SpecialOrderPo, int>, ISpecialOrderDao
    {
        public PageData<SpecialOrder> GetSpecialOrder(int currentPage, int pageSize, IDictionary<SpecialOrderSearchCriteria, object> searchCriteria, IList<Sorter<SpecialOrderSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<SpecialOrder>();
            var dataList = new List<SpecialOrder>();
            var rowCount = 0;

            //设置查询提交
            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@pageIndex", SqlDbType.Int) {Value = currentPage},
                new SqlParameter("@pageSize", SqlDbType.Int) {Value = pageSize},
                new SqlParameter("@sortField", SqlDbType.VarChar, 100) {Value = string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.VarChar, 10) {Value = "ASC"}
            };

            //设置排序条件
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case SpecialOrderSorterCriteria.CreateDate:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "AddDate";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc
                                ? "ASC"
                                : "DESC";
                            break;
                    }
                }
            }

            SpecialOrder specialOrder;
            using (
                var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure,
                    "up_specialorder_search", parmsList.ToArray()))
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
                    specialOrder = new SpecialOrder
                    {
                        SpecialOrderId = reader["special_id"].ParseTo<int>(),
                        CustomerId = reader["customer_id"].ParseTo<int>(),
                        CustomerName = reader["full_name"].ParseTo<string>(),
                        CustomerMail = reader["customer_email"].ParseTo<string>(),
                        Increase = reader["increase_money"].ParseTo<decimal>(),
                        CurrencyCode = reader["currency_code"].ParseTo<string>(),
                        Remark = reader["remark"].ParseTo<string>(),
                        IsNotifyCustomer = reader["is_notify_customer"].ParseTo<bool>(),
                        Status = reader["status"].ParseTo<int>(),
                        CreateTime = reader["date_created"].ParseTo<DateTime>(),
                        Creator = reader["admin_id"].ParseTo<int>(),
                        CreateAccount = reader["account_email"].ParseTo<string>(),
                    };
                    dataList.Add(specialOrder);
                }
            }
            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }
    }
}
