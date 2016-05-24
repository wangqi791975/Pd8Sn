using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Coupon;
using Com.Panduo.Service.Marketing.Coupon;
using Com.Panduo.Service.Marketing.Criteria;
using Com.Panduo.Service.Marketing.Gift;
using Com.Panduo.Service.Marketing.PlaceOrder;
using Com.Panduo.Service.Marketing.ShippingMarketing;
using Com.Panduo.Service.Order.ShippingOption;

namespace Com.Panduo.Service.Marketing
{
    /// <summary>
    /// 营销活动服务接口
    /// </summary>
    public interface IMarketingService
    {
        #region 常量

        /// <summary>
        /// 购物车不存在
        /// </summary>
        string ERROR_MARKETING_NOT_EXIST { get; }
        #endregion

        #region 后台
        /// <summary>
        /// 设置活动状态
        /// </summary>
        /// <param name="marketingId">订单活动Id</param>
        /// <param name="isValid">状态</param>
        void SetMarketingStatus(int marketingId, bool isValid);

        #region 订单折扣活动

        /// <summary>
        /// 设置订单活动
        /// </summary>
        /// <param name="marketing">订单折扣活动实体</param>
        /// <param name="orderAmountDiscounts">订单折扣明细</param>
        void SetOrderDiscountMarketing(OrderDiscountMarketing marketing, List<OrderAmountDiscount> orderAmountDiscounts);


        /// <summary>
        /// 删除订单活动
        /// </summary>
        /// <param name="marketingId"></param>
        void DeleteOrderDiscountMarketingById(int marketingId);

        /// <summary>
        /// 根据ID获取订单活动
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        OrderDiscountMarketing GetOrderDiscountMarketingById(int marketingId);

        /// <summary>
        /// 获取订单活动折扣明细
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        List<OrderAmountDiscount> GetOrderAmountDiscounts(int marketingId);

        /// <summary>
        /// 分页查询订单活动后台管理列表 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchDictionary"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns></returns>
        PageData<OrderDiscountMarketing> FindOrderDiscountMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria);

        #endregion

        #region 运费活动

        /// <summary>
        /// 删除运费活动
        /// </summary>
        /// <param name="marketingId"></param>
        void DeleteShippingMarketingById(int marketingId);

        /// <summary>
        /// 根据ID获取运费活动
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        ShippingMarketing.ShippingMarketing GetShippingMarketingById(int marketingId);

        #region 免运费

        /// <summary>
        /// 设置免运费活动
        /// </summary>
        /// <param name="marketing">运费活动实体</param>
        /// <param name="freeShipping">免运费活动明细</param>
        void SetFreeShippingMarketing(ShippingMarketing.ShippingMarketing marketing, FreeShipping freeShipping);


        /// <summary>
        /// 获取免运费活动明细
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        FreeShipping GetFreeShipping(int marketingId);
        
        #endregion

        #region 运费折扣

        /// <summary>
        /// 修改运费折扣活动
        /// </summary>
        /// <param name="marketing">运费活动实体</param>
        /// <param name="shippingDiscounts">运费折扣活动明细</param>
        void SetShippingDiscountMarketing(ShippingMarketing.ShippingMarketing marketing, List<ShippingDiscount> shippingDiscounts);

        /// <summary>
        /// 获取运费折扣明细
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        List<ShippingDiscount> GetShippingDiscounts(int marketingId);

        #endregion

        #region 运费升级

        /// <summary>
        /// 修改运费升级活动
        /// </summary>
        /// <param name="marketing">运费活动实体</param>
        /// <param name="shippingUpgrades">运费升级活动明细</param>
        void SetShippingUpgrade(ShippingMarketing.ShippingMarketing marketing, List<ShippingUpgrade> shippingUpgrades);

        /// <summary>
        /// 获取运费升级明细
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        List<ShippingUpgrade> GetShippingUpgrade(int marketingId);

        #endregion


