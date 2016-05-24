using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    [Serializable]
    public class OrderStatusDescriptionPk
    {
        /// <summary>
        /// 状态编码(如1、10、20)
        /// </summary> 
        public virtual int OrderStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 语言ID
        /// </summary> 
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
            if (obj is OrderStatusDescriptionPk)
            {
                return ((OrderStatusDescriptionPk)obj).LanguageId == this.LanguageId && ((OrderStatusDescriptionPk)obj).OrderStatus.Equals(this.OrderStatus);
            }
            return false; 
        }

        /// <summary>
        /// 重写HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OrderStatus.GetHashCode() + LanguageId.GetHashCode();
        }
    }
}
