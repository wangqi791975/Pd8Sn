using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Marketing.Criteria;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.Service.Order.ShoppingCart;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.Web.Common;
using Resources;

namespace Com.Panduo.Web.Controllers
{
	public class ShoppingCartController : BaseController
	{
		/// <summary>
		/// 当前客户的ShoppingCartId
		/// </summary>
		private int ShoppingCartId { get { return SessionHelper.ShoppingCartId; } }
		/// <summary>
		/// 当前语种
		/// </summary>
		private int LanguageId { get { return ServiceFactory.ConfigureService.SiteLanguageId; } }

		public JsonResult GetProdCount()
		{
			var itemCount = ServiceFactory.ShoppingCartService.GetShoppingCartProductCount(ShoppingCartId);
			SessionHelper.CurrentShoppingCarItemCount = itemCount;
			var hashtable = new Hashtable { { "prodcount", itemCount } };
			return Json(hashtable);
		}

		public ActionResult ShoppingCart()
		{
			var customer = SessionHelper.CurrentCustomer;
			//CheckCustomerIsLogined
			if (!customer.IsNullOrEmpty())
			{
				if (CookieHelper.CurrentShoppingCartId.HasValue && CookieHelper.CurrentShoppingCartId.Value < 0)
				{
					//合并购物车
					ServiceFactory.ShoppingCartService.MergeShoppingCart‎(customer.CustomerId, CookieHelper.CurrentShoppingCartId.Value);
				}
				//从购物车里移除已经Check的下架物品
				ServiceFactory.ShoppingCartService.RemoveShoppingCartUnAvailableProduct(customer.CustomerId);
			}
			var itemCount = ServiceFactory.ShoppingCartService.GetShoppingCartProductCount(ShoppingCartId);
			SessionHelper.CurrentShoppingCarItemCount = itemCount;
			ViewBag.ShoppingCartItemCount = itemCount;

			ViewBag.UnAvailableProductCount = ServiceFactory.ShoppingCartService.GetShoppingCartUnAvailableProductCount(ShoppingCartId, LanguageId);

			#region test

			var page = Request[UrlParameterKey.Page].ParseTo(1); //当前页
			var pageSize = ConfigHelper.MaxShoppingCartItemPageSize; //页大小
			var sortMode = Request[UrlParameterKey.Sort].ParseTo(0).ToEnum<ShoppingCartSorterCriteria>();//sort.ToEnum<ShoppingCartSorterCriteria>();
			if (Request.QueryString[UrlParameterKey.Sort].IsNullOrEmpty())
				sortMode = CookieHelper.CurrentShoppingCartListSortMode.ToEnum<ShoppingCartSorterCriteria>();
			else
				CookieHelper.CurrentShoppingCartListSortMode = Request[UrlParameterKey.Sort].ParseTo<int>();

			var sorter = new List<Sorter<ShoppingCartSorterCriteria>>
			{
				new Sorter<ShoppingCartSorterCriteria> {Key = sortMode, IsAsc = true}
			};

			var search = new Dictionary<ShoppingCartSearchCriteria, object>()
			{
				{ShoppingCartSearchCriteria.LanguageId, LanguageId}
			};

			var pageData = ServiceFactory.ShoppingCartService.FindVShoppingCartItems(ShoppingCartId,
				page, pageSize, search, sorter);

			ViewBag.Sort = sortMode;
			#endregion

			return Request.IsAjaxRequest() ? View("Partial/ShoppingCartList", pageData) : View(pageData);
		}


		[ChildActionOnly]
		public ActionResult ShippingModal()
		{
			try
			{
				Country country = null;
				var shippingAddress = PageHelper.GetDefaultShippingAddress(out country, CookieHelper.CustomerCountryId);

				//可以通过前台传过来，没必要重新获取一次
				var shoppingCartSummary = ServiceFactory.ShoppingCartService.GetShoppingCartSummary(ShoppingCartId,
					PageHelper.GetCurrentLanguage().LanguageId, PageHelper.GetCurrentCurrency().CurrencyId,
					country.CountryId);

				ViewBag.ShoppingCartSummary = shoppingCartSummary;
				/*ViewBag.GrossWeight=
				ViewBag.VolumeWeight
				ViewBag.PackageWeight
				ViewBag.ShippingWeight*/

				ViewBag.Country = country;
				ViewBag.CommonCountry = ServiceFactory.ConfigureService.GetCommonCountry();
				ViewBag.UnCommonCountry = ServiceFactory.ConfigureService.GetUnCommonCountry();
			}
			catch (Exception ex)
			{
			}
			return PartialView("Partial/ShippingModal");
		}

