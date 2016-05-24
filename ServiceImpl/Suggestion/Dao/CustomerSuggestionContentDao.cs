//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CustomerSuggestionContentDao.cs
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
using Com.Panduo.Entity.Suggestion;

namespace Com.Panduo.ServiceImpl.Suggestion.Dao
{
    public class CustomerSuggestionContentDao : BaseDao<CustomerSuggestionContentPo, int>, ICustomerSuggestionContentDao
    {
        public void UpdateCustomerSuggestionContent(int detailId, string replyContent, DateTime replyTime)
        {
            UpdateObjectByHql("UPDATE CustomerSuggestionContentPo SET ReplyContent = ? ,ReplyDate = ? WHERE DetailId = ?", new object[] { replyContent, replyTime, detailId });
        }
    }
}
