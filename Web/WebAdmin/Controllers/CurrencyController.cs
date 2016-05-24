using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class CurrencyController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index(int? id)
        {
            int remote = id ?? 0;
            ViewBag.Remote = remote;
            return View();
        }

        public ActionResult GetList()
        {
            int remote = Request["remote"].ParseTo(0);
            IList<Currency> currencies = ServiceFactory.ConfigureService.GetAllCurrencies();
            if (remote > 0)
            {
                IList<Currency> remoteCurrencies = ServiceFactory.ConfigureService.GetAllRemoteCurrencies();
                //currencies = currencies.Select(x => new Currency { ChineseName = x.ChineseName, ExchangeRate = x.ExchangeRate, ExchangeRateRemote = remoteCurrencies.FirstOrDefault(w => w.CurrencyCode == x.CurrencyCode).ExchangeRateRemote}).ToList();
                foreach (var currency in currencies)
                {
                    var list = remoteCurrencies.Where(x => x.CurrencyId == currency.CurrencyId).ToList();
                    if (list.Count > 0)
                    {
                        currency.ExchangeRateRemote = list[0].ExchangeRateRemote;
                    }
                }
            }
            ViewBag.Remote = remote;
            ViewData.Model = currencies;
            return View();
        }

        public ActionResult Submit()
        {
            string datas = Request["datas"];
            int adminId = 1;

            var currencies = datas.FromJson<IList<Currency>>();
            IList<KeyValuePair<int, decimal>> keyValuePairs = new List<KeyValuePair<int, decimal>>();
            keyValuePairs = currencies.Select(x => new KeyValuePair<int, decimal>(x.CurrencyId, x.ExchangeRateRemote ?? currencies.FirstOrDefault(w => w.CurrencyId == x.CurrencyId).ExchangeRate)).ToList();
            ServiceFactory.ConfigureService.ModfiyRates(keyValuePairs, adminId);

            #region 更新缓存
            CacheHelper.ClearCurrencyIis();
            ImplCacheHelper.ClearAllValidCurrencies();
            #endregion

            return RedirectToAction("Index", "Currency");
        }

    }
}
