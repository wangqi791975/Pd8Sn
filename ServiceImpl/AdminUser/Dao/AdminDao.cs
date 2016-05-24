using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public class AdminDao : BaseDao<AdminPo, int>, IAdminDao
    {
        public AdminPo GetAdminUserByEmail(string email)
        {
            return GetOneObject("from AdminPo where AccountEmail = ?", email);
        }

        public int GetMaxAdminUserId()
        {
            var obj = GetSingleObject("select max(Code*1) from AdminPo");

            return obj == null ? 0 : int.Parse(obj.ToString());
        }

        public int GetCountOfAdminUsersByRole(int roleId)
        {
            var obj = GetSingleObject("select count(Id) from AdminPo where RoleId=?", roleId);

            return obj == null ? 0 : int.Parse(obj.ToString());
        }

        public IList<AdminPo> GetAllAdminUserPosByRole(int roleId)
        {
            return FindDataByHql("from AdminPo where RoleId=?", roleId);
        }

        public void UpdateAdminStatus(int adminId, int status)
        {
            UpdateObjectByHql("UPDATE AdminPo SET Status = ? WHERE AdminId = ?", new object[] { status, adminId });
        }
    }
}
