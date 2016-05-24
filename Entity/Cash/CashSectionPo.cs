using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Cash
{
    /// <summary>
    ///描述：cash限额表
    ///创建人：Tianwen.Wan
    ///创建时间：2015-04-17 15:48:28
    /// </summary>
    [Class(Table = "t_cash_section", Lazy = false, NameType = typeof(CashSectionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CashSectionPo
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Id(1, Name = "Code", Column = "code")]
        [Generator(2, Class = "assigned")]
        public virtual int Code
        {
            get;
            set;
        }

        /// <summary>
        /// 最小金额
        /// </summary>
        [Property(Column = "amount_minimum")]
        public virtual int AmountMinimum
        {
            get;
            set;
        }

        /// <summary>
        /// 最大金额
        /// </summary>
        [Property(Column = "amount_maximum")]
        public virtual int AmountMaximum
        {
            get;
            set;
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime DateModified
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
