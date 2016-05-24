using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    [Serializable]
    public class OrderPayStatusDescriptionPk
    {
        /// <summary>
        /// 状态编码(如1、10、20)
        /// </summary>
        [Property(Column = "pay_status")]
        public virtual int PayStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 语言ID
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }

        /// <summary>
        /// 重写等于
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is OrderPayStatusDescriptionPk)
            {
                return ((OrderPayStatusDescriptionPk)obj).LanguageId == this.LanguageId && ((OrderPayStatusDescriptionPk)obj).PayStatus.Equals(this.PayStatus);
            }
            return false;
        }

        /// <summary>
        /// 重写HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return PayStatus.GetHashCode() + LanguageId.GetHashCode();
        }
    }
}
