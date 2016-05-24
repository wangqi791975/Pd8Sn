using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.ProductArea;
using Com.Panduo.ServiceImpl.Product.Dao;
using Com.Panduo.ServiceImpl.Product.ProductArea.Dao;
using Com.Panduo.ServiceImpl.SiteConfigure.Dao;
using NHibernate.Linq;

namespace Com.Panduo.ServiceImpl.Product.ProductArea
{
    /// <summary>
    /// 产品专区服务
    /// </summary>
    public class ProductAreaService : IProductAreaService
    {
        public IProductAreaDao ProductAreaDao { private get; set; }
        public IProductAreaLanguageDao ProductAreaLanguageDao { private get; set; }
        public IProductAreaRelativeDao ProductAreaRelativeDao { private get; set; }
        public IProductDao ProductDao { private get; set; }
        public ILanguageDao LanguageDao { private get; set; }

        #region 业务异常
        /// <summary>
        /// 产品不存在
        /// </summary>
        public string ERROR_PRODUCT_NOT_EXIST
        {
            get { return "ERROR_PRODUCT_NOT_EXIST"; }
        }
        /// <summary>
        /// 产品专区不存在
        /// </summary>
        public string ERROR_PRODUCTAREA_NOT_EXIST
        {
            get { return "ERROR_PRODUCTAREA_NOT_EXIST"; }
        }
        /// <summary>
        /// 产品专区该语种名称不存在
        /// </summary>
        public string ERROR_PRODUCTAREA_LANGUAGENAME_NOT_EXIST
        {
            get { return "ERROR_PRODUCTAREA_NOT_EXIST"; }
        }
        /// <summary>
        /// 产品专区名称已经存在
        /// </summary>
        public string ERROR_PRODUCTAREA_NAME_EXIST
        {
            get { return "ERROR_PRODUCTAREA_NAME_EXIST"; }
        }
        /// <summary>
        /// 产品专区-该语种名称已经存在
        /// </summary>
        public string ERROR_PRODUCTAREA_LANGUAGENAME_EXIST
        {
            get { return "ERROR_PRODUCTAREA_LANGUAGENAME_EXIST"; }
        }

        /// <summary>
        /// 该产品在产品专区已经存在
        /// </summary>
        public string ERROR_PRODUCTAREA_PRODUCT_IS_EXIST
        {
            get { return "ERROR_PRODUCTAREA_PRODUCT_IS_EXIST"; }
        }
        #endregion

        #region 专区
        /// <summary>
        /// 设置专区
        /// </summary>
        /// <param name="area">专区实体</param>
        public int SetProductArea(Service.Product.ProductArea.ProductArea area)
        {
            var productAreaPo = ProductAreaDao.GetProductAreaById(area.AreaId);
            if (productAreaPo.IsNullOrEmpty())
            {
                productAreaPo = new ProductAreaPo
                {
                    ChineseName = area.AreaName,
                    ShowIndex = area.IsShowHome,
                    Status = area.IsValid
                };
                area.AreaId = ProductAreaDao.AddObject(productAreaPo);
            }
            else
            {
                productAreaPo.ChineseName = area.AreaName;
                productAreaPo.ShowIndex = area.IsShowHome;
                productAreaPo.Status = area.IsValid;
                ProductAreaDao.UpdateObject(productAreaPo);
            }

            foreach (var areaLanguage in area.ProductAreaLanguages)
            {
                var productAreaDescPo = ProductAreaLanguageDao.GetAreaLanguage(area.AreaId, areaLanguage.LanguageId);

                //判断产品专区该语种是否存在
                if (productAreaDescPo.IsNullOrEmpty())
                {
                    productAreaDescPo = new ProductAreaDescPo
                    {
                        ProductAreaId = area.AreaId,
                        LanguageId = areaLanguage.LanguageId,
                        Name = areaLanguage.AreaName,
                        Home = areaLanguage.Home
                    };
                    ProductAreaLanguageDao.AddObject(productAreaDescPo);
                }
                else
                {
                    productAreaDescPo.Name = areaLanguage.AreaName;
                    productAreaDescPo.Home = areaLanguage.Home;
                    ProductAreaLanguageDao.UpdateObject(productAreaDescPo);
                }
            }
            return area.AreaId;
        }

