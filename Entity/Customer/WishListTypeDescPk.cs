using System;

namespace Com.Panduo.Entity.Customer
{
    [Serializable]
    public class WishListTypeDescPk
    {
        /// <summary>
        /// 类型Id(如1、10、20)
        /// </summary> 
        public virtual int TypeId
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
            if (obj is WishListTypeDescPk)
            {
                return ((WishListTypeDescPk)obj).LanguageId == this.LanguageId && ((WishListTypeDescPk)obj).TypeId.Equals(this.TypeId);
            }
            return false; 
        }

        /// <summary>
        /// 重写HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return TypeId.GetHashCode() + LanguageId.GetHashCode();
        }
    }
}
