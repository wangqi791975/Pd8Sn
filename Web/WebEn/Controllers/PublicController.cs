using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Product.Category;
using Com.Panduo.Web.Common;
using MailChimp.Net.Api;
using MailChimp.Net.Api.Domain;
using Resources;

namespace Com.Panduo.Web.Controllers
{
    public class PublicController : BaseController
    {
        /// <summary>
        /// 客户切换币种
        /// </summary>
        /// <returns></returns>
        public JsonResult ChangeCurrency(FormCollection form)
        {
            var hashtable = new Hashtable {{"result", ActionJsonResult.Failing}, {"msg", string.Empty}};

            try
            {
                var currencyCode = form["currencyCode"] as string;
                if (!currencyCode.IsNullOrEmpty())
                {
                    var currency = CacheHelper.Currencies.Find(x => x.CurrencyCode == currencyCode);
                    if (!currency.IsNullOrEmpty())
                    {
                        SessionHelper.CurrentCurrency = currency;
                        var preference = new Preference
                        {
                            CurrencyId = currency.CurrencyId
                        };
                        var customer = SessionHelper.CurrentCustomer;
                        if (!customer.IsNullOrEmpty())
                        {
                            //TODO: 记录Preference
                            preference.CustomerId = customer.CustomerId;
                            Service.ServiceFactory.CustomerService.SetPreference(preference);
                        }
                        CookieHelper.CurrentCustomerPreference = preference;
                        hashtable["result"] = ActionJsonResult.PageRefresh;
                    }
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }

        /// <summary>
        /// 客户订阅
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public JsonResult Newsletter(FormCollection form)
        {
            var hashtable = new Hashtable {{"result", ActionJsonResult.Failing}, {"msg", string.Empty}};
            try
            {
                var email = form["subscribeEmail"] as string;
                if (email.IsNullOrEmpty())
                {
                    hashtable["result"] = ActionJsonResult.Error;
                    hashtable["msg"] = Lang.TipCheckEmailFormat;
                }
                //TODO: 	成功订阅网站Newsletter后，数据传输到mailchimp，且异步调用mailchimp。
                //系统记录订阅数据时，需要区分语言站点，且对应语言站点订阅的信息传输到mailchimp后需要归入对应的语言分组，以方便业务后期发送对应语言的newsletter。
                //例如:若邮箱A在德语站订阅了网站的newsletter，则该邮箱A属于德语组

                #region 调用MailChimp接口
                IMailChimpApiService mailChimpApiService = new MailChimpApiService(ConfigHelper.MainChimpServiceConfig.ApiKey,
                    ConfigHelper.MainChimpServiceConfig.ServiceUrl, ConfigHelper.MainChimpServiceConfig.HelperRelatedSection,
                    ConfigHelper.MainChimpServiceConfig.ListsRelatedSection);

                var subscribeSources = new Grouping { Name = "Subscribe Source" };
                subscribeSources.Groups.Add("Site");

                var couponsGained = new Grouping { Name = "Coupons Gained" };
                couponsGained.Groups.Add("Coupon");

                var interests = new Grouping { Name = "Interests" };
                interests.Groups.Add("Extreme Games");

                //'ee40659380','f28016111d','bfcdc4a7a6','182e3fa2aa','6b3bd8db5c','b129414321','836445811e'

                var fields = new Dictionary<string, string> { { "GENDER", "Male" }, { "DATEBORN", DateTime.Now.ToString(CultureInfo.InvariantCulture) }, { "CITY", "Athens" }, { "COUNTRY", "Greece" } };

                var response = mailChimpApiService.Subscribe(email, new System.Collections.Generic.List<Grouping>() { subscribeSources, couponsGained, interests }, fields, true);
                //TODO: 处理response
                if (response.IsSuccesful)
                {
                    hashtable["result"] = ActionJsonResult.Success;
                    Console.WriteLine(response.ResponseJson);
                }

                #endregion


                hashtable["result"] = ActionJsonResult.Success;
            }
            catch (Exception ex)
            {
                //记录日志
                hashtable["result"] = ActionJsonResult.Error;
                hashtable["msg"] = ex.Message;
            }
            return Json(hashtable);
        }

        /// <summary>
        /// 搜索框提示
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public JsonResult SearchTip(FormCollection form)
        {
            var hashtable = new Hashtable();
            var query = form["query"] as string;
            //  test
            //for (var i = 0; i <= 10; i++)
            //{
            //    var j = i.ToString();
            //    hashtable.Add(j, query + j);
            //}
            //todo: 加提示内容后面抓取
            return Json(hashtable);
        }

    }
}
