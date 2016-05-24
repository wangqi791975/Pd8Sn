using System.Web;
using System.Web.Mvc;
using Com.Panduo.Web.Common.Filters;
using Com.Panduo.Web.Common.Mvc.Filter;

namespace Com.Panduo.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //登录拦截
            filters.Add(new LoginFilter());
            //Url拦截
            filters.Add(new UrlFilter());
            //异常拦截
            filters.Add(new ExceptionFilter());
            //压缩拦截器
            filters.Add(new GlobalCompressFilter());
        }
    }
}