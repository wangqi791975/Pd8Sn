using System;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public interface IAdminPasswordUsedDao : IBaseDao<AdminPasswordUsedPo, int>
    {
        AdminPasswordUsedPo GetAdminPasswordUsed(string password, DateTime halfTime);
    }
}