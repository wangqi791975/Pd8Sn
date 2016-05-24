//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：CountryIp1Dao.cs
//创 建 人：罗海明
//创建时间：2014/12/23 16:49:50 
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
    public class CountryIp1Dao : BaseDao<CountryIp1Po, int>, ICountryIp1Dao
    {
        public CountryIp1Po GetCountryIp1(long ip)
        {
            return GetOneObject("from CountryIp1Po where IpFrom <= ? And IpTo>= ?",new object[]{ ip, ip});
        }
    }
}
