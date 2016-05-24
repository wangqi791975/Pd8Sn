using System;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product.Club;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.ClubProduct;
using Com.Panduo.ServiceImpl.Product.ClubProduct.Dao;
using Com.Panduo.ServiceImpl.Product.Dailydeal.Dao;
using Com.Panduo.ServiceImpl.Product.Dao;
using Com.Panduo.ServiceImpl.Product.Promotion.Dao;

namespace Com.Panduo.ServiceImpl.Product.ClubProduct
{
    public class ClubProductService : IClubProductService
    {
        public IClubProductDao ClubProductDao { private get; set; }
        public IProductDao ProductDao { private get; set; }
        public IClubProductViewDao ClubProductViewDao { private get; set; }
        public IDailydealProductDao DailydealProductDao { private get; set; }
        public IPromotionProductDao PromotionProductDao { private get; set; }

        public string ERROR_PRODUCT_NOT_EXIST { get { return "ERROR_PRODUCT_NOT_EXIST"; } }
        public string ERROR_CLUBPRODUCT_EXIST { get { return "ERROR_CLUBPRODUCT_EXIST"; } }
        public string ERROR_CLUBPRODUCT_NOT_EXIST { get { return "ERROR_CLUBPRODUCT_NOT_EXIST"; } }
        public string ERROR_EXIST_IN_DAILYDEAL_PRODUCT { get { return "ERROR_EXIST_IN_DAILYDEAL_PRODUCT"; } }
        public string ERROR_EXIST_IN_PROMOTION_PRODUCT { get { return "ERROR_EXIST_IN_PROMOTION_PRODUCT"; } }
        public string ERROR_CLUBPRODUCT_REPETITION { get { return "ERROR_CLUBPRODUCT_REPETITION"; } }


        public Service.Product.ClubProduct.ClubProduct CreatedClubProductByImportData(string productNo, decimal discount)
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<string, string> SetClubProductList(List<Service.Product.ClubProduct.ClubProduct> clubProducts)
        {
            var clubProductIds = new List<int>();
            if (!clubProducts.IsNullOrEmpty())
            {
                foreach (var clubProduct in clubProducts)
                {
                    var product = ProductDao.GetObject(clubProduct.ProductId);
                    if (product.IsNullOrEmpty())
                    {
                        return new KeyValuePair<string, string>(ERROR_PRODUCT_NOT_EXIST, product.ProductModel);
                    }
                    if (!DailydealProductDao.GetDailydealProduct(clubProduct.ProductId).IsNullOrEmpty())
                    {
                        return new KeyValuePair<string, string>(ERROR_EXIST_IN_DAILYDEAL_PRODUCT, product.ProductModel);
                    }
                    if (PromotionProductDao.IsPromotionProduct(clubProduct.ProductId))
                    {
                        return new KeyValuePair<string, string>(ERROR_EXIST_IN_PROMOTION_PRODUCT, product.ProductModel);
                    }
                    if (!ClubProductDao.GetClubProduct(clubProduct.ProductId).IsNullOrEmpty())
                    {
                        return new KeyValuePair<string, string>(ERROR_CLUBPRODUCT_EXIST, product.ProductModel);
                    }
                    if (clubProductIds.Contains(clubProduct.ProductId))
                    {
                        return new KeyValuePair<string, string>(ERROR_CLUBPRODUCT_REPETITION, product.ProductModel);
                    }
                    clubProductIds.Add(clubProduct.ProductId);
                }
            }
            ClubProductDao.AddObjects(clubProducts.Select(GetClubProductPoFromVo));
            return new KeyValuePair<string, string>(string.Empty, string.Empty);
        }

        public void ClearClubProduct()
        {
            throw new NotImplementedException();
        }

        public Service.Product.ClubProduct.ClubProduct GetVaildClubProductByProductId(int productId)
        {
            var po = ClubProductDao.GetVaildClubProductByProductId(productId);
            return GetClubProductVoFromPo(po);
        }

