using System;
using Com.Panduo.Entity.Order;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product.Dailydeal
{
    [Serializable]
    public class DailydealTitlePk
    {
        /// <summary>
        /// 标语Id
        /// </summary>
        [Property(Column = "title_id")]
        public virtual int TitleId
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
            if (obj is DailydealTitlePk)
            {
                return ((DailydealTitlePk)obj).LanguageId == this.LanguageId && ((DailydealTitlePk)obj).TitleId.Equals(this.TitleId);
            }
            return false;
        }

        /// <summary>
        /// 重写HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return TitleId.GetHashCode() + LanguageId.GetHashCode();
        }
    }
}
