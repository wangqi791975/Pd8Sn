using System.Collections;
using System.Collections.Generic;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public interface IAdminUserModuleDao:IBaseDao<AdminUserModulePo,int>
    {
        /// <summary>
        /// 获取用户对应模块
        /// </summary>
        /// <param name="adminId">用户Id</param>
        /// <returns>用户对应模块</returns>
        IList<AdminUserModulePo> GetAdminUserModules(int adminId);

        /// <summary>
        /// 删除用户对应模块
        /// </summary>
        /// <param name="adminId">用户Id</param>
        void DeleteAdminUserModules(int adminId);
    }
}