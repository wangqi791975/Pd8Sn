//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IReviewProductDao.cs
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
    public interface IReviewProductDao:IBaseDao<ReviewProductPo,int>
    {
        /// <summary>
        /// 通过订单明细Id获取评论
        /// </summary>
        /// <param name="orderProductId">订单明细Id</param>
        /// <returns>评论状态</returns>
        ReviewProductPo GetReviewProduct(int orderProductId);

        /// <summary>
        /// 修改评论
        /// </summary>
        /// <param name="productReviewId">评论Id</param>
        /// <param name="adminId">答复人Id</param>
        /// <param name="replyDateTime">答复时间</param>
        /// <param name="replyContent">答复内容</param>
        void UpdateReviewProduct(int productReviewId, int adminId, DateTime replyDateTime, string replyContent);

        /// <summary>
        /// 修改评论状态
        /// </summary>
        /// <param name="productReviewId">评论Id</param>
        /// <param name="isValid">是否有效</param>
        void UpdateReviewProduct(int productReviewId, bool isValid);

        /// <summary>
        /// 修改评论审核状态
        /// </summary>
        /// <param name="productReviewId">评论Id</param>
        /// <param name="auditStatus">审核状态</param>
        void UpdateReviewProduct(int productReviewId, int auditStatus);

        /// <summary>
        /// 通过产品Id获取评论数
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>产品评论数</returns>
        int GetReviewProductCount(int productId);

        /// <summary>
        /// 获取客户所有评论数
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        int GetReivewCusotmerCount(int customerId);

        /// <summary>
        /// 通过产品Id获取平均分
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>产品平均分</returns>
        decimal GetReviewProductAvg(int productId);
    } 
}
   