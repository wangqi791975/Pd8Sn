//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ProductUnit.cs
//创 建 人：罗海明
//创建时间：2015/01/08 14:49:50 
//功能说明：产品单位Vo
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
    public class ProductUnit
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int UnitId
        {
            get;
            set;
        }

        /// <summary>
        /// code
        /// </summary>
        public virtual string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 中文名称
        /// </summary>
        public virtual string ChineseName
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }             
    }
}
