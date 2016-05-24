using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.Service.Payment;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Common.Mvc.Model;
using Com.Panduo.Web.Models.Order;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Service.ServiceConst;


namespace Com.Panduo.Web.Controllers
{
    public class OrderController : BaseController
    {
        private static WebClientHelper _webClientHelper = new WebClientHelper { Timeout = ServiceConfig.RequestTimeout };

        private int CustomerId
        {
            get { return SessionHelper.CurrentCustomer == null?0 : SessionHelper.CurrentCustomer.CustomerId; }
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderSearch()
        {
            var orderno = Request["orderno"] ?? string.Empty;
            var partno = Request["partno"] ?? string.Empty;
            var states = Request["status"] ?? "-1";
            var startdate = Request["startdate"] ?? string.Empty;
            var enddate = Request["enddate"] ?? string.Empty;
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(10);//页大小
            var search = new Dictionary<OrderSearchCriteria, object>();
            if (!orderno.IsNullOrEmpty())
            {
                search.Add(OrderSearchCriteria.OrderNo, orderno);
            }
            if (!partno.IsNullOrEmpty())
            {
                search.Add(OrderSearchCriteria.PartNo, partno);
            }
            if (states.ParseTo<int>(-1) != -1)
            {
                search.Add(OrderSearchCriteria.OrderStatus, states);
            }

            if (!startdate.IsNullOrEmpty() && !enddate.IsNullOrEmpty())
            {
                search.Add(OrderSearchCriteria.OrderDateFrom, startdate);
                search.Add(OrderSearchCriteria.OrderDateTo, enddate);
            }
            ViewBag.CustomerOrderStatus = CacheHelper.GetAllCustomerOrderStatus;

            ViewBag.Status = states.ToInt();
            var name = CacheHelper.GetAllCustomerOrderStatus.Where(x => x.Value == ViewBag.Status).Select(c => c.Name).FirstOrDefault();
            if (name.IsNullOrEmpty())
            {
                name = "All Orders";
            }
            ViewBag.StatusName = name;

            PageData<Order> order = ServiceFactory.OrderService.GetOrdersByCustomerId(CustomerId, page, pageSize, search, null);
            IList<Order> orderList = new List<Order>();
            foreach (var o in order.Data)
            {
                o.OrderCost = ServiceFactory.OrderService.GetOrderCostByOrderId‎(o.OrderId);
                orderList.Add(o);
            }
            order.Data = orderList;
            ViewBag.Sitemaps = Sitemap.GetMyAccountOrderSearchSitemap(ViewBag.StatusName);
            if (Request.IsAjaxRequest())
            {
                return View("OrderList", order);
            }
            return View(order);
        }

        /// <summary>
        /// 下载packlist模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PackingListTemplate()
        {
            var file = "Packing list - " + ServiceFactory.ConfigureService.SiteLanguageCode.ToLower() + ".xls";
            string fullPath = Path.Combine(Server.MapPath("~/PackingList/"), file);
            return File(fullPath, "application/vnd.ms-excel", file);
        }

        #region

        /// <summary>
        /// 加载Excel
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private IWorkbook LoadExcel(string fullPath)
        {
            var file = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var hssfworkbook = new HSSFWorkbook(file);
            file.Close();
            file.Dispose();
            return hssfworkbook;
        }


        private MemoryStream WriteToStream(IWorkbook workbook)
        {
            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            file.Close();
            file.Dispose();
            return file;
        }



        private void WriteToFile(IWorkbook workbook, string fullPath)
        {
            var file = new FileStream(fullPath, FileMode.Create);

            workbook.Write(file);

            file.Close();
            file.Dispose();
        }


        private static WebClient webClient = new WebClient();
        private static byte[] FetchImageData(string url, bool isResize, int width, int hight)
        {
            System.Drawing.Image image = null;
            try
            {
                var stream = webClient.OpenRead(url);
                if (stream != null)
                {
                    image = System.Drawing.Image.FromStream(stream);

                    ////是否需要缩放
                    //if (isResize)
                    //{
                    //    image = image.ResizeImageToSize(width, hight, true);
                    //    //var image = image.ResizeImageToEboard(width, hight);
                    //}
                    return GetByteImage(image);
                }
            }
            catch (Exception exception)
            {

            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                }
            }

            return null;
        }

        public static byte[] GetByteImage(Image img)
        {
            byte[] bt = null;

            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);

                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流

