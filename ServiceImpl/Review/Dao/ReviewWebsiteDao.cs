//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ReviewWebsiteDao.cs
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
    public class ReviewWebsiteDao : BaseDao<ReviewWebsitePo, int>, IReviewWebsiteDao
    {
        public void UpdateReviewWebSite(int webSiteReviewId, int adminId, DateTime replyDateTime, string replyContent)
        {
            UpdateObjectByHql("UPDATE ReviewWebsitePo SET AdminId = ? , ReplyContent = ? , ReplyDate = ? WHERE ReviewId = ?",
                new object[] { adminId, replyContent, replyDateTime, webSiteReviewId });
        }

        public void UpdateReviewWebSite(int webSiteReviewId, bool isValid)
        {
            UpdateObjectByHql("UPDATE ReviewWebsitePo SET IsValid = ? WHERE ReviewId = ?",
                new object[] { isValid, webSiteReviewId });
        }

        public void UpdateReviewWebSite(int webSiteReviewId, int auditStatus)
        {
            UpdateObjectByHql("UPDATE ReviewWebsitePo SET Status = ? WHERE ReviewId = ?",
                new object[] { auditStatus, webSiteReviewId });
        }

        public void UpdateReviewWebSiteRecommend(int webSiteReviewId, bool isRecommend)
        {
            if (isRecommend)
                UpdateObjectByHql("UPDATE ReviewWebsitePo SET Recommend = ?", false);
            UpdateObjectByHql("UPDATE ReviewWebsitePo SET Recommend = ? WHERE ReviewId = ?",
               new object[] { isRecommend, webSiteReviewId });
        }

        public IList<ReviewWebsitePo> GetReviewProductPos(int orderId)
        {
            return FindDataByHql("FROM ReviewWebsitePo WHERE OrderId = ?", orderId);
        }

        public IList<ReviewWebsitePo> GetTopNReviewProductPos(int count)
        {
            return FindDataByHqlLimit("FROM ReviewWebsitePo", count);
        }
    }
}