        /// <summary>
        /// 设置专区状态
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <param name="isValid">状态</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCTAREA_NOT_EXIST:产品专区不存在</value>
        /// </exception>
        public void SetProductAreaStatus(int productAreaId, bool isValid)
        {
            var productAreaPo = ProductAreaDao.GetProductAreaById(productAreaId);
            if (productAreaPo.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PRODUCTAREA_NOT_EXIST);
            }
            productAreaPo.Status = isValid;
            ProductAreaDao.UpdateObject(productAreaPo);
        }

        /// <summary>
        /// 获取专区
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <returns>专区实体</returns>
        public Service.Product.ProductArea.ProductArea GetProductAreaById(int productAreaId)
        {
            var productAreaPo = ProductAreaDao.GetProductAreaById(productAreaId);
            var lstProductAreaLanguage = ProductAreaLanguageDao.GetAllProductAreaLanguages(productAreaId);
            return ProductAreaPoConvertToVo(productAreaPo, lstProductAreaLanguage);
        }

        /// <summary>
        /// 删除专区（物理删除）
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        public void DeleteProductArea(int productAreaId)
        {
            var productAreaPo = ProductAreaDao.GetProductAreaById(productAreaId);
            if (productAreaPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PRODUCTAREA_NOT_EXIST);

            ProductAreaRelativeDao.DeleteProductAreaRelative(productAreaId);

            ProductAreaDao.DeleteObject(productAreaPo);
        }


