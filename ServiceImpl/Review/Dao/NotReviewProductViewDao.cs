using Com.Panduo.Entity.Review;

namespace Com.Panduo.ServiceImpl.Review.Dao
{
    public class NotReviewProductViewDao : BaseDao<NotReviewProductViewPo, int>, INotReviewProductViewDao
    {
        public NotReviewProductViewPo GetNotReviewProductViewPo(int customerId, int productId, int status)
        {
            return GetOneObject("FROM NotReviewProductViewPo WHERE CustomerId = ? AND ProductId = ? AND status = ?", new object[] { customerId, productId, status });
        }
    }
}