using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product.ProductArea
{
    /// <summary>
    /// 产品专区服务
    /// </summary>
    public interface IProductAreaService
    {

        #region 业务异常
        /// <summary>
        /// 产品不存在
        /// </summary>
        string ERROR_PRODUCT_NOT_EXIST { get; }
        /// <summary>
        /// 产品专区不存在
        /// </summary>
        string ERROR_PRODUCTAREA_NOT_EXIST { get; }
        /// <summary>
        /// 产品专区该语种名称不存在
        /// </summary>
        string ERROR_PRODUCTAREA_LANGUAGENAME_NOT_EXIST { get; }
        /// <summary>
        /// 产品专区名称已经存在
        /// </summary>
        string ERROR_PRODUCTAREA_NAME_EXIST { get; }
        /// <summary>
        /// 产品专区-该语种名称已经存在
        /// </summary>
        string ERROR_PRODUCTAREA_LANGUAGENAME_EXIST { get; }

        /// <summary>
        /// 该产品在产品专区已经存在
        /// </summary>
        string ERROR_PRODUCTAREA_PRODUCT_IS_EXIST { get; }
        #endregion

        #region 专区

        /// <summary>
        /// 设置专区
        /// </summary>
        /// <param name="area">专区实体</param>
        int SetProductArea(ProductArea area);

        /// <summary>
        /// 设置专区状态
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <param name="isValid">状态</param>
        void SetProductAreaStatus(int productAreaId, bool isValid);

        /// <summary>
        /// 获取专区
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <returns>专区实体</returns>
        ProductArea GetProductAreaById(int productAreaId);

        /// <summary>
        /// 删除产品专区
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        void DeleteProductArea(int productAreaId);

        /// <summary>
        /// 后台专区管理列表 查询专区
        /// 注意：列表里List(ProductAreaLanguage
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        PageData<ProductArea> FindProductAreas(int currentPage, int pageSize, IDictionary<ProductAreaSearchCriteria, object> searchCriteria, IList<Sorter<ProductAreaSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 获取专区多语言
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>专区多语言</returns>
        ProductAreaLanguage GetProductAreaLanguage(int productAreaId, int languageId);

        /// <summary>
        /// 生成营销专区URL
        /// </summary>
        /// <param name="languageId">语言</param>
        /// <param name="productAreaId">名称</param>
        /// <param name="categoryId">类别</param>
        /// <param name="productRouteName">产品专区路由名称</param>
        /// <returns></returns>
        string GetProductAreaURL(int languageId, int productAreaId, int categoryId, string productRouteName);

        /// <summary>
        /// 获取专区多语言列表
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <returns>专区多语言列表</returns>
        //IList<ProductAreaLanguage> GetAllProductAreaLanguages(int productAreaId);

        #endregion

        #region 专区产品

        /// <summary>
        /// 批量设置专区产品
        /// </summary>
        /// <param name="list">专区产品</param>
        void SetProductAreaRelativeList(List<ProductAreaRelative> list);

        /// <summary>
        /// 清空专区产品（物理删除）
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        void ClearProductAreaRelative(int productAreaId);

        /// <summary>
        /// 查询专产品
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        //PageData<ProductAreaRelative> FindProductAreaRelatives(int currentPage, int pageSize, IDictionary<ProductAreaRelativeSearchCriteria, object> searchCriteria, IList<Sorter<ProductAreaRelativeSorterCriteria>> sorterCriteria);
        #endregion
    }

    /// <summary>
    /// 专区查询条件
    /// </summary>
    public enum ProductAreaSearchCriteria
    {
        /// <summary>
        /// 专区名称
        /// </summary>
        AreaName
    }

    /// <summary>
    /// 专区排序条件
    /// </summary>
    public enum ProductAreaSorterCriteria
    {
        ProductCode
    }

    /*/// <summary>
    /// 专区产品查询条件
    /// </summary>
    public enum ProductAreaRelativeSearchCriteria
    {
        /// <summary>
        /// 产品货号
        /// </summary>
        ProductCode

    }

    /// <summary>
    /// 专区产品排序条件
    /// </summary>
    public enum ProductAreaRelativeSorterCriteria
    {

    }*/
}
