//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CustomerSuggestionAttachmentDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Suggestion;

namespace Com.Panduo.ServiceImpl.Suggestion.Dao
{ 
    public class CustomerSuggestionAttachmentDao:BaseDao<CustomerSuggestionAttachmentPo,int>, ICustomerSuggestionAttachmentDao
    {
        public IList<CustomerSuggestionAttachmentPo> GetCustomerSuggestionAttachments(int suggestionContentId)
        {
            return FindDataByHql("FROM CustomerSuggestionAttachmentPo WHERE DetailId = ?", suggestionContentId);
        }
    } 
}
   