		public ActionResult GetShippingItems(FormCollection form)
		{
			try
			{
				var countryId = form["countryId"].ParseTo<int>();
				var posatlCode = form["posatlCode"];
				var sort = form["sort"].ParseTo(0);
				var sortMode = sort.ToEnum<ShoppingAmountSorterCriteria>();
				Country country = null;
				var shippingAddress = PageHelper.GetDefaultShippingAddress(out country, countryId, posatlCode);

				if (!posatlCode.IsNullOrEmpty()) shippingAddress.ZipCode = posatlCode;

				//可以通过前台传过来，没必要重新获取一次
				var shoppingCartSummary = ServiceFactory.ShoppingCartService.GetShoppingCartSummary(ShoppingCartId,
					PageHelper.GetCurrentLanguage().LanguageId, PageHelper.GetCurrentCurrency().CurrencyId,
					country.CountryId);

				var shipppingCriteria = new ShipppingCriteria
				{
					CountryIsoCode2 = country.SimpleCode2,
					City = shippingAddress.City,
					PostCode = shippingAddress.ZipCode,
					GrossWeight = shoppingCartSummary.GrossWeight / 1000,
					VolumeWeight = shoppingCartSummary.ShippingWeight / 1000,
					ClubLevel =
						SessionHelper.CurrentCustomer.IsNullOrEmpty() ? 0 : SessionHelper.CurrentCustomer.ClubLevel,
					TotalAmount = shoppingCartSummary.OriginalProductAmount
				};

				#region 排序 前台js已经实现
				var sorter = new List<Sorter<ShoppingAmountSorterCriteria>>
				{
					new Sorter<ShoppingAmountSorterCriteria> {Key = sortMode, IsAsc = true}
				};
				var shippingAmounts = ServiceFactory.ShippingService.GetShippingAmounts(shipppingCriteria, sorter).ToList();
				//业务层未实现排序
				/*switch (sortMode)
				{
					case ShoppingAmountSorterCriteria.ShippingCostLowToHigh:
						shippingAmounts = shippingAmounts.OrderBy(x => x.TotalShippingCost).ToList();
						break;
					case ShoppingAmountSorterCriteria.ShippingCostHighToLow:
						shippingAmounts = shippingAmounts.OrderByDescending(x => x.TotalShippingCost).ToList();
						break;
					case ShoppingAmountSorterCriteria.ShippingTimeOldToNew:
						shippingAmounts = shippingAmounts.OrderBy(x => x.DayLow).ToList();
						break;
					case ShoppingAmountSorterCriteria.ShippingTimeNewToOld:
						shippingAmounts = shippingAmounts.OrderByDescending(x => x.DayLow).ToList();
						break;
				}*/
				#endregion

				var shippingId = CookieHelper.CustomerShippingId;
				if (shippingAmounts.Count > 0 && !shippingAmounts.Exists(x => x.ShippingId == shippingId))
				{
					var shippingAmount = shippingAmounts.Where(x => x.IsDefault).ToList();
					if (shippingAmount.Count > 0)
					{
						shippingId = shippingAmount[0].ShippingId;
					}
				}
				ViewBag.ShippingId = shippingId;
				ViewBag.ShippingLanguages = ServiceFactory.ShippingService.GetAllShippingDescs(LanguageId);
				ViewBag.ShippingAmounts = shippingAmounts;
				ViewBag.Sort = sortMode;
			}
			catch (Exception ex)
			{
			}
			return PartialView("Partial/_ShippingItems");
		}

