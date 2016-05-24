//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CurrencyDao.cs
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
    public class CurrencyDao:BaseDao<CurrencyPo,int>, ICurrencyDao
    {
        public IList<CurrencyPo> GetAllValidCurrency()
        {
            return FindDataByHql("from CurrencyPo where Status=?", true); 
        }
        public decimal GetSingleCurrencyRate(int currencyId)
        {
            var obj = GetSingleObject("select Value from CurrencyPo where CurrencyId=?", currencyId);

            return obj == null ? 0.0M : decimal.Parse(obj.ToString());
        }

        public decimal GetSingleCurrencyRate(string currencyCode)
        {
            var obj = GetSingleObject("select Value from CurrencyPo where CurrencyCode=?", currencyCode);

            return obj == null ? 0.0M : decimal.Parse(obj.ToString());
        }
   
    }
}
