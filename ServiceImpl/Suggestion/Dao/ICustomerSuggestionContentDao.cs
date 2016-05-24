//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerSuggestionContentDao.cs
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
using Com.Panduo.Entity.Suggestion;

namespace Com.Panduo.ServiceImpl.Suggestion.Dao
{
    public interface ICustomerSuggestionContentDao : IBaseDao<CustomerSuggestionContentPo, int>
    {
        /// <summary>
        /// 修改客户评分内容
        /// </summary>
        /// <param name="detailId">Id</param>
        /// <param name="replyContent">答复内容</param>
        /// <param name="replyTime">答复时间</param>
        void UpdateCustomerSuggestionContent(int detailId, string replyContent, DateTime replyTime);
    }
}