		public JsonResult GetSummary(FormCollection form = null)
		{
			var countryId = 0;
			var shippingId = 0;
			var posatlCode = string.Empty;
			var couponCustomerId = 0;
			var currencyId = 0;
			if (form != null)
			{
				countryId = form["countryId"].ParseTo<int>();
				posatlCode = form["posatlCode"];
				shippingId = form["shippingid"].ParseTo<int>();
				couponCustomerId = form["couponCustomerId"].ParseTo<int>();
				currencyId = form["currencyId"].ParseTo<int>();
			}
			shippingId = shippingId == 0 ? CookieHelper.CustomerShippingId : shippingId;
			Country country = null;
			var shippingAddress = PageHelper.GetDefaultShippingAddress(out country, countryId, posatlCode);

			var currentCurrency = PageHelper.GetCurrentCurrency();
			if (currencyId > 0)
			{
				currentCurrency = ServiceFactory.ConfigureService.GetCurrency(currencyId);
			}

			var shoppingCartSummary = ServiceFactory.ShoppingCartService.GetShoppingCartSummary(ShoppingCartId,
				PageHelper.GetCurrentLanguage().LanguageId, currentCurrency.CurrencyId, country.CountryId);

			var shipppingCriteria = new ShipppingCriteria
			{
				CountryIsoCode2 = country.SimpleCode2,
				City = shippingAddress.Province,
				PostCode = shippingAddress.ZipCode,
				GrossWeight = shoppingCartSummary.GrossWeight / 1000,
				VolumeWeight = shoppingCartSummary.VolumeWeight / 1000,
				ClubLevel = SessionHelper.CurrentCustomer.IsNullOrEmpty() ? 0 : SessionHelper.CurrentCustomer.ClubLevel,
				TotalAmount = shoppingCartSummary.OriginalProductAmount
			};

			var shippingAmount = ServiceFactory.ShippingService.GetShippingAmount(shippingId, shipppingCriteria);
			if (shippingAmount.IsNullOrEmpty())
				shippingAmount = new ShippingAmount
				{
					HandlingFeeForClub = 0,
					ShippingCost = 0,
					RemoteAmount = 0,
					HandlingFeeForFreeShipping = 0
				};
			shoppingCartSummary.GrandTotal += shippingAmount.HandlingFeeForClub + shippingAmount.TotalShippingCost + shippingAmount.HandlingFeeForFreeShipping;

			#region Coupon
			var couponAmount = 0M;
			if (couponCustomerId > 0)
			{
				//满足条件的Coupon
				var couponCustomers = ServiceFactory.CouponService.GetUsableCoupons(ShoppingCartId,
					new Dictionary<AmountType, decimal>
					{
						{AmountType.NormalAmount, shoppingCartSummary.NoDiscountProductAmount},
						{AmountType.TotalAmount, shoppingCartSummary.GrandTotal}
					}, countryId,
					currentCurrency.CurrencyId, PageHelper.GetCurrentLanguage().LanguageId);
				var coupon = couponCustomers.FirstOrDefault(x => x.Id == couponCustomerId);
				if (!coupon.IsNullOrEmpty())
				{
					var currency = ServiceFactory.ConfigureService.GetCurrency(coupon.AmountCurrencyId ?? 1);
					couponAmount = PageHelper.ExchangeMoneyToUsd(coupon.Amount ?? 0, currency);
					shoppingCartSummary.GrandTotal = shoppingCartSummary.GrandTotal - couponAmount;
				}
			}

			#endregion

			#region 获取凑单提醒
			var criteria = new PiecingOrderCriteria
			{
				CustomerId = ShoppingCartId,//!SessionHelper.CurrentCustomer.IsNullOrEmpty() ? SessionHelper.CurrentCustomer.CustomerId : 0,
				ClubLevel = !SessionHelper.CurrentCustomer.IsNullOrEmpty() ? SessionHelper.CurrentCustomer.ClubLevel : 0,
				GrandTotal = shoppingCartSummary.GrandTotal,
				NoDiscountProductAmount = shoppingCartSummary.NoDiscountProductAmount,
			};
			var piecingOrderResult = ServiceFactory.MarketingService.GetPiecingOrderInfo(criteria); //new PiecingOrderResult { IsClubFreeShipping = false, HasPiecingOrderTip = true, OrderBalance = 49.2M, OrderDiscount = 0.8M };//
			#endregion
			var shippingLanguages = ServiceFactory.ShippingService.GetShippingDescById(LanguageId, shippingAmount.ShippingId);
			var hashtable = new Hashtable 
			{ 
			{ "OriginalProductAmount", shoppingCartSummary.OriginalProductAmount.ToExchangeCurrencyMoneyString(currentCurrency) }, //.ToString("N")
			{ "PromotionBeforeAmount", shoppingCartSummary.PromotionBeforeAmount.ToExchangeCurrencyMoneyString(currentCurrency) } , 
			{ "PromotionAmount", shoppingCartSummary.PromotionAmount.ToExchangeCurrencyMoneyString(currentCurrency) } , 
			{ "PromotionDiscountAmount", shoppingCartSummary.PromotionDiscountAmount.ToExchangeCurrencyMoneyString(currentCurrency) } , 
			{ "NoDiscountProductAmount", shoppingCartSummary.NoDiscountProductAmount.ToExchangeCurrencyMoneyString(currentCurrency) } , 
			{ "VipAndRcdDiscountAmount", shoppingCartSummary.VipAndRcdDiscountAmount.ToExchangeCurrencyMoneyString(currentCurrency) } , 
			{ "OrderDiscountAmount", shoppingCartSummary.OrderDiscountAmount.ToExchangeCurrencyMoneyString(currentCurrency) } , 
			{ "CouponAmount", couponAmount.ToExchangeCurrencyMoneyString(currentCurrency) }, 
			{ "GrandTotal", shoppingCartSummary.GrandTotal.ToExchangeCurrencyMoneyString(currentCurrency) }, 
			{ "ProductTotal", PageHelper.ExchangeMoneyByUsd(shoppingCartSummary.GrandTotal - shippingAmount.HandlingFeeForClub - shippingAmount.TotalShippingCost - shippingAmount.HandlingFeeForFreeShipping, currentCurrency) },
			{ "IsShowOrderDiscountAmount", shoppingCartSummary.OrderDiscountAmount>0 } , 
			{ "IsShowVipAndRcdDiscountAmount", shoppingCartSummary.VipAndRcdDiscountAmount>0 } , 
			{ "IsShowPromotionAmount", shoppingCartSummary.PromotionBeforeAmount>0 } ,  
			{ "IsShowNoDiscountProductAmount", shoppingCartSummary.NoDiscountProductAmount>0 } , 
			{ "ShippingWeight", shoppingCartSummary.ShippingWeight.ToString("N")+" g" } ,
			
			{ "ShippingCost", shippingAmount.TotalShippingCost.ToExchangeCurrencyMoneyString(currentCurrency) }, 
			{ "ShippingCostValue", PageHelper.ExchangeMoneyByUsd(shippingAmount.TotalShippingCost, currentCurrency) }, 
			{ "FreeShippingCost", shippingAmount.HandlingFeeForFreeShipping.ToExchangeCurrencyMoneyString(currentCurrency) }, 
			{ "IsFreeShipping", shippingAmount.HandlingFeeForFreeShipping>0 }, 
			{ "ShippingId", shippingAmount.ShippingId },
			{ "ShippingName", shippingLanguages.IsNullOrEmpty()?"":shippingLanguages.Name } ,
			{ "IsShowHandlingFee", shippingAmount.HandlingFeeForClub>0 }, 
			{ "HandlingFee", shippingAmount.HandlingFeeForClub.ToExchangeCurrencyMoneyString(currentCurrency) }, 
			//凑单提醒
			{ "HasPiecingOrderTip",  piecingOrderResult.HasPiecingOrderTip},
			{ "IsClubFreeShipping", piecingOrderResult.IsClubFreeShipping },
			{ "PiecingOrderDiscountLackOrderAmount", piecingOrderResult.OrderBalance.ToExchangeCurrencyMoneyString(currentCurrency) },
			{ "PiecingOrderDiscount", PageHelper.GetShowDiscount(piecingOrderResult.OrderDiscount) },

			//当前折扣提醒
			{ "HasCurrentDiscountTip",  shoppingCartSummary.HasCurrentDiscountTip},
			{ "CurrentDiscountType",  shoppingCartSummary.CurrentDiscountType},
			{ "CurrentDiscount",  PageHelper.GetShowDiscount(shoppingCartSummary.CurrentDiscount)},
			{ "ReplacingDiscount",  PageHelper.GetShowDiscount(shoppingCartSummary.ReplacingDiscount)},
			{ "ReplacingDiscountAmount",  shoppingCartSummary.ReplacingDiscountAmount.ToExchangeCurrencyMoneyString(currentCurrency)},
			{ "IsRcd",  !SessionHelper.CurrentCustomer.IsNullOrEmpty() && SessionHelper.CurrentCustomer.IsRcd},
			};
			return Json(hashtable);
		}

