using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Promotion;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class PromotionController : Controller
    {
        //
        // GET: /Promotion/

        [HttpGet]
        public ActionResult Promotion()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PromotionAreaList(string keyword, PromotionAreaSorterCriteria? sorter)
        {
            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<PromotionAreaSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<PromotionAreaSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(PromotionAreaSearchCriteria.PromotionName, keyword);
            }
            if (sorter.HasValue)
            {
                var sort = new Sorter<PromotionAreaSorterCriteria>(sorter.Value, true);
                sorterCriteria.Add(sort);
            }

            var findPromotionAreas = ServiceFactory.PromotionService.FindPromotionAreas(page, pageSize, searchCriteria, sorterCriteria);
            return View(findPromotionAreas);
        }

        public JsonResult PromotionAreaDelete(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };

            try
            {
                var promotionAreaId = form["promotionAreaId"].ParseTo<int>();
                if (promotionAreaId > 0)
                {
                    ServiceFactory.PromotionService.DeletePromotionArea(promotionAreaId);
                    hashtable["result"] = BaseController.ActionJsonResult.Success;
                }
                else
                {
                    hashtable["result"] = BaseController.ActionJsonResult.Failing;
                    hashtable["msg"] = "ID错误";
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = bussinessException.Message;
                switch (bussinessException.GetError())
                {
                    case "ERROR_promotionArea_NOT_EXIST":
                        hashtable["msg"] = "promotionArea不存在.";
                        break;
                }
            }

            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult PromotionAreaEdit(int id)
        {
            //  取运费活动信息
            var promotionArea = new PromotionArea { SaleStartTime = DateTime.Now, SaleEndTime = DateTime.Now.AddMonths(1), IsValid = true };
            if (id > 0)
            {
                try
                {
                    promotionArea = ServiceFactory.PromotionService.GetPromotionAreaById(id);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_PROMOTIONAREA_NOT_EXIST":
                            ViewBag.ErrorMsg = "促销区不存在！";
                            break;
                    }
                }
            }

            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("促销区设置", Url.Content("/Promotion/Promotion")));

            return View(promotionArea);
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        public JsonResult CleanPromotionProduct(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var promotionAreaId = form["promotionAreaId"].ParseTo<int>();
                ServiceFactory.PromotionService.ClearProductPromotion(promotionAreaId);
                hashtable["result"] = BaseController.ActionJsonResult.Success;
            }
            catch (Exception ex)
            {
                //记录日志
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = ex.Message;
            }
            return Json(hashtable);
        }

        public JsonResult ImportPromotionProduct(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };

            try
            {
                var lstProductPromotion = new List<ProductPromotion> { };

                var promotionAreaId = form["txtPromotionAreaId"].ParseTo<int>();
                var file = Request.Files["file_promotionproducts"];
                if (file != null)
                {
                    var importFilePath = Path.Combine(HttpContext.Server.MapPath("../Upload/ImportPromotionProduct"), DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName));
                    file.SaveAs(importFilePath);
                    hashtable = ExcelReadToList(importFilePath, promotionAreaId, out lstProductPromotion);
                }

                if (hashtable["result"].ToString() != BaseController.ActionJsonResult.Success)
                {
                    return Json(hashtable);
                }

                if (!lstProductPromotion.IsNullOrEmpty())
                {
                    ServiceFactory.PromotionService.SetPromotionProductList(lstProductPromotion, promotionAreaId);

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
                        hashtable["msg"] = "产品不存在.请修改Excel数据正确后再重新导入.";
                        break;
                    case "ERROR_PROMOTIONAREA_NOT_EXIST":
                        hashtable["msg"] = "促销区不存在.";
                        break;
                    case "ERROR_PROMOTIONAREA_PRODUCT_EXIST":
                        hashtable["msg"] = "该促销区内产品已经存在.请修改Excel数据正确后再重新导入.";
                        break;
                    case "ERROR_PROMOTIONAREA_PRODUCT_DISCOUNT_EXIST":
                        hashtable["msg"] = "促销产品折扣已经存在.请修改Excel数据正确后再重新导入.";
                        break;
                    case "ERROR_PROMOTIONAREA_EXPIRED":
                        hashtable["msg"] = "请修改促销区有效期.再重新导入产品.";
                        break;
                }
            }

            return Json(hashtable);
        }

        /// <summary>
        /// 读取处理内容信息
        /// </summary>
        /// <param name="filePath">文件路劲</param>
        /// <param name="promotionAreaId"></param>
        /// <param name="lstProductPromotion"></param>
        private Hashtable ExcelReadToList(string filePath, int promotionAreaId, out List<ProductPromotion> lstProductPromotion)
        {
            lstProductPromotion = new List<ProductPromotion> { };
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                using (var excelHelper = new ExcelHelper(filePath))
                {
                    var dt = excelHelper.ExcelToDataTable(null, true);
                    foreach (DataRow row in dt.Rows)
                    {
                        var productNo = row["商品编号"].ToString().Trim();
                        var discount = row["折扣"].ToString().Trim().ParseTo<decimal>();
                        if (discount > 1)
                            discount = 1 - discount / 100.0M;
                        if (productNo.IsNullOrEmpty() || discount <= 0)
                            continue;

                        if (!productNo.IsNullOrEmpty() && discount <= 0)
                        {
                            hashtable["result"] = BaseController.ActionJsonResult.Error;
                            hashtable["msg"] = string.Format("商品编号:{0}的[折扣]不符合规则.请修改Excel数据正确后再重新导入.", productNo);
                            return hashtable;
                        }
                        var productPromotion =
                            ServiceFactory.PromotionService.CreatedProductPromotionByImportData(promotionAreaId, productNo, discount);
                        if (!productPromotion.IsNullOrEmpty())
                        {
                            lstProductPromotion.Add(productPromotion);
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
        public JsonResult PromotionSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty }, { "promotionid", string.Empty } };
            try
            {
                var promotionAreaId = form["promotionAreaId"].ParseTo<int>();
                var isShowHome = form["IsShowHome"].ParseTo<bool>();
                var isValid = form["IsValid"].ParseTo<bool>();
                var limitBeginDate = form["limitBeginDate"];
                var limitBeginTime = form["limitBeginTime"];
                var saleStartTime = Convert.ToDateTime(limitBeginDate + " " + limitBeginTime);
                var limitEndDate = form["limitEndDate"];
                var limitEndTime = form["limitEndTime"];
                var saleEndTime = Convert.ToDateTime(limitEndDate + " " + limitEndTime);
                var promotionName = form["PromotionName"];
                var lstLanguages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                var lstPromotionDesc = (from language in lstLanguages select new PromotionDesc { LanguageId = language.LanguageId, PromotionAreaId = promotionAreaId, PromotionName = form[string.Format("{0}{1}", "txtPromotionName_", language.LanguageId)], PromotionHome = form[string.Format("{0}{1}", "txtPromotionHome_", language.LanguageId)] }).ToList();
                var promotionId = ServiceFactory.PromotionService.SetPromotionArea(new PromotionArea
                {
                    PromotionAreaId = promotionAreaId,
                    PromotionName = promotionName,
                    IsShowHome = isShowHome,
                    IsValid = isValid,
                    SaleStartTime = saleStartTime,
                    SaleEndTime = saleEndTime,
                    PromotionDescs = lstPromotionDesc
                });

                hashtable["promotionid"] = promotionId;
                hashtable["result"] = BaseController.ActionJsonResult.Success;
            }
            catch (Exception exp)
            {
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["msg"] = exp.Message;
            }
            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult PromotionURL()
        {
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Discounts = ServiceFactory.PromotionService.GetPromotionDiscount();
            ViewBag.Category = ServiceFactory.CategoryService.GetCategoryTreeRecursiveCache(ServiceFactory.ConfigureService.EnglishLangId);
            return View();
        }

        [HttpPost]
        public ActionResult PromotionURL(int language, int discount, int category)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", string.Empty }, { "url", string.Empty } };

            if (language == -1)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "语言为必填项！";
                return Json(hashtable);
            }
            if (discount == -1 && category == -1)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "“折扣”和 “类别”二者必须至少有一个选中了具体的值，请选择！";
                return Json(hashtable);
            }

            string discountParam = "";
            if (discount != -1)
            {
                discountParam = "?" + "discount=" + Convert.ToInt32(discount).ToString(CultureInfo.InvariantCulture);
            }

            string host = ServiceFactory.ConfigureService.GetAllValidLanguage().ToList().Find(m => m.LanguageId == language).Host;
            if (category != -1)
            {
                var firstOrDefault = ServiceFactory.CategoryService.GetCategoryLanguageById(category,
                    ServiceFactory.ConfigureService.EnglishLangId);
                if (!firstOrDefault.IsNullOrEmpty())
                {
                    string categoryName = firstOrDefault.CategoryEnglishName;
                    string url = host + "/" + categoryName.Replace(' ', '-') + "/" + category + "-1.html" + discountParam;
                    hashtable["url"] = url;
                }
                else
                {
                    hashtable["error"] = true;
                    hashtable["msg"] = "类别不存在！";
                }
            }
            else
            {
                string url = host + "1.html" + discountParam;
                hashtable["url"] = url;
            }
            return Json(hashtable);
        }

    }
}
