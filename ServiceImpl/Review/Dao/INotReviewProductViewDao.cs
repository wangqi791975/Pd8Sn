using Com.Panduo.Entity.Review;

namespace Com.Panduo.ServiceImpl.Review.Dao
{
    public interface INotReviewProductViewDao : IBaseDao<NotReviewProductViewPo, int>
    {
        NotReviewProductViewPo GetNotReviewProductViewPo(int customerId, int productId,int status);
    }
}