using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public interface IAdminDao : IBaseDao<AdminPo, int>
    {
        AdminPo GetAdminUserByEmail(string email);

        int GetMaxAdminUserId();

        int GetCountOfAdminUsersByRole(int roleId);

        IList<AdminPo> GetAllAdminUserPosByRole(int roleId);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="adminId">用户Id</param>
        /// <param name="status">用户状态</param>
        void UpdateAdminStatus(int adminId,int status);
    }
}
