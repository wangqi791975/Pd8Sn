using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Payment;
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Models.Order;

namespace Com.Panduo.Web.Controllers
{
	public class MailMessageController : Controller
	{






        /// <summary>
        /// 客户注册
        /// </summary>
        /// <returns></returns>
		public ActionResult CustomerRegister()
		{
			var currentCurrency = PageHelper.GetCurrentCurrency();
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			ViewBag.CustomerEmail = "xiafeng.liu@panduo.com.cn";
			ViewBag.CouponAmount = 10M.ToExchangeCurrencyMoneyString(currentCurrency);
			return View();
		}

        /// <summary>
        /// 订单确认
        /// </summary>
        /// <returns></returns>
		public ActionResult OrderConfirmation()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var orderno = "SE1503261820";//Request["orderno"] ?? string.Empty;
			var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
			var orderDetail = ServiceFactory.OrderService.GetOrderDetailListByOrderId(order.OrderId);
			
			var orderInvoiceVo = new OrderInvoiceVo
			{
				Order = order,
				OrderShippingAddress = ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId),
				ShippingName = CacheHelper.GetShippingName(order.ShippingId)
			};
			var productInfos = ServiceFactory.ProductService.GetProductInfos(orderDetail.Select(p => p.ProductId).ToList(), isIncludeProductStock: true, isIncludeProductImage: false, isIncludeProductProperty: false, isIncludeProductPrice: true, isJudgeHotSeller: true, isJudgeHasSimilarProuct: false);

