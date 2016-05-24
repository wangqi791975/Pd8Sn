using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index(int? id)
        {
            int page = id ?? 1;
            ViewBag.Page = page;
            return View();
        }

        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = 20;
            string keyWord = Request["keyword"] != null ? Request["keyword"].Trim() : string.Empty;

            IDictionary<ProductSearchCriteria, object> searchDictionary = new Dictionary<ProductSearchCriteria, object>();
            if (!keyWord.IsNullOrEmpty())
            {
                searchDictionary.Add(ProductSearchCriteria.Keyword, keyWord);
            }
            PageData<Product> pageData = ServiceFactory.ProductService.FindProductsForAdminList(page, pageSize, searchDictionary);
            //if (pageData.Pager.TotalRowCount == 1)
            //{
            //    return Detail(pageData.Data[0].ProductId);
            //}

            ViewBag.KeyWord = keyWord;
            ViewData.Model = pageData;
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            Product product = ServiceFactory.ProductService.GetProductById(id);
            if (product == null)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            int page = Request["page"].ParseTo(1);

            IList<KeyValuePair<string, string>> breadcrumbs = new List<KeyValuePair<string, string>>();
            breadcrumbs.Add(new KeyValuePair<string, string>("产品管理", "/Product/Index"));
            breadcrumbs.Add(new KeyValuePair<string, string>(product.ProductCode, ""));

            IList<Language> languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            IList<ProductLanguage> productLanguages = ServiceFactory.ProductService.GetProductLanguages(id);
            IList<Property> properties = ServiceFactory.PropertyService.GetAllPropertiesByProductId(id);
            IList<ProductUnit> productUnits = ServiceFactory.ProductService.GetAllProductUnit();
            ProductStock productStock = ServiceFactory.ProductService.GetProductStock(id);
            IList<ProductPropertyValue> productPropertyValues =
                ServiceFactory.ProductService.GetProductBindedAllPropertyValues(id);
            IList<ProductStepPrice> productStepPrices = ServiceFactory.ProductService.GetProductSalePrices(id, 100);
            IList<ProductPriceRise> productPriceRises = ServiceFactory.ProductService.GetAllProductPriceRise();

            ViewBag.Id = id;
            ViewBag.Page = page;
            ViewBag.Product = product;
            ViewBag.Languages = languages;
            ViewBag.ProductLanguages = productLanguages;
            ViewBag.Properties = properties;
            ViewBag.ProductUnits = productUnits;
            ViewBag.ProductStock = productStock;
            ViewBag.ProductPropertyValues = productPropertyValues;
            ViewBag.ProductStepPrices = productStepPrices;
            ViewBag.ProductPriceRises = productPriceRises;
            ViewBag.Breadcrumbs = breadcrumbs;
            return View();
        }

        public JsonResult Submit()
        {
            int page = Request["page"].ParseTo(1);
            var hashtable = new Hashtable();
            hashtable.Add("msg", "修改成功!");
            hashtable.Add("error", false);
            try
            {
                #region 修改Product
                int productId = Request["id"].ParseTo(0);
                decimal weight = Request["weight"].Trim().ParseTo(0M);
                decimal volumeWeight = Request["volume_weight"].Trim().ParseTo(-1M);
                int groupQuantity = Request["group_quantity"].Trim().ParseTo(0);
                int unitId = Request["unit_id"].Trim().ParseTo(0);
                decimal costPriceRmb = Request["cost_price_rmb"].Trim().ParseTo(-1M);
                decimal increaseProportion = Request["increase_proportion"].Trim().ParseTo(-1M);

                if (productId < 1)
                {
                    hashtable["msg"] = "产品不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (weight < 1)
                {
                    hashtable["msg"] = "产品净重不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (volumeWeight < 0)
                {
                    hashtable["msg"] = "产品体积重不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (groupQuantity < 1)
                {
                    hashtable["msg"] = "每组数量不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (unitId < 1)
                {
                    hashtable["msg"] = "单位不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (costPriceRmb < 0)
                {
                    hashtable["msg"] = "成本价不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (increaseProportion < 0)
                {
                    hashtable["msg"] = "上浮比例不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                Product product = new Product();
                product.ProductId = productId;
                product.Weight = weight;
                product.VolumeWeight = volumeWeight;
                product.GroupQuantity = groupQuantity;
                product.UnitId = unitId;
                //product.GroupQuantity = Request["group_quantity"].Trim().ParseTo(1);
                product.CostPriceRmb = costPriceRmb;
                product.IncreaseProportion = increaseProportion;
                product.Status = EnumHelper.ToEnum<ProductStatus>(Request["status"].Trim().ParseTo(1));
                ServiceFactory.ProductService.UpdateProduct(product);
                #endregion

                #region 修改ProductLanguage
                string[] languageIds = Request["language_ids"].Split(',');
                IList<ProductLanguage> productLanguages = new List<ProductLanguage>();
                foreach (var languageId in languageIds)
                {
                    ProductLanguage productLanguage = new ProductLanguage();
                    productLanguage.ProductId = productId;
                    productLanguage.LanguageId = languageId.ParseTo(1);
                    productLanguage.ProductName = Request["product_name_" + languageId];
                    productLanguage.MarketingTitle = Request["marketing_title_" + languageId];
                    productLanguages.Add(productLanguage);
                }
                ServiceFactory.ProductService.UpdateProductLanguages(productLanguages);
                #endregion

                #region 修改库存
                ProductStock productStock = new ProductStock();
                productStock.StockId = Request["stock_id"].ParseTo(0);
                if (productStock.StockId > 1)
                {
                    productStock.BindStockType = EnumHelper.ToEnum<StockStatus>(Request["bind_stock_type"].ParseTo(0));
                    productStock.StockNumber = Request["stock_number"].ParseTo(1);
                    ServiceFactory.ProductService.UpdateProductStock(productStock);
                }
                #endregion

                #region 修改ProductPropertyValue
                string[] propertyIds = Request["property_ids"].Split(',');
                IList<ProductPropertyValue> productPropertyValues = new List<ProductPropertyValue>();
                foreach (var propertyId in propertyIds)
                {
                    ProductPropertyValue productPropertyValue = new ProductPropertyValue();
                    productPropertyValue.ProductId = productId;
                    productPropertyValue.PropertyId = propertyId.ParseTo(0);
                    productPropertyValue.PropertyValueId = Request["property_value_id_" + propertyId].ParseTo(0);
                    productPropertyValues.Add(productPropertyValue);
                }
                ServiceFactory.ProductService.UpdateProductPropertyValues(productPropertyValues);
                #endregion

                var productStepPriceIds = Request["product_step_price_ids"].Split(',');
                var productStepPrices = productStepPriceIds.Select(productQuantity => new ProductStepPrice
                {
                    ProductId = productId,
                    Quantity = productQuantity.ParseTo(1),
                    ProfitCoefficient = Request["profit_coefficient_" + productQuantity].Trim().ParseTo(10M)
                }).ToList();
                ServiceFactory.ProductService.UpdateProductStepPrices(productStepPrices);

                //清除缓存
                ImplCacheHelper.ClearProduct(productId);
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
                return Json(hashtable);
            }

            return Json(hashtable);
        }

        public ActionResult BestMatchItems()
        {
            return View();
        }

        public JsonResult UploadBestMatchItems()
        {
            var hashtable = new Hashtable { { "msg", "上传成功!" }, { "error", false } };
            try
            {
                foreach (string upload in Request.Files)
                {
                    var file = Request.Files[upload];
                    var filePath = Path.Combine(HttpContext.Server.MapPath("../Upload/BestMatch"),
                        upload + Path.GetExtension(file.FileName));
                    file.SaveAs(filePath);
                    var list = ExcelReadToDictionary(filePath);
                    if (list.IsNullOrEmpty()) continue;
                    bool cando = true;
                    foreach (var item in list)
                    {
                        if (!ServiceFactory.ProductService.CanSetBestMatch(item))
                        {
                            hashtable["msg"] = "商品编号：" + item.Key + " 匹配商品编号：" + item.Value + " 部分产品不存在！";
                            hashtable["error"] = true;
                            cando = false;
                            break;
                        }
                    }
                    if (cando)
                    {
                        ServiceFactory.ProductService.SetBestMatch(list);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
                return Json(hashtable);
            }
            return Json(hashtable);
        }


        /// <summary>
        /// 读取处理内容信息
        /// </summary>
        /// <param name="file">文件路劲</param>
        private IList<KeyValuePair<string, string>> ExcelReadToDictionary(string file)
        {
            try
            {
                var custonrtInfo = new List<KeyValuePair<string, string>>();
                using (var excelHelper = new ExcelHelper(file))
                {
                    var dt = excelHelper.ExcelToDataTable("MySheet", true);
                    custonrtInfo.AddRange(from DataRow row in dt.Rows let productno = row["商品编号"].ToString().Trim() let matchproductno = row["匹配商品编号"].ToString().Trim() select new KeyValuePair<string, string>(productno, matchproductno));
                }
                return custonrtInfo;
            }
            catch
            {
                return null;
            }
        }

        #region 上货

        public ActionResult UploadProduct()
        {
            return View();
        }


        /// <summary>
        /// 读取处理内容信息
        /// </summary>
        /// <param name="file">文件路劲</param>
        private DataTable ExcelReadToDataTable(string file, out string errMsg, int firstRowNum = 0)
        {
            errMsg = string.Empty;
            try
            {
                using (var excelHelper = new ExcelHelper(file))
                {
                    return excelHelper.ExcelToDataTable("Sheet1", false, firstRowNum);
                }
            }
            catch (Exception exp)
            {
                errMsg = exp.Message;
            }
            return null;
        }
        private List<dynamic> ZipReadToList(string filePath)
        {
            var items = new List<dynamic>();
            try
            {
                /*
                 20151024-EN, 20151024-DE. 每个子文件夹中的子文件，一般为txt格式。文件名称直接为商品编号，如B12345.txt
                 * 
                 * ProductCode == prod.ProductCode && x.Lanage == "EN");
                        if (!desc.IsNullOrEmpty())
                            prod.ProductEnDesc = desc.Desc
                 */
                //var unZipDir = Path.Combine(filePath.Replace(Path.GetFileName(filePath), ""));
                var unZipDir = filePath.Replace(Path.GetFileName(filePath), Path.GetFileNameWithoutExtension(filePath));
                ZipHelper.UnZip(filePath, unZipDir);
                foreach (var item in System.IO.Directory.GetDirectories(unZipDir))
                {
                    foreach (var file in System.IO.Directory.GetFiles(item))
                    {
                        items.Add(new
                        {
                            Lanage = item.Right(2),
                            ProductCode = Path.GetFileName(file).Replace(Path.GetExtension(file), ""),
                            Desc = FileHelper.LoadAnsiFile(file)
                        });
                    }
                }

                FileHelper.DeleteDirectoryAndFile(unZipDir);
            }
            catch (Exception exp)
            {
                return null;
            }
            return items;
        }
        public JsonResult SaveUploadProduct()
        {
            var hashtable = new Hashtable { { "msg", "失败,请稍后再试!" }, { "error", false }, { "result", BaseController.ActionJsonResult.Failing } };
            var errMsg = string.Empty;
            try
            {
                var path = Path.Combine(UploadFileHelper.GetUploadFileSavePath(UploadFileType.UploadProduct), DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                var fileExcel = Request.Files["file_products"];
                var fileZip = Request.Files["file_products_desc"];
                if (fileExcel == null)
                {
                    hashtable["msg"] = "请上传Excel文件";
                    hashtable["result"] = BaseController.ActionJsonResult.Error;
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                List<dynamic> prodDescs = null;
                if (fileZip == null)
                {
                    hashtable["msg"] = "请上传Zip文件";
                    hashtable["result"] = BaseController.ActionJsonResult.Error;
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                else
                {
                    var filePath = string.Format("{0}{1}", path, Path.GetExtension(fileZip.FileName));
                    fileZip.SaveAs(filePath);
                    prodDescs = ZipReadToList(filePath);
                    if (prodDescs.IsNullOrEmpty())
                    {
                        hashtable["msg"] = "解析Zip文件失败";
                        hashtable["result"] = BaseController.ActionJsonResult.Error;
                        hashtable["error"] = true;
                        return Json(hashtable);
                    }
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                if (!prodDescs.IsNullOrEmpty())
                {
                    var filePath = string.Format("{0}{1}", path, Path.GetExtension(fileExcel.FileName));
                    fileExcel.SaveAs(filePath);
                    var table = ExcelReadToDataTable(filePath, out errMsg, 4);
                    if (table.IsNullOrEmpty())
                    {
                        hashtable["msg"] = "解析Excel文件失败";
                        hashtable["result"] = BaseController.ActionJsonResult.Error;
                        hashtable["error"] = true;
                        return Json(hashtable);
                    }
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    var uploadProducts = (from DataRow row in table.Rows
                                          where row[1].ParseTo("") != ""
                                          select new UploadProduct
                                          {
                                              ProductClass = row[0].ParseTo("").TrimStart('\''),
                                              ProductCode = row[1].ParseTo(""),
                                              Price = row[2].ParseTo(0),
                                              PropertyName = row[3].ParseTo(""),
                                              PriceStep = row[4].ParseTo(0),
                                              Weight = row[5].ParseTo(0),
                                              VolumeWeight = row[6].ParseTo(0),
                                              PropertyValues = row[7].ParseTo("").Split(';'),
                                              ProductEnName = row[8].ParseTo(""),
                                              ProductEnDesc = row[9].ParseTo(""),
                                              ProductDeName = row[10].ParseTo(""),
                                              ProductDeDesc = row[11].ParseTo(""),
                                              ProductRuName = row[12].ParseTo(""),
                                              ProductRuDesc = row[13].ParseTo(""),
                                              ProductFrName = row[14].ParseTo(""),
                                              ProductFrDesc = row[15].ParseTo(""),
                                              ProductEsName = row[16].ParseTo(""),
                                              ProductEsDesc = row[17].ParseTo(""),
                                              ProductLtName = row[18].ParseTo(""),
                                              ProductLtDesc = row[19].ParseTo(""),
                                              ProductJpName = row[20].ParseTo(""),
                                              ProductJpDesc = row[21].ParseTo(""),
                                              LimitStock = row[22].ParseTo(""),
                                              StockQty = row[23].ParseTo(0),
                                          }).ToList();

                    foreach (var prod in uploadProducts)
                    {
                        var desc = prodDescs.Find(x => x.ProductCode == prod.ProductCode && x.Lanage == "EN");
                        if (desc != null)
                            prod.ProductEnDesc = desc.Desc;

                        desc = prodDescs.Find(x => x.ProductCode == prod.ProductCode && x.Lanage == "DE");
                        if (desc != null)
                            prod.ProductDeDesc = desc.Desc;

                        desc = prodDescs.Find(x => x.ProductCode == prod.ProductCode && x.Lanage == "RU");
                        if (desc != null)
                            prod.ProductRuDesc = desc.Desc;

                        desc = prodDescs.Find(x => x.ProductCode == prod.ProductCode && x.Lanage == "FR");
                        if (desc != null)
                            prod.ProductFrDesc = desc.Desc;

                        desc = prodDescs.Find(x => x.ProductCode == prod.ProductCode && x.Lanage == "ES");
                        if (desc != null)
                            prod.ProductEsDesc = desc.Desc;

                        desc = prodDescs.Find(x => x.ProductCode == prod.ProductCode && x.Lanage == "JP");
                        if (desc != null)
                            prod.ProductJpDesc = desc.Desc;

                        desc = prodDescs.Find(x => x.ProductCode == prod.ProductCode && x.Lanage == "IT");
                        if (desc != null)
                            prod.ProductLtDesc = desc.Desc;
                    }
                    var result = ServiceFactory.ProductService.SaveUploadProducts(uploadProducts);
                    if (result)
                    {
                        hashtable["msg"] = "导入成功！";
                        hashtable["result"] = BaseController.ActionJsonResult.Success;
                        hashtable["error"] = false;
                    }
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = errMsg.IsNullOrEmpty() ? bussinessException.Message : errMsg;
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
            }
            catch (Exception exception)
            {
                hashtable["msg"] = errMsg.IsNullOrEmpty() ? exception.Message : errMsg;
                hashtable["result"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
            }
            return Json(hashtable);
        }


        #endregion

    }
}
