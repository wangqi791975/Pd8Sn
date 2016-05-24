using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common.Mvc.Model
{
    /// <summary>
    /// Json数据返回
    /// </summary>
    [Serializable]
    public class JsonData
    {
        /// <summary>
        /// 是否成功:true代表成功,false代表失败
        /// </summary>
        public virtual bool Succeed { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public virtual string Message { get; set; }
        /// <summary>
        /// 传输数据
        /// </summary>
        public virtual object Data { get; set; }
        /// <summary>
        /// 传输而外的数据
        /// </summary>
        public virtual object ExtraData { get; set; }
    }
}