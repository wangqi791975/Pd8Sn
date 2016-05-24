using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public class AdminRoleDao : BaseDao<AdminRolePo, int>, IAdminRoleDao
    {
        public bool IsRoleNameUsed(string roleName,int exceptId)
        {
            return ExistObjectExceptId("Name", roleName, "Id", exceptId);
        }
    }
}
