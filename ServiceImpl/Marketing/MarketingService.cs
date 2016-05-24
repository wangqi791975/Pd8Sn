using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Marketing;
using Com.Panduo.Service;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Marketing;
using Com.Panduo.Service.Marketing.Coupon;
using Com.Panduo.Service.Marketing.Criteria;
using Com.Panduo.Service.Marketing.Gift;
using Com.Panduo.Service.Marketing.PlaceOrder;
using Com.Panduo.Service.Marketing.ShippingMarketing;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.ServiceImpl.Marketing.Dao;
using Com.Panduo.ServiceImpl.Order.Dao;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;
using NHibernate.Mapping;

namespace Com.Panduo.ServiceImpl.Marketing
{
    public class MarketingService : IMarketingService
    {
        #region IOC
        public IMarketingDao MarketingDao { private get; set; }
        public IMarketingCustomerClaimDao MarketingCustomerClaimDao { private get; set; }

        #region 订单折扣
        public IMarketingOrderDiscountDao MarketingOrderDiscountDao { private get; set; }
        #endregion

        #region 运费活动
        public IMarketingShippingDao MarketingShippingDao { private get; set; }
        public IMarketingShippingDiscountDao MarketingShippingDiscountDao { private get; set; }
        public IMarketingShippingUpgradeDao MarketingShippingUpgradeDao { private get; set; }
        public IMarketingFreeShippingDao MarketingFreeShippingDao { private get; set; }
        public IVMarketingMarketingShippingDao VMarketingMarketingShippingDao { private get; set; }
        #endregion

        #region 下单
        public IMarketingPlaceOrderDao MarketingPlaceOrderDao { private get; set; }
        #endregion

        #region Coupon
        public IMarketingCouponDao MarketingCouponDao { private get; set; }
        public IVMarketingCouponDao VMarketingCouponDao { private get; set; }
        #endregion

        #region Gift
        public IMarketingGiftDao MarketingGiftDao { private get; set; }
        #endregion

        public IOrderDao OrderDao { private get; set; }
        #endregion

        #region 常量
        public string ERROR_MARKETING_NOT_EXIST { get { return "ERROR_MARKETING_NOT_EXIST"; } }
        /// <summary>
        /// 导入客户条件为空
        /// </summary>
        public string ERROR_MARKETING_CUSTOMERCLAIM_ISNULL { get { return "ERROR_MARKETING_CUSTOMERCLAIM_ISNULL"; } }
        /// <summary>
        /// 订单不存在
        /// </summary>
        public string ERROR_ORDER_NOT_EXIST { get; private set; }
        #endregion

        #region 后台

        /// <summary>
        /// 保存活动主表
        /// </summary>
        /// <param name="marketing"></param>
        private void SetMarketing(Service.Marketing.Marketing marketing)
        {
            //  1.保存营销主表
            var marketingPo = MarketingDao.GetObject(marketing.Id);
            if (marketingPo.IsNullOrEmpty())
            {
                marketingPo = GetMarketingPoFromVo(marketing);
                marketing.Id = MarketingDao.AddObject(marketingPo);
            }
            else
            {
                marketingPo.Name = marketing.Name;
                marketingPo.CustomerType = marketing.CustomerType.ParseTo<int>();
                marketingPo.Targetcustomerviplevel = string.Join("|", marketing.CustomerVipIds);
                marketingPo.Isexcludeclub = marketing.IsExcludeClub;
                marketingPo.Isexcludechannels = marketing.IsExcludeChannels;
                marketingPo.Targetcountry = string.Join("|", marketing.CountryIds);
                marketingPo.Targetcurrencies = string.Join("|", marketing.CurrencyIds);
                marketingPo.Targetlanguages = string.Join("|", marketing.LanguageIds);
                marketingPo.Effectivebegin = marketing.EffectiveBegin;
                marketingPo.Effectiveend = marketing.EffectiveEnd;
                marketingPo.MarketingType = marketing.MarketingType.ParseTo<int>();
                marketingPo.Status = marketing.Status;
                marketingPo.AmountType = marketing.AmountType.ParseTo<int>();
                marketingPo.Lastmodifytime = DateTime.Now;
                marketingPo.Lastmodifywho = marketing.LastModifyWho;

                MarketingDao.UpdateObject(marketingPo);
            }

            //  2.设置导入客户
            SetMarketingCustomerClaim(marketing);
        }

        /// <summary>
        /// 设置活动导入客户条件
        /// </summary>
        /// <param name="marketing"></param>
        private void SetMarketingCustomerClaim(Service.Marketing.Marketing marketing)
        {
            //因为不存在客户管理功能，此处要先删除之前导入的客户
            MarketingCustomerClaimDao.DeleteObjectByHql("delete from MarketingCustomerClaimPo where MarketingId=?", new object[] { marketing.Id });
            if (marketing.CustomerType != MarketingCustomerType.ImportCustomer) return;
            if (marketing.CustomerInfo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_CUSTOMERCLAIM_ISNULL);

            var lstCustomerClaim = marketing.CustomerInfo.Select(customerInfo => new MarketingCustomerClaimPo
            {
                MarketingId = marketing.Id,
                CustomerId = customerInfo.Key,
                Email = customerInfo.Value,
                ImportTime = DateTime.Now
            });
            MarketingCustomerClaimDao.AddObjects(lstCustomerClaim);
        }

