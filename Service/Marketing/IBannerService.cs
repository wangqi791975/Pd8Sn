//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：IBannerService.cs
//创 建 人：罗海明
//创建时间：2015/04/07 14:40:40 
//功能说明：横幅广告接口
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using Com.Panduo.Service.Marketing.Banner;


namespace Com.Panduo.Service.Marketing
{
    public interface IBannerService
    {
        /// <summary>
        /// 设置Banner广告
        /// </summary>
        /// <param name="banner"></param>
        void SetBanner(IList<BannerInfo> banner);

        /// <summary>
        /// 获取Banner广告(编辑时获取数据)
        /// </summary>
        /// <param name="isCountdown">是否倒计时Banner</param>
        /// <returns></returns>
        IList<BannerInfo> GetBanner(bool isCountdown);

        /// <summary>
        /// 前台获取Banner广告（优先获取倒计时Banner）
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        BannerInfo GetBanner(int languageId);

        /// <summary>
        /// 首页区域设置
        /// </summary>
        /// <param name="setting">HomeAreaSetting</param>
        void SetHomeAreaSetting(IList<HomeAreaSetting> setting);

        /// <summary>
        /// 根据区域类型获取首页区域设置信息
        /// </summary>
        /// <param name="type">区域类型</param>
        /// <returns></returns>
        IList<HomeAreaSetting> GetHomeAreaSetting(HomeAreaType type);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="areaType"></param>
        /// <returns></returns>
        HomeAreaSetting GetHomeAreaSetting(int languageId,HomeAreaType areaType);

    }
}
