using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product.Dailydeal;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.DailyDeal;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl.Product.Dailydeal.Dao;
using Com.Panduo.ServiceImpl.Product.Dao;
using NHibernate.Linq;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.ServiceImpl.Product.Dailydeal
{
    public class ProductDailyDealService : IProductDailyDealService
    {
        #region 注入
        public IDailydealProductDao DailydealProductDao { private get; set; }
        public IVdailydealProductDao VdailydealProductDao { private get; set; }
        public IProductDao ProductDao { private get; set; }
        public IDailydealTitleDao DailydealTitleDao { private get; set; }
        public IDailydealDescDao DailydealDescDao { private get; set; }
        public IDailydealLabelDao DailydealLabelDao { private get; set; }

        public IConfigureService ConfigureService { private get; set; }
        #endregion

        #region 业务异常
        /// <summary>
        /// 产品不存在
        /// </summary>
        public string ERROR_PRODUCT_NOT_EXIST
        {
            get { return "ERROR_PRODUCT_NOT_EXIST"; }
        }
        /// <summary>
        /// 一口价产品不存在
        /// </summary>
        public string ERROR_DAILYDEAL_PRODUCT_NOT_EXIST
        {
            get { return "ERROR_DAILYDEAL_PRODUCT_NOT_EXIST"; }
        }
        /// <summary>
        /// 一口价产品已经存在
        /// </summary>
        public string ERROR_DAILYDEAL_PRODUCT_IS_EXIST
        {
            get { return "ERROR_DAILYDEAL_PRODUCT_IS_EXIST"; }
        }
        /// <summary>
        /// DailyDeal标语库为空
        /// </summary>
        public string ERROR_DAILYDEAL_TITLE_IS_NULL
        {
            get { return "ERROR_DAILYDEAL_TITLE_IS_NULL"; }
        }
        #endregion


        /// <summary>
        /// Dailydeal 产品Po 转换Vo
        /// </summary>
        /// <param name="dailydealProductPo">Po</param>
        private ProductDailyDeal DailydealProductPoConvertToVo(DailydealProductPo dailydealProductPo)
        {
            //判断产品是否存在
            if (dailydealProductPo.IsNullOrEmpty())
                return null;

            return new ProductDailyDeal
            {
                ProductId = dailydealProductPo.ProductId,
                Price = dailydealProductPo.Price,
                ProductImage = dailydealProductPo.Image,
                StartDateTime = dailydealProductPo.DateStarted,
                EndDateTime = dailydealProductPo.DateEnded,
                SaledQuantity = dailydealProductPo.SoldQuantity.HasValue ? dailydealProductPo.SoldQuantity.Value : 0,
                IsValid = dailydealProductPo.Status,
                DateUpdate = dailydealProductPo.DateUpdate,
                TitleId = dailydealProductPo.TitleId
            };
        }

        /// <summary>
        /// 获取产品DailyDea价格
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>产品一口价</returns>
        public decimal GetProductDailyDealPrice(int productId)
        {
            var dailydealProductPo = DailydealProductDao.GetDailydealProduct(productId);
            //判断产品是否存在
            if (dailydealProductPo.IsNullOrEmpty())
                return 0;

            return dailydealProductPo.Price;
        }

        /// <summary>
        /// 视图 Dailydeal ViewPo 转换Vo
        /// </summary>
        /// <param name="dailydealProductPo"></param>
        /// <returns></returns>
        private ProductDailyDeal VDailydealProductPoConvertToVo(VdailydealProductPo dailydealProductPo)
        {
            //判断产品是否存在
            if (dailydealProductPo.IsNullOrEmpty())
                return null;

            return new ProductDailyDeal
            {
                ProductId = dailydealProductPo.ProductId,
                Price = dailydealProductPo.Price,
                ProductImage = dailydealProductPo.Image,
                StartDateTime = dailydealProductPo.DateStarted,
                EndDateTime = dailydealProductPo.DateEnded,
                SaledQuantity = dailydealProductPo.SoldQuantity.HasValue ? dailydealProductPo.SoldQuantity.Value : 0,
                IsValid = dailydealProductPo.Status,
                DateUpdate = dailydealProductPo.DateUpdate,
                TitleId = dailydealProductPo.TitleId,
                Title = dailydealProductPo.Title,
                ProductCode = dailydealProductPo.ProductModel,
                ProductName = dailydealProductPo.ProductName,
                ProductEnName = dailydealProductPo.ProductEnName,

                LanguageId = dailydealProductPo.LanguageId,
                DailyProductPrice = dailydealProductPo.DailyProductPrice,
                Discount = dailydealProductPo.Discount,
                SaveMoney = dailydealProductPo.DailyProductPrice - dailydealProductPo.Price
            };
        }

        /// <summary>
        /// 设置Dailydeal 有效期
        /// 保存到Config
        /// </summary>
        /// <param name="startTime">有效期开始</param>
        /// <param name="endTime">有效期结束</param>
        public void SetDailydealValidityPeriod(DateTime startTime, DateTime endTime)
        {
            ConfigureService.DailydealsDateBegin = startTime;
            ConfigureService.DailydealsDateBegin = endTime;

        }

        /// <summary>
        /// 获取该语种所有标语库
        /// </summary>
        /// <param name="languageId">语种Id</param>
        public List<DailyDealTitle> GetAllTitles(int languageId)
        {
            var lstDailyDealTitleVo = ImplCacheHelper.GetDailyDealTitlesByLanguageId(languageId);
            if (lstDailyDealTitleVo.IsNullOrEmpty())
            {
                var lstDailyDealTitlePo = DailydealTitleDao.GetAllTitles(languageId);
                lstDailyDealTitleVo = lstDailyDealTitlePo.Select(x => new DailyDealTitle
                {
                    Id = x.Id.TitleId,
                    LanguageId = x.Id.LanguageId,
                    Name = x.Name
                }).ToList();

                ImplCacheHelper.SetDailyDealTitles(languageId, lstDailyDealTitleVo);
            }
            return lstDailyDealTitleVo;
        }

        /// <summary>
        /// 根据导入的Excel数据生成ProductDailyDeal对象
        /// <para>注意，该方法会做：</para>
        /// <para>1.生成随机数给SaledQuantity </para>
        /// <para>2.随机从标语库里匹配一个标语给Title</para>
        /// </summary>
        /// <param name="productNo">产品货号 B00001</param>
        /// <param name="price">产品一口价价格</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="languageId">语种Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_PRODUCT_NOT_EXIST:产品数据不存在</value>
        /// <value>ERROR_DAILYDEAL_PRODUCT_IS_EXIST:该产品已经设置为Dailydeal</value>
        /// <value>ERROR_DAILYDEAL_TITLE_IS_NULL:Dailydeal标语库为空</value>
        /// </exception>
        /// <returns></returns>
        public ProductDailyDeal CreatedProductDailyDealByImportData(string productNo, decimal price, DateTime startDateTime, int languageId = -1)
        {
            var product = ProductDao.GetProductByCode(productNo);
            if (product.IsNullOrEmpty())
                throw new BussinessException(ERROR_PRODUCT_NOT_EXIST);

            var dailydealProductPo = DailydealProductDao.GetDailydealProduct(product.ProductId);
            //判断促销区该产品是否已经存在
            if (!dailydealProductPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_DAILYDEAL_PRODUCT_IS_EXIST);
            if (languageId < 0)
                languageId = ConfigureService.SiteLanguageId;

            var ran = new Random();//生成随机数对象
            var lstDailyDealTitleVo = GetAllTitles(languageId);
            if (lstDailyDealTitleVo.IsNullOrEmpty())
                throw new BussinessException(ERROR_DAILYDEAL_TITLE_IS_NULL);

            var title = lstDailyDealTitleVo[ran.Next(0, lstDailyDealTitleVo.Count - 1)];

            var productDailyDeal = new ProductDailyDeal
            {
                ProductId = product.ProductId,
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(24),
                Price = price,
                TitleId = title.Id,
                Title = title.Name,
                ProductImage = string.Format("{0}A.jpg", productNo),
                SaledQuantity = ran.Next(500, 1000)//需求：从500-1000中随机获取一个数字作为商品的SaledQuantity初始数据
            };

            return productDailyDeal;
        }

        /// <summary>
        /// 导入批量设置DailyDeal产品
        /// <para>注意：标语Id（TitleId）传进来已经是有值、已售数量（SoldQuantity）有值传进来
        /// 调用方法：CreatedProductDailyDealByImportData
        /// </para>
        /// </summary>
        /// <param name="list">ProductDailyDeal List</param>
        /// <param name="dailyDealLabels"></param>
        public void SetDailyDealList(List<ProductDailyDeal> list, List<DailyDealLabel> dailyDealLabels)
        {
            var lstDailydealProductPo = list.Select(productDailyDeal => new DailydealProductPo
            {
                ProductId = productDailyDeal.ProductId,
                DateStarted = productDailyDeal.StartDateTime,
                DateEnded = productDailyDeal.EndDateTime,
                Status = productDailyDeal.IsValid,
                Price = productDailyDeal.Price,
                Image = productDailyDeal.ProductImage,
                TitleId = productDailyDeal.TitleId,
                SoldQuantity = productDailyDeal.SaledQuantity
            });
            var dailyDealIds = DailydealProductDao.AddObjects(lstDailydealProductPo);
            foreach (int dailyDealId in dailyDealIds)
            {
                var newDailyDealLabels = new List<DailyDealLabel>();
                foreach (var dailyDealLabel in dailyDealLabels)
                {
                    dailyDealLabel.DailyDealId = dailyDealId;
                    newDailyDealLabels.Add(dailyDealLabel);
                }
                var dailyDealLabelPos = newDailyDealLabels.Select(GetDailyDealLabelPoFromVo);
                DailydealLabelDao.AddObjects(dailyDealLabelPos);
            }
        }

        /// <summary>
        /// 设置产品DailyDeal状态
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="isEnable">是否DailyDeal</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_DAILYDEAL_PRODUCT_NOT_EXIST:一口价产品不存在</value>
        /// </exception>
        public void SetProductDailyDealStatus(int productId, bool isEnable)
        {
            var dailydealProductPo = DailydealProductDao.GetDailydealProduct(productId);
            //判断产品是否存在
            if (dailydealProductPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_DAILYDEAL_PRODUCT_NOT_EXIST);
            else
            {
                dailydealProductPo.Status = isEnable;
                DailydealProductDao.UpdateObject(dailydealProductPo);
            }
        }

        /// <summary>
        /// 删除DailyDeal产品（物理删除）
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <exception cref="BussinessException">
        /// <value>ERROR_DAILYDEAL_PRODUCT_NOT_EXIST:一口价产品不存在</value>
        /// </exception>
        public void DeleteProductDailyDeal(int productId)
        {
            var dailydealProductPo = DailydealProductDao.GetDailydealProduct(productId);
            //判断产品是否存在
            if (dailydealProductPo.IsNullOrEmpty())
                throw new BussinessException(ERROR_DAILYDEAL_PRODUCT_NOT_EXIST);
            else DailydealProductDao.DeleteObject(dailydealProductPo);
        }


        /// <summary>
        /// 获取产品DailyDeal信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>产品DailyDeal信息</returns>
        public ProductDailyDeal GetProductDailyDeal(int productId)
        {
            var dailydealProductPo = DailydealProductDao.GetDailydealProduct(productId);
            return DailydealProductPoConvertToVo(dailydealProductPo);
        }

        /// <summary>
        /// 批量获取一批产品DailyDeal信息
        /// </summary>
        /// <param name="productIds">产品Id 集合</param>
        public List<ProductDailyDeal> GetProductDailyDeals(IList<int> productIds)
        {
            var lstProductDailyDeal = new List<ProductDailyDeal>();
            var productIdStr = productIds.Join(",");
            var dailydealProductPos = DailydealProductDao.GetDailydealProductsByProductIds(productIdStr);
            if (!dailydealProductPos.IsNullOrEmpty())
            {
                lstProductDailyDeal = dailydealProductPos.Select(DailydealProductPoConvertToVo).ToList();
            }
            return lstProductDailyDeal;
        }

        /// <summary>
        /// 判断产品是否DailyDeal产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>是否DailyDeal产品</returns>
        public bool IsProductDailyDeal(int productId)
        {
            return DailydealProductDao.IsProductDailyDeal(productId);
        }

        /// <summary>
        /// 获取有效的一口价商品，前台显示
        /// </summary>
        /// <param name="languageId">语种ID</param>
        public List<ProductDailyDeal> GetValidDailyDealProducts(int languageId)
        {
            var lstProductDailyDeal = new List<ProductDailyDeal>();

            var dailydealProductPos = VdailydealProductDao.GetValidDailyDealProducts(languageId);
            if (!dailydealProductPos.IsNullOrEmpty())
            {
                lstProductDailyDeal = dailydealProductPos.Select(VDailydealProductPoConvertToVo).ToList();
            }
            return lstProductDailyDeal;
        }

        /// <summary>
        /// 分页查询DailyDeal产品 后台管理列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的DailyDeal产品列表</returns>
        public PageData<ProductDailyDeal> FindProductDailyDeals(int currentPage, int pageSize, IDictionary<ProductDailyDealSearchCriteria, object> searchCriteria, IList<Sorter<ProductDailyDealSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM VdailydealProductPo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                //TODO 查询条件
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ProductDailyDealSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("language_id", HqlOperator.Like, "LanguageId", item.Value);
                            break;
                        case ProductDailyDealSearchCriteria.ProductCode:
                            hqlHelper.AddWhere("ProductModel", HqlOperator.Like, "ProductCode", item.Value);
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
            else
            {
                hqlHelper.AddSorter("DateEnded", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = VdailydealProductDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<ProductDailyDeal>();
            var voList = pageDataPo.Data.Select(VDailydealProductPoConvertToVo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        #region DailyDeal 配置


        public void SetDailydealDesc(DailyDealDesc dailyDealDesc)
        {
            var dailydealDescPo = DailydealDescDao.GetOneObject("from DailydealDescPo where LanguageId=?", new object[] { dailyDealDesc.LanguageId });
            //判断产品是否存在
            if (dailydealDescPo.IsNullOrEmpty())
            {
                DailydealDescDao.AddObject(new DailydealDescPo
                {
                    LanguageId = dailyDealDesc.LanguageId,
                    HeaderImg = dailyDealDesc.HeaderImg,
                    MiddleAreaHtml = dailyDealDesc.MiddleAreaHtml,
                    RecommendAreaHtml = dailyDealDesc.RecommendAreaHtml,
                    IsValid = dailyDealDesc.IsValid
                });
            }
            else
            {
                dailydealDescPo.HeaderImg = dailyDealDesc.HeaderImg.IsNullOrEmpty() ? dailydealDescPo.HeaderImg : dailyDealDesc.HeaderImg;
                dailydealDescPo.MiddleAreaHtml = dailyDealDesc.MiddleAreaHtml;
                dailydealDescPo.RecommendAreaHtml = dailyDealDesc.RecommendAreaHtml;
                dailydealDescPo.IsValid = dailyDealDesc.IsValid;
                DailydealDescDao.UpdateObject(dailydealDescPo);
            }
        }

        public DailyDealDesc GetDailydealDesc(int languageId)
        {
            var po = DailydealDescDao.GetOneObject("from DailydealDescPo where LanguageId=?", new object[] { languageId });
            if (!po.IsNullOrEmpty())
                return new DailyDealDesc
                {
                    LanguageId = po.LanguageId,
                    HeaderImg = po.HeaderImg,
                    MiddleAreaHtml = po.MiddleAreaHtml,
                    RecommendAreaHtml = po.RecommendAreaHtml,
                    IsValid = po.IsValid
                };
            return new DailyDealDesc
            {
                LanguageId = languageId,
            };
        }

        public List<DailyDealDesc> GetAllDailydealDesc()
        {
            var pos = DailydealDescDao.GetAll();
            if (!pos.IsNullOrEmpty())
                return pos.Select(x => new DailyDealDesc
                {
                    LanguageId = x.LanguageId,
                    HeaderImg = x.HeaderImg,
                    IsValid = x.IsValid,
                    MiddleAreaHtml = x.MiddleAreaHtml,
                    RecommendAreaHtml = x.RecommendAreaHtml
                }).ToList();
            return new List<DailyDealDesc> { };
        }

        #endregion
        #region 辅助方法

        internal static DailyDealLabelPo GetDailyDealLabelPoFromVo(DailyDealLabel dailyDealLabel)
        {
            DailyDealLabelPo dailyDealLabelPo = null;
            if (!dailyDealLabel.IsNullOrEmpty())
            {
                dailyDealLabelPo = new DailyDealLabelPo
                {
                    DailyDealId = dailyDealLabel.DailyDealId,
                    LanguageId = dailyDealLabel.LanguageId,
                    LabelName = dailyDealLabel.LabelName
                };
            }
            return dailyDealLabelPo;
        }
        #endregion
    }
}
