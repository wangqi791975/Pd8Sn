
using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.AdminUser
{
    [Serializable]
    public class AdminMenu
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 菜单下的模块
        /// </summary>
        public virtual List<AdminModule> AdminModules { get; set; } 
    }
}

