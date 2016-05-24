//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ConfigDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
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
    public class ConfigDao : BaseDao<ConfigPo, int>, IConfigDao
    {
        public ConfigPo GetConfigByKey(string key)
        {
            return GetOneObject("from ConfigPo where Key=?", key);
        }

        public IList<ConfigPo> GetConfigListByKey(string key)
        {
            return FindDataByHql("from ConfigPo where Key=?", key);
        }

        public void UpdateConfig(string key, string value)
        {
            UpdateObjectByHql("UPDATE ConfigPo SET Value = ? WHERE Key = ?", new object[] { value, key });
        }
    }
}
