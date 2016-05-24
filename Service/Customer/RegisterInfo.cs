using System;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 注册信息
    /// </summary>
    [Serializable]
    public class RegisterInfo
    {
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime? DateCreated { get; set; }
        /// <summary>
        /// 注册ip
        /// </summary>
        public virtual string RegisterIp { get; set; }
        /// <summary>
        /// 浏览器语言
        /// </summary>
        public virtual string UserLanguage { get; set; }

        /// <summary>
        /// 浏览器用户代理信息
        /// </summary>
        public virtual string UserAgent { get; set; }
        /// <summary>
        /// 浏览器来源URl
        /// </summary>
        public virtual string UrlReferrer { get; set; }
        /// <summary>
        /// 来源类型（手机，网站）
        /// </summary>
        public virtual SourceType SourceType { get; set; }
    }

    public enum SourceType
    {
        W = 1,//PC站
        M = 2,//手机站
    }
}