using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：t_order_remark_by_admin ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:34:06
    /// </summary>
    [Class(Table = "t_order_remark_by_admin", Lazy = false, NameType = typeof(OrderRemarkByAdminPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderRemarkByAdminPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "RemarkId", Column = "remark_id")]
        [Generator(2, Class = "native")]
        public virtual int RemarkId
        {
            get;
            set;
        }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 如10:业务优惠金额，20:业务附加费，30:Seller Memo
        /// </summary>
        [Property(Column = "remark_type")]
        public virtual int RemarkType
        {
            get;
            set;
        }

        /// <summary>
        /// 备注内容
        /// </summary>
        [Property(Column = "remark_content")]
        public virtual string RemarkContent
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// 管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}

