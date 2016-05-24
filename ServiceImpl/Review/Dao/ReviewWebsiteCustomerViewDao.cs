using Com.Panduo.Entity.Review;

namespace Com.Panduo.ServiceImpl.Review.Dao
{
    public class ReviewWebsiteCustomerViewDao : BaseDao<ReviewWebsiteCustomerViewPo, int>, IReviewWebsiteCustomerViewDao
    {
        public ReviewWebsiteCustomerViewPo GetRecommendReview()
        {
            return GetOneObject("FROM ReviewWebsiteCustomerViewPo WHERE Recommend = 1 AND IsValid = ? ORDER BY DateCreated DESC", true);
        }
    }
}