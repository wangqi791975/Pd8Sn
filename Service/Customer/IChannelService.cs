//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：IChannelService.cs
//创 建 人：罗海明
//创建时间：2015/01/30 11:59:40 
//功能说明：渠道商接口
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;

namespace Com.Panduo.Service.Customer
{
   public interface IChannelService
   {
       /// <summary>
       /// 客户已经是club会员
       /// </summary>
       string ERROR_CUSTOMER_IS_CLUB { get; }
      

       /// <summary>
       /// 渠道商已经存在
       /// </summary>
       string ERROR_CUSTOMER_EXIST { get; }
       

        /// <summary>
        /// 客户邮箱不存在
        /// </summary>
       string ERROR_CUSTOMER_EMAIL_NOT_EXIST { get; }

       /// <summary>
       /// 添加渠道商
       /// </summary>
       /// <param name="email">客户邮箱</param>
       /// <param name="adminId">操作人Id</param>
       void AddChannel(string email, int adminId);

       /// <summary>
       /// 删除渠道商
       /// </summary>
       /// <param name="customerId">客户Id</param>
       /// <param name="adminId">操作人Id</param>
       void DeleteChannelByCustomerId(int customerId,int adminId);

       /// <summary>
       /// 删除渠道商
       /// </summary>
       /// <param name="id">表Id</param>
       /// <param name="adminId">操作人Id</param>
       void DeleteChannelById(int id,int adminId);

       /// <summary>
       /// 根据分页获取该渠道商列表
       /// </summary>
       /// <param name="currentPage">当前页码</param>
       /// <param name="pageSize">分页大小</param>
       /// <param name="searchCriteria">渠道商查询条件</param>
       /// <param name="sorterCriteria">渠道商排序条件</param>
       /// <returns>包含分页的渠道商列表</returns>
       PageData<Channel> GetChannel(int currentPage, int pageSize, IDictionary<ChannelSearchCriteria, object> searchCriteria, IList<Sorter<ChannelSorterCriteria>> sorterCriteria);



   }

    /// <summary>
    /// 渠道商查询条件
    /// </summary>
   public enum ChannelSearchCriteria
   {

   }

    /// <summary>
    /// 渠道商排序条件
    /// </summary>
   public enum ChannelSorterCriteria
   {

   }
    
}
