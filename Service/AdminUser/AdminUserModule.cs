using System;

namespace Com.Panduo.Service.AdminUser
{
    /// <summary>
    ///描述：管理员表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-05 14:57:32
    /// </summary>
    [Serializable]
    public class AdminUserModule
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 后台用户Id
        /// </summary>
        public virtual int AdminId { get; set; }

        /// <summary>
        /// 模块编号
        /// </summary>
        public virtual string ModuleCode { get; set; }
    }
}

