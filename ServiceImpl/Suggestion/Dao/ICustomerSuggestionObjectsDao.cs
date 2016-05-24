//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICustomerSuggestionObjectsDao.cs
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
    public interface ICustomerSuggestionObjectsDao:IBaseDao<CustomerSuggestionObjectsPo,int>
    {
        /// <summary>
        /// 通过项Id获取评分对象
        /// </summary>
        /// <param name="itemId">项Id</param>
        /// <returns>评分对象集合</returns>
        IList<CustomerSuggestionObjectsPo> GetSuggestionObjectsPos(int itemId);
    } 
}
   