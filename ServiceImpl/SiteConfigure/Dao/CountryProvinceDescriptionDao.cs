//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CountryProvinceDescriptionDao.cs
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
using Com.Panduo.Entity.SiteConfigure;

namespace Com.Panduo.ServiceImpl.SiteConfigure.Dao
{ 
    public class CountryProvinceDescriptionDao:BaseDao<CountryProvinceDescriptionPo,int>, ICountryProvinceDescriptionDao
    {
        public IList<CountryProvinceDescriptionPo> GetProvinceLanguages(int provinceId)
        { 
            return FindDataByHql("from CountryProvinceDescriptionPo where ProvinceId=?", provinceId);
        }

        public CountryProvinceDescriptionPo GetProvinceLanguage(int provinceId, int languageid)
        {
            return GetOneObject("from CountryProvinceDescriptionPo where ProvinceId = ? and LanguageId=?", new object[]{provinceId,languageid});
        }
    } 
}
   