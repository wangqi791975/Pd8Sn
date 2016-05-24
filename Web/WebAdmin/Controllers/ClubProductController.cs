using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer.Product;
using Com.Panduo.Service.Product.ClubProduct;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class ClubProductController : Controller
    {
        //
        // GET: /ClubProduct/
        [HttpGet]
        public ActionResult ClubProduct()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ClubProductList(ClubProductType clubProductType = ClubProductType.New, int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<ClubProductSearchCriteria, object>
            {
                { ClubProductSearchCriteria.ClubProductType, clubProductType },
                { ClubProductSearchCriteria.LanguageId, ServiceFactory.ConfigureService.EnglishLangId }
            };
            var clubProducts = ServiceFactory.ClubProductService.FindAllClubProducts(page, pageSize, searchCriteria, new List<Sorter<ClubProductSorterCriteria>>());
            return View(clubProducts);
        }

        [HttpPost]
        public ActionResult ImportClubProduct(ClubProductType clubProductType)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", string.Empty } };
            foreach (string upload in Request.Files)
            {
                var file = Request.Files[upload];
                var path = HttpContext.Server.MapPath("../Upload/ImportClubProduct");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (file != null)
                {
                    string fileName = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
                    string filePath = Path.Combine(path, fileName + Path.GetExtension(file.FileName));
                    file.SaveAs(filePath);
                    var returnInfo =
                        ServiceFactory.ClubProductService.SetClubProductList(ExcelReadToClubProduct(filePath, hashtable,
                            clubProductType));
                    if (!returnInfo.Key.IsEmpty())
                    {
                        switch (returnInfo.Key)
                        {
                            case "ERROR_PRODUCT_NOT_EXIST":
                                hashtable["error"] = true;
                                hashtable["msg"] = returnInfo.Value + "产品不存在！";
                                break;
                            case "ERROR_EXIST_IN_DAILYDEAL_PRODUCT":
                                hashtable["error"] = true;
                                hashtable["msg"] = returnInfo.Value + "为一口价，不允许导入club会员商品专区，请确认。！";
                                break;
                            case "ERROR_EXIST_IN_PROMOTION_PRODUCT":
                                hashtable["error"] = true;
                                hashtable["msg"] = returnInfo.Value + "已有促销折扣，不允许导入club会员商品专区，请确认。！";
                                break;
                            case "ERROR_CLUBPRODUCT_EXIST":
                                hashtable["error"] = true;
                                hashtable["msg"] = returnInfo.Value + "产品已经存在于club会员区域！";
                                break;
                            case "ERROR_CLUBPRODUCT_REPETITION":
                                hashtable["error"] = true;
                                hashtable["msg"] = "存在重复编号" + returnInfo.Value + "，请确认！";
                                break;
                        }
                    }
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            }
            if (Request.Files.Count == 0)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "请选择一个文件！";
            }
            return Json(hashtable);
        }

        [HttpPost]
        public ActionResult DeleteClubProduct(int id)
        {
            var hashtable = new Hashtable { { "error", false }, { "msg", string.Empty } };
            try
            {
                ServiceFactory.ClubProductService.RemoveClubProduct(id);
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_CLUBPRODUCT_NOT_EXIST":
                        hashtable["error"] = true;
                        hashtable["msg"] = "Club会员产品不存在！";
                        break;
                }
            }
            return Json(hashtable);
        }


        #region 辅助方法

        /// <summary>
        /// 读取客户绑定商品信息
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="hashtable"></param>
        /// <param name="clubProductType">club产品类型</param>
        /// <returns></returns>
        private List<ClubProduct> ExcelReadToClubProduct(string file, Hashtable hashtable, ClubProductType clubProductType)
        {
            try
            {
                var clubProducts = new List<ClubProduct>();
                using (var excelHelper = new ExcelHelper(file))
                {
                    var dt = excelHelper.ExcelToDataTable("Sheet1", true);
                    foreach (DataRow row in dt.Rows)
                    {
                        string productModel = row[0].ToString().Trim();
                        decimal discount = Int32.Parse(row[1].ToString().Trim());
                        var product = ServiceFactory.ProductService.GetProductByCode(productModel);
                        if (!product.IsNullOrEmpty())
                        {
                            var clubProduct = new ClubProduct
                            {
                                ProductId = product.ProductId,
                                Discount = discount / 100,
                                Type = clubProductType,
                                CreateDateTime = DateTime.Now
                            };
                            clubProducts.Add(clubProduct);
                        }
                    }
                }
                return clubProducts;
            }
            catch (BussinessException bussinessException)
            {
                hashtable["error"] = true;
                hashtable["msg"] = "excel格式不正确";
                return null;
            }
        }
        #endregion
    }
}
