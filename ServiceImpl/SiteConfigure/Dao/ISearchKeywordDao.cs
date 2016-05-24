//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ISearchKeywordDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System.Collections.Generic;
using Com.Panduo.Entity.SiteConfigure;
using Com.Panduo.Service.SiteConfigure;

namespace Com.Panduo.ServiceImpl.SiteConfigure.Dao
{ 
    public interface ISearchKeywordDao:IBaseDao<SearchKeywordPo,int>
    {
        /// <summary>
        /// 获取关键词信息
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <param name="name">关键词名称</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        SearchKeywordPo GetOneSearchKeyword(int languageId, string name, int type);

        /// <summary>
        /// 根据关键词类型获取关键词列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>关键词列表</returns>
        IList<SearchKeywordPo> GetSearchKeywordByType(KeywordType type);
    } 
}
   