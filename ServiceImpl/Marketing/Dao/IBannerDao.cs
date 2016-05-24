//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IBannerDao.cs
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
    public interface IBannerDao:IBaseDao<BannerPo,int>
    {
        /// <summary>
        /// 获取Banner广告(编辑时获取数据)
        /// </summary>
        /// <param name="isCountdown">是否倒计时Banner</param>
        /// <returns></returns>
        IList<BannerPo> GetBanner(bool isCountdown);

        /// <summary>
        /// 前台获取Banner广告
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="isCountdown"></param>
        /// <returns></returns>
        BannerPo GetBanner(int languageId, bool isCountdown);
    } 
}
   