		public ActionResult GetUnAvailableProductList()
		{
			var page = Request[UrlParameterKey.Page].ParseTo(1); //当前页
			var pageSize = Request[UrlParameterKey.PageSize].ParseTo(ConfigHelper.MaxShoppingCartItemPageSize);//页大小
			var search = new Dictionary<ShoppingCartSearchCriteria, object>()
			{
				{ShoppingCartSearchCriteria.LanguageId, PageHelper.GetCurrentLanguage().LanguageId}
			};
			var pageData = ServiceFactory.ShoppingCartService.FindAllUnAvailableProducts(ShoppingCartId, page, pageSize, search, null);
			return View("Partial/ShoppingCartUnAvailableList", pageData);
		}

		public JsonResult GetShoppingCartItem(FormCollection form)
		{
			var productId = form["productId"].ParseTo<int>();
			//if (productId > 0) { }
			var shoppingCartItem = ServiceFactory.ShoppingCartService.GetShoppingCartProduct(ShoppingCartId, productId);
			var hashtable = new Hashtable
			{
			{"originalpriceformat",shoppingCartItem.OriginalPrice.ToExchangeCurrencyMoneyString(PageHelper.GetCurrentCurrency())},
			{"priceformat",shoppingCartItem.Price.ToExchangeCurrencyMoneyString(PageHelper.GetCurrentCurrency())},
			{"productsubtotalformat",shoppingCartItem.ProductSubTotal.ToExchangeCurrencyMoneyString(PageHelper.GetCurrentCurrency())},
			{"quantity",shoppingCartItem.Quantity},
			{"remark",shoppingCartItem.Remark},
			{"tip",shoppingCartItem.Tip},
			{"originalprice",shoppingCartItem.OriginalPrice},
			{"price",shoppingCartItem.Price},
			};
			return Json(hashtable);
		}

