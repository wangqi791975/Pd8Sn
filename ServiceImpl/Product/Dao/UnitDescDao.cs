//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：UnitDescDao.cs
//创 建 人：罗海明
//创建时间：2015/01/08 17:20:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{
    public class UnitDescDao : BaseDao<UnitDescPo, int>, IUnitDescDao
    {
        public IList<UnitDescPo> GetAllUnitDescByUnitId(int unitId)
        {
            return FindDataByHql("from UnitDescPo where UnitId = ? ", unitId);
        }

        public UnitDescPo GetUnitDesc(int unitId, int languageId)
        {
            return GetOneObject("from UnitDescPo where UnitId = ? And LanguageId = ? ", new object[] { unitId, languageId });
        }

        public IList<UnitDescPo> GetAllUnitDescByUnitIdAndLangId(string unitIds, int languageId)
        {
            if (unitIds.IsNullOrEmpty())
            {
                return new List<UnitDescPo>();
            }

            return FindDataByHql(string.Format("from UnitDescPo where UnitId in ({0}) and LanguageId = {1}", unitIds, languageId));
        }
    } 
}
   