        /// <summary>
        /// 分页查询运费活动后台管理列表 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchDictionary"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns></returns>
        PageData<ShippingMarketing.ShippingMarketing> FindShippingMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria);

        #endregion

        #region 下单活动

        /// <summary>
        /// 设置下单活动
        /// </summary>
        /// <param name="marketing">订单活动实体</param>
        /// <param name="placeOrderDetails"></param>
        void SetPlaceOrderMarketing(PlaceOrderMarketing marketing, List<PlaceOrderDetail> placeOrderDetails);

        /// <summary>
        /// 删除下单活动
        /// </summary>
        /// <param name="marketingId"></param>
        void DeletePlaceOrderMarketingById(int marketingId);

        /// <summary>
        /// 根据ID获取下单活动
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        PlaceOrderMarketing GetPlaceOrderMarketingById(int marketingId);

        /// <summary>
        /// 获取下单活动明细
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        List<PlaceOrderDetail> GetPlaceOrderDetails(int marketingId);

        /// <summary>
        /// 分页查询下单活动后台管理列表 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchDictionary"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns></returns>
        PageData<PlaceOrderMarketing> FindPlaceOrderMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria);


        #endregion

        #region Coupon 活动

        /// <summary>
        /// 添加订单活动
        /// </summary>
        /// <param name="marketing"></param>
        void AddCouponMarketing(CouponMarketing marketing);

        /// <summary>
        /// 修改订单活动
        /// </summary>
        /// <param name="marketing">订单活动实体</param>
        void UpdateCouponMarketing(CouponMarketing marketing);

        /// <summary>
        /// 删除订单活动
        /// </summary>
        /// <param name="marketingId"></param>
        void DeleteCouponMarketingById(int marketingId);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="marketingId">活动Id</param>
        /// <param name="status">状态</param>
        void UpdateCouponMarketingStatus(int marketingId,bool status);

        /// <summary>
        /// 获取订单活动根据ID
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        CouponMarketing GetCouponMarketingById(int marketingId);

        /// <summary>
        /// 获取coupon活动
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        CouponMarketing GetCouponMarketingByCode(string couponCode);

        /// <summary>
        /// 通过couponCode获取coupon活动
        /// </summary>
        /// <param name="couponCode">CouponCOde</param>
        /// <returns>coupon活动</returns>
        bool CheckMarketingCouponByCode(string couponCode);

        /// <summary>
        /// 查询订单活动后台管理列表 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchDictionary"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns></returns>
        PageData<VMarketingCoupon> FindCouponMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria);

        #endregion

        #region Gift活动
        /// <summary>
        /// 添加Gift活动
        /// </summary>
        /// <param name="marketing"></param>
        void AddGiftMarketing(GiftMarketing marketing);

        /// <summary>
        /// 修改Gift活动
        /// </summary>
        /// <param name="marketing">订单活动实体</param>
        void UpdateGiftMarketing(GiftMarketing marketing);

        /// <summary>
        /// 删除Gift活动
        /// </summary>
        /// <param name="marketingId"></param>
        void DeleteGiftMarketingById(int marketingId);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="marketingId">活动Id</param>
        /// <param name="status">状态</param>
        void UpdateGiftMarketingStatus(int marketingId,bool status);

        /// <summary>
        /// 获取Gift活动根据ID
        /// </summary>
        /// <param name="marketingId"></param>
        /// <returns></returns>
        GiftMarketing GetGiftMarketingById(int marketingId);

        /// <summary>
        /// 查询Gift活动后台管理列表 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchDictionary"></param>
        /// <param name="sorterCriteria"></param>
        /// <returns></returns>
        PageData<VMarketingGift> FindGiftMarketings(int currentPage, int pageSize, IDictionary<MarketingSearchCriteria, object> searchDictionary, IList<Sorter<MarketingSorterCriteria>> sorterCriteria);

        #endregion
        #endregion

        #region 前台

        #region 注册
        /// <summary>
        /// 前台注册页面提示送Coupon的信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>Couon编号</returns>
        List<Service.Coupon.Coupon> GetCouponCodeForRegister(CouponCriteria criteria);

        /// <summary>
        /// 前台注册送Coupon：方法里要实现送给该客户
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>CouponCustomer</returns>
        Service.Coupon.Coupon SendCouponCodeForRegister(CouponCriteria criteria);

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
        PiecingOrderResult GetPiecingOrderInfo(PiecingOrderCriteria criteria);
        #endregion

        #region 下单
        /// <summary>
        /// 前台下单送：方法里要实现送礼和送Coupon给这个订单所属客户
        /// </summary>
        /// <param name="orderId">送给哪个订单</param>
        /// <param name="criteria"></param>
        /// <returns>送的结果</returns>
        PlaceOrderResult MarketingForPlaceOrder(int orderId, PlaceOrderCriteria criteria);

        #endregion

        #region 订单折扣
        /// <summary>
        /// 前台购物车根据条件获取订单折扣扣 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>折扣 例如：20% 是 0.8 ；不满足返回1</returns>
        decimal MarketingForOrderDiscount(OrderDiscountCriteria criteria);

        /// <summary>
        /// 把当前长期折扣取出来前台展示
        /// </summary>
        /// <param name="countryIsoCode2">国家二级简码</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>折扣 例如：20% 是 0.8 ；不满足返回1</returns>
        List<OrderAmountDiscount> GetMarketingForOrderDiscount(string countryIsoCode2, int languageId);
        #endregion

        #region 运费
        /// <summary>
        /// 购物车和下单页根据运费活动计算处理当前所有运输方式最终运费
        /// 要实现：1免运费，2运费折扣，3运送方式升级
        /// </summary>
        /// <param name="shippingAmounts"></param>
        /// <param name="criteria"></param>
        /// <returns>处理后的运费相关信息</returns>
        List<ShippingAmount> MarketingForShippingFee(List<ShippingAmount> shippingAmounts, ShipppingCriteria criteria);
        #endregion

        #region 生日
        //这个是服务完成
        #endregion
        #endregion
    }

    public enum MarketingSearchCriteria
    {
        /// <summary>
        /// 营销活动名称
        /// </summary>
        Name,

        /// <summary>
        /// 活动类型
        /// </summary>
        MarketingType,

        /// <summary>
        /// 场景
        /// </summary>
        RewardType

    }

    public enum MarketingSorterCriteria
    {
        /// <summary>
        /// 状态
        /// </summary>
        Status,

        /// <summary>
        /// 编号从小到大
        /// </summary>
        IdAsc,

        /// <summary>
        /// 编号从大到小
        /// </summary>
        IdDesc,

        /// <summary>
        /// 开始时间从远到近
        /// </summary>
        EffectiveBeginAsc,

        /// <summary>
        /// 开始时间从近到远
        /// </summary>
        EffectiveBeginDesc
    }
}
