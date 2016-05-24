using System.Collections.Generic;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public class AdminUserModuleDao : BaseDao<AdminUserModulePo, int>, IAdminUserModuleDao
    {
        public IList<AdminUserModulePo> GetAdminUserModules(int adminId)
        {
            return FindDataByHql("FROM AdminUserModulePo WHERE AdminId = ?", adminId);
        }

        public void DeleteAdminUserModules(int adminId)
        {
            DeleteObjectByHql("DELETE FROM AdminUserModulePo WHERE AdminId = ?", adminId);
        }
    }
}