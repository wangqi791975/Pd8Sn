//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerSuggestionDetailDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections;
using System.Collections.Generic;
using Com.Panduo.Entity.Suggestion;

namespace Com.Panduo.ServiceImpl.Suggestion.Dao
{ 
    public interface ICustomerSuggestionDetailDao:IBaseDao<CustomerSuggestionDetailPo,int>
    {
        /// <summary>
        /// 获取评分明细
        /// </summary>
        /// <param name="suggestionContentId">客户建议Id</param>
        /// <returns>评分明细集合</returns>
        IList<CustomerSuggestionDetailPo> GetCustomerSuggestionDetails(int suggestionContentId);
    } 
}
   