        /// <summary>
        /// 查询专区
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        public PageData<Service.Product.ProductArea.ProductArea> FindProductAreas(int currentPage, int pageSize, IDictionary<ProductAreaSearchCriteria, object> searchCriteria, IList<Sorter<ProductAreaSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM ProductAreaPo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                //TODO 查询条件
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ProductAreaSearchCriteria.AreaName:
                            hqlHelper.AddWhere("ChineseName", HqlOperator.Like, "AreaName", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                //TODO 排序条件
            }
            else
            {
                hqlHelper.AddSorter("ProductAreaId", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = ProductAreaDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<Service.Product.ProductArea.ProductArea>();
            var voList = pageDataPo.Data.Select(x => ProductAreaPoConvertToVo(x)).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        /// <summary>
        /// 专区Po 转换 Vo
        /// </summary>
        /// <param name="productAreaPo">专区Po对象</param>
        private Service.Product.ProductArea.ProductArea ProductAreaPoConvertToVo(ProductAreaPo productAreaPo, IEnumerable<ProductAreaDescPo> lstProductAreaDescPo = null)
        {
            if (productAreaPo.IsNullOrEmpty())
            {
                //throw new BussinessException(ERROR_PRODUCTAREA_NOT_EXIST);
                return null;
            }
            var lstProductAreaLanguage = new List<ProductAreaLanguage>();
            if (!lstProductAreaDescPo.IsNullOrEmpty())
            {
                lstProductAreaLanguage = lstProductAreaDescPo.Select(x => new ProductAreaLanguage
                {
                    AreaId = x.ProductAreaId,
                    LanguageId = x.LanguageId,
                    AreaName = x.Name,
                    Home = x.Home
                }).ToList();
            }
            var productAreaVo = new Service.Product.ProductArea.ProductArea
            {
                AreaId = productAreaPo.ProductAreaId,
                AreaName = productAreaPo.ChineseName,
                IsShowHome = productAreaPo.ShowIndex,
                IsValid = productAreaPo.Status,
                ProductAreaLanguages = lstProductAreaLanguage
            };
            return productAreaVo;
        }

        /// <summary>
        /// 获取专区多语言
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>专区多语言</returns>
        public ProductAreaLanguage GetProductAreaLanguage(int productAreaId, int languageId)
        {
            var productAreaDescPo = ProductAreaLanguageDao.GetAreaLanguage(productAreaId, languageId);
            if (productAreaDescPo.IsNullOrEmpty())
            {
                //throw new BussinessException(ERROR_PRODUCTAREA_LANGUAGENAME_NOT_EXIST);
                return null;
            }
            var productAreaDescVo = new Service.Product.ProductArea.ProductAreaLanguage
            {
                AreaId = productAreaDescPo.ProductAreaId,
                AreaName = productAreaDescPo.Name,
                LanguageId = productAreaDescPo.LanguageId
            };
            return productAreaDescVo;
        }

        public string GetProductAreaURL(int languageId, int productAreaId, int categoryId, string productRouteName)
        {
            var productArea = ProductAreaDao.GetObject(productAreaId);
            if (productArea.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_PRODUCTAREA_NOT_EXIST);
            }
            var language = LanguageDao.GetObject(languageId);
            string host = language.Host;
            string areaName = productArea.EnglishName;
            string url = string.Format("http://{0}/{1}/{2}/{3}-{4}-1.html", host, productRouteName, areaName, productAreaId, categoryId);
            return url;
        }



        /// <summary>
        /// 获取专区多语言列表
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <returns>专区多语言列表</returns>
        //public IList<ProductAreaLanguage> GetAllProductAreaLanguages(int productAreaId)
        //{
        //    var lstProductAreaDescPos = ProductAreaLanguageDao.GetAllProductAreaLanguages(productAreaId);
        //    if (lstProductAreaDescPos.IsNullOrEmpty())
        //    {
        //        //throw new BussinessException(ERROR_PRODUCTAREA_LANGUAGENAME_NOT_EXIST);
        //        return new List<ProductAreaLanguage>();
        //    }
        //    var lstProductAreaDescVos = lstProductAreaDescPos.Select<ProductAreaDescPo, ProductAreaLanguage>(productAreaDescPo => new ProductAreaLanguage
        //    {
        //        AreaId = productAreaDescPo.ProductAreaId,
        //        AreaName = productAreaDescPo.Name,
        //        LanguageId = productAreaDescPo.LanguageId
        //    });
        //    return lstProductAreaDescVos.ToList();
        //}
        #endregion

        #region 专区产品
        /// <summary>
        /// 设置专区产品
        /// </summary>
        /// <param name="list">专区产品实体</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCTAREA_NOT_EXIST:产品专区不存在</value>
        /// <value>ERROR_PRODUCTAREA_PRODUCT_IS_EXIST:该产品在产品专区已经存在</value>
        /// </exception>
        public void SetProductAreaRelativeList(List<ProductAreaRelative> list)
        {
            var lstProductAreaRelativePo = new List<ProductAreaRelativePo>();
            foreach (var productAreaRelative in list)
            {
                if (!ProductDao.ExistObject("ProductId", productAreaRelative.ProductId))
                    throw new BussinessException(ERROR_PRODUCT_NOT_EXIST);

                var productAreaPo = ProductAreaDao.GetProductAreaById(productAreaRelative.AreaId);
                //if (!ProductAreaDao.ExistObject("ProductAreaId", area.AreaId))
                if (productAreaPo.IsNullOrEmpty())
                    throw new BussinessException(ERROR_PRODUCTAREA_NOT_EXIST);

                var productAreaRelativePo = ProductAreaRelativeDao.GetProductAreaRelative(productAreaRelative.AreaId, productAreaRelative.ProductId);
                //判断产品专区该产品是否存在
                if (!productAreaRelativePo.IsNullOrEmpty())
                    throw new BussinessException(ERROR_PRODUCTAREA_PRODUCT_IS_EXIST);

                lstProductAreaRelativePo.Add(new ProductAreaRelativePo
                {
                    ProductAreaId = productAreaRelative.AreaId,
                    ProductId = productAreaRelative.ProductId
                });
            }
            ProductAreaRelativeDao.AddObjects(lstProductAreaRelativePo);
        }

        public void ClearProductAreaRelative(int productAreaId)
        {
            ProductAreaRelativeDao.DeleteProductAreaRelative(productAreaId);
        }

        #endregion




        /*

        /// <summary>
        /// 批量设置专区产品
        /// </summary>
        /// <param name="list">专产品</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCTAREA_NOT_EXIST:产品专区不存在</value>
        /// </exception>
        public void SetProductAreaRelativeList(List<ProductAreaRelative> list)
        {
            list.ForEach(SetProductAreaRelative);
        }

        /// <summary>
        /// 批量设置专区产品
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <param name="list">专区产品</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCTAREA_NOT_EXIST:产品专区不存在</value>
        /// </exception>
        public void SetProductAreaRelativeList(int productAreaId, List<ProductAreaRelative> list)
        {
            var lstProductAreaRelativePo = ProductAreaRelativeDao.GetAllProductAreaRelativesByAreaId(productAreaId);
            var lstRemoveProductAreaRelative =
                lstProductAreaRelativePo.Where(x => !list.Exists(y => y.ProductId == x.ProductId));
            lstRemoveProductAreaRelative.ForEach(x => DeleteProductAreaRelative(x.ProductAreaId, x.ProductId));
            list.ForEach(SetProductAreaRelative);
        }

        /// <summary>
        /// 删除专区产品（物理删除）
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <param name="productId">产品Id</param>
        public void DeleteProductAreaRelative(int productAreaId, int productId)
        {
            var productAreaRelativePo = ProductAreaRelativeDao.GetProductAreaRelative(productAreaId, productId);
            if (!productAreaRelativePo.IsNullOrEmpty())
                ProductAreaRelativeDao.DeleteObject(productAreaRelativePo);
        }

        /// <summary>
        /// 获取专区产品信息
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns>ProductAreaRelative</returns>
        public ProductAreaRelative GetProductAreaRelative(int productAreaId, int productId)
        {
            var productAreaRelativePo = ProductAreaRelativeDao.GetProductAreaRelative(productAreaId, productId);
            return ProductAreaRelativePoToVo(productAreaRelativePo);
        }

        /// <summary>
        /// 专区产品Po 对象 转换为Vo
        /// </summary>
        /// <param name="productAreaRelativePo">专区产品Po</param>
        private static ProductAreaRelative ProductAreaRelativePoToVo(ProductAreaRelativePo productAreaRelativePo)
        {
            if (productAreaRelativePo.IsNullOrEmpty())
                return null;
            var productAreaRelativeVo = new ProductAreaRelative
            {
                ProductId = productAreaRelativePo.ProductId,
                AreaId = productAreaRelativePo.ProductAreaId
            };
            return productAreaRelativeVo;
        }


        /// <summary>
        /// 查询专区产品
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        public Service.PageData<ProductAreaRelative> FindProductAreaRelatives(int currentPage, int pageSize, IDictionary<ProductAreaRelativeSearchCriteria, object> searchCriteria, IList<Service.Sorter<ProductAreaRelativeSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM ProductAreaRelativePo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                //TODO 查询条件
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ProductAreaRelativeSearchCriteria.ProductCode:
                            //hqlHelper.AddWhere("ProductModel", HqlOperator.Like, "ProductCode", item.Value);
                            break;
                        //case ProductDailyDealSearchCriteria.ProductName:
                        //    hqlHelper.AddWhere("ProductName", HqlOperator.Like, "ProductName", item.Value);
                        //    break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                //TODO 排序条件
            }
            //3.执行查询并返回数据
            var pageDataPo = ProductAreaRelativeDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ProductAreaRelative>();
            var voList = pageDataPo.Data.Select(ProductAreaRelativePoToVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }
        #endregion
        */
    }

    public enum ProductAreaRelativeSearchCriteria
    {
        ProductCode
    }

    public enum ProductAreaRelativeSorterCriteria
    {

    }
}
