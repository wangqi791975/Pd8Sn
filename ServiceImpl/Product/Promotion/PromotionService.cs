using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Promotion;
using Com.Panduo.ServiceImpl.Product.Dao;
using Com.Panduo.ServiceImpl.Product.Promotion.Dao;
using NHibernate.Linq;

namespace Com.Panduo.ServiceImpl.Product.Promotion
{
    public class PromotionService : IPromotionService
    {
        public IPromotionDao PromotionDao { private get; set; }
        public IPromotionDescDao PromotionDescDao { private get; set; }
        public IPromotionProductDao PromotionProductDao { private get; set; }
        public IProductDao ProductDao { private get; set; }

        #region 业务异常
        /// <summary>
        /// 产品不存在
        /// </summary>
        public string ERROR_PRODUCT_NOT_EXIST
        {
            get { return "ERROR_PRODUCT_NOT_EXIST"; }
        }
        /// <summary>
        /// 促销专区已经存在
        /// </summary>
        public string ERROR_PROMOTIONAREA_NAME_EXIST
        {
            get { return "ERROR_PROMOTIONAREA_NAME_EXIST"; }
        }
        /// <summary>
        /// 促销区不存在
        /// </summary>
        public string ERROR_PROMOTIONAREA_NOT_EXIST
        {
            get { return "ERROR_PROMOTIONAREA_NOT_EXIST"; }
        }
        /// <summary>
        /// 促销区中该产品已经存在
        /// </summary>
        public string ERROR_PROMOTIONAREA_PRODUCT_EXIST
        {
            get { return "ERROR_PROMOTIONAREA_PRODUCT_EXIST"; }
        }
        /// <summary>
        /// 判断商品折扣在促销区内是否存在时间重叠
        /// </summary>
        public string ERROR_PROMOTIONAREA_PRODUCT_DISCOUNT_EXIST
        {
            get { return "ERROR_PROMOTIONAREA_PRODUCT_DISCOUNT_EXIST"; }
        }
        /// <summary>
        /// 促销区中产品不存在
        /// </summary>
        public string ERROR_PROMOTIONAREA_PRODUCT_NOT_EXIST
        {
            get { return "ERROR_PROMOTIONAREA_PRODUCT_NOT_EXIST"; }
        }
        /// <summary>
        /// 该促销区已经过期
        /// </summary>
        public string ERROR_PROMOTIONAREA_EXPIRED
        {
            get { return "ERROR_PROMOTIONAREA_EXPIRED"; }
        }
        #endregion


        #region 促销区
        /// <summary>
        /// 修改促销区
        /// 如果促销区下存在产品，不允许修改
        /// </summary>
        /// <param name="area">促销区实体</param>
        public int SetPromotionArea(PromotionArea area)
        {
            #region 保存促销区
            var promotionPo = PromotionDao.GetPromotionAreaById(area.PromotionAreaId);
            //if (!PromotionDao.ExistObject("PromotionId", area.PromotionAreaId))
            if (promotionPo.IsNullOrEmpty())
            {
                promotionPo = new PromotionPo
                {
                    Name = area.PromotionName,
                    DateStarted = area.SaleStartTime,
                    DateEnded = area.SaleEndTime,
                    Status = area.IsValid
                };
                promotionPo.PromotionId = PromotionDao.AddObject(promotionPo);
            }
            else
            {
                promotionPo.Name = area.PromotionName;
                promotionPo.Status = area.IsValid;
                promotionPo.DateStarted = area.SaleStartTime;
                promotionPo.DateEnded = area.SaleEndTime;
                promotionPo.IsShowHome = area.IsShowHome;
                PromotionDao.UpdateObject(promotionPo);

                #region 重置该促销区所有产品有效期
                var lstPromotionProduct = PromotionProductDao.GetAllPromotionProductsByPromotionId(area.PromotionAreaId);
                if (!lstPromotionProduct.IsNullOrEmpty())
                {
                    foreach (var item in lstPromotionProduct)
                    {
                        item.Status = area.IsValid;
                        item.DateStarted = area.SaleStartTime;
                        item.DateEnded = area.SaleEndTime;
                        PromotionProductDao.UpdateObject(item);
                    }
                }
                #endregion
            }
            #endregion

            #region 保存促销区多语种
            foreach (var promotionDesc in area.PromotionDescs)
            {
                var promotionDescPo = PromotionDescDao.GetOneObject("from PromotionDescPo where PromotionId=? and LanguageId=?", new object[] { promotionPo.PromotionId, promotionDesc.LanguageId });
                if (promotionDescPo.IsNullOrEmpty())
                {
                    promotionDescPo = new PromotionDescPo
                    {
                        PromotionId = promotionPo.PromotionId,
                        Home = promotionDesc.PromotionHome,
                        LanguageId = promotionDesc.LanguageId,
                        Name = promotionDesc.PromotionName
                    };
                    PromotionDescDao.AddObject(promotionDescPo);
                }
                else
                {
                    promotionDescPo.Home = promotionDesc.PromotionHome;
                    promotionDescPo.Name = promotionDesc.PromotionName;
                    PromotionDescDao.UpdateObject(promotionDescPo);
                }
            }
            #endregion

            return promotionPo.PromotionId;
        }

