using Com.Panduo.Common;
using Com.Panduo.Entity.SEO;
using Com.Panduo.Service.SEO;
using Com.Panduo.ServiceImpl.SEO.Dao;
using System.Collections.Generic;
using System.Linq;

namespace Com.Panduo.ServiceImpl.SEO
{
    public class MetaService : IMetaService
    {
        public IMetaHomeDao MetaHomeDao { private get; set; }
        public IMetaListDao MetaListDao { private get; set; }
        public IMetaAreaDao MetaAreaDao { private get; set; }

        #region Meta首页
        public void SetMetaHome(IList<MetaHome> metaHomes)
        {
            foreach (var vo in metaHomes)
            {
                var po = MetaHomeDao.GetMetaHomeByType((int)vo.PageType, vo.LanguageId);
                if (po.IsNullOrEmpty())
                {
                    po = new MetaHomePo{};
                    ObjectHelper.CopyProperties(vo, po, new[] { "PageType" });
                    po.PageType = (int)vo.PageType;
                    MetaHomeDao.AddObject(po);
                }
                else
                {
                    po.Breadcrumb = vo.Breadcrumb;
                    po.Description = vo.Description;
                    po.Keywords = vo.Keywords;
                    po.Title = vo.Title;
                    MetaHomeDao.UpdateObject(po);
                }
            }
        }

        public IList<MetaHome> GetMetaHomesByLanguageId(int languageId)
        {
            return MetaHomeDao.GetMetaHomesByLanguageId(languageId).Select(GetMetaHomeVoFromPo).ToList();
        }

        public MetaHome GetMetaHomeByType(MetaHomePageType type, int languageId)
        {
            return GetMetaHomeVoFromPo(MetaHomeDao.GetMetaHomeByType((int)type, languageId));
        }
        #endregion

        #region Meta列表
        public void SetMetaList(IList<MetaList> metaLists)
        {
            foreach (var vo in metaLists)
            {
                var po = MetaListDao.GetMetaListByType(vo.CategoryId, vo.LanguageId, (int)vo.PageType);
                if (po.IsNullOrEmpty())
                {
                    po = new MetaListPo
                    {
                        PageType = (int)vo.PageType,
                        LanguageId = vo.LanguageId,
                        Breadcrumb = vo.Breadcrumb,
                        Description = vo.Description,
                        Keywords = vo.Keywords,
                        Title = vo.Title,
                        Alias = vo.Alias,
                        CategoryId = vo.CategoryId,
                        DescriptionPro = vo.DescriptionPro,
                        KeywordsPro = vo.KeywordsPro,
                        TitlePro = vo.TitlePro
                    };
                    MetaListDao.AddObject(po);
                }
                else
                {
                    po.Breadcrumb = vo.Breadcrumb;
                    po.Description = vo.Description;
                    po.Keywords = vo.Keywords;
                    po.Title = vo.Title;
                    po.Alias = vo.Alias;
                    po.DescriptionPro = vo.DescriptionPro;
                    po.KeywordsPro = vo.KeywordsPro;
                    po.TitlePro = vo.TitlePro;
                    MetaListDao.UpdateObject(po);
                }
            }
        }

        public IList<MetaList> GetMetaListByType(int categoryId, int languageId)
        {
            return MetaListDao.GetMetaListByType(categoryId, languageId).Select(GetMetaListVoFromPo).ToList();
        }

        public MetaList GetMetaListByType(int categoryId, int languageId, MetaListPageType type)
        {
            return GetMetaListVoFromPo(MetaListDao.GetMetaListByType(categoryId, languageId, (int)type));
        }
        #endregion

        #region Meta专区
        public void SetMetaArea(IList<MetaArea> metaAreas)
        {
            foreach (var vo in metaAreas)
            {
                var po = MetaAreaDao.GetOneObject("FROM MetaAreaPo WHERE AreaId = ? and LanguageId = ?", new object[] { vo.AreaId, vo.LanguageId });
                if (po.IsNullOrEmpty())
                {
                    po = new MetaAreaPo { };
                    ObjectHelper.CopyProperties(vo, po, new []{""});
                    MetaAreaDao.AddObject(po);
                }
                else
                {
                    po.PageName = vo.PageName;
                    po.Description = vo.Description;
                    po.Keywords = vo.Keywords;
                    po.Title = vo.Title;
                    MetaAreaDao.UpdateObject(po);
                }
            }
        }

