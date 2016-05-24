using Com.Panduo.Entity.Review;

namespace Com.Panduo.ServiceImpl.Review.Dao
{
    public interface IReviewWebsiteCustomerViewDao : IBaseDao<ReviewWebsiteCustomerViewPo,int>
    {
        /// <summary>
        /// 获取首页推荐评论
        /// </summary>
        /// <returns>首页推荐评论</returns>
        ReviewWebsiteCustomerViewPo GetRecommendReview();
    }
}