//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CountryDao.cs
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
using Com.Panduo.Entity;
using Com.Panduo.Entity.SiteConfigure;

namespace Com.Panduo.ServiceImpl.SiteConfigure.Dao
{ 
    public class CountryDao:BaseDao<CountryPo,int>, ICountryDao
    {
        public IList<CountryPo> GetAllCommonCountry()
        {
            return FindDataByHql("from CountryPo where IsCommon=?", true);
        }

        public IList<CountryPo> GetAllUnCommonCountry()
        {
            return FindDataByHql("from CountryPo where IsCommon=?", false);
        }

        public IList<CountryPo> GetAllValidCountry()
        {
            return FindDataByHql("from CountryPo where Status=?", true);
        }

        public CountryPo GetCountryBySimpleCode2(string simpleCode2)
        {
            return GetOneObject("from CountryPo where CountryIsoCode2 = ?", simpleCode2);
        }

        public string GetCountryAddressFormat(int countryId)
        {
            return
                GetSingleObject("SELECT a.AddressFormat FROM CountryPo as c , AddressFormatPo as a  WHERE a.AddressFormatId = c.AddressFormat AND c.CountryId=?", countryId) as string;

        }
    } 
}
   