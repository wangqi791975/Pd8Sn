using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Cash
{
    /// <summary>
    /// cash限额
    /// </summary>
    public class CashSection
    {
        /// <summary>
        /// 编号
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// 最小金额
        /// </summary>
        public virtual int AmountMinimum
        {
            get;
            set;
        }

        /// <summary>
        /// 最大金额
        /// </summary>
        public virtual int AmountMaximum
        {
            get;
            set;
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime DateModified
        {
            get;
            set;
        }

        /// <summary>
        /// 管理员ID
        /// </summary>
        public virtual int AdminId
        {
            get;
            set;
        }

    }
}