		public JsonResult AddToCart(FormCollection form)
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty }, { "shoppingcarturl", UrlRewriteHelper.GetShoppingCartUrl() } };
			try
			{
				if (CheckCurrentProdSeveral())
				{
					var productId = form["productId"].ParseTo<int>();
					var productQty = form["productQty"].ParseTo<int>();
					var remark = form["remark"];
					if (productId <= 0 || productQty <= 0)
					{
						hashtable["result"] = ActionJsonResult.Error;
						return Json(hashtable);
					}

					hashtable["result"] = ActionJsonResult.Success;
					var validQty = productQty;
					var prodStock = ServiceFactory.ProductService.GetProductStock(productId);
					if (!prodStock.IsNullOrEmpty() && prodStock.BindStockType == StockStatus.Bind &&
						prodStock.StockNumber - productQty <= 0)
					{
						validQty = prodStock.StockNumber;
						hashtable["msg"] = validQty;
						hashtable["result"] = ActionJsonResult.Warning;
					}

					#region Add To Cart

					var shoppingcartItem = new ShoppingCartItem
					{
						ShoppingCartId = ShoppingCartId,
						ProductId = productId,
						Quantity = validQty,
						Remark = remark
					};
					var saveResult = ServiceFactory.ShoppingCartService.AddProductToShoppingCart‎(shoppingcartItem);

					#endregion

					GetProdCount();
				}
				else
				{
					//弹出登陆框
					Response.StatusCode = 488;
					Response.StatusDescription = "";
				}
			}
			catch (Exception ex)
			{
				//记录日志
				hashtable["result"] = ActionJsonResult.Error;
				hashtable["msg"] = ex.Message;
			}
			return Json(hashtable);
		}

		public JsonResult BatchAddToCart(FormCollection form)
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty }, { "shoppingcarturl", UrlRewriteHelper.GetShoppingCartUrl() } };
			try
			{
				if (CheckCurrentProdSeveral())
				{
					var items = form["items"];
					if (items.IsNullOrEmpty())
					{
						hashtable["result"] = ActionJsonResult.Error;
						hashtable["msg"] = "no product";
						return Json(hashtable);
					}
					var lstShoppingcartItem = new List<ShoppingCartItem>();
					foreach (var shoppingcartItem in items.Split('|').Where(item => item.Contains(",")).Select(item => new ShoppingCartItem
					{
						ShoppingCartId = ShoppingCartId,
						ProductId = item.Split(',')[0].ParseTo<int>(),
						Quantity = item.Split(',')[1].ParseTo<int>(),
						Remark = string.Empty
					}).Where(shoppingcartItem => shoppingcartItem.ProductId > 0))
					{
						if (lstShoppingcartItem.Exists(x => x.ProductId == shoppingcartItem.ProductId))
							lstShoppingcartItem.RemoveAll(x => x.ProductId == shoppingcartItem.ProductId);
						lstShoppingcartItem.Add(shoppingcartItem);
					}
					if (lstShoppingcartItem.Any())
					{
						ServiceFactory.ShoppingCartService.BatchAddProductToShoppingCart(lstShoppingcartItem);
						hashtable["result"] = ActionJsonResult.Success;

						GetProdCount();
					}
					else
                        hashtable["msg"] = Lang.MsgPresenceProducts;
				}
				else
				{
                    hashtable["msg"] = Lang.MsgPleaseLogin;
					//弹出登陆框
					Response.StatusCode = 488;
					Response.StatusDescription = "";
				}
			}
			catch (BussinessException bexp)
			{
				hashtable["result"] = ActionJsonResult.Error;
				switch (bexp.GetError())
				{
					case "ERROR_SHOPPINGCART_ADD_PRODUCT":
                        hashtable["msg"] = Lang.MsgAddCartFail;
						break;
				}
			}
			catch (Exception ex)
			{
				//记录日志
				hashtable["result"] = ActionJsonResult.Error;
				hashtable["msg"] = ex.Message;
			}
			return Json(hashtable);
		}

		public JsonResult SaveRemark(FormCollection form)
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
			try
			{
				var productId = form["productId"].ParseTo<int>();
				if (productId < 0)
				{
					hashtable["result"] = ActionJsonResult.Error;
				}

				var remark = form["remark"];
				if (remark.IsNullOrEmpty())
				{
					hashtable["result"] = ActionJsonResult.Success;
					return Json(hashtable);
				}
				ServiceFactory.ShoppingCartService.SetShoppingCartProductRemark‎(ShoppingCartId, productId, remark);

				hashtable["result"] = ActionJsonResult.Success;
			}
			catch (BussinessException bexp)
			{
				hashtable["result"] = ActionJsonResult.Error;
				switch (bexp.GetError())
				{
					case "ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST":
                        hashtable["msg"] = Lang.MsgCartNotExist;
						break;
				}
			}
			catch (Exception ex)
			{
				//记录日志
				hashtable["result"] = ActionJsonResult.Error;
				hashtable["msg"] = ex.Message;
			}
			return Json(hashtable);
		}

		public JsonResult SaveQuantity(FormCollection form)
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
			try
			{
				var productId = form["productId"].ParseTo<int>();
				if (productId < 0)
				{
					hashtable["result"] = ActionJsonResult.Error;
				}

				var qty = form["qty"].ParseTo<int>();
				if (qty == -5)
				{
					//qty = ServiceFactory.ShoppingCartService.GetShoppingCartProductsQuantity(ShoppingCartId, productId);
					hashtable["msg"] = "";
                    hashtable["result"] = Lang.MsgNothing;
				}
				else if (qty <= 0)
				{
					ServiceFactory.ShoppingCartService.DeleteShoppingCartItemByProductId(ShoppingCartId, productId);
					var cartcount = ServiceFactory.ShoppingCartService.GetShoppingCartProductCount(ShoppingCartId);
					hashtable["msg"] = "";
					hashtable["result"] = ActionJsonResult.Success;
					if (cartcount <= 0)
						hashtable["result"] = ActionJsonResult.PageRefresh;
				}
				else
				{
					var validQty = qty;
					hashtable["msg"] = "";
					hashtable["result"] = ActionJsonResult.Warning;
					var prodStock = ServiceFactory.ProductService.GetProductStock(productId);
					if (!prodStock.IsNullOrEmpty() && prodStock.BindStockType == StockStatus.Bind)
					{
						if (prodStock.StockNumber <= 0)
						{
							//ServiceFactory.ShoppingCartService.DeleteShoppingCartItemByProductId(ShoppingCartId, productId);
							var product = ServiceFactory.ProductService.GetProductById(productId);
							product.Status = ProductStatus.OffLine;
							ServiceFactory.ProductService.UpdateProduct(product);
                            hashtable["msg"] = string.Format(Lang.MsgItemNoStock, product.ProductCode);
							hashtable["result"] = ActionJsonResult.Failing;
						}
						else
							if (prodStock.StockNumber - qty <= 0)
							{
								validQty = prodStock.StockNumber;
								hashtable["msg"] = validQty;
							}
					}
					ServiceFactory.ShoppingCartService.UpdateShoppingCartProductQty‎(ShoppingCartId, productId, validQty);
				}
			}
			catch (BussinessException bexp)
			{
				hashtable["result"] = ActionJsonResult.Error;
				switch (bexp.GetError())
				{
					case "ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST":
                        hashtable["msg"] = Lang.MsgCartNotExist;
						break;
				}
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
		/// 移动购物车中所有产品到Withlist
		/// </summary>
		public JsonResult MoveAllToWishlist()
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
			try
			{
				ServiceFactory.ShoppingCartService.MoveAllToWishlist(ShoppingCartId);
				hashtable["result"] = ActionJsonResult.Success;
				GetProdCount();
			}
			catch (BussinessException bexp)
			{
				hashtable["result"] = ActionJsonResult.Error;
				switch (bexp.GetError())
				{
					case "ERROR_CUSTOMER_NOT_EXIST":
                        hashtable["msg"] = Lang.MsgCustomerNotExist;
						break;
				}
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
		/// 移动购物车中所有产品到Withlist
		/// </summary>
		public JsonResult MoveToWishlist(FormCollection form)
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
			try
			{
				var productId = form["productId"].ParseTo<int>();
				if (productId < 0)
				{
					hashtable["result"] = ActionJsonResult.Error;
				}
				ServiceFactory.ShoppingCartService.MoveToWishlist(ShoppingCartId, productId);

				GetProdCount();
				var cartcount = ServiceFactory.ShoppingCartService.GetShoppingCartProductCount(ShoppingCartId);
				hashtable["result"] = ActionJsonResult.Success;
				if (cartcount <= 0)
					hashtable["result"] = ActionJsonResult.PageRefresh;
			}
			catch (BussinessException bexp)
			{
				hashtable["result"] = ActionJsonResult.Error;
				switch (bexp.GetError())
				{
					case "ERROR_CUSTOMER_NOT_EXIST":
                        hashtable["msg"] = Lang.MsgCustomerNotExist;
						break;
					case "ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST":
                        hashtable["msg"] = Lang.MsgCartNotExist;
						break;
				}
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
		/// 清空购物车
		/// </summary>
		public JsonResult CleanShoppingCart()
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
			try
			{
				ServiceFactory.ShoppingCartService.CleanShoppingCart(ShoppingCartId);
				hashtable["result"] = ActionJsonResult.Success;
				GetProdCount();
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
		/// 删除单个购物车产品
		/// </summary>
		public JsonResult DeleteItem(FormCollection form)
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
			try
			{
				var productId = form["productId"].ParseTo<int>();
				if (productId < 0)
				{
					hashtable["result"] = ActionJsonResult.Error;
				}
				ServiceFactory.ShoppingCartService.DeleteShoppingCartItemByProductId(ShoppingCartId, productId);

				GetProdCount();
				var cartcount = ServiceFactory.ShoppingCartService.GetShoppingCartProductCount(ShoppingCartId);
				hashtable["result"] = ActionJsonResult.Success;
				if (cartcount <= 0)
					hashtable["result"] = ActionJsonResult.PageRefresh;
			}
			catch (BussinessException bexp)
			{
				hashtable["result"] = ActionJsonResult.Error;
				switch (bexp.GetError())
				{
					case "ERROR_SHOPPINGCART_PRODUCT_NOT_EXIST":
                        hashtable["msg"] = Lang.MsgCartNotExist;
						break;
				}
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
		/// 快速添加判断产品是否存在
		/// </summary>
		public JsonResult GetProductByProductId(FormCollection form)
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty }, { "qty", 1 } };
			try
			{
				var productCode = form["productId"] as string;
				if (productCode.IsNullOrEmpty())
				{
					hashtable["result"] = ActionJsonResult.Error;
				}
				var product = ServiceFactory.ProductService.GetProductByCode(productCode);
				if (!product.IsNullOrEmpty())
				{
					hashtable["prodid"] = product.ProductId;
					if (product.Status != ProductStatus.OnSale && product.Status != ProductStatus.BackOrder)
					{
						hashtable["result"] = ActionJsonResult.Failing;
						hashtable["msg"] = "This item has been removed.";//不是用于前台显示消息，只用于区分
					}
					else
					{
						hashtable["result"] = ActionJsonResult.Success;
						var prodQty = ServiceFactory.ShoppingCartService.GetShoppingCartProductsQuantity(ShoppingCartId, product.ProductId);
						var prodStock = ServiceFactory.ProductService.GetProductStock(product.ProductId);
						if (!prodStock.IsNullOrEmpty() && prodStock.BindStockType == StockStatus.Bind)
						{
							//validQty = prodStock.StockNumber - productQty > 0 ? productQty : prodStock.StockNumber;
							hashtable["msg"] = string.Format("Limited Stock, Stock quantity :{0}", prodStock.StockNumber);//不是用于前台显示消息，只用于区分
							hashtable["maxqty"] = prodStock.StockNumber;
							hashtable["result"] = ActionJsonResult.Warning;
							prodQty = prodStock.StockNumber > 1 ? 1 : 0;
						}
						hashtable["qty"] = prodQty > 0 ? prodQty : 1;
					}
				}
				else
				{
					hashtable["result"] = ActionJsonResult.Failing;
					hashtable["msg"] = "err";
				}
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
		/// 判断当前客户是否可以继续向购物车添加产品
		/// </summary>
		/// <returns></returns>
		public bool CheckCurrentProdSeveral()
		{
			//客户未登录操作add to cart时，进行判断，若已经超过20款商品，则出现登录注册弹框。
			//例如，若第21款商品是backorder商品，则在backorder弹框中，点击Add to Cart的时候出现登录注册弹框
			if (ShoppingCartId >= 0) return true;

			var prodSeveral = ServiceFactory.ShoppingCartService.GetShoppingCartProductCount(ShoppingCartId);
			return prodSeveral < ConfigHelper.MaxNotLoggedShoppingCartItemCount;
		}

		public JsonResult ChangeShippingMethod(FormCollection form)
		{
			var hashtable = new Hashtable { { "result", ActionJsonResult.Failing }, { "msg", string.Empty } };
			try
			{
				var countryId = form["countryId"].ParseTo<int>();
				var posatlCode = form["posatlCode"];
				var shippingId = form["shippingid"].ParseTo<int>();
				if (shippingId <= 0)
				{
					hashtable["result"] = ActionJsonResult.Error;
				}
				if (countryId > 0)
					CookieHelper.CustomerCountryId = countryId;
				CookieHelper.CustomerShippingId = shippingId;
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

	}
}
