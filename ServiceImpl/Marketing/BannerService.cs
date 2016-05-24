//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：BannerService.cs
//创 建 人：罗海明
//创建时间：2015/04/14 16:40:40 
//功能说明：横幅广告服务
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using Com.Panduo.Common;
using Com.Panduo.Entity.Banner;
using Com.Panduo.Service.Marketing;
using Com.Panduo.Service.Marketing.Banner;
using Com.Panduo.ServiceImpl.Marketing.Dao;

namespace Com.Panduo.ServiceImpl.Marketing
{
    public class BannerService:IBannerService
    {
        #region IOC
        public IHomeAreaSettingDao HomeAreaSettingDao { private get; set; }
        public IBannerDao BannerDao { private get; set; }
        #endregion

        /// <summary>
        /// 设置Banner广告
        /// </summary>
        /// <param name="banner"></param>
        public void SetBanner(IList<BannerInfo> banner)
        {
            var listUpdate = new List<BannerPo>();
            var listAdd = new List<BannerPo>();
            foreach (var vo in banner)
            {
                var po = BannerDao.GetBanner(vo.LanguageId,vo.IsCountdown);
                if (!po.IsNullOrEmpty())
                {
                    po.AdWord = vo.Content;
                    po.ShowIndex = vo.IsShowHome;
                    po.StartTime = vo.BannerStartTime;
                    po.EndTime = vo.BannerEndTime;
                    po.Status = vo.IsValid;
                    po.IsCountdown = vo.IsCountdown;
                    listUpdate.Add(po);
                }
                else
                {
                    if (!vo.Content.IsNullOrEmpty())
                    {
                        po = new BannerPo()
                        {
                            AdWord = vo.Content,
                            ShowIndex = vo.IsShowHome,
                            Status = vo.IsValid,
                            LanguageId = vo.LanguageId,
                            StartTime = vo.BannerStartTime,
                            EndTime = vo.BannerEndTime,
                            IsCountdown = vo.IsCountdown,
                            DateCreated=DateTime.Now,
                        };
                        listAdd.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                BannerDao.UpdateObjects(listUpdate);
            }
            if (!listAdd.IsNullOrEmpty())
            {
                BannerDao.AddObjects(listAdd);
            }
        }

        /// <summary>
        /// 获取Banner广告(编辑时获取数据)
        /// </summary>
        /// <param name="isCountdown">是否倒计时Banner</param>
        /// <returns></returns>
        public IList<BannerInfo> GetBanner(bool isCountdown)
        {
             var bannerInfoList = new List<BannerInfo>();
             var list = BannerDao.GetBanner(isCountdown);
             if (!list.IsNullOrEmpty())
             {
                 foreach (var po in list)
                 {
                     bannerInfoList.Add(GetBannerVoFromPo(po));
                 }
             }
             return bannerInfoList;
        }

        /// <summary>
        /// 前台获取Banner广告（优先获取倒计时Banner）
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        public BannerInfo GetBanner(int languageId)
        {
           var bannerPo= BannerDao.GetBanner(languageId, true);
            if (!bannerPo.IsNullOrEmpty())
            {
                return GetBannerVoFromPo(bannerPo);
            }
            else
            {
               var po= BannerDao.GetBanner(languageId, false);
               return GetBannerVoFromPo(po);
            }
        }

        /// <summary>
        /// 首页区域设置
        /// </summary>
        /// <param name="setting">HomeAreaSetting</param>
        public void SetHomeAreaSetting(IList<HomeAreaSetting> setting)
        {
            var listUpdate = new List<HomeAreaSettingPo>();
            var listAdd = new List<HomeAreaSettingPo>();
            foreach (var vo in setting)
            {
                var po = HomeAreaSettingDao.GetHomeAreaSetting(vo.LanguageId, (int)vo.AreaType);
                if (!po.IsNullOrEmpty())
                {
                    po.Description = vo.Content;
                    listUpdate.Add(po);
                }
                else
                {
                    if (!vo.Content.IsNullOrEmpty())
                    {
                        po = new HomeAreaSettingPo()
                        {
                            Description = vo.Content,
                            LanguageId = vo.LanguageId,
                            AreaType = (int)vo.AreaType,
                            DateCreated = DateTime.Now,
                        };
                        listAdd.Add(po);
                    }
                }
            }
            if (!listUpdate.IsNullOrEmpty())
            {
                HomeAreaSettingDao.UpdateObjects(listUpdate);
            }
            if (!listAdd.IsNullOrEmpty())
            {
                HomeAreaSettingDao.AddObjects(listAdd);
            }
        }


        /// <summary>
        /// 根据区域类型获取首页区域设置信息
        /// </summary>
        /// <param name="type">区域类型</param>
        /// <returns></returns>
        public IList<HomeAreaSetting> GetHomeAreaSetting(HomeAreaType type)
        {
            var homeAreaSettingList = new List<HomeAreaSetting>();
            var list = HomeAreaSettingDao.GetHomeAreaSetting((int) type);
            if (!list.IsNullOrEmpty())
            {
                foreach (var po in list)
                {
                    homeAreaSettingList.Add(GetHomeAreaSettingVoFromPo(po));
                }
            }
            return homeAreaSettingList;
        }

        public HomeAreaSetting GetHomeAreaSetting(int languageId, HomeAreaType areaType)
        {
            var po = HomeAreaSettingDao.GetHomeAreaSetting(languageId, (int)areaType);
            return GetHomeAreaSettingVoFromPo(po);
        }


        /// <summary>
        /// Banner Po转Vo
        /// </summary>
        /// <param name="bannerPo"></param>
        /// <returns></returns>
        internal static BannerInfo GetBannerVoFromPo(BannerPo bannerPo)
        {
            BannerInfo bannerInfo = null;
            if (!bannerPo.IsNullOrEmpty())
            {
                bannerInfo = new BannerInfo
                {
                    IsShowHome=bannerPo.ShowIndex,
                    IsValid = bannerPo.Status,
                    Content = bannerPo.AdWord,
                    LanguageId = bannerPo.LanguageId,
                    BannerStartTime=bannerPo.StartTime,
                    BannerEndTime = bannerPo.EndTime,
                    IsCountdown = bannerPo.IsCountdown,
                };
            }
            return bannerInfo;
        }

        /// <summary>
        /// HomeAreaSetting Po转Vo
        /// </summary>
        /// <param name="homeAreaSettingPo"></param>
        /// <returns></returns>
        internal static HomeAreaSetting GetHomeAreaSettingVoFromPo(HomeAreaSettingPo homeAreaSettingPo)
        {
            HomeAreaSetting homeAreaSetting = null;
            if (!homeAreaSettingPo.IsNullOrEmpty())
            {
                homeAreaSetting = new HomeAreaSetting
                {
                    AreaId = homeAreaSettingPo.AreaId,
                    Content = homeAreaSettingPo.Description,
                    AreaType = homeAreaSettingPo.AreaType.ToEnum<HomeAreaType>(),
                    LanguageId=homeAreaSettingPo.LanguageId,
                };
            }
            return homeAreaSetting;
        }
    }
}