			var items = productInfos.Select(c => new OrderDetailItemVo { ProductInfo = c, OrderDetail = orderDetail.FirstOrDefault(w => w.ProductId == c.Product.ProductId) }).ToList();
			orderInvoiceVo.OrderDetailList = items;
			ViewBag.OrderInvoice = orderInvoiceVo;
			return View();
		}

        /// <summary>
        /// 订单状态变更
        /// </summary>
        /// <returns></returns>
		public ActionResult OrderStatusUpdated()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var orderno = "SE1503261820";//Request["orderno"] ?? string.Empty;
			var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
			ViewBag.Order = order;
			return View();
		}

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <returns></returns>
		public ActionResult OrderShipped()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var orderno = "SE1503261820";//Request["orderno"] ?? string.Empty;
			var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
			var orderInvoiceVo = new OrderInvoiceVo
			{
				Order = order,
				OrderShippingAddress = ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId),
				ShippingName = CacheHelper.GetShippingName(order.ShippingId)
			};
			ViewBag.OrderInvoice = orderInvoiceVo;
			return View();
		}

        /// <summary>
        /// VIP等级更新
        /// </summary>
        /// <returns></returns>
		public ActionResult VipLevelUpdated()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// Club付款成功
        /// </summary>
        /// <returns></returns>
		public ActionResult CustomerBecomeClub()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// 生日邮件送coupon
        /// </summary>
        /// <returns></returns>
		public ActionResult CustomerBirthday()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// Club客户生日邮件送coupon
        /// </summary>
        /// <returns></returns>
		public ActionResult ClubCustomerBirthday()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}
        
        /// <summary>
        /// 后台操作特殊订单,发送一封邮件给客户
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasesAmountUpdated()
		{
			ViewBag.CustomerFullName = "LXF";
            ViewBag.BannerUrl = "";
            var orderno = "SE1503261820";//Request["orderno"] ?? string.Empty;
            var order = ServiceFactory.OrderService.GetOrderByOrderNo(orderno);
            var orderInvoiceVo = new OrderInvoiceVo
            {
                Order = order,
                OrderShippingAddress = ServiceFactory.OrderService.GetOrderShippingAddressByOrderId‎(order.OrderId),
                ShippingName = CacheHelper.GetShippingName(order.ShippingId)
            };
            ViewBag.OrderInvoice = orderInvoiceVo;
			return View();
		}

        /// <summary>
        /// 销售回复Testimonial则需要发送一封邮件告知客户
        /// </summary>
        /// <returns></returns>
        public ActionResult TestimonialReply()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// 销售回复Review则需要发送一封邮件告知客户
        /// </summary>
        /// <returns></returns>
        public ActionResult ReviewReply()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// 客户上传的头像审核结果
        /// </summary>
        /// <returns></returns>
        public ActionResult AvatarVerifyResult()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// 创建Balance记录,发送邮件告知客户
        /// </summary>
        /// <returns></returns>
        public ActionResult Balance()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// 销售回复Suggestion则需要发送一封邮件告知客户
        /// </summary>
        /// <returns></returns>
        public ActionResult SuggestionReply()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// 销售修改客户邮箱,发送邮件告知客户
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerLoginEmailChanged()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
		}

        /// <summary>
        /// 成功修改密码邮件
        /// </summary>
        /// <returns></returns>
        public ActionResult PasswordUpdated()
		{
			ViewBag.CustomerFullName = "LXF";
			ViewBag.BannerUrl = "";
			var customerId = "SE1503261820";//Request["orderno"] ?? string.Empty;
			return View();
        }



        #region 前台web邮件模板
        #region OEM & Sourcing邮件模板
        /// <summary>
        ///OEM & Sourcing邮件模板（客户）
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerSourcingInformation()
        {
            ViewBag.Title = Request["title"] ?? string.Empty;
            ViewBag.Content = Request["content"] ?? string.Empty;
            ViewBag.Name = Request["fullname"] ?? string.Empty;
            ViewBag.Email = Request["email"] ?? string.Empty;
            return View();
        }

        /// <summary>
        /// OEM & Sourcing邮件模板（销售）
        /// </summary>
        /// <returns></returns>
        public ActionResult SourcingInformationToSalesTemplete()
        {
            ViewBag.Title = Request["title"] ?? string.Empty;
            ViewBag.Content = Request["content"] ?? string.Empty;
            ViewBag.Name = Request["fullname"] ?? string.Empty;
            ViewBag.Email = Request["email"] ?? string.Empty;
            return View();
        } 
        #endregion

        #region Suggestion邮件模板
        /// <summary>
        /// Suggestion邮件模板（客户）
        /// </summary>
        /// <returns></returns>
        public ActionResult SuggestionToCustomerTemplete()
        {
            int contentId = Request["contentId"].ParseTo<int>(0);
            var jj=ServiceFactory.SuggestionService.GetAllSuggestionItems(ServiceFactory.ConfigureService.SiteLanguageId);
            var model=ServiceFactory.SuggestionService.GetSuggestionContent(contentId);
            return View(model);
        }
        /// <summary>
        /// Suggestion邮件模板（销售）
        /// </summary>
        /// <returns></returns>
        public ActionResult SuggestionToSalesTemplete()
        {

            return View();
        } 
        #endregion

	    /// <summary>
        /// Testimonial邮件模板（销售）
        /// </summary>
        /// <returns></returns>
	    public ActionResult TestimonialTemplete()
	    {
            ViewBag.CustomerName = Request["customername"] ?? string.Empty;
            ViewBag.Testimonial = Request["testimonial"] ?? string.Empty;
            ViewBag.Email = Request["email"] ?? string.Empty;
            return View();
	    }

        /// <summary>
        /// 忘记密码邮件模板（客户）
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPwdTemplete()
	    {
            ViewBag.CustomerName = Request["name"] ?? string.Empty;
            ViewBag.Email = Request["email"] ?? string.Empty;
            ViewBag.ResetUrl = Request["resetUrl"] ?? string.Empty;
            ViewBag.CancelUrl = Request["cancelUrl"] ?? string.Empty;
            return View();
	    }


        /// <summary>
        ///客户提交头像邮件模板（销售）
        /// </summary>
        /// <returns></returns>
	    public ActionResult CustomerHeadTemplete()
	    {
            return View();
	    }

        /// <summary>
        ///商品详情页面Ask a question（销售）
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerAskQuestionTemplete()
	    {
            return View();
	    }

        /// <summary>
        ///商品详情页面-Review（销售）
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerReviewTemplete()
	    {
            return View();
	    }

        /// <summary>
        ///提交线下支付信息的邮件（销售）
        /// </summary>
        /// <returns></returns>
        public ActionResult MakePaymentToSalesTemplete()
	    {
            return View();
	    }

        /// <summary>
        ///提交线下支付信息的邮件（客户）
        /// </summary>
        /// <returns></returns>
        public ActionResult MakePaymentToCustomerTemplete()
        {
            return View();
        }


	    #region Contact Us
        /// <summary>
        /// Contact Us邮件模板（客户）
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactUsToCustomerTemplete()
        {
            ViewBag.CustomerName = Request["fullname"] ?? string.Empty;
            ViewBag.Email = Request["email"] ?? string.Empty;
            ViewBag.Message = Request["message"] ?? string.Empty;
            return View();
        }

        /// <summary>
        /// Contact Us邮件模板（销售）
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactUsToSalesTemplete()
        {
            ViewBag.CustomerName = Request["fullname"] ?? string.Empty;
            ViewBag.Email = Request["email"] ?? string.Empty;
            ViewBag.Message = Request["message"] ?? string.Empty;
            return View();
        } 
        #endregion

        /// <summary>
        /// 新增购物车未登陆邮件模板
        /// </summary>
        /// <returns></returns>
	    public ActionResult ShoppingCartNotUpdateTemplete()
	    {
            ViewBag.Name = Request["name"] ?? string.Empty;
	        return View();
	    }

	    #endregion
    }
}
