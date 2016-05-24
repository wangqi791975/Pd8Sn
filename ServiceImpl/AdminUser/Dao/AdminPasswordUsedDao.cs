using System;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public class AdminPasswordUsedDao : BaseDao<AdminPasswordUsedPo, int>, IAdminPasswordUsedDao
    {
        public AdminPasswordUsedPo GetAdminPasswordUsed(string password, DateTime halfTime)
        {
            return GetOneObject("FROM AdminPasswordUsedPo WHERE Password = ? AND DateCreated >= ?", new object[] { password, halfTime });
        }
    }
}