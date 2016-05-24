//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ChannelDao.cs
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
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{ 
    public class ChannelDao:BaseDao<ChannelPo,int>, IChannelDao
    {
        public void DeleteChannelByCustomerId(int customerId)
        {
            DeleteObjectByHql("delete from ChannelPo where CustomerId=?", customerId);
        }

        public bool IsExists(int customerId)
        {
            return GetOneObject("from ChannelPo where CustomerId=?", new object[] { customerId }) != null;
        }

        public PageData<Channel> GetChannel(int currentPage, int pageSize, IDictionary<ChannelSearchCriteria, object> searchCriteria, IList<Sorter<ChannelSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<Channel>();
            var dataList = new List<Channel>();
            var rowCount = 0;
            Channel channel = null;

            var parmsList = new List<SqlParameter> {
                                  new SqlParameter("@pageIndex", SqlDbType.Int){Value =  currentPage}, 
                                  new SqlParameter("@pageSize", SqlDbType.Int){Value =  pageSize},
                              };

            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_channel_search", parmsList.ToArray()))
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
                    channel = new Channel();
                    channel.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    channel.CustomerId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    channel.CustomerEmail = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                    channel.AddDateTime = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                    channel.LastOrderDateTime = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4);
                    channel.AdminUser = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                    dataList.Add(channel);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }
    } 
}
   