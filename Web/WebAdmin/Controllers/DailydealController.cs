using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Service;
using Com.Panduo.Common;
using Com.Panduo.Service.Product.DailyDeal;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Dailydeal;
using Com.Panduo.Service.ServiceConst;

namespace Com.Panduo.Web.Controllers
{
    public class DailydealController : Controller
    {
        //
        // GET: /Dailydeal/

        public ActionResult Dailydeal()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DailydealEdit()
        {
            var lstLanguages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.AllLanguage = lstLanguages;
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("商品管理", Url.Content("/Dailydeal/Dailydeal")));

            #region Dailydeal前台页面配置
            var lstDailydealModel = new List<DailydealModel> { };
            //lstDailydealModel.AddRange(from language in lstLanguages
            //                           let headerImgFilePath = Path.Combine(HttpContext.Server.MapPath("../BussinessHtml/Dailydeal/"), string.Format("headerImg_{0}.jpg", language.LanguageId))
            //                           let middleAreaHtmlFilePath = Path.Combine(HttpContext.Server.MapPath("../BussinessHtml/Dailydeal/"), string.Format("{0}{1}.html", "MiddleAreaHtml_", language.LanguageId))
            //                           let recommendAreaHtmlFilePath = Path.Combine(HttpContext.Server.MapPath("../BussinessHtml/Dailydeal/"), string.Format("{0}{1}.html", "RecommendAreaHtml_", language.LanguageId))
            //                           select new DailydealModel
            //                           {
            //                               HeaderImg = headerImgFilePath,
            //                               LanguageId = language.LanguageId,
            //                               MiddleAreaHtml = FileHelper.LoadAnsiFile(middleAreaHtmlFilePath),
            //                               RecommendAreaHtml = FileHelper.LoadAnsiFile(recommendAreaHtmlFilePath)
            //                           });

            var lstDailydealDesc = ServiceFactory.ProductDailyDealService.GetAllDailydealDesc();
            lstDailydealModel = lstDailydealDesc.Select(x => new DailydealModel
            {
                LanguageId = x.LanguageId,
                HeaderImg = x.HeaderImg,
                MiddleAreaHtml = x.MiddleAreaHtml,
                RecommendAreaHtml = x.RecommendAreaHtml
            }).ToList();
            #endregion

            return View(lstDailydealModel);
        }

        [HttpGet]
        public ActionResult DailydealProductList(string startDateTime, string endDateTime, ProductDailyDealSorterCriteria? sorter)
        {
            PageData<ProductDailyDeal> findProductDailyDeals = null;
            try
            {
                var page = Request["page"].ParseTo(1);
                var pageSize = Request["pageSize"].ParseTo(20);

                var searchCriteria = new Dictionary<ProductDailyDealSearchCriteria, object>
                {
                    {ProductDailyDealSearchCriteria.LanguageId, ServiceFactory.ConfigureService.EnglishLangId}
                };
                var sorterCriteria = new List<Sorter<ProductDailyDealSorterCriteria>>();
                if (!startDateTime.IsNullOrEmpty())
                {
                    searchCriteria.Add(ProductDailyDealSearchCriteria.StartDateTime, startDateTime);
                }
                if (!endDateTime.IsNullOrEmpty())
                {
                    searchCriteria.Add(ProductDailyDealSearchCriteria.EndDateTime, endDateTime);
                }
                if (sorter.HasValue)
                {
                    var sort = new Sorter<ProductDailyDealSorterCriteria>(sorter.Value, true);
                    sorterCriteria.Add(sort);
                }
                findProductDailyDeals = ServiceFactory.ProductDailyDealService.FindProductDailyDeals(page, pageSize,
                                    searchCriteria, sorterCriteria);
            }
            catch (Exception exp)
            {
            }
            return View(findProductDailyDeals);
        }

        public JsonResult DailydealProductDelete(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };

            try
            {
                var productId = form["productId"].ParseTo<int>();
                if (productId > 0)
                {
                    ServiceFactory.ProductDailyDealService.DeleteProductDailyDeal(productId);
                    hashtable["result"] = BaseController.ActionJsonResult.Success;
                }
                else
                {
                    hashtable["result"] = BaseController.ActionJsonResult.Failing;
                    hashtable["msg"] = "商品ID错误";
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_DAILYDEAL_PRODUCT_NOT_EXIST":
                        hashtable["msg"] = "DAILYDEAL产品不存在.";
                        break;
                }
            }

