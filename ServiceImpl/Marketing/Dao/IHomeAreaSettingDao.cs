//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IHomeAreaSettingDao.cs
//创 建 人：罗海明
//创建时间：2015/04/14 17:59:50 
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
    public interface IHomeAreaSettingDao:IBaseDao<HomeAreaSettingPo,int>
    {

        /// <summary>
        /// 根据区域类型获取首页区域设置信息
        /// </summary>
        /// <param name="type">区域类型</param>
        /// <returns></returns>
        IList<HomeAreaSettingPo> GetHomeAreaSetting(int type);


        /// <summary>
        /// 根据区域类型和语种Id获取首页区域设置信息
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <param name="type">区域类型</param>
        /// <returns></returns>
        HomeAreaSettingPo GetHomeAreaSetting(int languageId,int type);
    } 
}
   