        /// <summary>
        /// 设置促销区状态
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        /// <param name="isValid">状态</param>
        public void SetPromotionAreaStatus(int promotionAreaId, bool isValid)
        {
            var promotionPo = PromotionDao.GetPromotionAreaById(promotionAreaId);
            //if (!PromotionDao.ExistObject("PromotionId", area.PromotionAreaId))
            if (promotionPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_NOT_EXIST);

            promotionPo.Status = isValid;
            PromotionDao.UpdateObject(promotionPo);
        }

        public void DeletePromotionArea(int promotionAreaId)
        {
            var promotionPo = PromotionDao.GetPromotionAreaById(promotionAreaId);
            //if (!PromotionDao.ExistObject("PromotionId", area.PromotionAreaId))
            if (promotionPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_NOT_EXIST);

            PromotionDescDao.DeleteObjectByHql("delete from PromotionProductPo where PromotionId=?", new object[] { promotionAreaId });
            PromotionDescDao.DeleteObjectByHql("delete from PromotionDescPo where PromotionId=?", new object[] { promotionAreaId });
            PromotionDao.DeleteObjectByHql("delete from PromotionPo where PromotionId=?", new object[] { promotionAreaId });
        }

        /// <summary>
        /// 获取促销区
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        /// <returns>促销区</returns>
        public PromotionArea GetPromotionAreaById(int promotionAreaId)
        {
            var promotionPo = PromotionDao.GetPromotionAreaById(promotionAreaId);
            var promotionDescs = PromotionDescDao.FindDataByHql("from PromotionDescPo where PromotionId=?",
                new object[] { promotionAreaId });
            return PromotionAreaPoConvertToVo(promotionPo, promotionDescs);
        }

