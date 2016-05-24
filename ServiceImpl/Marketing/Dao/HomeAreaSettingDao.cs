//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：HomeAreaSettingDao.cs
//创 建 人：罗海明
//创建时间：2015/04/14 18:08:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Banner;

namespace Com.Panduo.ServiceImpl.Marketing.Dao
{ 
    public class HomeAreaSettingDao:BaseDao<HomeAreaSettingPo,int>, IHomeAreaSettingDao
    {
        public IList<HomeAreaSettingPo> GetHomeAreaSetting(int type)
        {
            return FindDataByHql("from HomeAreaSettingPo where AreaType = ?", type);
        }

        public HomeAreaSettingPo GetHomeAreaSetting(int languageId, int type)
        {
            return GetOneObject("from HomeAreaSettingPo where LanguageId = ? And AreaType=?", new object[] { languageId, type });
        }
    } 
}
   