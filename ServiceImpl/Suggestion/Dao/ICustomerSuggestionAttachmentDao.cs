//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerSuggestionAttachmentDao.cs
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
    public interface ICustomerSuggestionAttachmentDao:IBaseDao<CustomerSuggestionAttachmentPo,int>
    {
        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="suggestionContentId">客户建议Id</param>
        /// <returns>附件集合</returns>
        IList<CustomerSuggestionAttachmentPo> GetCustomerSuggestionAttachments(int suggestionContentId);
    } 
}
   