//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：PackageDetailDao.cs
//创 建 人：罗海明
//创建时间：2015/02/10 13:59:50 
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
    public class PackageDetailDao : BaseDao<PackageDetailPo, int>, IPackageDetailDao
    {
        public PageData<PackageDetail> FindOrderPackages(int currentPage, int pageSize, IDictionary<PackageSearchCriteria, object> searchCriteria, IList<Sorter<PackageSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<PackageDetail>();
            var dataList = new List<PackageDetail>();
            var rowCount = 0;

            //设置查询提交
            var parmsList = new List<SqlParameter>
                                {
                                    new SqlParameter("@pageIndex", SqlDbType.Int){Value =  currentPage}, 
                                    new SqlParameter("@pageSize", SqlDbType.Int){Value =  pageSize},
                                    new SqlParameter("@customerId",SqlDbType.Int){Value =  searchCriteria.TryGet(PackageSearchCriteria.CustomerId)},
                                    new SqlParameter("@packingno", SqlDbType.VarChar,20){Value =  searchCriteria.TryGet(PackageSearchCriteria.PackingNo).ToSqlString()},
                                    new SqlParameter("@orderno", SqlDbType.VarChar,20){Value =  searchCriteria.TryGet(PackageSearchCriteria.OrderNo).ToSqlString()}, 
                                    new SqlParameter("@partno", SqlDbType.VarChar,20){Value =  searchCriteria.TryGet(PackageSearchCriteria.PartNo).ToSqlString()},
                                    new SqlParameter("@trackingnumber", SqlDbType.VarChar,20){Value =  searchCriteria.TryGet(PackageSearchCriteria.TrackingNumber).ToSqlString()},
                                    new SqlParameter("@sortField", SqlDbType.VarChar,100){Value =  string.Empty},
                                    new SqlParameter("@sortDirecton", SqlDbType.VarChar,10){Value =  "ASC"}
                                };

            //设置排序条件(暂时不需要提供)
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case PackageSorterCriteria.PackageId:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "packageid";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc ? "ASC" : "DESC";
                            break;
                    }
                }
            }

            PackageDetail packageDetail;
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_package_slip_search", parmsList.ToArray()))
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
                    packageDetail = new PackageDetail
                    {
                        OrderId = reader["order_id"].ParseTo<int>(),
                        ProductQty = reader["product_qty"].ParseTo<int>(),
                        TotalShipped = reader["total_shipped"].ParseTo<int>(),
                        ShippedQty = reader["shipped_qty"].ParseTo<int>(),
                        ProductModel = reader["product_model"].ParseTo<string>(),
                        ProductId = reader["product_id"].ParseTo<int>(),
                    };
                    dataList.Add(packageDetail);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }
    } 
}
   