using System;

namespace Com.Panduo.Service.AdminUser
{
    [Serializable]
    public class AdminModule
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        public virtual string ModuleCode { get; set; }

        /// <summary>
        /// 菜单编号
        /// </summary>
        public virtual string MenuCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public virtual string Controller { get; set; }

        /// <summary>
        /// action
        /// </summary>
        public virtual string Action { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual string Sort { get; set; }
    }
}

