using System;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 销售备注
    /// </summary>
    [Serializable]
    public class CustomerRemark
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// adminId
        /// </summary>
        public virtual int AdminId
        {
            get;
            set;
        } 
    }
}