        public IList<MetaArea> GetMetaAreasByAreaId(int areaId)
        {
            return MetaAreaDao.FindDataByHql("FROM MetaAreaPo WHERE AreaId = ?", areaId).Select(GetMetaAreaVoFromPo).ToList();
        }

        public MetaArea GetMetaAreasByAreaId(int areaId, int languageId)
        {
            return GetMetaAreaVoFromPo(MetaAreaDao.GetOneObject("FROM MetaAreaPo WHERE AreaId = ? and LanguageId = ?", new object[] { areaId, languageId }));
        }
        #endregion

        #region 辅助方法
        internal static MetaHome GetMetaHomeVoFromPo(MetaHomePo metaHomePo)
        {
            MetaHome metaHome = null;
            if (!metaHomePo.IsNullOrEmpty())
            {
                metaHome = new MetaHome
                {
                    Id = metaHomePo.Id,
                    PageType = metaHomePo.PageType.ToEnum<MetaHomePageType>(),
                    LanguageId = metaHomePo.LanguageId,
                    Breadcrumb = metaHomePo.Breadcrumb,
                    Description = metaHomePo.Description,
                    Keywords = metaHomePo.Keywords,
                    Title = metaHomePo.Title
                };
            }
            return metaHome;
        }

        internal static MetaHomePo GetMetaHomePoFromVo(MetaHome metaHome)
        {
            MetaHomePo metaHomePo = null;
            if (!metaHome.IsNullOrEmpty())
            {
                metaHomePo = new MetaHomePo
                {
                    Id = metaHome.Id,
                    PageType = (int)metaHome.PageType,
                    LanguageId = metaHome.LanguageId,
                    Breadcrumb = metaHome.Breadcrumb,
                    Description = metaHome.Description,
                    Keywords = metaHome.Keywords,
                    Title = metaHome.Title
                };
            }
            return metaHomePo;
        }

        internal static MetaList GetMetaListVoFromPo(MetaListPo metaListPo)
        {
            MetaList metaList = null;
            if (!metaListPo.IsNullOrEmpty())
            {
                metaList = new MetaList
                {
                    Id = metaListPo.Id,
                    PageType = metaListPo.PageType.ToEnum<MetaListPageType>(),
                    LanguageId = metaListPo.LanguageId,
                    Breadcrumb = metaListPo.Breadcrumb,
                    Description = metaListPo.Description,
                    Keywords = metaListPo.Keywords,
                    Title = metaListPo.Title,
                    Alias = metaListPo.Alias,
                    CategoryId = metaListPo.CategoryId,
                    DescriptionPro = metaListPo.DescriptionPro,
                    KeywordsPro = metaListPo.KeywordsPro,
                    TitlePro = metaListPo.TitlePro
                };
            }
            return metaList;
        }

        internal static MetaListPo GetMetaListPoFromVo(MetaList metaList)
        {
            MetaListPo metaListPo = null;
            if (!metaList.IsNullOrEmpty())
            {
                metaListPo = new MetaListPo
                {
                    Id = metaList.Id,
                    PageType = (int)metaList.PageType,
                    LanguageId = metaList.LanguageId,
                    Breadcrumb = metaList.Breadcrumb,
                    Description = metaList.Description,
                    Keywords = metaList.Keywords,
                    Title = metaList.Title,
                    Alias = metaList.Alias,
                    CategoryId = metaList.CategoryId,
                    DescriptionPro = metaList.DescriptionPro,
                    KeywordsPro = metaList.KeywordsPro,
                    TitlePro = metaList.TitlePro
                };
            }
            return metaListPo;
        }
        internal static MetaArea GetMetaAreaVoFromPo(MetaAreaPo metaAreaPo)
        {
            var metaArea = new MetaArea();
            if (!metaAreaPo.IsNullOrEmpty())
            {
                ObjectHelper.CopyProperties(metaAreaPo, metaArea, new[] { "" });
            }
            return metaArea;
        }
        #endregion
    }
}