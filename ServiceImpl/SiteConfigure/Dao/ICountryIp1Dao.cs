﻿//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ICountryIp1Dao.cs
//创 建 人：罗海明
//创建时间：2014/12/23 16:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using Com.Panduo.Entity.SiteConfigure;

namespace Com.Panduo.ServiceImpl.SiteConfigure.Dao
{
    public interface ICountryIp1Dao : IBaseDao<CountryIp1Po, int>
    {
        CountryIp1Po GetCountryIp1(long ip);
    } 
}
