using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    namespace Com.Panduo.Service.Customer.Product
    {
        /// <summary>
        /// 后台客户心愿单产品Vo
        /// </summary>
        [Serializable]
        public class CustomerWishListProduct
        {
            /// <summary>
            /// Id
            /// </summary>
            public virtual int Id { get; set; }

            /// <summary>
            /// 客户Id
            /// </summary>
            public virtual int CustomerId { get; set; }

            /// <summary>
            /// 客户邮箱
            /// </summary>
            public virtual string CustomerEmail { get; set; }

            /// <summary>
            /// 客户姓名
            /// </summary>
            public virtual string CustomerFull { get; set; }

            /// <summary>
            /// 产品Id
            /// </summary>
            public virtual int ProductId { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public virtual int Count { get; set; }

            /// <summary>
            /// 添加时间
            /// </summary>
            public virtual DateTime AddDateTime { get; set; }

            /// <summary>
            /// 喜爱类型Id
            /// </summary>
            public virtual WishListType WishListType { get; set; }

            /// <summary>
            /// 修改时间
            /// </summary>
            public virtual DateTime? DateModified { get; set; }

            /// <summary>
            /// 是否历史心愿单
            /// </summary>
            public virtual bool IsHistory
            {
                get;
                set;
            }
        }
    }

