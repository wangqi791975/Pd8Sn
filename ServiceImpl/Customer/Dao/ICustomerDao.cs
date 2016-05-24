//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
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
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{ 
    public interface ICustomerDao:IBaseDao<CustomerPo,int>
    {
        /// <summary>
        /// 通过客户Id获取客户数量
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>客户数量</returns>
        int GetCustomerCount(int customerId);

        /// <summary>
        /// 获取当天当前IP地址注册的客户数
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>注册数</returns>
        int GetRegisterCount(string ip);

        /// <summary>
        /// 通过邮箱获取客户
        /// </summary>
        /// <param name="email"></param>
        /// <returns>客户</returns>
        CustomerPo GetCustomerByEmail(string email);

        /// <summary>
        /// 查询数据库得到所有客户
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        PageData<CustomerPo> FindAllCustomers(int currentPage, int pageSize);

        /// <summary>
        /// 通过facebookId获取客户
        /// </summary>
        /// <param name="facebookId">facebookId</param>
        /// <returns>客户</returns>
        CustomerPo GetCustomerByFacebookId(string facebookId);

        /// <summary>
        /// 添加facebook信息
        /// </summary>
        /// <param name="faceboolInfo">facebook信息</param>
        void AddFacebookInfo(FacebookInfo faceboolInfo);

        /// <summary>
        /// 修改客户邮箱
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="email"></param>
        void UpdateCustomerEmail(int customerId, string email);

        /// <summary>
        /// 修改客户状态
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="status">客户状态</param>
        void UpdateCustomerStatus(int customerId, bool status);

        /// <summary>
        /// 修改客户头像
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="avatarPath"></param>
        void UpDateCustomerAvatar(int customerId, string avatarPath);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="newPassword"></param>
        void UpdateCustomerPassword(int customerId, string newPassword);

        /// <summary>
        /// 修改客户默认货运地址
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="shippingDefaultAddress">地址Id</param>
        void UpdateShippingDefaultAddress(int customerId, int shippingDefaultAddress);

        /// <summary>
        /// 修改客户默认账单地址
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="billingDefaultAddress">地址Id</param>
        void UpdateBillingDefaultAddress(int customerId, int billingDefaultAddress);

        /// <summary>
        /// 修改客户登陆信息（登陆次数，登陆时间）
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="loginCount">登陆次数</param>
        void UpdateCustomerLogin(int customerId, int loginCount);
    } 
}
   