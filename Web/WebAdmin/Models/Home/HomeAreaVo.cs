using System.Collections.Generic;
using Com.Panduo.Service.Marketing.Banner;

namespace Com.Panduo.Web.Models.Home
{
    public class HomeAreaVo
    {

        public virtual IList<HomeAreaSetting> HomeAreaSetting { get; set; }
    }
}