        public IList<Service.Product.ClubProduct.ClubProduct> GetVaildClubProductByProductIds(IList<int> productIds)
        {
            var clubList = new List<Service.Product.ClubProduct.ClubProduct>();
            var polist = ClubProductDao.GetVaildClubProductByProductIds(productIds);
            if (!polist.IsNullOrEmpty())
            {
                foreach (var clubPo in polist)
                {
                    clubList.Add(GetClubProductVoFromPo(clubPo));
                }
            }
            return clubList;
        }

        public void RemoveClubProduct(int productId)
        {
            var clubProduct = ClubProductDao.GetClubProduct(productId);
            if (clubProduct.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_CLUBPRODUCT_NOT_EXIST);
            }
            ClubProductDao.DeleteClubProduct(productId);
        }

        public List<Service.Product.ClubProduct.ClubProduct> GetClubProductsByType(ClubProductType type)
        {
            throw new NotImplementedException();
        }

        public PageData<ClubProductView> FindAllClubProducts(int currentPage, int pageSize, IDictionary<ClubProductSearchCriteria, object> searchCriteria, IList<Sorter<ClubProductSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM ClubProductViewPo");
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case ClubProductSearchCriteria.ProductCode:
                            hqlHelper.AddWhere("ProductModel", HqlOperator.Like, "ProductModel", item.Value);
                            break;
                        case ClubProductSearchCriteria.ClubProductType:
                            hqlHelper.AddWhere("ClubProductType", HqlOperator.Eq, "ClubProductType", item.Value);
                            break;
                        case ClubProductSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("LanguageId", HqlOperator.Eq, "LanguageId", item.Value);
                            break;
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {

                }
            }
            else
            {
                hqlHelper.AddSorter("DateCreated", false);
            }

            var pageDataPo = ClubProductViewDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);

            var pageDataVo = new PageData<ClubProductView>();
            var voList = pageDataPo.Data.Select(GetClubProductViewVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;

            return pageDataVo;
        }

        #region 辅助方法

        /// <summary>
        /// Po转Vo
        /// </summary>
        /// <param name="clubProductPo">club产品Po</param>
        /// <returns></returns>
        internal static Service.Product.ClubProduct.ClubProduct GetClubProductVoFromPo(ClubProductPo clubProductPo)
        {
            Service.Product.ClubProduct.ClubProduct clubProduct = null;
            if (!clubProductPo.IsNullOrEmpty())
            {
                clubProduct = new Service.Product.ClubProduct.ClubProduct
                {
                    ProductId = clubProductPo.ProductId,
                    Discount = clubProductPo.Discount,
                    EndDate = clubProductPo.DateEnded,
                    CreateDateTime = clubProductPo.DateCreated,
                };
            }
            return clubProduct;
        }

        internal static ClubProductPo GetClubProductPoFromVo(Service.Product.ClubProduct.ClubProduct clubProduct)
        {
            ClubProductPo clubProductPo = null;
            if (!clubProduct.IsNullOrEmpty())
            {
                clubProductPo = new ClubProductPo
                {
                    ProductId = clubProduct.ProductId,
                    ClubProductType = (int)clubProduct.Type,
                    Discount = clubProduct.Discount,
                    DateEnded = clubProduct.EndDate,
                    DateCreated = clubProduct.CreateDateTime,
                };
            }
            return clubProductPo;
        }

        internal static ClubProductView GetClubProductViewVoFromPo(ClubProductViewPo clubProductViewPo)
        {
            ClubProductView clubProductView = null;
            if (!clubProductViewPo.IsNullOrEmpty())
            {
                clubProductView = new ClubProductView
                {
                    Id = clubProductViewPo.Id,
                    ProductId = clubProductViewPo.ProductId,
                    LanguageId = clubProductViewPo.LanguageId,
                    ProductModel = clubProductViewPo.ProductModel,
                    Name = clubProductViewPo.Name,
                    ClubProductType = clubProductViewPo.ClubProductType,
                    Discount = clubProductViewPo.Discount,
                    DateCreated = clubProductViewPo.DateCreated,
                    DateEnded = clubProductViewPo.DateEnded,
                    Image = clubProductViewPo.Image,
                    ProfitRate = clubProductViewPo.ProfitRate
                };
            }
            return clubProductView;
        }

        #endregion

    }

}
