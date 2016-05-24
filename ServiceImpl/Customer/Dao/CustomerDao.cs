//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CustomerDao.cs
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
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public class CustomerDao : BaseDao<CustomerPo, int>, ICustomerDao
    {
        public int GetCustomerCount(int customerId)
        {
            var obj = GetSingleObject("SELECT COUNT(CustomerId) FROM CustomerPo WHERE CustomerId = ?", customerId);
            return obj == null ? 0 : int.Parse(obj.ToString());
        }

        public int GetRegisterCount(string ip)
        {
            var obj = GetSingleObject("SELECT COUNT(CustomerId) FROM CustomerPo WHERE RegisterIp = ? AND DateCreated >= CONVERT(VARCHAR(20),GETDATE(),23) AND DateCreated < CONVERT(VARCHAR(20),GETDATE(),23) + ' 23:59:59'", ip);
            return obj == null ? 0 : int.Parse(obj.ToString());
        }

        public CustomerPo GetCustomerByEmail(string email)
        {
            return GetOneObject("FROM CustomerPo WHERE CustomerEmail = ?", email);
        }

        public PageData<CustomerPo> FindAllCustomers(int currentPage, int pageSize)
        {
            var hqlHelper = new HqlHelper("Select c from CustomerPo c");
            hqlHelper.AddSorter("c.CustomerId", false);
            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }

        public CustomerPo GetCustomerByFacebookId(string facebookId)
        {
            return GetOneObject("FROM CustomerPo WHERE FacebookId = ?", facebookId);
        }

        public void AddFacebookInfo(FacebookInfo faceboolInfo)
        {
            UpdateObjectByHql("UPDATE CustomerPo SET FacebookId = ?,DateFacebook = ? WHERE CustomerId = ?", new object[] { faceboolInfo.FaceBookId, faceboolInfo.CreateDateTime, faceboolInfo.CustomerId });
        }

        public void UpdateCustomerEmail(int customerId, string email)
        {
            UpdateObjectByHql("UPDATE CustomerPo SET CustomerEmail = ? WHERE CustomerId = ?", new object[] { email, customerId });
        }

        public void UpdateCustomerStatus(int customerId, bool status)
        {
            UpdateObjectByHql("UPDATE CustomerPo SET Status = ? WHERE CustomerId = ?", new object[] { status, customerId });
        }

        public void UpDateCustomerAvatar(int customerId, string avatarPath)
        {
            UpdateObjectByHql("UPDATE CustomerPo SET Avatar = ? WHERE CustomerId = ?", new object[] { avatarPath, customerId });
        }

        public void UpdateCustomerPassword(int customerId, string newPassword)
        {
            UpdateObjectByHql("UPDATE CustomerPo SET Password = ? WHERE CustomerId = ?", new object[] { newPassword, customerId });
        }

        public void UpdateShippingDefaultAddress(int customerId, int shippingDefaultAddress)
        {
            UpdateObjectByHql("UPDATE CustomerPo SET DefaultShippingAddress = ? WHERE CustomerId = ?", new object[] { shippingDefaultAddress, customerId });
        }

        public void UpdateBillingDefaultAddress(int customerId, int billingDefaultAddress)
        {
            UpdateObjectByHql("UPDATE CustomerPo SET DefaultBillingAddress = ? WHERE CustomerId = ?", new object[] { billingDefaultAddress, customerId });
        }

        public void UpdateCustomerLogin(int customerId, int loginCount)
        {
            UpdateObjectByHql("UPDATE CustomerPo SET TotalLoginCount = ?, DateLogin = ? WHERE CustomerId = ?", new object[] { loginCount, DateTime.Now, customerId });
        }
    }
}