        /// <summary>
        /// 后台管理列表用 查询促销区
        /// </summary>
        public PageData<PromotionArea> FindPromotionAreas(int currentPage, int pageSize, IDictionary<PromotionAreaSearchCriteria, object> searchCriteria,
            IList<Sorter<PromotionAreaSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM PromotionPo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                //TODO 查询条件
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case PromotionAreaSearchCriteria.PromotionName:
                            hqlHelper.AddWhere("PromotionName", HqlOperator.Like, "PromotionName", item.Value);
                            break;
                        case PromotionAreaSearchCriteria.SaleStartTime:
                            hqlHelper.AddWhere("DateStarted", HqlOperator.Like, "DateStarted", item.Value);
                            break;
                        case PromotionAreaSearchCriteria.SaleEndTime:
                            hqlHelper.AddWhere("DateEnded", HqlOperator.Like, "DateEnded", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                //TODO 排序条件
            }
            //3.执行查询并返回数据
            var pageDataPo = PromotionDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<PromotionArea>();
            var voList = pageDataPo.Data.Select(promotionPo => PromotionAreaPoConvertToVo(promotionPo)).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        /// <summary>
        /// 获取该促销区下所有已经存在商品折扣
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PROMOTIONAREA_NOT_EXIST:促销区不存在</value>
        /// </exception>
        public List<decimal> GetPromotionAllDiscount(int promotionAreaId)
        {
            var promotionPo = PromotionDao.GetPromotionAreaById(promotionAreaId);
            if (promotionPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_NOT_EXIST);
            var lstDiscounts = new List<decimal>();

            var lst = PromotionProductDao.FindObjectByHql("select Discount from PromotionProductPo where PromotionId=? group by Discount", promotionAreaId);
            if (!lst.IsNullOrEmpty())
                lstDiscounts = lst.Select(c => c.ParseTo(0.00M)).ToList();

            return lstDiscounts;
        }


        public string ProduceSaleUrl(int promotionAreaId, int languageId, int categoryId, decimal discount)
        {
            var url = string.Empty;

            return url;
        }

        /// <summary>
        /// PromotionArea Po 转换为 Vo
        /// </summary>
        /// <param name="promotionPo">Po</param>
        /// <param name="promotionDescs"></param>
        /// <returns>Vo</returns>
        private PromotionArea PromotionAreaPoConvertToVo(PromotionPo promotionPo, IEnumerable<PromotionDescPo> promotionDescs = null)
        {
            if (promotionPo.IsNullOrEmpty())
                return null;

            if (promotionDescs != null)
                return new PromotionArea
                {
                    PromotionAreaId = promotionPo.PromotionId,
                    PromotionName = promotionPo.Name,
                    SaleStartTime = promotionPo.DateStarted,
                    SaleEndTime = promotionPo.DateEnded,
                    IsValid = promotionPo.Status,
                    HasProduct = promotionPo.HasProduct,
                    IsShowHome = promotionPo.IsShowHome,
                    PromotionDescs = promotionDescs.Select(x => new PromotionDesc
                    {
                        Id = x.Id,
                        LanguageId = x.LanguageId,
                        PromotionAreaId = x.PromotionId,
                        PromotionHome = x.Home,
                        PromotionName = x.Name
                    }).ToList()
                };
            else
                return new PromotionArea
                {
                    PromotionAreaId = promotionPo.PromotionId,
                    PromotionName = promotionPo.Name,
                    SaleStartTime = promotionPo.DateStarted,
                    SaleEndTime = promotionPo.DateEnded,
                    IsValid = promotionPo.Status,
                    HasProduct = promotionPo.HasProduct,
                    IsShowHome = promotionPo.IsShowHome,
                    PromotionDescs = new List<PromotionDesc>()
                };
        }

        #endregion

        #region 促销产品

        /// <summary>
        /// 根据导入的Excel数据生成ProductPromotion对象
        /// </summary>
        /// <param name="promotionAreaId">促销区Id</param>
        /// <param name="productNo">产品货号 B00001</param>
        /// <param name="discount">折扣</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品数据不存在</value>
        /// <value>ERROR_PROMOTIONAREA_NOT_EXIST:促销区不存在</value>
        /// <value>ERROR_PROMOTIONAREA_PRODUCT_EXIST:促销区中该产品已经存在</value>
        /// </exception>
        /// <returns></returns>
        public ProductPromotion CreatedProductPromotionByImportData(int promotionAreaId, string productNo, decimal discount)
        {
            var promotionPo = PromotionDao.GetPromotionAreaById(promotionAreaId);
            if (promotionPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_NOT_EXIST);

            //该促销区已经过期
            if (promotionPo.DateEnded <= DateTime.Now)
                throw new BussinessException(ERROR_PROMOTIONAREA_EXPIRED);

            var product = ProductDao.GetProductByCode(productNo);
            if (product.IsNullOrEmpty())
                throw new BussinessException(ERROR_PRODUCT_NOT_EXIST);

            var dailydealProductPo = PromotionProductDao.GetPromotionProduct(promotionAreaId, product.ProductId);
            //判断促销区该产品是否已经存在
            if (!dailydealProductPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_PRODUCT_EXIST);

            //判断商品折扣在促销区内是否存在时间重叠
            if (PromotionProductDao.CheckProductForPromotionIsExists(product.ProductId, promotionPo.DateStarted, promotionPo.DateEnded))
                throw new BussinessException(ERROR_PROMOTIONAREA_PRODUCT_DISCOUNT_EXIST);



            var productPromotion = new ProductPromotion
            {
                PromotionAreaId = promotionPo.PromotionId,
                ProductId = product.ProductId,
                Discount = discount,
                SaleStartTime = promotionPo.DateStarted,
                SaleEndTime = promotionPo.DateEnded,
                IsDisplay = true
            };

            return productPromotion;
        }

        /// <summary>
        /// 批量设置促销产品
        /// </summary>
        /// <param name="list">促销产品集合</param>
        /// <param name="promotionAreaId">促销区Id</param>
        public void SetPromotionProductList(List<ProductPromotion> list, int promotionAreaId = -1)
        {
            if (!list.IsNullOrEmpty())
            {
                var firstOrDefault = list.FirstOrDefault();
                if (promotionAreaId < 0 && !firstOrDefault.IsNullOrEmpty())
                {
                    // ReSharper disable once PossibleNullReferenceException
                    promotionAreaId = firstOrDefault.PromotionAreaId;
                }

                var promotionPo = PromotionDao.GetPromotionAreaById(promotionAreaId);
                if (promotionPo.IsNullOrEmpty())
                    throw new BussinessException(ERROR_PROMOTIONAREA_NOT_EXIST);

                var lstAddObjects = new List<PromotionProductPo>();
                foreach (var productPromotion in list)
                {
                    var promotionProductPo = PromotionProductDao.GetPromotionProduct(productPromotion.PromotionAreaId, productPromotion.ProductId);

                    //判断促销区该产品是否存在
                    if (!promotionProductPo.IsNullOrEmpty())
                        throw new BussinessException(ERROR_PROMOTIONAREA_PRODUCT_EXIST);

                    lstAddObjects.Add(PromotionProductVoToPo(productPromotion));
                }
                if (lstAddObjects.IsNullOrEmpty()) return;
                PromotionProductDao.AddObjects(lstAddObjects);
                promotionPo.HasProduct = true;
                PromotionDao.UpdateObject(promotionPo);
            }
        }

        /// <summary>
        /// 清空促销产品（物理删除）
        /// </summary>
        /// <param name="promotionId">促销区Id</param>
        public void ClearProductPromotion(int promotionId)
        {
            var promotionPo = PromotionDao.GetPromotionAreaById(promotionId);
            if (promotionPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_NOT_EXIST);

            PromotionProductDao.DeleteObjectByHql("delete from PromotionProductPo where PromotionId=?", new object[] { promotionId });

            promotionPo.HasProduct = false;
            PromotionDao.UpdateObject(promotionPo);
        }

        /// <summary>
        /// 根据产品Id获取单个产品促销信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        public ProductPromotion GetProductPromotionByProductId(int productId)
        {
            var promotionProductPo = PromotionProductDao.GetProductPromotionByProductId(productId);
            return PromotionProductPoConvertToVo(promotionProductPo);
        }

        /// <summary>
        /// 根据产品Id列表批量获取产品促销信息
        /// </summary>
        /// <param name="productIds">产品Id列表</param>
        /// <returns></returns>
        public IList<ProductPromotion> GetProductPromotionByProductIds(IList<int> productIds)
        {
            var vos = new List<ProductPromotion>();
            var productIdStr = productIds.Join(",");
            var pos = PromotionProductDao.GetProductPromotionByProductIds(productIdStr);
            if (pos.IsNullOrEmpty()) return vos;
            vos.AddRange(pos.Select(PromotionProductPoConvertToVo).Where(vo => vo != null));
            return vos;
        }

        /// <summary>
        /// 获取该促销区内单个产品
        /// </summary>
        /// <param name="promotionId">促销区Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        public ProductPromotion GetProductPromotion(int promotionId, int productId)
        {
            var promotionProductPo = PromotionProductDao.GetPromotionProduct(promotionId, productId);
            return PromotionProductPoConvertToVo(promotionProductPo);
        }

        public IList<int> GetPromotionDiscount()
        {
            var promotionDiscounts = new List<int>();
            const string cmdSql = " SELECT discount FROM t_promotion_product WITH(NOLOCK) GROUP BY discount";
            using (var conn = new SqlConnection(SqlHelper.CONN_STRING))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    using (var reader = SqlHelper.ExecuteReader(transaction, CommandType.Text, cmdSql, null))
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                promotionDiscounts.Add((((reader["discount"]).ParseTo<decimal>()) * 100).ParseTo<int>());
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            var number = ex.Number;
                            if (number == 60000)
                            {
                                throw new BussinessException(ex.Message);
                            }
                        }
                        finally
                        {
                            reader.Close();
                            conn.Close();
                            transaction.Dispose();
                            conn.Dispose();
                        }
                    }
                }
            }
            return promotionDiscounts;
        }

        /// <summary>
        /// 判断产品是否促销产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        public bool IsPromotionProduct(int productId)
        {
            return PromotionProductDao.IsPromotionProduct(productId);
        }

        /// <summary>
        /// 判断产品是否促销产品
        /// </summary>
        /// <param name="productPromotion"></param>
        /// <param name="stockQty"></param>
        /// <returns></returns>
        public bool IsPromotionProduct(ProductPromotion productPromotion, int? stockQty)
        {
            var isPromotion = false;

            //1.促销总开关需要打开
            if (ServiceFactory.ConfigureService.IsPromotion && productPromotion != null)
            {
                //2.促销商品状态必须是有效的
                if (productPromotion.IsDisplay)
                {
                    //3.当前时间需要在促销区间内
                    if (DateTime.Now >= productPromotion.SaleStartTime && DateTime.Now <= productPromotion.SaleEndTime)
                    {
                        //4.对于绑定了库存的还需要判断是否有库存
                        //if (stockQty.HasValue)
                        //{
                        //    //绑定库存的再判断下库存是否还充足
                        //    isPromotion = stockQty.Value > 0;
                        //}
                        //else
                        //{
                        //不需要判断库存的到这里就认定是促销商品了
                        isPromotion = true;
                        //}
                    }
                }
            }

            return isPromotion;
        }

        /// <summary>
        /// 促销产品Po 转换为 Vo
        /// </summary>
        /// <param name="promotionProductPo">Po</param>
        /// <returns>Vo</returns>
        private ProductPromotion PromotionProductPoConvertToVo(PromotionProductPo promotionProductPo)
        {
            if (promotionProductPo.IsNullOrEmpty())
                return null;
            return new ProductPromotion
            {
                PromotionAreaId = promotionProductPo.PromotionId,
                ProductId = promotionProductPo.ProductId,
                Discount = promotionProductPo.Discount,
                SaleStartTime = promotionProductPo.DateStarted,
                SaleEndTime = promotionProductPo.DateEnded,
                IsDisplay = promotionProductPo.Status
            };
        }

        /// <summary>
        /// 促销产品Vo 转换为 Po
        /// </summary>
        /// <param name="productPromotion">Vo</param>
        /// <returns>Po</returns>
        private PromotionProductPo PromotionProductVoToPo(ProductPromotion productPromotion)
        {
            if (productPromotion.IsNullOrEmpty())
                return null;

            return new PromotionProductPo
            {
                PromotionId = productPromotion.PromotionAreaId,
                ProductId = productPromotion.ProductId,
                Discount = productPromotion.Discount,
                DateStarted = productPromotion.SaleStartTime,
                DateEnded = productPromotion.SaleEndTime,
                Status = productPromotion.IsDisplay
            };
        }
        #endregion

        /*
        #region 促销产品

        /// <summary>
        /// 设置促销产品
        /// </summary>
        /// <param name="productPromotion">促销产品实体</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PROMOTIONAREA_NOT_EXIST:促销专区不存在</value>
        /// </exception>
        public void SetProductPromotion(ProductPromotion productPromotion)
        {
            var promotionPo = PromotionDao.GetPromotionAreaById(productPromotion.PromotionAreaId);
            //if (!PromotionDao.ExistObject("PromotionId", area.PromotionAreaId))
            if (promotionPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_NOT_EXIST);

            var promotionProductPo = PromotionProductDao.GetPromotionProduct(productPromotion.PromotionAreaId, productPromotion.ProductId);

            //判断促销区该产品是否存在
            if (!promotionProductPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_PRODUCT_EXIST);

            promotionProductPo = PromotionProductVoToPo(productPromotion);
            PromotionProductDao.AddObject(promotionProductPo);
        }
        
        /// <summary>
        /// 设置产品促销状态
        /// </summary>
        /// <param name="promotionId">促销区Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="isEnable">是否促销</param>
        public void SetProductPromotionStatus(int promotionId, int productId, bool isEnable)
        {
            var promotionProductPo = PromotionProductDao.GetPromotionProduct(promotionId, productId);
            //判断促销区该产品是否存在
            if (promotionProductPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_PRODUCT_NOT_EXIST);
            else
            {
                promotionProductPo.Status = isEnable;
                PromotionProductDao.UpdateObject(promotionProductPo);
            }
        }

        /// <summary>
        /// 删除促销产品（物理删除）
        /// </summary>
        /// <param name="promotionId">促销区Id</param>
        /// <param name="productId">产品Id</param>
        public void DeleteProductPromotion(int promotionId, int productId)
        {
            var promotionProductPo = PromotionProductDao.GetPromotionProduct(promotionId, productId);

            //判断促销区该产品是否存在
            if (promotionProductPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_PROMOTIONAREA_PRODUCT_NOT_EXIST);
            else
                PromotionProductDao.DeleteObject(promotionProductPo);
        }

        public IList<ProductPromotion> GetAllProductsByPromotionId(int promotionId)
        {
            var lstPromotionProductPo = PromotionProductDao.GetAllPromotionProductsByPromotionId(promotionId);
            if (lstPromotionProductPo.IsNullOrEmpty())
                return new List<ProductPromotion>();
            return lstPromotionProductPo.Select(PromotionProductPoConvertToVo).ToList();
        }

        /// <summary>
        /// 查询促销产品
        /// </summary>
        public PageData<ProductPromotion> FindProductPromotions(int currentPage, int pageSize,
            IDictionary<ProductPromotionSearchCriteria, object> searchCriteria,
            IList<Sorter<ProductPromotionSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM PromotionProductPo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                //TODO 查询条件
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        //case ProductPromotionSearchCriteria.ProductCode:
                        //    //hqlHelper.AddWhere("ProductModel", HqlOperator.Like, "ProductCode", item.Value);
                        //    break;
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
            var pageDataPo = PromotionProductDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ProductPromotion>();
            var voList = pageDataPo.Data.Select(PromotionProductPoConvertToVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        #endregion

        */
    }

    public enum ProductPromotionSearchCriteria
    {
    }

    public enum ProductPromotionSorterCriteria
    {

    }
}