            return Json(hashtable);
        }

        public JsonResult ImportDailydealProduct(FormCollection form, string labelnames)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };
            var labelNameList = new List<DailyDealLabel>();
            var labelNameArray = labelnames.Split(';');
            foreach (var labelName in labelNameArray)
            {
                int languageId = int.Parse(labelName.Split(',')[0]);
                string name = labelName.Split(',')[1];
                labelNameList.Add(new DailyDealLabel { LanguageId = languageId, LabelName = name });
            }
            try
            {
                var lstDailyDeal = new List<ProductDailyDeal> { };

                var file = Request.Files["file_dailydealproducts"];
                if (file != null)
                {
                    var importFilePath = Path.Combine(HttpContext.Server.MapPath("../Upload/ImportDailydealProduct"), DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName));
                    file.SaveAs(importFilePath);
                    hashtable = ExcelReadToList(importFilePath, out lstDailyDeal);
                    if (System.IO.File.Exists(importFilePath))
                        System.IO.File.Delete(importFilePath);
                }

                if (hashtable["result"].ToString() != BaseController.ActionJsonResult.Success)
                {
                    return Json(hashtable);
                }

                if (!lstDailyDeal.IsNullOrEmpty())
                {
                    ServiceFactory.ProductDailyDealService.SetDailyDealList(lstDailyDeal, labelNameList);

                    hashtable["result"] = BaseController.ActionJsonResult.Success;
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_PRODUCT_NOT_EXIST":
                        hashtable["msg"] = "产品不存在.";
                        break;
                    case "ERROR_DAILYDEAL_PRODUCT_IS_EXIST":
                        hashtable["msg"] = "DAILYDEAL产品已经存在.";
                        break;
                    case "ERROR_DAILYDEAL_TITLE_IS_NULL":
                        hashtable["msg"] = "Title 库为空.";
                        break;
                }
            }

            return Json(hashtable);
        }

        /// <summary>
        /// 读取处理内容信息
        /// </summary>
        /// <param name="filePath">文件路劲</param>
        /// <param name="lstDailyDeal"></param>
        private Hashtable ExcelReadToList(string filePath, out List<ProductDailyDeal> lstDailyDeal)
        {
            lstDailyDeal = new List<ProductDailyDeal> { };
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                using (var excelHelper = new ExcelHelper(filePath))
                {
                    var dt = excelHelper.ExcelToDataTable(null, true);
                    foreach (DataRow row in dt.Rows)
                    {
                        var productNo = row["商品编号"].ToString().Trim();
                        var exceltime = row["开始日期"].ToString();
                        var startTime = exceltime.ParseTo<DateTime>();//方法获得时间；如果就是一个时间，那么就直接
                        try
                        {
                            if (startTime <= "1901-01-01".ParseTo<DateTime>())
                                startTime = DateTime.ParseExact(exceltime, "M/d/yy", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                        }
                        catch
                        {
                            startTime = "1900-01-01".ParseTo<DateTime>();
                        }

                        var price = row["售价"].ToString().Trim().ParseTo<decimal>();
                        if (productNo.IsNullOrEmpty() || price <= 0)
                            continue;

                        if (!productNo.IsNullOrEmpty() && price <= 0)
                        {
                            hashtable["result"] = BaseController.ActionJsonResult.Error;
                            hashtable["msg"] = string.Format("商品编号:{0}的[售价]为空.请修改Excel数据正确后再重新导入.", productNo);
                            return hashtable;
                        }
                        if (!productNo.IsNullOrEmpty() && startTime <= "1901-01-01".ParseTo<DateTime>())
                        {
                            hashtable["result"] = BaseController.ActionJsonResult.Error;
                            hashtable["msg"] = string.Format("商品编号:{0}的[开始日期]不正确.请修改Excel数据正确后再重新导入.", productNo);
                            return hashtable;
                        }
                        var dailyDeal =
                            ServiceFactory.ProductDailyDealService.CreatedProductDailyDealByImportData(productNo, price,
                                startTime, 1);
                        if (!dailyDeal.IsNullOrEmpty())
                        {
                            lstDailyDeal.Add(dailyDeal);
                        }
                    }
                }
                hashtable["result"] = BaseController.ActionJsonResult.Success;
            }
            catch (BussinessException bussinessException)
            {
                throw;
            }
            catch (Exception exp)
            {
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return hashtable;
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DailydealSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var lstLanguages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                foreach (var language in lstLanguages)
                {
                    #region 存文件
                    var headerImgFile = Request.Files[string.Format("{0}{1}", "file_headerimg_", language.LanguageId)];
                    var uploadFilePath = string.Empty;
                    if (headerImgFile != null)
                    {
                        uploadFilePath = Path.Combine(
                            HttpContext.Server.MapPath("../BussinessHtml/Dailydeal/"),
                            string.Format("headerImg_{0}{1}", language.LanguageId,
                                Path.GetExtension(headerImgFile.FileName)));
                        headerImgFile.SaveAs(uploadFilePath);
                    }

                    var middleAreaHtml =
                        form[string.Format("{0}{1}", "txtMiddleAreaHtml_", language.LanguageId)] as string;
                    //if (!middleAreaHtml.IsNullOrEmpty())
                    //{
                    //    FileHelper.SaveAnsiFile(Path.Combine(HttpContext.Server.MapPath("../BussinessHtml/Dailydeal/"), string.Format("{0}{1}.html", "MiddleAreaHtml_", language.LanguageId)), middleAreaHtml);
                    //}

                    var recommendAreaHtml = form[string.Format("{0}{1}", "txtRecommendAreaHtml_", language.LanguageId)] as string;
                    //if (!recommendAreaHtml.IsNullOrEmpty())
                    //{
                    //    FileHelper.SaveAnsiFile(Path.Combine(HttpContext.Server.MapPath("../BussinessHtml/Dailydeal/"), string.Format("{0}{1}.html", "RecommendAreaHtml_", language.LanguageId)), recommendAreaHtml);
                    //}
                    #endregion

                    ServiceFactory.ProductDailyDealService.SetDailydealDesc(new DailyDealDesc
                    {
                        LanguageId = language.LanguageId,
                        HeaderImg = uploadFilePath,
                        MiddleAreaHtml = middleAreaHtml,
                        RecommendAreaHtml = recommendAreaHtml
                        //IsValid = true,
                    });


                }
                hashtable["result"] = BaseController.ActionJsonResult.Success;
            }
            catch (Exception exp)
            {
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }

    }
}
