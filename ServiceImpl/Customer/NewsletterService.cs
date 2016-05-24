using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Customer;
using Com.Panduo.Service.Customer;
using Com.Panduo.ServiceImpl.Customer.Dao;

namespace Com.Panduo.ServiceImpl.Customer
{
    public class NewsletterService : INewsletterService
    {
        public ISubscribeDao SubscribeDao { private get; set; }
        public ICustomerDao CustomerDao { private get; set; }

        public string ERROR_CUSTOMER_NOT_EXIST
        {
            get { return "ERROR_CUSTOMER_NOT_EXIST"; }
        }

        public void Subscribe(Newsletter newsletter)
        {
            var subscribeId = SubscribeDao.GetSubscribeId(newsletter.LanguageId, newsletter.Email);
            if (subscribeId == 0)
            {
                SubscribeDao.AddObject(GetSubscribePoFromVo(newsletter));
            }
        }

        public void Subscribe(List<Newsletter> newsletters)
        {
            SubscribeDao.AddObjects(newsletters.Select(GetSubscribePoFromVo));
        }

        public void UnSubscribe(string email)
        {
            throw new System.NotImplementedException();
        }

        public Newsletter GetNewsletter(int customerId)
        {
            Newsletter newsletter = null;
            var customer = CustomerDao.GetObject(customerId);
            if (!customer.IsNullOrEmpty())
            {
                newsletter = GetSubscribeVoFromPo(SubscribeDao.GetSubscribe(customer.CustomerEmail));
            }
            return newsletter;
        }

        #region 辅助方法

        internal static SubscribePo GetSubscribePoFromVo(Newsletter newsletter)
        {
            SubscribePo subscribePo = null;
            if (!newsletter.IsNullOrEmpty())
            {
                subscribePo = new SubscribePo
                {
                    Id = newsletter.Id,
                    FullName = newsletter.FullName,
                    CustomerId = newsletter.CustomerId, //游客Id为0
                    Email = newsletter.Email,
                    DateCreated = newsletter.NewsletterDateTime,
                    Status = newsletter.IsUnNewsletter,
                    LanguageId = newsletter.LanguageId
                };
            }
            return subscribePo;
        }

        internal static Newsletter GetSubscribeVoFromPo(SubscribePo subscribe)
        {
            Newsletter newsletter = null;
            if (!subscribe.IsNullOrEmpty())
            {
                newsletter = new Newsletter
                {
                    Id = subscribe.Id,
                    FullName = subscribe.FullName,
                    CustomerId = subscribe.CustomerId, //游客Id为0
                    Email = subscribe.Email,
                    NewsletterDateTime = subscribe.DateCreated,
                    IsUnNewsletter = subscribe.Status,
                    LanguageId = subscribe.LanguageId
                };
            }
            return newsletter;
        }
        #endregion
    }
}