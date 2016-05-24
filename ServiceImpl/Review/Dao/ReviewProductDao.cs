//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ReviewProductDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Review;

namespace Com.Panduo.ServiceImpl.Review.Dao
{
    public class ReviewProductDao : BaseDao<ReviewProductPo, int>, IReviewProductDao
    {
        public ReviewProductPo GetReviewProduct(int orderProductId)
        {
            return GetOneObject("FROM  ReviewProductPo WHERE OrderProductId = ?", orderProductId);
        }

        public void UpdateReviewProduct(int productReviewId, int adminId, DateTime replyDateTime, string replyContent)
        {
            UpdateObjectByHql("UPDATE ReviewProductPo SET AdminId = ? , ReplyDate = ? ,ReplyContent = ? WHERE ReviewId = ?",
                new object[] { adminId, replyDateTime, replyContent, productReviewId });
        }

        public void UpdateReviewProduct(int productReviewId, bool isValid)
        {
            UpdateObjectByHql("UPDATE ReviewProductPo SET IsValid = ? WHERE ReviewId = ?",
                new object[] { isValid, productReviewId });
        }

        public void UpdateReviewProduct(int productReviewId, int auditStatus)
        {
            UpdateObjectByHql("UPDATE ReviewProductPo SET Status = ? WHERE ReviewId = ?",
                new object[] { auditStatus, productReviewId });
        }

        public int GetReviewProductCount(int productId)
        {
            var obj = GetSingleObject("SELECT COUNT(ReviewId) FROM ReviewProductPo WHERE ProductId = ?", productId);
            return obj == null ? 0 : int.Parse(obj.ToString());
        }

        public int GetReivewCusotmerCount(int customerId)
        {
            var obj = GetSingleObject("SELECT COUNT(ReviewId) FROM ReviewProductPo WHERE CustomerId = ?", customerId);
            return obj == null ? 0 : int.Parse(obj.ToString());
        }

        public decimal GetReviewProductAvg(int productId)
        {
            var obj = GetSingleObject("SELECT AVG(ReviewRating) FROM ReviewProductPo WHERE ProductId = ?", productId);
            return obj == null ? 0.0M : int.Parse(obj.ToString());
        }
    }
}
