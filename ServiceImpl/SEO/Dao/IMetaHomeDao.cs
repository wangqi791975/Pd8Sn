//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IMetaHomeDao.cs
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
    public interface IMetaHomeDao : IBaseDao<MetaHomePo, int>
    {
        /// <summary>
        /// 根据语种取得所有的meta首页信息
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns>MetaHomePo实体list</returns>
        IList<MetaHomePo> GetMetaHomesByLanguageId(int languageId);

        /// <summary>
        /// 根据页面类型、语种Id取得meta首页信息
        /// </summary>
        /// <param name="type">MetaHomePageType</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>MetaHomePo实体</returns>
        MetaHomePo GetMetaHomeByType(int type, int languageId);
    } 
}
   