                    bt = new byte[mostream.Length];

                    mostream.Position = 0;//设置留的初始位置

                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));

                }

            }

            return bt;

        }

        public byte[] ExportExcel()
        {
            int page = 1;//当前页
            int pageSize = 100000;//页大小
            var orderno = Request["orderno"] ?? string.Empty;
            var partno = Request["partno"] ?? string.Empty;
            var trackingnumber = Request["trackingnumber"] ?? string.Empty;
            var packingno = Request["packingno"] ?? string.Empty;

            #region 1.读取数据
            var fetchDataBeginDate = DateTime.Now;
            var list = PackageDetailInfoList(page, pageSize, orderno, partno, trackingnumber, packingno).Data;
            #endregion

            #region 2.生成Excel文件
            return GenerateExcel(list).GetBuffer();
            #endregion

        }

        private static ICellStyle CopyCellStyle(IWorkbook wb, ICellStyle orginalCellStyle)
        {
            var style = wb.CreateCellStyle();
            style.CloneStyleFrom(orginalCellStyle);

            return style;
        }
        private MemoryStream GenerateExcel(IList<PackageDetailInfoItemVo> sourceList)
        {
            var beginDate = DateTime.Now;
            var workBook = LoadExcel(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "temple.xls"));
            var sheet = workBook.GetSheet("Sheet1");

            var rowCount = sourceList.Count;
            var columnCount = 8;
            var templetRow = sheet.GetRow(1);
            for (var i = 2; i < rowCount + 2; i++)
            {
                var row = sheet.CreateRow(i);

                row.Height = templetRow.Height;

                for (var j = 0; j < columnCount; j++)
                {
                    var column = row.CreateCell(j);
                    var orginalCell = templetRow.GetCell(j);
                    column.CellStyle = CopyCellStyle(workBook, orginalCell.CellStyle);//行样式
                }
            }

            //设置行信息
            var positionIndex = 1;

            #region 设置默认超连接的样式，蓝色带下划线
            ICellStyle hlinkStyle = workBook.CreateCellStyle();
            IFont hlinkFont = workBook.CreateFont();
            hlinkFont.Underline = FontUnderlineType.Single;
            hlinkFont.Color = HSSFColor.Blue.Index;
            hlinkStyle.SetFont(hlinkFont);
            hlinkStyle.Alignment = HorizontalAlignment.Center;
            hlinkStyle.VerticalAlignment = VerticalAlignment.Center;
            #endregion

            foreach (var item in sourceList)
            {
                var data = FetchImageData(ImageHelper.GetImageUrl(item.ProductInfo.Product.MainImage, 80), false, 80, 80);
                var patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                var anchor = new HSSFClientAnchor(0, 0, 255, 255, 0, positionIndex, 0, positionIndex) { AnchorType = 0 };

                var picture = (HSSFPicture)patriarch.CreatePicture(anchor, workBook.AddPicture(data, PictureType.JPEG));

                picture.LineStyle = LineStyle.DashGel;
                picture.Resize();//Reset the image to the original size. 

                var cell = sheet.GetRow(positionIndex).GetCell(1);
                cell.SetCellValue(item.PackageDetail.ProductModel);
                var link = new HSSFHyperlink(HyperlinkType.Url);
                link.Address = UrlRewriteHelper.GetProductDetailUrl(item.PackageDetail.ProductId, item.ProductInfo.ProductEnName);
                cell.Hyperlink = link;
                cell.CellStyle = hlinkStyle;

                //sheet.GetRow(positionIndex).GetCell(1).SetCellValue(item.PackageDetail.ProductModel);
                sheet.GetRow(positionIndex).GetCell(2).SetCellValue(item.PackageDetail.ProductQty);
                sheet.GetRow(positionIndex).GetCell(3).SetCellValue(item.PackageDetail.TotalShipped);
                sheet.GetRow(positionIndex).GetCell(4).SetCellValue(item.PackageDetail.ShippedQty);
                sheet.GetRow(positionIndex).GetCell(5).SetCellValue(item.PackageDetail.ProductQty - item.PackageDetail.TotalShipped - item.PackageDetail.ShippedQty);
                sheet.GetRow(positionIndex).GetCell(6).SetCellValue(item.PackageList.IsNullOrEmpty() ? string.Empty : item.PackageList[0].OrderNumber);
                var trackingNumber = string.Empty;
                foreach (var positionItem in item.PackageList)
                {
                    trackingNumber += " " + positionItem.TrackingNumber;
                }
                sheet.GetRow(positionIndex).GetCell(7).SetCellValue(trackingNumber);
                positionIndex++;
            }
            return WriteToStream(workBook);
            // Console.WriteLine(string.Format("已经生成模板4类型的excel数据,共{0}个文件,耗时:[{1}]", index - 1, DateTime.Now - beginDate));
        }

        private PageData<PackageDetailInfoItemVo> PackageDetailInfoList(int page, int pageSize, string orderno, string partno, string trackingnumber, string packingno)
        {
            var search = new Dictionary<PackageSearchCriteria, object>();

            search.Add(PackageSearchCriteria.CustomerId, CustomerId);

            if (!orderno.IsNullOrEmpty())
            {
                search.Add(PackageSearchCriteria.OrderNo, orderno);
            }

            if (!partno.IsNullOrEmpty())
            {
                search.Add(PackageSearchCriteria.PartNo, partno);
            }

            if (!trackingnumber.IsNullOrEmpty())
            {
                search.Add(PackageSearchCriteria.TrackingNumber, trackingnumber);
            }

            if (!packingno.IsNullOrEmpty())
            {
                search.Add(PackageSearchCriteria.PackingNo, packingno);
            }

            var packdetails = ServiceFactory.OrderService.FindOrderPackages(page, pageSize, search, null);

            var packdetailLists = packdetails.Data;

            var productInfos = ServiceFactory.ProductService.GetProductInfos(packdetailLists.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);

            var items = productInfos.Select(c => new PackageDetailInfoItemVo
            {
                ProductInfo = c,
                PackageDetail = packdetailLists.FirstOrDefault(w => w.ProductId == c.Product.ProductId),
                PackageList = ServiceFactory.OrderService.GetOrderPackageByOrderId(packdetailLists.FirstOrDefault(w => w.ProductId == c.Product.ProductId).OrderId)//反查包裹主表信息
            }).ToList();

            var pageData = new PageData<PackageDetailInfoItemVo>
            {
                Data = items,
                Pager = packdetails.Pager
            };
            return pageData;
        }

        #endregion

        public ActionResult DownloadPackingSlip()
        {
            var stream = ExportExcel();
            var file = "Packing list - " + ServiceFactory.ConfigureService.SiteLanguageCode.ToLower() + ".xls";
            string fullPath = Path.Combine(Server.MapPath("~/PackingList/"), file);
            return File(stream, "application/vnd.ms-excel", file);
        }

        /// <summary>
        /// 包裹查询
        /// </summary>
        /// <returns></returns>
        public ActionResult PackingSlip()
        {
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(10);//页大小
            var orderno = Request["orderno"] ?? string.Empty;
            var partno = Request["partno"] ?? string.Empty;
            var trackingnumber = Request["trackingnumber"] ?? string.Empty;
            var packingno = Request["packingno"] ?? string.Empty;

            var search = new Dictionary<PackageSearchCriteria, object>();

            search.Add(PackageSearchCriteria.CustomerId, CustomerId);

            if (!orderno.IsNullOrEmpty())
            {
                search.Add(PackageSearchCriteria.OrderNo, orderno);
            }

            if (!partno.IsNullOrEmpty())
            {
                search.Add(PackageSearchCriteria.PartNo, partno);
            }

            if (!trackingnumber.IsNullOrEmpty())
            {
                search.Add(PackageSearchCriteria.TrackingNumber, trackingnumber);
            }

            if (!packingno.IsNullOrEmpty())
            {
                search.Add(PackageSearchCriteria.PackingNo, packingno);
            }

            var packdetails = ServiceFactory.OrderService.FindOrderPackages(page, pageSize, search, null);

            var packdetailLists = packdetails.Data;

            var productInfos = ServiceFactory.ProductService.GetProductInfos(packdetailLists.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);

            var items = productInfos.Select(c => new PackageDetailInfoItemVo
                {
                    ProductInfo = c,
                    PackageDetail = packdetailLists.FirstOrDefault(w => w.ProductId == c.Product.ProductId),
                    PackageList = ServiceFactory.OrderService.GetOrderPackageByOrderId(packdetailLists.FirstOrDefault(w => w.ProductId == c.Product.ProductId).OrderId)//反查包裹主表信息
                }).ToList();


            var pageData = new PageData<PackageDetailInfoItemVo>
            {
                Data = items,
                Pager = packdetails.Pager
            };

            ViewBag.Sitemaps = Sitemap.GetMyAccountPackingSlipSitemap();
            if (Request.IsAjaxRequest())
            {
                return View("PackingSlipList", pageData);
            }
            return View(pageData);
        }

        /// <summary>
        /// 订单明细
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetail()
        {
            int page = Request["page"].ParseTo(1);//当前页
            int pageSize = Request["pagesize"].ParseTo(10);//页大小
            var orderno = Request["orderno"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);

            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            if (order.CustomerId != CustomerId)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }


            var orderDetail = ServiceFactory.OrderService.GetOrderDetsById(CustomerId, order.OrderId, page, pageSize, null);

            if (orderDetail.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }


            var orderDetailVo = new OrderDetailVo();
            orderDetailVo.Order = order;
            orderDetailVo.OrderShippingAddress =
                ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId);//获取货运地址
            if (!orderDetailVo.OrderShippingAddress.IsNullOrEmpty())
            {
                orderDetailVo.ShippingDay = ServiceFactory.ShippingService.GetShippingDay(order.ShippingId,
                                                                                          CacheHelper.GetCountryCode(
                                                                                              orderDetailVo
                                                                                                  .OrderShippingAddress
                                                                                                  .Country));
            }
            orderDetailVo.PaymentName = GetPaymentName(order.PaymentMethod.ToEnum<PaymentType>());
            orderDetailVo.ShippingName = CacheHelper.GetShippingName(order.ShippingId);//运送方式名称
            orderDetailVo.PackageList = new List<Package>();
            orderDetailVo.PackageList = ServiceFactory.OrderService.GetOrderPackageByOrderId(order.OrderId);

            var orderdetailLists = orderDetail.Data;
            var productInfos = ServiceFactory.ProductService.GetProductInfos(orderdetailLists.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);

            var items = productInfos.Select(c => new OrderDetailItemVo { ProductInfo = c, OrderDetail = orderdetailLists.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();

            var pageData = new PageData<OrderDetailItemVo>
            {
                Data = items,
                Pager = orderDetail.Pager
            };
            orderDetailVo.OrderDetailList = pageData;


            if (Request.IsAjaxRequest())
            {
                return View("OrderDetailList", orderDetailVo);
            }
            //订单状态List
            orderDetailVo.OrderStatusHistoryList =
                ServiceFactory.OrderService.GetOrderStatusHistoryByOrderId‎(order.OrderId);
            var statusname =
                CacheHelper.GetAllCustomerOrderStatus.First(
                    x => x.Value == orderDetailVo.Order.OrderStatus.ParseTo<int>()).Name;
            ViewBag.Sitemaps = Sitemap.GetMyAccountOrderSearchDetailsSitemap(orderno, orderDetailVo.Order.OrderStatus.ParseTo<int>(), statusname);
            return View(orderDetailVo);
        }

        /// <summary>
        /// 订单快照
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderSnapshot()
        {
            var orderdetialid = Request["id"].ParseTo(-1);

            #region 判断快照是否存在
            if (orderdetialid < 1)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            OrderImpression orderImpression = ServiceFactory.OrderService.GetOrderImpressionByOrderId‎(orderdetialid);
            if (orderImpression.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            #endregion

            var snap = new OrderItemSnapshotVo { OrderSnapshot = orderImpression };
            var detailitem = ServiceFactory.OrderService.GetOrderDetsById(CustomerId, orderImpression.OrderItemId);
            if (detailitem.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            snap.OrderDetail = detailitem;

            var order = ServiceFactory.OrderService.GetOrderByOrderId(detailitem.OrderId);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            snap.Order = order;
            var productInfo = ServiceFactory.ProductService.GetProductInfos(new List<int>() { detailitem.ProductId }, false, false, false, false, false, false);
            if (productInfo.IsNullOrEmpty() || productInfo.Count < 1)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            snap.ProductInfo = productInfo[0];
            return View(snap);
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetailPayment()
        {
            var orderno = Request["orderno"] ?? string.Empty;
            var currentCustomerId = SessionHelper.CurrentCustomer.CustomerId;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var orderDetailVo = new OrderDetailVo();
            orderDetailVo.Order = order;
            orderDetailVo.OrderShippingAddress =
                ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId);//获取货运地址
            if (!orderDetailVo.OrderShippingAddress.IsNullOrEmpty())
            {
                orderDetailVo.ShippingDay = ServiceFactory.ShippingService.GetShippingDay(order.ShippingId,
                                                                                          CacheHelper.GetCountryCode(
                                                                                              orderDetailVo
                                                                                                  .OrderShippingAddress
                                                                                                  .Country));
            }
            orderDetailVo.PaymentName = GetPaymentName(order.PaymentMethod.ToEnum<PaymentType>());
            orderDetailVo.ShippingName = CacheHelper.GetShippingName(order.ShippingId);//运送方式名称

            int countryId = orderDetailVo.OrderShippingAddress.IsNullOrEmpty() ? 0 : orderDetailVo.OrderShippingAddress.Country;
            int currencyId = CacheHelper.GetCurrencyByCode(order.Currency).CurrencyId;
            //  客户存在欠款
            ViewBag.debtCashUsd = ServiceFactory.CashService.GetCustomerArrear(currentCustomerId);
            //  客户Cash
            ViewBag.cashBalanceUsd = ServiceFactory.CashService.GetCustomerBalance(currentCustomerId);

            var countryAll = ServiceFactory.ConfigureService.GetAllCountryLanguages();
            var countryLanguages = countryAll.Where(x => x.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).ToList();
            var commonCountries = ServiceFactory.ConfigureService.GetCommonCountry();
            ViewBag.commonCountryLanguages = commonCountries.Select(x => new CountryLanguage { CountryId = x.CountryId, CountryName = countryLanguages.FirstOrDefault(w => w.CountryId == x.CountryId && w.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId).CountryName }).ToList();
            ViewBag.countryLanguages = countryLanguages;
            ViewBag.countryLanguage = ServiceFactory.ConfigureService.GetCountryLanguage(countryId, ServiceFactory.ConfigureService.SiteLanguageId);

            ViewBag.gcErrorMessage = SessionHelper.GlobalCollectErrorMessage ?? null;
            SessionHelper.GlobalCollectErrorMessage = null;

            #region 判断是否能够使用支付配置
            ViewBag.canUseBankOfChina = ServiceFactory.PaymentService.CanUseBankOfChina(currentCustomerId, countryId, currencyId);
            ViewBag.canUseWesternUnion = ServiceFactory.PaymentService.CanUseWesternUnion(currentCustomerId, countryId, currencyId);
            ViewBag.canUseHsbc = ServiceFactory.PaymentService.CanUseHsbc(currentCustomerId, countryId, currencyId);
            ViewBag.canUseMoneyGram = ServiceFactory.PaymentService.CanUseMoneyGram(currentCustomerId, countryId, currencyId);
            ViewBag.canUsePaypal = ServiceFactory.PaymentService.CanUsePaypal(currentCustomerId, countryId, currencyId);
            //ViewBag.canUsePaypalExpress = ServiceFactory.PaymentService.CanUsePaypalExpress(currentCustomerId, countryId, currencyId);
            ViewBag.canUseGlobalCollect = ServiceFactory.PaymentService.CanUseGlobalCollect(currentCustomerId, countryId, currencyId);
            ViewBag.canUseWebmoney = ServiceFactory.PaymentService.CanUseOceanPayment("Webmoney", currentCustomerId, countryId, currencyId);
            ViewBag.canUseYandex = ServiceFactory.PaymentService.CanUseOceanPayment("Yandex", currentCustomerId, countryId, currencyId);
            ViewBag.canUseCreditCard = ServiceFactory.PaymentService.CanUseOceanPayment("Credit Card", currentCustomerId, countryId, currencyId);
            ViewBag.canUseQiWi = ServiceFactory.PaymentService.CanUseOceanPayment("QiWi", currentCustomerId, countryId, currencyId);
            ViewBag.canUseOceanPayment = ViewBag.canUseWebmoney || ViewBag.canUseYandex || ViewBag.canUseCreditCard || ViewBag.canUseQiWi;
            ViewBag.canUsePaypalCreditCard = !ViewBag.canUseGlobalCollect;
            #endregion

            #region 读取支付配置
            var paymentConfig = new Dictionary<string, object>();
            if (ViewBag.canUseBankOfChina)
            {
                paymentConfig.Add("BankOfChina", ServiceFactory.PaymentService.GetBankOfChinaConfig());
            }
            if (ViewBag.canUseWesternUnion)
            {
                paymentConfig.Add("WesternUnion", ServiceFactory.PaymentService.GetWesternUnionConfig());
            }
            if (ViewBag.canUseHsbc)
            {
                paymentConfig.Add("Hsbc", ServiceFactory.PaymentService.GetHsbcConfig());
            }
            if (ViewBag.canUseMoneyGram)
            {
                paymentConfig.Add("MoneyGram", ServiceFactory.PaymentService.GetMoneyGramConfig());
            }
            if (ViewBag.canUsePaypal)
            {
                paymentConfig.Add("Paypal", ServiceFactory.PaymentService.GetPaypalConfig());
            }
            //if (ViewBag.canUsePaypalExpress)
            //{
            //    paymentConfig.Add("PaypalExpress", ServiceFactory.PaymentService.GetPaypalExpressConfig());
            //}
            if (ViewBag.canUseGlobalCollect)
            {
                paymentConfig.Add("GlobalCollect", ServiceFactory.PaymentService.GetGlobalCollectConfig());
            }
            if (ViewBag.canUseOceanPayment)
            {
                paymentConfig.Add("OceanPayment", ServiceFactory.PaymentService.GetOceanPaymentConfig());
            }
            ViewBag.paymentConfig = paymentConfig;
            #endregion

            return View(orderDetailVo);
        }

        /// <summary>
        /// 快速Reorder
        /// </summary>
        /// <returns></returns>
        public ActionResult QuickReorder()
        {
            var orderno = Request["orderno"] ?? string.Empty;
            if (orderno.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            var orderDetail = ServiceFactory.OrderService.GetOrderDetsById(CustomerId, order.OrderId, 1, int.MaxValue, null);
            if (orderDetail.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            IList<ShoppingCartItem> cartItems = new List<ShoppingCartItem>();
            foreach (var item in orderDetail.Data)
            {
                cartItems.Add(
                new ShoppingCartItem()
                    {
                        ShoppingCartId = CustomerId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }
                );
            }

            ServiceFactory.ShoppingCartService.BatchAddProductToShoppingCart(cartItems);
            return RedirectToRoute("ShoppingCart");

        }

        #region 订单商品图片下载 

        /// <summary>
        /// 图片下载加密秘钥
        /// </summary>
        private static string _encryptKey = "560AF6B1";//对称加密秘钥，必须是8位字符
        /// <summary>
        /// 请求下载单张订单项图片
        /// </summary>
        [HttpPost]
        public ActionResult RequestDownloadImage()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var imageName = Request["imageName"] ?? string.Empty;
            var orderDetailId = Request["orderDetailId"].ParseTo(0);
            var customerId = CustomerId; 
            #endregion

            #region 验证数据
            //图片名称为空
            if (imageName.IsNullOrEmpty())
            {
                jsonData.Message = "ERROR_IMANGE_NAME_EMPTY";
                return Json(jsonData); 
            }

            //图片明细ID为空
            if (orderDetailId <=0)
            {
                jsonData.Message = "ERROR_ORDER_DETAIL_ID_EMPTY";
                return Json(jsonData);
            }

            //todo 需要验证该订单明细项是否属于当前客户的订单以及对应订单是否是发运完成或者客户签收状态，其他情况不允许下载
            #endregion

            var imagePath = ServiceFactory.OrderService.RequestDownloadImage(imageName);
            if (!imagePath.IsNullOrEmpty())
            {
                jsonData.Succeed = true;
                jsonData.Data = CryptHelper.EncryptDes(imagePath, _encryptKey).ToUrl();  
            } 

            return Json(jsonData,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 请求批量下载订单项图片
        /// </summary>
        [HttpPost]
        public ActionResult RequestDownloadImageBatch()
        {
            var jsonData = new JsonData();

            #region 获取参数
            var orderId = Request["order_id"].ParseTo(0); 
            var customerId = CustomerId;
            #endregion

            #region 验证数据  
            //图片明细为空
            if (orderId <= 0)
            {
                jsonData.Message = "ERROR_ORDER_ID_EMPTY";
                return Json(jsonData);
            }

            //todo 需要验证该订单明细项是否属于当前客户的订单以及对应订单是否是发运完成或者客户签收状态，其他情况不允许下载
            #endregion

            var imagePath = ServiceFactory.OrderService.RequestDownloadImageBatch(orderId);
            if (!imagePath.IsNullOrEmpty())
            {
                jsonData.Succeed = true;
                jsonData.Data = CryptHelper.EncryptDes(imagePath, _encryptKey).ToUrl(); 
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载订单项图片
        /// </summary>
        public ActionResult DownloadImageFile()
        {
            var imangeFileName = Request["fileName"] ?? string.Empty;
            var imageFilePath = Request["imageFilePath"] ?? string.Empty;
            var isAjax = Request["isAjax"].ParseTo(false);
            if (!imageFilePath.IsNullOrEmpty())
            {
                try
                {
                    var imageFilePathUrl = imageFilePath;
                    if (isAjax)
                    {
                        imageFilePathUrl = imageFilePathUrl.FromUrl();
                    }
                    var imageFileUrl = CryptHelper.DecryptDes(imageFilePathUrl, _encryptKey);

                    if (!imageFileUrl.IsNullOrEmpty())
                    {
                        var fileName = Path.GetFileName(imangeFileName);
                        var fileExtension = Path.GetExtension(imangeFileName);
                        var contentType = MimeHelper.GetContentType(fileExtension);

                        byte[] byteData = null;
                        try
                        {
                            byteData = _webClientHelper.DownloadData(imageFileUrl);
                        }
                        catch (Exception exception)
                        {
                            var subject = "DownloadPicture-下载图片出错-下载地址为:" + imageFileUrl + "\n\r,imangeFileName:" + imangeFileName + "\n\r,错误信息:" + exception.Message;
                            
                            LoggerHelper.GetLogger(LoggerType.Exception).Error(subject, exception);
                        }

                        if (!byteData.IsNullOrEmpty())
                        {
                            return File(byteData, contentType, fileName);
                        }
                    }
                }
                catch (Exception exception)
                {
                    var subject = "DownloadPicture-下载图片出错-下载文件为:" + imageFilePath + "\n\r,imangeFileName:" + imangeFileName + "\n\r,错误信息:" + exception.Message;

                    LoggerHelper.GetLogger(LoggerType.Exception).Error(subject, exception);
                }
            }

            return Content("");
        }


        #endregion

        /// <summary>
        /// 查看线下支付信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewInfo()
        {
            var orderNo = Request["orderno"] ?? string.Empty;
            if (orderNo.IsNullOrEmpty())
            {
                return View("ViewInfo/Default", null);
            }
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderNo);
            if (order != null)
            {
                switch (order.PaymentMethod.ToEnum<PaymentType>())
                {
                    case PaymentType.Paypal:
                        break;
                    case PaymentType.Webmoney:
                        break;
                    case PaymentType.Yandex:
                        break;
                    case PaymentType.QiWi:
                        break;
                    case PaymentType.OceanCreditCard:
                        break;
                    case PaymentType.Gc:
                        break;
                    case PaymentType.Hsbc:
                        var hsbcinfos = ServiceFactory.PaymentService.GetHsbcInfo(order.OrderNo);
                        return View("ViewInfo/HSBCInfo", hsbcinfos);
                    case PaymentType.BankOfChina:
                        var bankOfChinaInfos = ServiceFactory.PaymentService.GetBankOfChinaInfo(order.OrderNo);
                        return View("ViewInfo/BankOfChinaInfo", bankOfChinaInfos);
                    case PaymentType.WesternUnion:
                        var westernUnionInfos = ServiceFactory.PaymentService.GetWesternUnionInfo(order.OrderNo);
                        return View("ViewInfo/WesternUnionInfo", westernUnionInfos);
                    case PaymentType.MoneyGram:
                        var moneyGramInfos = ServiceFactory.PaymentService.GetMoneyGramInfo(order.OrderNo);
                        return View("ViewInfo/MoneyGramInfo", moneyGramInfos);
                    default:
                        return View("ViewInfo/Default", null);
                }
            }
            return View("ViewInfo/Default", null);
        }

        /// <summary>
        /// 获取支付方式对应名称
        /// </summary>
        /// <param name="payment">支付类型</param>
        /// <returns>对应名称</returns>
        private string GetPaymentName(PaymentType payment)
        {
            switch (payment)
            {
                case PaymentType.Paypal:
                    return Lang.TipPaymentNamePaypal;

                case PaymentType.Hsbc:
                    return Lang.TipPaymentNameHsbc;

                case PaymentType.BankOfChina:
                    return Lang.TipPaymentNameBankOfChina;

                case PaymentType.WesternUnion://西联汇款
                    return Lang.TipPaymentNameWesternUnion;

                case PaymentType.Gc://GC信用卡
                    return Lang.TipPaymentNameGc;

                case PaymentType.MoneyGram://MoneyGram汇款
                    return Lang.TipPaymentNameMoneyGram;

                case PaymentType.Webmoney://Webmoney支付
                    return Lang.TipPaymentNameWebmoney;

                case PaymentType.Yandex://Yandex支付
                    return Lang.TipPaymentNameYandex;

                case PaymentType.QiWi://QiWi支付
                    return Lang.TipPaymentNameQiWi;

                case PaymentType.OceanCreditCard://钱海信用卡支付
                    return Lang.TipPaymentNameOceanCreditCard;

                case PaymentType.Cash://Cash全额支付
                    return Lang.TipPaymentNameCash;

                default:
                    return "找不到对应";

            }
        }

        public ActionResult OrderInvoice()
        {
            var orderno = Request["orderno"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);

            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            if (order.CustomerId != CustomerId)
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }

            var orderDetail = ServiceFactory.OrderService.GetOrderDetailListByOrderId(order.OrderId);

            if (orderDetail.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var orderInvoiceVo = new OrderInvoiceVo();
            orderInvoiceVo.Order = order;
            orderInvoiceVo.OrderShippingAddress =
                ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId);//获取货运地址
            orderInvoiceVo.PaymentName = GetPaymentName(order.PaymentMethod.ToEnum<PaymentType>());
            orderInvoiceVo.ShippingName = CacheHelper.GetShippingName(order.ShippingId);//运送方式名称
            var productInfos = ServiceFactory.ProductService.GetProductInfos(orderDetail.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);

            var items = productInfos.Select(c => new OrderDetailItemVo { ProductInfo = c, OrderDetail = orderDetail.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();
            orderInvoiceVo.OrderDetailList = items;
            return View(orderInvoiceVo);
        }

        public ActionResult ServerOrderInvoice()
        {
            var orderno = Request["orderno"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);

            if (order.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var orderDetail = ServiceFactory.OrderService.GetOrderDetailListByOrderId(order.OrderId);

            if (orderDetail.IsNullOrEmpty())
            {
                return RedirectToRoute(CommonConfigHelper.PageNotFoundRouteName);
            }
            var orderInvoiceVo = new OrderInvoiceVo();
            orderInvoiceVo.Order = order;
            orderInvoiceVo.OrderShippingAddress =
                ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId);//获取货运地址
            orderInvoiceVo.PaymentName = GetPaymentName(order.PaymentMethod.ToEnum<PaymentType>());
            orderInvoiceVo.ShippingName = CacheHelper.GetShippingName(order.ShippingId);//运送方式名称
            var productInfos = ServiceFactory.ProductService.GetProductInfos(orderDetail.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);

            var items = productInfos.Select(c => new OrderDetailItemVo { ProductInfo = c, OrderDetail = orderDetail.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();
            orderInvoiceVo.OrderDetailList = items;
            return View("OrderInvoice", orderInvoiceVo);
        }

        public ActionResult DownloadPdf()
        {
            string path = ConfigurationManager.AppSettings["PdfPath"] ?? string.Empty;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename=Guid.NewGuid() + ".pdf";
            var orderno = Request["orderno"] ?? string.Empty;
            string url = UrlFuncitonHelper.GetHost(true) + "/order/ServerOrderInvoice?orderno=" + orderno;
            try
            {
                PdfConvert.ConvertHtmlToPdf(new PdfDocument
                {
                    Url = url,
                },
                new PdfOutput
                {
                    OutputFilePath = Path.Combine(path, filename)
                });
            }
            catch (Exception exception)
            {
                //var subject = "DownloadPicture-下载图片出错-下载地址为:" + imageFileUrl + "\n\r,imangeFileName:" + imangeFileName + "\n\r,错误信息:" + exception.Message;
                //LoggerHelper.GetLogger(LoggerType.Exception).Error(subject, exception);
            }
            //return File(pdf, "application/pdf", "ExportPdf.pdf");
            return File(new FileStream(Path.Combine(path,filename), FileMode.OpenOrCreate), "application/stream", "ExportOrderInvoice.pdf");
        }

    }
}
