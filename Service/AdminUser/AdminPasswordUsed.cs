using System;

namespace Com.Panduo.Service.AdminUser
{
    public class AdminPasswordUsed
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
        /// 用户名Id
        /// </summary>
        public virtual int AdminId
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 修改密码时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
    }
}