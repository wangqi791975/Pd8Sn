//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：HomeAreaSetting.cs
//创 建 人：罗海明
//创建时间：2015/04/07 15:20:40 
//功能说明：首页区域设置实体
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

namespace Com.Panduo.Service.Marketing.Banner
{
    public class HomeAreaSetting
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        public virtual int AreaId { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 0 请选择 1首页横导航 2 类别展示右侧、3类别展示下方
        /// </summary>
        public virtual HomeAreaType AreaType { get; set; }


    }

    /// <summary>
    /// Home区域类型枚举
    /// </summary>
    public enum HomeAreaType
    {
        //首页横导航
        Navigation=1,

        //类别展示右侧
        RightCategory=2,

        //类别展示下方
        BelowCategory = 3,
    }
}
