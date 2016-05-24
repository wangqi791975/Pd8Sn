using System.Collections;
using System.Collections.Generic;

namespace Com.Panduo.Service.SEO
{
    /// <summary>
    /// Meta信息服务接口
    /// </summary>
    public interface IMetaService
    {
        #region 常量
        #endregion

        #region 方法
        #region Meta首页
        /// <summary>
        /// 设置meta首页信息
        /// </summary>
        /// <param name="metaHomes">MetaHome实体list</param>
        void SetMetaHome(IList<MetaHome> metaHomes);

        /// <summary>
        /// 根据语种取得所有的meta首页信息
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns>MetaHome实体list</returns>
        IList<MetaHome> GetMetaHomesByLanguageId(int languageId);

        /// <summary>
        /// 根据页面类型、语种Id取得meta首页信息
        /// </summary>
        /// <param name="type">MetaHomePageType</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>MetaHome实体</returns>
        MetaHome GetMetaHomeByType(MetaHomePageType type, int languageId);
        #endregion

        #region Meta列表
        /// <summary>
        /// 设置meta列表信息
        /// </summary>
        /// <param name="metaLists">MetaList实体list</param>
        void SetMetaList(IList<MetaList> metaLists);

        /// <summary>
        /// 根据类别Id、语种Id取得所有类型meta列表信息
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>MetaList实体</returns>
        IList<MetaList> GetMetaListByType(int categoryId, int languageId);

        /// <summary>
        /// 根据类别Id、语种Id、页面类型取得meta列表信息
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="languageId">语种Id</param>
        /// <param name="type">页面类型MetaListPageType</param>
        /// <returns>MetaList实体</returns>
        MetaList GetMetaListByType(int categoryId, int languageId, MetaListPageType type);
        #endregion

        #region Meta专区
        /// <summary>
        /// 设置meta专区信息
        /// </summary>
        /// <param name="metaAreas">metaAreas实体list</param>
        void SetMetaArea(IList<MetaArea> metaAreas);

        /// <summary>
        /// 根据专区id取得meta专区信息
        /// </summary>
        /// <param name="areaId">专区Id</param>
        /// <returns>MetaArea实体list</returns>
        IList<MetaArea> GetMetaAreasByAreaId(int areaId);

        /// <summary>
        /// 根据专区id取得meta专区信息
        /// </summary>
        /// <param name="areaId">专区Id</param>
        /// <param name="languageId">语言id</param>
        /// <returns>MetaArea实体list</returns>
        MetaArea GetMetaAreasByAreaId(int areaId, int languageId);
        #endregion
        #endregion
    }
}