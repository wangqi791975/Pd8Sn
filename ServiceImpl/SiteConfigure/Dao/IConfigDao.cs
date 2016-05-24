//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IConfigDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.SiteConfigure;

namespace Com.Panduo.ServiceImpl.SiteConfigure.Dao
{
    public interface IConfigDao : IBaseDao<ConfigPo, int>
    {
        /// <summary>
        /// 获取单个配置项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>ConfigPo</returns>
        ConfigPo GetConfigByKey(string key);

        /// <summary>
        /// 获取配置项列表
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>ConfigPo列表</returns>
        IList<ConfigPo> GetConfigListByKey(string key);

        /// <summary>
        /// 通过key修改config
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void UpdateConfig(string key, string value);
    }
}
