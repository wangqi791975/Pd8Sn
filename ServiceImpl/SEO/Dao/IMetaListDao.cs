//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IMetaListDao.cs
//创 建 人 ：xiaoyong.lv
//创建时间：2015-02-09
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections;
using System.Collections.Generic;
using Com.Panduo.Entity.SEO;

namespace Com.Panduo.ServiceImpl.SEO.Dao
{
    public interface IMetaListDao : IBaseDao<MetaListPo, int>
    {
        /// <summary>
        /// 根据类别Id、语种Id取得所有类型meta列表信息
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>MetaListPo实体</returns>
        IList<MetaListPo> GetMetaListByType(int categoryId, int languageId);

        /// <summary>
        /// 根据类别Id、语种Id、页面类型取得meta列表信息
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="languageId">语种Id</param>
        /// <param name="type">页面类型MetaListPageType</param>
        /// <returns>MetaListPo实体</returns>
        MetaListPo GetMetaListByType(int categoryId, int languageId, int type);
    } 
}
   