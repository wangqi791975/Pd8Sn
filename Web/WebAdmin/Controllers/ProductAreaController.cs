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
using Com.Panduo.Service.Product.ProductArea;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class ProductAreaController : Controller
    {
        [HttpGet]
        public ActionResult ProductArea()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProductAreaList(string keyword, ProductAreaSorterCriteria? sorter)
        {
            var page = Request["page"].ParseTo(1);
            var pageSize = Request["pageSize"].ParseTo(20);

            var searchCriteria = new Dictionary<ProductAreaSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<ProductAreaSorterCriteria>>();
            if (!keyword.IsNullOrEmpty())
            {
                searchCriteria.Add(ProductAreaSearchCriteria.AreaName, keyword);
            }
            if (sorter.HasValue)
            {
                var sort = new Sorter<ProductAreaSorterCriteria>(sorter.Value, true);
                sorterCriteria.Add(sort);
            }

            var findProductAreas = ServiceFactory.ProductAreaService.FindProductAreas(page, pageSize, searchCriteria, sorterCriteria);
            return View(findProductAreas);
        }

        public JsonResult ProductAreaDelete(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };

            try
            {
                var productAreaId = form["productAreaId"].ParseTo<int>();
                if (productAreaId > 0)
                {
                    ServiceFactory.ProductAreaService.DeleteProductArea(productAreaId);
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
                    case "ERROR_PRODUCTAREA_NOT_EXIST":
                        hashtable["msg"] = "产品专区不存在.";
                        break;
                }
            }

            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult ProductAreaEdit(int id)
        {
            //  取运费活动信息
            var productArea = new ProductArea { IsShowHome = false, IsValid = true };
            if (id > 0)
            {
                try
                {
                    productArea = ServiceFactory.ProductAreaService.GetProductAreaById(id);
                }
                catch (BussinessException bussinessException)
                {
                    switch (bussinessException.GetError())
                    {
                        case "ERROR_PROMOTIONAREA_NOT_EXIST":
                            ViewBag.ErrorMsg = "产品专区不存在！";
                            break;
                    }
                }
            }

            ViewBag.AllLanguage = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Breadcrumbs = new List<KeyValuePair<string, string>>();
            ViewBag.Breadcrumbs.Add(new KeyValuePair<string, string>("产品专区设置", Url.Content("/ProductArea/ProductArea")));

            return View(productArea);
        }

        /// <summary>
        /// 清空商品专区
        /// </summary>
        public JsonResult CleanProductAreaProduct(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                var productAreaId = form["productAreaId"].ParseTo<int>();
                ServiceFactory.ProductAreaService.ClearProductAreaRelative(productAreaId);
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

        public JsonResult ImportProductAreaProduct(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };

            try
            {
                var lstProductAreaRelative = new List<ProductAreaRelative> { };

                var productAreaId = form["txtProductAreaId"].ParseTo<int>();
                var file = Request.Files["file_areaproducts"];
                if (file != null)
                {
                    var importFilePath = Path.Combine(HttpContext.Server.MapPath("../Upload/ImportProductAreaProduct"), DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName));
                    if (!Directory.Exists(importFilePath))
                        Directory.CreateDirectory(importFilePath);
                    file.SaveAs(importFilePath);
                    hashtable = ExcelReadToList(importFilePath, productAreaId, out lstProductAreaRelative);
                }

                if (hashtable["result"].ToString() != BaseController.ActionJsonResult.Success)
                {
                    return Json(hashtable);
                }

                if (!lstProductAreaRelative.IsNullOrEmpty())
                {
                    ServiceFactory.ProductAreaService.SetProductAreaRelativeList(lstProductAreaRelative);

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
                        hashtable["msg"] = "产品专区不存在.";
                        break;
                    case "ERROR_PROMOTIONAREA_PRODUCT_EXIST":
                        hashtable["msg"] = "该产品专区内产品已经存在.请修改Excel数据正确后再重新导入.";
                        break;
                    case "ERROR_PROMOTIONAREA_PRODUCT_DISCOUNT_EXIST":
                        hashtable["msg"] = "促销产品折扣已经存在.请修改Excel数据正确后再重新导入.";
                        break;
                    case "ERROR_PROMOTIONAREA_EXPIRED":
                        hashtable["msg"] = "请修改产品专区有效期.再重新导入产品.";
                        break;
                }
            }

            return Json(hashtable);
        }

        /// <summary>
        /// 读取处理内容信息
        /// </summary>
        /// <param name="filePath">文件路劲</param>
        /// <param name="productAreaId"></param>
        /// <param name="lstProductAreaRelative"></param>
        private Hashtable ExcelReadToList(string filePath, int productAreaId, out List<ProductAreaRelative> lstProductAreaRelative)
        {
            lstProductAreaRelative = new List<ProductAreaRelative> { };
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty } };
            try
            {
                using (var excelHelper = new ExcelHelper(filePath))
                {
                    var dt = excelHelper.ExcelToDataTable(null, true);
                    foreach (DataRow row in dt.Rows)
                    {
                        var productNo = row["商品编号"].ToString().Trim();
                        if (productNo.IsNullOrEmpty())
                            continue;
                        var product = ServiceFactory.ProductService.GetProductByCode(productNo);
                        if (product.IsNullOrEmpty())
                        {
                            hashtable["result"] = BaseController.ActionJsonResult.Error;
                            hashtable["msg"] = string.Format("商品编号:{0}的产品不存在.请修改Excel数据正确后再重新导入.", productNo);
                            return hashtable;
                        }
                        lstProductAreaRelative.Add(new ProductAreaRelative
                        {
                            AreaId = productAreaId,
                            ProductId = product.ProductId
                        });

                    }
                }
                hashtable["result"] = BaseController.ActionJsonResult.Success;
            }
            catch (BussinessException bussinessException)
            {
                throw bussinessException;
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
        public JsonResult ProductAreaSave(FormCollection form)
        {
            var hashtable = new Hashtable { { "result", BaseController.ActionJsonResult.Failing }, { "msg", string.Empty }, { "productAreaId", string.Empty } };
            try
            {
                var productAreaId = form["productAreaId"].ParseTo<int>();
                var isShowHome = form["IsShowHome"].ParseTo<bool>();
                var isValid = form["IsValid"].ParseTo<bool>();
                var areaName = form["AreaName"];
                var lstLanguages = ServiceFactory.ConfigureService.GetAllValidLanguage();
                var lstProductAreaLanguage = (from language in lstLanguages select new ProductAreaLanguage { LanguageId = language.LanguageId, AreaId = productAreaId, AreaName = form[string.Format("{0}{1}", "txtProductAreaName_", language.LanguageId)], Home = form[string.Format("{0}{1}", "txtProductAreaHome_", language.LanguageId)] }).ToList();
                var promotionId = ServiceFactory.ProductAreaService.SetProductArea(new ProductArea
                {
                    AreaId = productAreaId,
                    AreaName = areaName,
                    IsShowHome = isShowHome,
                    IsValid = isValid,
                    ProductAreaLanguages = lstProductAreaLanguage
                });

                hashtable["productAreaId"] = promotionId;
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
        public ActionResult ProductAreaURL()
        {
            ViewBag.Languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.ProductAreas = ServiceFactory.ProductAreaService.FindProductAreas(1, 10000, null, null).Data;
            ViewBag.Discounts = ServiceFactory.PromotionService.GetPromotionDiscount();
            ViewBag.Category = ServiceFactory.CategoryService.GetCategoryTreeRecursiveCache(ServiceFactory.ConfigureService.EnglishLangId);
            return View();
        }

        [HttpPost]
        public ActionResult ProductAreaURL(int language, int productAreaId, int discount, int category)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", string.Empty }, { "url", string.Empty } };

            if (language == -1)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "语言为必填项！";
                return Json(hashtable);
            }
            if (productAreaId == -1)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "名称为必填项！";
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
            var productArea = ServiceFactory.ProductAreaService.GetProductAreaById(productAreaId);
            if (category != -1)
            {
                var firstOrDefault = ServiceFactory.CategoryService.GetCategoryLanguageById(category, ServiceFactory.ConfigureService.EnglishLangId);
                if (!firstOrDefault.IsNullOrEmpty())
                {
                    var productAreaEnName = productArea.ProductAreaLanguages.Find(m => m.LanguageId == ServiceFactory.ConfigureService.EnglishLangId).AreaName;
                    if (productAreaEnName.IsNullOrEmpty())
                    {
                        hashtable["error"] = true;
                        hashtable["msg"] = "请设置英语名称！";
                        return Json(hashtable);
                    }
                    string categoryName = firstOrDefault.CategoryEnglishName;
                    string url = host + "/" + productAreaEnName + "/" + productArea.AreaId + "/" + category + "-1.html" + discountParam;
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
                string url = host + "/" + productArea.AreaName + "/" + productArea.AreaId + "/" + "1.html" + discountParam;
                hashtable["url"] = url;
            }
            return Json(hashtable);
        }
    }
}
