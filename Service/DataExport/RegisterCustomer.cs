
using System;

namespace Com.Panduo.Service.DataExport
{
    /// <summary>
    ///描述：客户注册数据
    ///创建人: 万天文
    ///创建时间：04/08/2015 16:04:01
    /// </summary>
    [Serializable]
    public class RegisterCustomer
    {
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual string RegisterDateTime {  get; set; }

        /// <summary>
        /// 注册网站
        /// </summary>
        public virtual string RegisterWebSite { get; set; }

        /// <summary>
        /// 注册数
        /// </summary>
        public virtual string RegisterNumber { get; set; }
    }
}

