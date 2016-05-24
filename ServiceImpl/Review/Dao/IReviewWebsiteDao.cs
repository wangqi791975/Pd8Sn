//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IReviewWebsiteDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
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
    public interface IReviewWebsiteDao:IBaseDao<ReviewWebsitePo,int>
    {
        /// <summary>
        /// 修改评论
        /// </summary>
        /// <param name="webSiteReviewId">评论Id</param>
        /// <param name="adminId">答复人Id</param>
        /// <param name="replyDateTime">答复时间</param>
        /// <param name="replyContent">答复内容</param>
        void UpdateReviewWebSite(int webSiteReviewId, int adminId, DateTime replyDateTime, string replyContent);

        /// <summary>
        /// 修改评论状态
        /// </summary>
        /// <param name="webSiteReviewId">评论Id</param>
        /// <param name="isValid">是否有效</param>
        void UpdateReviewWebSite(int webSiteReviewId, bool isValid);

        /// <summary>
        /// 修改评论审核状态
        /// </summary>
        /// <param name="webSiteReviewId"></param>
        /// <param name="auditStatus"></param>
        void UpdateReviewWebSite(int webSiteReviewId, int auditStatus);

        /// <summary>
        /// 修改评论首页推荐
        /// </summary>
        /// <param name="webSiteReviewId"></param>
        /// <param name="isRecommend"></param>
        void UpdateReviewWebSiteRecommend(int webSiteReviewId, bool isRecommend);

        /// <summary>
        /// 通过订单Id获取评论
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>评论集合</returns>
        IList<ReviewWebsitePo> GetReviewProductPos(int orderId);

        /// <summary>
        /// 获取TopN
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns>评论集合</returns>
        IList<ReviewWebsitePo> GetTopNReviewProductPos(int count);
    } 
}
   