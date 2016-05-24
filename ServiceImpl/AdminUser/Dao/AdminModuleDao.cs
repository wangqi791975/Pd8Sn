using System.Collections.Generic;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public class AdminModuleDao : BaseDao<AdminModulePo, int>, IAdminModuleDao
    {
        public AdminModulePo GetAdminModulePo(string controller, string action)
        {
            return GetOneObject("FROM AdminModulePo WHERE Controller = ? AND Action = ?",new object[] { controller, action });
        }

        public IList<AdminModulePo> GetAdminModulePos(string adminMenuCode)
        {
            return FindDataByHql("FROM AdminModulePo WHERE MenuCode = ?", adminMenuCode);
        }
    }
}