using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public interface IAdminRoleDao : IBaseDao<AdminRolePo, int>
    {
        bool IsRoleNameUsed(string roleName, int exceptId);
    }
}
