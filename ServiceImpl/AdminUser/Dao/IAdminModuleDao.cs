using System.Collections;
using System.Collections.Generic;
using Com.Panduo.Entity.AdminUser;

namespace Com.Panduo.ServiceImpl.AdminUser.Dao
{
    public interface IAdminModuleDao : IBaseDao<AdminModulePo, int>
    {
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="action">action</param>
        /// <returns></returns>
        AdminModulePo GetAdminModulePo(string controller, string action);

        /// <summary>
        /// 通过菜单编号获取对应模块
        /// </summary>
        /// <param name="adminMenuCode"></param>
        /// <returns></returns>
        IList<AdminModulePo> GetAdminModulePos(string adminMenuCode);
    }
}