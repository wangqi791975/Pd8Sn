//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IPropertyValueDescDao.cs
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
using Com.Panduo.Entity.Product.Property;

namespace Com.Panduo.ServiceImpl.Product.Property.Dao
{ 
    public interface IPropertyValueDescDao:IBaseDao<PropertyValueDescPo,int>
    {
        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="propertyValueId">属性ID</param>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        PropertyValueDescPo GetPropertyDescPo(int propertyValueId, int languageId);

        /// <summary>
        /// 得到所有属性值当前语种列表
        /// </summary>
        /// <param name="languageId">语种ID</param>
        /// <returns></returns>
        IList<PropertyValueDescPo> GetPropertyDescPos(int languageId);
    } 
}
   