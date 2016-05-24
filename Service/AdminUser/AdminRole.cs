using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.AdminUser
{  
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AdminRole
    { /// <summary>
        /// 自增id
        /// </summary> 
        public virtual int RoleId
        {
            get;
            set;
        }
        /// <summary>
        /// 角色名称
        /// </summary> 
        public virtual string RoleName
        {
            get;
            set;
        }
        /// <summary>
        /// 状态, 1有效 0无效
        /// </summary> 
        public virtual bool RoleStatus
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
    }
}
