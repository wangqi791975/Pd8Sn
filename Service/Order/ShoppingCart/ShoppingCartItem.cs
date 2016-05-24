using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShoppingCart
{
    /// <summary>
    /// 购物车产品
    /// </summary>
    [Serializable]
    public class ShoppingCartItem
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        //public virtual int CustomerId { get; set; }

        /// <summary>
        /// 购物车Id 未登录是临时ID(生成的负值）
        /// </summary>
        public virtual int ShoppingCartId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public virtual int Quantity { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? DateModified
        {
            get;
            set;
        }
    }
}
