using Com.Panduo.Service.Coupon;

namespace Com.Panduo.Web.Models.Coupon
{
    public class CouponCustomerVo
    {
        /// <summary>
        /// CouponCustomer实体
        /// </summary>
        public CouponCustomer CouponCustomer { get; set; }

        /// <summary>
        /// Coupon币种符号
        /// </summary>
        public string SymbolLeft { get; set; }

        /// <summary>
        /// 当前短日期格式时间
        /// </summary>
        public string CurrentShortDate { get; set; }
    }
}