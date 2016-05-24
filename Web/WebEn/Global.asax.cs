using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Web.Common.Mvc.Engine;
using Com.Panduo.Web.Common.Mvc.Error;
using Spring.Web.Mvc;

namespace Com.Panduo.Web
{
    public class MvcApplication : SpringMvcApplication
    {
        protected void Application_Start()
        {
            //注册区域
            AreaRegistration.RegisterAllAreas();

            //注册自定义视图引擎
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CustomRazorViewEngine());

            //注册全局过滤器
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //注册路由
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //错误过滤器
            new InstallerModule().Init(this);

            //禁止显示MVC版本信息
            MvcHandler.DisableMvcResponseHeader = true;

            //初始化Log4net组建
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\log4net.config");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
            

            LoggerHelper.GetLogger(LoggerType.Web).Info("系统已启动");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError().GetBaseException();
            var sb = new StringBuilder();
            sb.Append("错误发生时间:" + DateTime.Now.ToString() + "\r\n");
            sb.Append("错误页面:" + Request.Url.ToString() + "\r\n");
            sb.Append("IP:" + Request.UserHostAddress + "\r\n");
            sb.Append("浏览器语言:" + Request.UserLanguages + "\r\n");
            sb.Append("UserAgent:" + Request.UserAgent + "\r\n");
            sb.Append("错误信息:" + error.Message + "\r\n");
            sb.Append("堆栈信息:" + "\r\n" + error.StackTrace + "\r\n");
            //记录日志
            LoggerHelper.GetLogger(LoggerType.Web).Error(sb, error);

            //发送邮件
            //MailHelper.SendToAdmin("FoxAdmin程序错误", sb.ToString().Replace("\r\n", "<br />"));
        }

        protected void Application_End(object sender, EventArgs e)
        {
            LoggerHelper.GetLogger(LoggerType.Web).Info("系统已停止运行");
        }
    }
}