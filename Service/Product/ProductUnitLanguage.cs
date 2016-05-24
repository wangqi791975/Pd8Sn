//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ProductUnitLanguage.cs
//创 建 人：罗海明
//创建时间：2015/01/08 14:49:50 
//功能说明：产品单位多语种Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;

namespace Com.Panduo.Service.Product
{
    [Serializable]
    public class ProductUnitLanguage
    {
        /// <summary>
        /// 单位id
        /// </summary>
        public virtual int UnitId
        {
            get;
            set;
        }
        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 单位多语种名称
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }
    }
}