        /// <summary>
        /// 设置活动状态
        /// </summary>
        /// <param name="marketingId">活动Id</param>
        /// <param name="isValid">状态</param>
        public void SetMarketingStatus(int marketingId, bool isValid)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);
            }
            marketingPo.Status = isValid;
            MarketingDao.UpdateObject(marketingPo);
        }

        #region 订单折扣活动

        /// <summary>
        /// 设置订单活动
        /// </summary>
        /// <param name="marketing">订单活动实体</param>
        /// <param name="orderAmountDiscounts"></param>
        public void SetOrderDiscountMarketing(OrderDiscountMarketing marketing, List<OrderAmountDiscount> orderAmountDiscounts)
        {
            #region 1.修改Marketing主表
            marketing.MarketingType = MarketingType.OrderDiscount;
            //marketing.AmountType = MarketingAmountType.NoDiscountTotalAmount;
            SetMarketing(marketing);
            #endregion

            #region 2.设置订单折扣明细
            //if (MarketingOrderDiscountDao.GetOneObject("from MarketingOrderDiscountPo where MarketingId=?", new object[] { marketing.Id }) != null)
            MarketingOrderDiscountDao.DeleteObjectByHql("delete from MarketingOrderDiscountPo where MarketingId=?", marketing.Id);

            var lstOrderDiscountPo = orderAmountDiscounts.Select(x => new MarketingOrderDiscountPo
            {
                MarketingId = marketing.Id,
                Amount = x.Amount,
                Discount = x.Discount
            });
            MarketingOrderDiscountDao.AddObjects(lstOrderDiscountPo);
            #endregion
        }


        /// <summary>
        /// 删除订单活动
        /// </summary>
        /// <param name="marketingId"></param>
        public void DeleteOrderDiscountMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);
            MarketingCustomerClaimDao.DeleteObjectByHql("delete from MarketingCustomerClaimPo where MarketingId=?", new object[] { marketingId });
            MarketingOrderDiscountDao.DeleteObjectByHql("delete from MarketingOrderDiscountPo where MarketingId=?", new object[] { marketingId });

            MarketingDao.DeleteObjectById(marketingId);
        }

        /// <summary>
        /// 根据ID获取订单活动
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        public OrderDiscountMarketing GetOrderDiscountMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            var marketingOrderDiscountPos = MarketingOrderDiscountDao.FindDataByHql("from MarketingOrderDiscountPo where MarketingId=?", new object[] { marketingId });

            return OrderDiscountMarketingPoConvertToVo(marketingPo, marketingOrderDiscountPos);
        }

        /// <summary>
        /// OrderDiscountMarketing PO 转 VO
        /// </summary>
        private OrderDiscountMarketing OrderDiscountMarketingPoConvertToVo(MarketingPo marketingPo, IEnumerable<MarketingOrderDiscountPo> marketingOrderDiscountPos = null)
        {
            if (marketingPo.IsNullOrEmpty())
                return null;

            if (marketingOrderDiscountPos != null)
                return new OrderDiscountMarketing
                {
                    Id = marketingPo.Id,
                    Name = marketingPo.Name,
                    AmountType = marketingPo.AmountType.ToEnum<MarketingAmountType>(),
                    MarketingType = marketingPo.MarketingType.ToEnum<MarketingType>(),
                    CustomerType = marketingPo.CustomerType.ToEnum<MarketingCustomerType>(),
                    IsExcludeClub = marketingPo.Isexcludeclub,
                    IsExcludeChannels = marketingPo.Isexcludechannels,
                    CustomerVipIds = marketingPo.Targetcustomerviplevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CountryIds = marketingPo.Targetcountry.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CurrencyIds = marketingPo.Targetcurrencies.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    LanguageIds = marketingPo.Targetlanguages.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    EffectiveBegin = marketingPo.Effectivebegin,
                    EffectiveEnd = marketingPo.Effectiveend,
                    Status = marketingPo.Status,
                    LastModifyWho = marketingPo.Lastmodifywho,
                    LastModifyTime = marketingPo.Lastmodifytime,
                    OrderAmountDiscounts = marketingOrderDiscountPos.Select(x => new OrderAmountDiscount
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        Discount = x.Discount
                    }).ToList()
                };
            else

                return new OrderDiscountMarketing
                {
                    Id = marketingPo.Id,
                    Name = marketingPo.Name,
                    AmountType = marketingPo.AmountType.ToEnum<MarketingAmountType>(),
                    MarketingType = marketingPo.MarketingType.ToEnum<MarketingType>(),
                    CustomerType = marketingPo.CustomerType.ToEnum<MarketingCustomerType>(),
                    IsExcludeClub = marketingPo.Isexcludeclub,
                    IsExcludeChannels = marketingPo.Isexcludechannels,
                    CustomerVipIds = marketingPo.Targetcustomerviplevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CountryIds = marketingPo.Targetcountry.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CurrencyIds = marketingPo.Targetcurrencies.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    LanguageIds = marketingPo.Targetlanguages.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    EffectiveBegin = marketingPo.Effectivebegin,
                    EffectiveEnd = marketingPo.Effectiveend,
                    Status = marketingPo.Status,
                    LastModifyWho = marketingPo.Lastmodifywho,
                    LastModifyTime = marketingPo.Lastmodifytime,
                    OrderAmountDiscounts = new List<OrderAmountDiscount>()
                };
        }

        /// <summary>
        /// 获取订单活动折扣明细
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        public List<OrderAmountDiscount> GetOrderAmountDiscounts(int marketingId)
        {
            if (MarketingDao.ExistObject("Id", marketingId))
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            var lstOrderAmountDiscountPo = MarketingOrderDiscountDao.FindDataByHql("from MarketingOrderDiscountPo where MarketingId=?", new object[] { marketingId });

            if (lstOrderAmountDiscountPo.IsNullOrEmpty()) return new List<OrderAmountDiscount>();
            return lstOrderAmountDiscountPo.Select(x => new OrderAmountDiscount
            {
                Id = x.Id,
                Amount = x.Amount,
                Discount = x.Discount
            }).ToList();
        }

        /// <summary>
        /// 分页查询订单折扣活动后台管理列表 
        /// </summary>
        public PageData<OrderDiscountMarketing> FindOrderDiscountMarketings(int currentPage, int pageSize,
            IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM MarketingPo");
            //1.构建查询条件
            if (!searchDictionary.IsNullOrEmpty())
            {
                searchDictionary.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case MarketingSearchCriteria.MarketingType:
                            hqlHelper.AddWhere("MarketingType", HqlOperator.Eq, "MarketingType", item.Value);
                            break;
                        case MarketingSearchCriteria.Name:
                            hqlHelper.AddWhere("Name", HqlOperator.Like, "Name", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        case MarketingSorterCriteria.Status:
                            hqlHelper.AddSorter("Status", sorter.IsAsc);
                            break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("Id", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = MarketingDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<OrderDiscountMarketing>();
            var voList = pageDataPo.Data.Select(orderdiscount => OrderDiscountMarketingPoConvertToVo(orderdiscount)).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        #endregion

        #region 运费活动

        private void SetShippingMarketing(ShippingMarketing marketing)
        {
            var marketingShippingPo = MarketingShippingDao.GetObject(marketing.ShippingMarketingId);
            if (marketingShippingPo.IsNullOrEmpty())
            {
                marketingShippingPo = GetShippingMarketingPoFromVo(marketing);
                marketing.ShippingMarketingId = MarketingShippingDao.AddObject(marketingShippingPo);
            }
            else
            {
                marketingShippingPo.Marketingid = marketing.Id;
                marketingShippingPo.Rewardtype = (int)marketing.RewardType;
                marketingShippingPo.Shippingids = string.Join("|", marketing.ShippingIds);
                marketingShippingPo.WeightType = (int)marketing.WeightType;
                marketingShippingPo.WeightLimit = marketing.WeightLimit;
                MarketingShippingDao.UpdateObject(marketingShippingPo);
            }
        }

        /// <summary>
        /// 删除运费活动
        /// </summary>
        /// <param name="marketingId"></param>
        public void DeleteShippingMarketingById(int marketingId)
        {
            if (MarketingDao.GetObject(marketingId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);
            }
            MarketingCustomerClaimDao.DeleteObjectByHql("delete from MarketingCustomerClaimPo where MarketingId=?", new object[] { marketingId });

            var shippingMarketingPo = MarketingShippingDao.GetMarketingShippingByMarketingId(marketingId);
            if (shippingMarketingPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);
            }

            switch (shippingMarketingPo.Rewardtype.ParseTo<int>().ToEnum<ShippingRewardType>())
            {
                case ShippingRewardType.ShippingDiscount:
                    MarketingShippingDiscountDao.DeleteObjectByHql("delete from MarketingShippingDiscountPo where Marketingshippingid=?", shippingMarketingPo.Id);
                    break;
                case ShippingRewardType.ShippingUpgrade:
                    MarketingShippingUpgradeDao.DeleteObjectByHql("delete from MarketingShippingUpgradePo where Marketingshippingid=?", shippingMarketingPo.Id);
                    break;
                case ShippingRewardType.FreeShipping:
                default:
                    MarketingFreeShippingDao.DeleteObjectByHql("delete from MarketingFreeShippingPo where Marketingshippingid=?", shippingMarketingPo.Id);
                    break;
            }

            MarketingShippingDao.DeleteObjectById(shippingMarketingPo.Id);
            MarketingDao.DeleteObjectById(marketingId);
        }

        /// <summary>
        /// 根据ID获取运费活动
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        public ShippingMarketing GetShippingMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);
            }
            return GetShippingMarketingVoFromPo(marketingPo);
        }

        /// <summary>
        /// 分页查询运费活动后台管理列表 
        /// </summary>
        public PageData<ShippingMarketing> FindShippingMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM VMarketingMarketingShippingPo");
            //1.构建查询条件
            if (!searchDictionary.IsNullOrEmpty())
            {
                searchDictionary.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case MarketingSearchCriteria.MarketingType:
                            hqlHelper.AddWhere("MarketingType", HqlOperator.Eq, "MarketingType", item.Value);
                            break;
                        case MarketingSearchCriteria.Name:
                            hqlHelper.AddWhere("Name", HqlOperator.Like, "Name", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        case MarketingSorterCriteria.Status:
                            hqlHelper.AddSorter("Status", sorter.IsAsc);
                            break;
                        case MarketingSorterCriteria.IdAsc:
                            hqlHelper.AddSorter("Id", true);
                            break;
                        case MarketingSorterCriteria.IdDesc:
                            hqlHelper.AddSorter("Id", false);
                            break;
                        case MarketingSorterCriteria.EffectiveBeginAsc:
                            hqlHelper.AddSorter("EffectiveBegin", true);
                            break;
                        case MarketingSorterCriteria.EffectiveBeginDesc:
                            hqlHelper.AddSorter("EffectiveBegin", false);
                            break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("Id", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = VMarketingMarketingShippingDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ShippingMarketing>();
            var mpo = new MarketingPo { };
            var voList = pageDataPo.Data.Select(po => GetShippingMarketingVoFromPo(ObjectHelper.CopyProperties<VMarketingMarketingShippingPo, MarketingPo>(po, mpo, new[] { "Rewardtype" }), false)).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        #region 免运费

        /// <summary>
        /// 设置免运费活动
        /// </summary>
        /// <param name="marketing">运费活动实体</param>
        /// <param name="freeShipping">FreeShipping实体</param>
        public void SetFreeShippingMarketing(ShippingMarketing marketing, FreeShipping freeShipping)
        {
            //  1.保存活动主表 和 设置导入客户条件
            marketing.MarketingType = MarketingType.Shipping;
            SetMarketing(marketing);

            //  2.保存运费活动主表
            marketing.RewardType = ShippingRewardType.FreeShipping;
            SetShippingMarketing(marketing);

            //  3.修改免运费表
            //MarketingFreeShippingDao.DeleteObjectByHql("delete from MarketingFreeShippingPo where Marketingshippingid=?", freeShipping.Marketingshippingid);
            var freeShippingPo = MarketingFreeShippingDao.GetObject(freeShipping.Id);
            if (freeShippingPo.IsNullOrEmpty())
            {
                freeShippingPo = new MarketingFreeShippingPo
                {
                    Marketingshippingid = marketing.ShippingMarketingId,
                    Amount = freeShipping.Amount,
                    Baseshippingid = freeShipping.Baseshippingid,
                    Freeshippingfee = freeShipping.FreeShippingFee
                };
                MarketingFreeShippingDao.AddObject(freeShippingPo);
            }
            else
            {
                freeShippingPo.Marketingshippingid = marketing.ShippingMarketingId;
                freeShippingPo.Amount = freeShipping.Amount;
                freeShippingPo.Baseshippingid = freeShipping.Baseshippingid;
                freeShippingPo.Freeshippingfee = freeShipping.FreeShippingFee;
                MarketingFreeShippingDao.UpdateObject(freeShippingPo);
            }
        }

        /// <summary>
        /// 获取免运费活动明细
        /// </summary>
        /// <param name="marketingShippingId">MarketingShippingPo.Id</param>
        /// <returns></returns>
        public FreeShipping GetFreeShipping(int marketingShippingId)
        {
            var marketingFreeShippingPo = MarketingFreeShippingDao.GetOneObject("FROM MarketingFreeShippingPo where Marketingshippingid = ?", marketingShippingId);
            if (marketingFreeShippingPo.IsNullOrEmpty())
            {
                return new FreeShipping { };
            }
            return new FreeShipping
            {
                Id = marketingFreeShippingPo.Id,
                Marketingshippingid = marketingFreeShippingPo.Marketingshippingid,
                Amount = marketingFreeShippingPo.Amount,
                Baseshippingid = marketingFreeShippingPo.Baseshippingid,
                FreeShippingFee = marketingFreeShippingPo.Freeshippingfee
            };
        }

        #endregion

        #region 运费折扣

        /// <summary>
        /// 设置运费折扣活动
        /// </summary>
        /// <param name="marketing"></param>
        /// <param name="shippingDiscounts">实体</param>
        public void SetShippingDiscountMarketing(ShippingMarketing marketing, List<ShippingDiscount> shippingDiscounts)
        {
            //  1.保存活动主表 和 设置导入客户条件
            marketing.MarketingType = MarketingType.Shipping;
            SetMarketing(marketing);

            //  2.保存运费活动主表
            marketing.RewardType = ShippingRewardType.ShippingDiscount;
            SetShippingMarketing(marketing);

            //  3.保存运费折扣表
            MarketingShippingDiscountDao.DeleteObjectByHql("delete from MarketingShippingDiscountPo where Marketingshippingid=?", marketing.ShippingMarketingId);
            var shippingDiscountPos = shippingDiscounts.Select(x => new MarketingShippingDiscountPo
            {
                Marketingshippingid = marketing.ShippingMarketingId,
                Amount = x.Amount,
                Discount = x.Discount
            });
            MarketingShippingDiscountDao.AddObjects(shippingDiscountPos);
        }

        /// <summary>
        /// 获取运费折扣明细
        /// </summary>
        /// <param name="marketingShippingId">MarketingShippingPo.Id</param>
        /// <returns></returns>
        public List<ShippingDiscount> GetShippingDiscounts(int marketingShippingId)
        {
            var marketingShippingDiscountPos = MarketingShippingDiscountDao.FindDataByHql("FROM MarketingShippingDiscountPo where Marketingshippingid = ?", marketingShippingId);
            if (marketingShippingDiscountPos.IsNullOrEmpty())
            {
                return new List<ShippingDiscount> { };
            }
            return marketingShippingDiscountPos.Select(x => new ShippingDiscount
            {
                Id = x.Id,
                Marketingshippingid = x.Marketingshippingid,
                Amount = x.Amount,
                Discount = x.Discount
            }).ToList();
        }

        #endregion

        #region 运费升级

        /// <summary>
        /// 保存运费升级活动
        /// </summary>
        /// <param name="marketing"></param>
        /// <param name="shippingUpgrades">实体</param>
        public void SetShippingUpgrade(ShippingMarketing marketing, List<ShippingUpgrade> shippingUpgrades)
        {
            //  1.保存活动主表 和 设置导入客户条件
            marketing.MarketingType = MarketingType.Shipping;
            SetMarketing(marketing);

            //  2.修改运费活动主表
            marketing.RewardType = ShippingRewardType.ShippingUpgrade;
            SetShippingMarketing(marketing);

            //  3.保存运费升级表
            MarketingShippingUpgradeDao.DeleteObjectByHql("delete from MarketingShippingUpgradePo where Marketingshippingid=?", marketing.ShippingMarketingId);
            var shippingUpgradePos = shippingUpgrades.Select(x => new MarketingShippingUpgradePo
            {
                Marketingshippingid = marketing.ShippingMarketingId,
                Amount = x.Amount,
                Shippingid = x.ShippingId,
                Upshippingid = x.Upshippingid
            });
            MarketingShippingUpgradeDao.AddObjects(shippingUpgradePos);
        }

        /// <summary>
        /// 获取运费升级明细
        /// </summary>
        /// <param name="marketingShippingId">MarketingShippingPo.Id</param>
        /// <returns></returns>
        public List<ShippingUpgrade> GetShippingUpgrade(int marketingShippingId)
        {
            var marketingShippingUpgradePos = MarketingShippingUpgradeDao.FindDataByHql("FROM MarketingShippingUpgradePo where Marketingshippingid = ?", marketingShippingId);
            if (marketingShippingUpgradePos.IsNullOrEmpty())
            {
                return new List<ShippingUpgrade> { };
            }
            return marketingShippingUpgradePos.Select(x => new ShippingUpgrade
            {
                Id = x.Id,
                Marketingshippingid = x.Marketingshippingid,
                Amount = x.Amount,
                ShippingId = x.Shippingid,
                Upshippingid = x.Upshippingid
            }).ToList();
        }

        #endregion

        #endregion

        #region 下单活动

        /// <summary>
        /// 设置下单活动
        /// </summary>
        /// <param name="marketing">订单活动实体</param>
        /// <param name="placeOrderDetails"></param>
        public void SetPlaceOrderMarketing(PlaceOrderMarketing marketing, List<PlaceOrderDetail> placeOrderDetails)
        {
            #region 1.修改Marketing主表
            marketing.MarketingType = MarketingType.PlaceOrder;

            SetMarketing(marketing);
            #endregion

            #region 2.设置活动导入客户条件
            SetMarketingCustomerClaim(marketing);
            #endregion

            #region 3.设置下单送礼明细
            //if (MarketingPlaceOrderDao.GetOneObject("from MarketingPlaceOrderPo where MarketingId=?", new object[] { marketing.Id }) != null)
            MarketingPlaceOrderDao.DeleteObjectByHql("delete from MarketingPlaceOrderPo where MarketingId=?", new object[] { marketing.Id });

            var lstPlaceOrderPo = placeOrderDetails.Select(x => new MarketingPlaceOrderPo
            {
                Id = x.Id,
                MarketingId = marketing.Id,
                Amount = x.Amount,
                GiftLevel = x.GiftLevel,
                CouponCode = x.CouponCode,
                CurrencyId = x.CurrencyId
            });
            MarketingPlaceOrderDao.AddObjects(lstPlaceOrderPo);
            #endregion
        }

        /// <summary>
        /// 删除下单活动
        /// </summary>
        /// <param name="marketingId"></param>
        public void DeletePlaceOrderMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            MarketingCustomerClaimDao.DeleteObjectByHql("delete from MarketingCustomerClaimPo where MarketingId=?", new object[] { marketingId });
            MarketingPlaceOrderDao.DeleteObjectByHql("delete from MarketingPlaceOrderPo where MarketingId=?", new object[] { marketingId });

            MarketingDao.DeleteObjectById(marketingId);
        }

        /// <summary>
        /// 根据ID获取下单活动
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        public PlaceOrderMarketing GetPlaceOrderMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            var marketingPlaceOrderPos = MarketingPlaceOrderDao.FindDataByHql("from MarketingPlaceOrderPo where MarketingId=?", new object[] { marketingId });

            return PlaceOrderMarketingPoConvertToVo(marketingPo, marketingPlaceOrderPos);
        }

        /// <summary>
        /// PlaceOrderMarketing PO 转 VO
        /// </summary>
        private PlaceOrderMarketing PlaceOrderMarketingPoConvertToVo(MarketingPo marketingPo, IEnumerable<MarketingPlaceOrderPo> marketingPlaceOrderPos = null)
        {
            if (marketingPo.IsNullOrEmpty())
                return null;

            if (marketingPlaceOrderPos != null)
                return new PlaceOrderMarketing
                {
                    Id = marketingPo.Id,
                    Name = marketingPo.Name,
                    AmountType = marketingPo.AmountType.ToEnum<MarketingAmountType>(),
                    MarketingType = marketingPo.MarketingType.ToEnum<MarketingType>(),
                    CustomerType = marketingPo.CustomerType.ToEnum<MarketingCustomerType>(),
                    IsExcludeClub = marketingPo.Isexcludeclub,
                    IsExcludeChannels = marketingPo.Isexcludechannels,
                    CustomerVipIds = marketingPo.Targetcustomerviplevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CountryIds = marketingPo.Targetcountry.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CurrencyIds = marketingPo.Targetcurrencies.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    LanguageIds = marketingPo.Targetlanguages.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    EffectiveBegin = marketingPo.Effectivebegin,
                    EffectiveEnd = marketingPo.Effectiveend,
                    Status = marketingPo.Status,
                    LastModifyWho = marketingPo.Lastmodifywho,
                    LastModifyTime = marketingPo.Lastmodifytime,
                    PlaceOrderDetails = marketingPlaceOrderPos.Select(x => new PlaceOrderDetail
                        {
                            Id = x.Id,
                            Amount = x.Amount,
                            CouponCode = x.CouponCode,
                            CurrencyId = x.CurrencyId,
                            GiftLevel = x.GiftLevel,
                            MarketingId = x.MarketingId
                        }).ToList()
                };
            else

                return new PlaceOrderMarketing
                {
                    Id = marketingPo.Id,
                    Name = marketingPo.Name,
                    AmountType = marketingPo.AmountType.ToEnum<MarketingAmountType>(),
                    MarketingType = marketingPo.MarketingType.ToEnum<MarketingType>(),
                    CustomerType = marketingPo.CustomerType.ToEnum<MarketingCustomerType>(),
                    IsExcludeClub = marketingPo.Isexcludeclub,
                    IsExcludeChannels = marketingPo.Isexcludechannels,
                    CustomerVipIds = marketingPo.Targetcustomerviplevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CountryIds = marketingPo.Targetcountry.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CurrencyIds = marketingPo.Targetcurrencies.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    LanguageIds = marketingPo.Targetlanguages.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    EffectiveBegin = marketingPo.Effectivebegin,
                    EffectiveEnd = marketingPo.Effectiveend,
                    Status = marketingPo.Status,
                    LastModifyWho = marketingPo.Lastmodifywho,
                    LastModifyTime = marketingPo.Lastmodifytime,
                    PlaceOrderDetails = new List<PlaceOrderDetail>()
                };
        }

        /// <summary>
        /// 获取下单活动明细
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        public List<PlaceOrderDetail> GetPlaceOrderDetails(int marketingId)
        {
            if (MarketingDao.ExistObject("Id", marketingId))
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            var lstPlaceOrderPo = MarketingPlaceOrderDao.FindDataByHql("from MarketingPlaceOrderPo where MarketingId=?", marketingId);

            if (lstPlaceOrderPo.IsNullOrEmpty()) return new List<PlaceOrderDetail>();
            return lstPlaceOrderPo.Select(x => new PlaceOrderDetail
            {
                Id = x.Id,
                MarketingId = x.MarketingId,
                Amount = x.Amount,
                CouponCode = x.CouponCode,
                CurrencyId = x.CurrencyId,
                GiftLevel = x.GiftLevel
            }).ToList();
        }

        /// <summary>
        /// 分页查询下单活动后台管理列表 
        /// </summary>
        public PageData<PlaceOrderMarketing> FindPlaceOrderMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM MarketingPo");
            //1.构建查询条件
            if (!searchDictionary.IsNullOrEmpty())
            {
                searchDictionary.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case MarketingSearchCriteria.MarketingType:
                            hqlHelper.AddWhere("MarketingType", HqlOperator.Eq, "MarketingType", item.Value);
                            break;
                        case MarketingSearchCriteria.Name:
                            hqlHelper.AddWhere("Name", HqlOperator.Like, "Name", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        case MarketingSorterCriteria.Status:
                            hqlHelper.AddSorter("Status", sorter.IsAsc);
                            break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("Id", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = MarketingDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<PlaceOrderMarketing>();
            var voList = pageDataPo.Data.Select(marketingPo => PlaceOrderMarketingPoConvertToVo(marketingPo)).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        #endregion

        #region Coupon 活动

        /// <summary>
        /// 添加送Coupon活动
        /// </summary>
        public void AddCouponMarketing(CouponMarketing marketing)
        {
            #region 1.添加Marketing主表
            marketing.MarketingType = MarketingType.SendCoupon;
            SetMarketing(marketing);
            #endregion

            #region 2.设置Coupon活动明细
            //MarketingCouponDao.DeleteObjectByHql("from MarketingCouponPo where MarketingId=?", marketing.Id);

            var marketingCouponPo = new MarketingCouponPo
            {
                MarketingId = marketing.Id,
                CouponCode = marketing.CouponCode,
                RewardType = marketing.RewardType.ParseTo<int>()
            };
            MarketingCouponDao.AddObject(marketingCouponPo);
            #endregion
        }

        /// <summary>
        /// 修改送Coupon活动
        /// </summary>
        public void UpdateCouponMarketing(CouponMarketing marketing)
        {
            #region 1.修改Marketing主表
            marketing.MarketingType = MarketingType.SendCoupon;
            SetMarketing(marketing);
            #endregion

            #region 2.设置Coupon活动明细
            MarketingCouponDao.DeleteObjectByHql("DELETE from MarketingCouponPo where MarketingId=?", marketing.Id);

            var marketingCouponPo = new MarketingCouponPo
            {
                MarketingId = marketing.Id,
                CouponCode = marketing.CouponCode,
                RewardType = marketing.RewardType.ParseTo<int>()
            };
            MarketingCouponDao.AddObject(marketingCouponPo);

            #endregion
        }

        /// <summary>
        /// 删除送Coupon活动
        /// </summary>
        public void DeleteCouponMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            MarketingCouponDao.DeleteObjectByHql("DELETE from MarketingCouponPo where MarketingId=?", marketingId);

            MarketingDao.DeleteObjectById(marketingId);
        }

        public void UpdateCouponMarketingStatus(int marketingId, bool status)
        {
            MarketingDao.UpdateMarketingStatus(marketingId, status);
        }

        /// <summary>
        /// 获取送Coupon活动根据ID
        /// </summary>
        public CouponMarketing GetCouponMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            var marketingCouponPo = MarketingCouponDao.GetOneObject("from MarketingCouponPo where MarketingId=?", marketingId);

            if (marketingCouponPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            return GetCouponMarketingVoFromPo(marketingPo, marketingCouponPo);
        }

        public CouponMarketing GetCouponMarketingByCode(string couponCode)
        {
            var marketingCouponPo = MarketingCouponDao.GetMarketingCoupon(couponCode);
            MarketingPo marketingPo = null;
            if (!marketingCouponPo.IsNullOrEmpty())
                marketingPo = MarketingDao.GetObject(marketingCouponPo.MarketingId);
            return GetCouponMarketingVoFromPo(marketingPo, marketingCouponPo);
        }

        public bool CheckMarketingCouponByCode(string couponCode)
        {
            return !MarketingCouponDao.GetMarketingCoupon(couponCode).IsNullOrEmpty();
        }

        /// <summary>
        /// 送Coupon活动 Po 转 VO
        /// </summary>
        internal static VMarketingCoupon VCouponMarketingVoToPo(VMarketingCouponPo vMarketingCouponPo)
        {
            return new VMarketingCoupon
            {
                Id = vMarketingCouponPo.Id,
                Name = vMarketingCouponPo.Name,
                AmountType = vMarketingCouponPo.AmountType,
                MarketingType = vMarketingCouponPo.MarketingType.ToEnum<MarketingType>(),
                CustomerType = vMarketingCouponPo.CustomerType.ToEnum<MarketingCustomerType>(),
                IsExcludeclub = vMarketingCouponPo.IsExcludeclub,
                IsExcludeChannels = vMarketingCouponPo.IsExcludeChannels,
                Targetcustomerviplevel = vMarketingCouponPo.Targetcustomerviplevel,
                TargetClubLevel = vMarketingCouponPo.TargetClubLevel,
                TargetCountry = vMarketingCouponPo.TargetCountry,
                TargetCurrencies = vMarketingCouponPo.TargetCurrencies,
                TargetLanguages = vMarketingCouponPo.TargetLanguages,
                EffectiveBegin = vMarketingCouponPo.EffectiveBegin,
                EffectiveEnd = vMarketingCouponPo.EffectiveEnd,
                Status = vMarketingCouponPo.Status,
                Lastmodifywho = vMarketingCouponPo.Lastmodifywho,
                Lastmodifytime = vMarketingCouponPo.Lastmodifytime,
                RewardType = vMarketingCouponPo.RewardType
            };
        }

        internal static CouponMarketing GetCouponMarketingVoFromPo(MarketingPo marketingPo, MarketingCouponPo marketingCouponPo)
        {
            CouponMarketing couponMarketing = null;
            if (marketingPo != null)
            {
                couponMarketing = new CouponMarketing
                {
                    Id = marketingPo.Id,
                    Name = marketingPo.Name,
                    AmountType = marketingPo.AmountType.ToEnum<MarketingAmountType>(),
                    MarketingType = marketingPo.MarketingType.ToEnum<MarketingType>(),
                    CustomerType = marketingPo.CustomerType.ToEnum<MarketingCustomerType>(),
                    IsExcludeClub = marketingPo.Isexcludeclub,
                    IsExcludeChannels = marketingPo.Isexcludechannels,
                    CustomerVipIds = marketingPo.Targetcustomerviplevel.IsNullOrEmpty() ? null : marketingPo.Targetcustomerviplevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    ClubLevels = marketingPo.TargetClubLevel.IsNullOrEmpty() ? null : marketingPo.TargetClubLevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CountryIds = marketingPo.Targetcountry.IsNullOrEmpty() ? null : marketingPo.Targetcountry.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    CurrencyIds = marketingPo.Targetcurrencies.IsNullOrEmpty() ? null : marketingPo.Targetcurrencies.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    LanguageIds = marketingPo.Targetlanguages.IsNullOrEmpty() ? null : marketingPo.Targetlanguages.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                    EffectiveBegin = marketingPo.Effectivebegin,
                    EffectiveEnd = marketingPo.Effectiveend,
                    Status = marketingPo.Status,
                    LastModifyWho = marketingPo.Lastmodifywho,
                    LastModifyTime = marketingPo.Lastmodifytime,

                    CouponMarketingId = marketingCouponPo.Id,
                    RewardType = marketingCouponPo.RewardType,
                    CouponCode = marketingCouponPo.CouponCode
                };
            }
            return couponMarketing;
        }


        /// <summary>
        /// 查询送Coupon活动后台管理列表 
        /// </summary>
        public PageData<VMarketingCoupon> FindCouponMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM VMarketingCouponPo");
            //1.构建查询条件
            if (!searchDictionary.IsNullOrEmpty())
            {
                searchDictionary.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case MarketingSearchCriteria.Name:
                            hqlHelper.AddWhere("Name", HqlOperator.Like, "Name", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        case MarketingSorterCriteria.Status:
                            hqlHelper.AddSorter("Status", sorter.IsAsc);
                            break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("Id", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = VMarketingCouponDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<VMarketingCoupon>();
            var voList = pageDataPo.Data.Select(VCouponMarketingVoToPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        #endregion
        #region Gift活动
        public IVMarketingGiftDao VMarketingGiftDao { private get; set; }

        public void AddGiftMarketing(GiftMarketing marketing)
        {
            marketing.MarketingType = MarketingType.Gift;
            SetMarketing(marketing);

            var mrketingGift = new MarketingGiftPo
            {
                MarketingId = marketing.Id,
                RewardType = (int)marketing.RewardType,
                GiftLevel = marketing.GiftLevel
            };

            MarketingGiftDao.AddObject(mrketingGift);
        }

        public void UpdateGiftMarketing(GiftMarketing marketing)
        {
            #region 1.修改Marketing主表
            marketing.MarketingType = MarketingType.Gift;
            SetMarketing(marketing);
            #endregion

            #region 2.设置Coupon活动明细
            if (marketing.Id != 0)
                MarketingGiftDao.DeleteObjectByHql("DELETE from MarketingGiftPo where MarketingId = ?", marketing.Id);

            var mrketingGift = new MarketingGiftPo
            {
                MarketingId = marketing.Id,
                RewardType = (int)marketing.RewardType,
                GiftLevel = marketing.GiftLevel
            };
            MarketingGiftDao.AddObject(mrketingGift);

            #endregion
        }

        public void DeleteGiftMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            MarketingGiftDao.DeleteObjectByHql("DELETE from MarketingGiftPo where MarketingId = ?", marketingId);

            MarketingDao.DeleteObjectById(marketingId);
        }

        public void UpdateGiftMarketingStatus(int marketingId, bool status)
        {
            MarketingDao.UpdateMarketingStatus(marketingId, status);
        }

        public GiftMarketing GetGiftMarketingById(int marketingId)
        {
            var marketingPo = MarketingDao.GetObject(marketingId);
            if (marketingPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            var marketingGiftPo = MarketingGiftDao.GetOneObject("FROM MarketingGiftPo WHERE MarketingId = ?", marketingId);

            if (marketingGiftPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_MARKETING_NOT_EXIST);

            return GetGiftMarketingVoFromPo(marketingPo, marketingGiftPo);
        }

        public PageData<VMarketingGift> FindGiftMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM VMarketingGiftPo");
            //1.构建查询条件
            if (!searchDictionary.IsNullOrEmpty())
            {
                searchDictionary.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case MarketingSearchCriteria.MarketingType:
                            hqlHelper.AddWhere("MarketingType", HqlOperator.Eq, "MarketingType", item.Value);
                            break;
                        case MarketingSearchCriteria.Name:
                            hqlHelper.AddWhere("Name", HqlOperator.Like, "Name", item.Value);
                            break;
                        case MarketingSearchCriteria.RewardType:
                            hqlHelper.AddWhere("RewardType", HqlOperator.Eq, "RewardType", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        case MarketingSorterCriteria.IdAsc:
                            hqlHelper.AddSorter("Id", sorter.IsAsc);
                            break;
                        case MarketingSorterCriteria.IdDesc:
                            hqlHelper.AddSorter("Id", !sorter.IsAsc);
                            break;
                        case MarketingSorterCriteria.EffectiveBeginAsc:
                            hqlHelper.AddSorter("EffectiveBegin", sorter.IsAsc);
                            break;
                        case MarketingSorterCriteria.EffectiveBeginDesc:
                            hqlHelper.AddSorter("EffectiveBegin", !sorter.IsAsc);
                            break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("Id", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = VMarketingGiftDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<VMarketingGift>();
            var voList = pageDataPo.Data.Select(GetVMarketingGiftVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        internal VMarketingGift GetVMarketingGiftVoFromPo(VMarketingGiftPo vMarketingGiftPo)
        {
            return new VMarketingGift
            {
                Id = vMarketingGiftPo.Id,
                Name = vMarketingGiftPo.Name,
                AmountType = vMarketingGiftPo.AmountType,
                MarketingType = vMarketingGiftPo.MarketingType.ToEnum<MarketingType>(),
                CustomerType = vMarketingGiftPo.CustomerType.ToEnum<MarketingCustomerType>(),
                IsExcludeclub = vMarketingGiftPo.IsExcludeclub,
                IsExcludeChannels = vMarketingGiftPo.IsExcludeChannels,
                Targetcustomerviplevel = vMarketingGiftPo.Targetcustomerviplevel,
                TargetClubLevel = vMarketingGiftPo.TargetClubLevel,
                TargetCountry = vMarketingGiftPo.TargetCountry,
                TargetCurrencies = vMarketingGiftPo.TargetCurrencies,
                TargetLanguages = vMarketingGiftPo.TargetLanguages,
                EffectiveBegin = vMarketingGiftPo.EffectiveBegin,
                EffectiveEnd = vMarketingGiftPo.EffectiveEnd,
                Status = vMarketingGiftPo.Status,
                Lastmodifywho = vMarketingGiftPo.Lastmodifywho,
                Lastmodifytime = vMarketingGiftPo.Lastmodifytime,
                RewardType = vMarketingGiftPo.RewardType
            };
        }

        internal GiftMarketing GetGiftMarketingVoFromPo(MarketingPo marketingPo, MarketingGiftPo marketingGiftPo)
        {
            GiftMarketing giftMarketing = null;
            if (marketingPo != null)
            {
                if (!marketingGiftPo.IsNullOrEmpty())
                    giftMarketing = new GiftMarketing
                    {
                        Id = marketingPo.Id,
                        Name = marketingPo.Name,
                        AmountType = marketingPo.AmountType.ToEnum<MarketingAmountType>(),
                        MarketingType = marketingPo.MarketingType.ToEnum<MarketingType>(),
                        CustomerType = marketingPo.CustomerType.ToEnum<MarketingCustomerType>(),
                        IsExcludeClub = marketingPo.Isexcludeclub,
                        IsExcludeChannels = marketingPo.Isexcludechannels,
                        CustomerVipIds = marketingPo.Targetcustomerviplevel.IsNullOrEmpty() ? null : marketingPo.Targetcustomerviplevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        ClubLevels = marketingPo.TargetClubLevel.IsNullOrEmpty() ? null : marketingPo.TargetClubLevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        CountryIds = marketingPo.Targetcountry.IsNullOrEmpty() ? null : marketingPo.Targetcountry.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        CurrencyIds = marketingPo.Targetcurrencies.IsNullOrEmpty() ? null : marketingPo.Targetcurrencies.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        LanguageIds = marketingPo.Targetlanguages.IsNullOrEmpty() ? null : marketingPo.Targetlanguages.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        EffectiveBegin = marketingPo.Effectivebegin,
                        EffectiveEnd = marketingPo.Effectiveend,
                        Status = marketingPo.Status,
                        LastModifyWho = marketingPo.Lastmodifywho,
                        LastModifyTime = marketingPo.Lastmodifytime,

                        GiftMarketingId = marketingGiftPo.Id,
                        RewardType = marketingGiftPo.RewardType,
                        GiftLevel = marketingGiftPo.GiftLevel
                    };
                else
                    giftMarketing = new GiftMarketing
                    {
                        Id = marketingPo.Id,
                        Name = marketingPo.Name,
                        AmountType = marketingPo.AmountType.ToEnum<MarketingAmountType>(),
                        MarketingType = marketingPo.MarketingType.ToEnum<MarketingType>(),
                        CustomerType = marketingPo.CustomerType.ToEnum<MarketingCustomerType>(),
                        IsExcludeClub = marketingPo.Isexcludeclub,
                        IsExcludeChannels = marketingPo.Isexcludechannels,
                        CustomerVipIds = marketingPo.Targetcustomerviplevel.IsNullOrEmpty() ? null : marketingPo.Targetcustomerviplevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        ClubLevels = marketingPo.TargetClubLevel.IsNullOrEmpty() ? null : marketingPo.TargetClubLevel.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        CountryIds = marketingPo.Targetcountry.IsNullOrEmpty() ? null : marketingPo.Targetcountry.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        CurrencyIds = marketingPo.Targetcurrencies.IsNullOrEmpty() ? null : marketingPo.Targetcurrencies.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        LanguageIds = marketingPo.Targetlanguages.IsNullOrEmpty() ? null : marketingPo.Targetlanguages.Split('|').Select(x => x.ParseTo<int>()).ToList(),
                        EffectiveBegin = marketingPo.Effectivebegin,
                        EffectiveEnd = marketingPo.Effectiveend,
                        Status = marketingPo.Status,
                        LastModifyWho = marketingPo.Lastmodifywho,
                        LastModifyTime = marketingPo.Lastmodifytime,

                    };
            }
            return giftMarketing;
        }
        #endregion
        #endregion

        #region 前台

        #region 注册
        /// <summary>
        /// 前台注册页面提示送Coupon的信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>Couon编号</returns>
        public List<Service.Coupon.Coupon> GetCouponCodeForRegister(CouponCriteria criteria)
        {
            return MarketingCouponDao.GetCouponCodeForRegister(criteria);
        }

        /// <summary>
        /// 前台注册送Coupon：方法里要实现送给该客户
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>CouponCustomer</returns>
        public Service.Coupon.Coupon SendCouponCodeForRegister(CouponCriteria criteria)
        {
            var registerCoupon = MarketingCouponDao.SendCouponCodeForRegister(criteria);
            if (criteria.CustomerId.HasValue && !registerCoupon.IsNullOrEmpty())
            {
                ServiceFactory.CouponService.SendCoupon(registerCoupon.CouponId, criteria.CustomerId.Value, 0);
                return registerCoupon;
            }
            return null;
        }

        #endregion

        #region 凑单
        /// <summary>
        /// 购物车和下单页调用：方法里要计算当前金额对Club免运费、VIP折扣、订单折扣等活动的匹配度
        /// 具体为 凑单需满足的金额*60% ≤ 当前金额 ＜ 凑单需满足的金额 时， 才显示
        /// 1.club
        /// 2.订单折扣
        /// 3.VIP
        /// </summary>
        /// <returns></returns>
        public PiecingOrderResult GetPiecingOrderInfo(PiecingOrderCriteria criteria)
        {
            return MarketingDao.GetPiecingOrderInfo(criteria);
        }

        #endregion

        #region 下单
        /// <summary>
        /// 前台下单送：方法里要实现送礼和送Coupon给这个订单所属客户
        /// </summary>
        /// <param name="orderId">送给哪个订单</param>
        /// <param name="criteria"></param>
        /// <returns>送的结果</returns>
        public PlaceOrderResult MarketingForPlaceOrder(int orderId, PlaceOrderCriteria criteria)
        {
            if (OrderDao.GetObject(orderId).IsNullOrEmpty())
                throw new BussinessException(ERROR_ORDER_NOT_EXIST);

            return MarketingPlaceOrderDao.MarketingForPlaceOrder(orderId, criteria);
        }

        #endregion

        #region 订单折扣
        /// <summary>
        /// 前台购物车根据条件获取订单折扣扣 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>折扣 例如：20% 是 0.8 ；不满足返回1</returns>
        public decimal MarketingForOrderDiscount(OrderDiscountCriteria criteria)
        {
            return MarketingOrderDiscountDao.MarketingForOrderDiscount(criteria);
        }

        /// <summary>
        /// 把当前长期折扣取出来前台展示
        /// </summary>
        public List<OrderAmountDiscount> GetMarketingForOrderDiscount(string countryIsoCode2, int languageId)
        {
            return MarketingOrderDiscountDao.GetMarketingForOrderDiscount(countryIsoCode2, languageId);
        }
        #endregion

        #region 运费
        /// <summary>
        /// 购物车和下单页根据运费活动计算处理当前所有运输方式最终运费
        /// 要实现：1免运费，2运费折扣，3运送方式升级
        /// </summary>
        /// <param name="shippingAmounts"></param>
        /// <param name="criteria"></param>
        /// <returns>处理后的运费相关信息</returns>
        public List<ShippingAmount> MarketingForShippingFee(List<ShippingAmount> shippingAmounts, ShipppingCriteria criteria)
        {
            //var retList = new List<ShippingAmount>();

            ////  渠道商和club会员（包括一期和二期）均不能享受运费活动优惠
            //if (criteria.IsChannel || criteria.ClubLevel > 0)
            //{
            //    return retList;
            //}

            ////  ToDo
            //return retList;
            //return shippingAmounts.Select(x => MarketingShippingDao.GetMarketingForShippingFee(x, criteria)).ToList();
            return MarketingShippingDao.GetMarketingForShippingFee(shippingAmounts, criteria);
        }

        #endregion

        #region 生日
        //这个是服务完成
        #endregion
        #endregion

        #region 辅助方法

        /// <summary>
        /// Marketing VO 转 PO
        /// </summary>
        /// <returns></returns>
        internal MarketingPo GetMarketingPoFromVo(Service.Marketing.Marketing marketing)
        {
            MarketingPo marketingPo = null;
            if (!marketing.IsNullOrEmpty())
            {
                marketingPo = new MarketingPo
                {
                    Id = marketing.Id,
                    Name = marketing.Name,
                    CustomerType = marketing.CustomerType.ParseTo<int>(),
                    Targetcustomerviplevel = string.Join("|", marketing.CustomerVipIds),
                    Isexcludeclub = marketing.IsExcludeClub,
                    Isexcludechannels = marketing.IsExcludeChannels,
                    Targetcountry = string.Join("|", marketing.CountryIds),
                    Targetcurrencies = string.Join("|", marketing.CurrencyIds),
                    Targetlanguages = string.Join("|", marketing.LanguageIds),
                    Effectivebegin = marketing.EffectiveBegin,
                    Effectiveend = marketing.EffectiveEnd,
                    MarketingType = marketing.MarketingType.ParseTo<int>(),
                    Status = marketing.Status,
                    AmountType = marketing.AmountType.ParseTo<int>(),
                    Lastmodifytime = DateTime.Now,
                    //Lastmodifywho = "a"
                };
            }
            return marketingPo;
        }

        /// <summary>
        /// ShippingMarketing PO 转 VO
        /// </summary>
        /// <returns></returns>
        internal ShippingMarketing GetShippingMarketingVoFromPo(MarketingPo marketingPo, bool isGetAll = true)
        {
            if (marketingPo.IsNullOrEmpty())
                return null;
            var shippingMarketing = new ShippingMarketing
            {
                Id = marketingPo.Id,
                Name = marketingPo.Name,
                MarketingType = marketingPo.MarketingType.ToEnum<MarketingType>(),
                IsExcludeChannels = marketingPo.Isexcludechannels,
                IsExcludeClub = marketingPo.Isexcludeclub,
                CustomerType = marketingPo.CustomerType.ToEnum<MarketingCustomerType>(),
                //CustomerInfo = null,
                CustomerVipIds = marketingPo.Targetcustomerviplevel.Split<int>("|").ToList(),
                LanguageIds = marketingPo.Targetlanguages.Split<int>("|").ToList(),
                CurrencyIds = marketingPo.Targetcurrencies.Split<int>("|").ToList(),
                CountryIds = marketingPo.Targetcountry.Split<int>("|").ToList(),
                AmountType = marketingPo.AmountType.ToEnum<MarketingAmountType>(),
                EffectiveBegin = marketingPo.Effectivebegin,
                EffectiveEnd = marketingPo.Effectiveend,
                Status = marketingPo.Status,

            };
            var marketingShippingPo = MarketingShippingDao.GetMarketingShippingByMarketingId(marketingPo.Id);
            if (marketingShippingPo.IsNullOrEmpty()) return shippingMarketing;
            shippingMarketing.ShippingMarketingId = marketingShippingPo.Id;
            shippingMarketing.RewardType = marketingShippingPo.Rewardtype.ToEnum<ShippingRewardType>();
            shippingMarketing.WeightLimit = marketingShippingPo.WeightLimit;
            shippingMarketing.WeightType = marketingShippingPo.WeightType.ToEnum<ShippingWeightType>();
            shippingMarketing.ShippingIds = marketingShippingPo.Shippingids.Split<int>("|").ToList();

            if (!isGetAll) return shippingMarketing;
            shippingMarketing.FreeShipping = GetFreeShipping(marketingShippingPo.Id);
            shippingMarketing.ShippingDiscounts = GetShippingDiscounts(marketingShippingPo.Id);
            shippingMarketing.ShippingUpgrades = GetShippingUpgrade(marketingShippingPo.Id);
            return shippingMarketing;
        }

        /// <summary>
        /// ShippingMarketing VO 转 PO
        /// </summary>
        /// <returns></returns>
        internal static MarketingShippingPo GetShippingMarketingPoFromVo(ShippingMarketing shippingMarketing)
        {
            MarketingShippingPo marketingShippingPo = null;
            if (!shippingMarketing.IsNullOrEmpty())
            {
                marketingShippingPo = new MarketingShippingPo
                {
                    Id = shippingMarketing.ShippingMarketingId,
                    Marketingid = shippingMarketing.Id,
                    Rewardtype = (int)shippingMarketing.RewardType,
                    Shippingids = string.Join("|", shippingMarketing.ShippingIds),
                    WeightType = (int)shippingMarketing.WeightType,
                    WeightLimit = shippingMarketing.WeightLimit
                };
            }
            return marketingShippingPo;
        }
        #endregion
    }
}
