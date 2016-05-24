//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ChannelService.cs
//创 建 人：罗海明
//创建时间：2015/01/30 13:40:40 
//功能说明：渠道商接口实现服务
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.ServiceImpl.Customer.Dao;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl.Customer
{
  public  class ChannelService:IChannelService
    {
      public IChannelDao ChannelDao { get; private set; }
      public ICustomerDao CustomerDao { get; private set; }

      public string ERROR_CUSTOMER_IS_CLUB
      {
          get { return "ERROR_CUSTOMER_IS_CLUB"; }
      }

      public string ERROR_CUSTOMER_EMAIL_NOT_EXIST
      {
          get { return "ERROR_CUSTOMER_EMAIL_NOT_EXIST"; }
      }

      public string ERROR_CUSTOMER_EXIST
      {
          get { return "ERROR_CUSTOMER_EXIST"; }
      }

      public void AddChannel(string email, int adminId)
      {
          var c = CustomerDao.GetCustomerByEmail(email);
          if (!c.IsNullOrEmpty())
          {
              if (ChannelDao.IsExists(c.CustomerId))
              {
                  throw new BussinessException(ERROR_CUSTOMER_EXIST);
              }
              else if (c.ClubLevel != 0)
              {
                  throw new BussinessException(ERROR_CUSTOMER_IS_CLUB);
              }
              else
              {
                  var po = new ChannelPo() {CustomerId = c.CustomerId, AdminId = adminId, DateCreated = DateTime.Now};
                  ChannelDao.AddObject(po);
              }
          }
          else
          {
              throw new BussinessException(ERROR_CUSTOMER_EMAIL_NOT_EXIST);
          }        
      }


      public void DeleteChannelByCustomerId(int customerId, int adminId)
      {
          ChannelDao.DeleteChannelByCustomerId(customerId);
      }

      public void DeleteChannelById(int id, int adminId)
      {
          ChannelDao.DeleteObjectById(id);
      }

      public PageData<Channel> GetChannel(int currentPage, int pageSize, IDictionary<ChannelSearchCriteria, object> searchCriteria, IList<Sorter<ChannelSorterCriteria>> sorterCriteria)
      {
          return ChannelDao.GetChannel(currentPage, pageSize, searchCriteria, sorterCriteria);
      }
    }
}
