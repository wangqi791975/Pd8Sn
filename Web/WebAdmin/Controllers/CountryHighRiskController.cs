using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Com.Panduo.Service;
using Com.Panduo.Service.SiteConfigure;

namespace Com.Panduo.Web.Controllers
{
    public class CountryHighRiskController : BaseController
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            var countryHighRisks = ServiceFactory.ConfigureService.GetAllCountryHighRisks();
            ViewData.Model = countryHighRisks;
            return View();
        }

        public ActionResult Submit()
        {
            var hashtable = new Hashtable();
            hashtable.Add("getlist", true);
            hashtable.Add("msg", "添加成功!");
            hashtable.Add("error", false);
            try
            {
                string countryHighRisks = Request["country_high_risks"].Trim();
                if (countryHighRisks == string.Empty)
                {
                    hashtable["msg"] = "请输入国家!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                
                int adminId = 6;
                int count = 0;
                var countryHighRisksArr = countryHighRisks.Split(new char[] { ',', '，' });
                var countries = ServiceFactory.ConfigureService.GetAllCountry();
                IList<CountryHighRisk> countryHighRiskList = new List<CountryHighRisk>();
                foreach (var country in countryHighRisksArr)
                {
                    if (!country.Equals("") && country.Length > 5)
                    {
                        var countryObject = countries.Where(x => x.CountryName == country).ToList();
                        if (countryObject.Count > 0)
                        {
                            var isHighRisk = ServiceFactory.ConfigureService.IsCountryHighRisk(countryObject[0].CountryId);
                            if (!isHighRisk)
                            {
                                var risk = new CountryHighRisk
                                {
                                    CountryId = countryObject[0].CountryId,
                                    CountryName = country,
                                    DateCreated = DateTime.Now,
                                    AdminId = adminId
                                };
                                countryHighRiskList.Add(risk);
                                count++;
                            }
                        }
                    }
                }
                ServiceFactory.ConfigureService.AddCountryHighRisks(countryHighRiskList);
                hashtable["msg"] = string.Format("成功添加<b>{0}</b>个", count);

            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = ActionJsonResult.Error;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

    }
}
