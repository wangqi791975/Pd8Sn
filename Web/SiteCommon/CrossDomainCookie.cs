using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common
{
    public partial class CrossDomainCookie : IHttpModule
    {
        private string _rootDomain = string.Empty;

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            _rootDomain = ConfigManager.CrossDomainCookie;
            context.EndRequest += ContextEndRequest;
        }

        void ContextEndRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            for (var i = 0; i < app.Context.Response.Cookies.Count; i++)
            {
                app.Context.Response.Cookies[i].Domain = _rootDomain;
            }
        }

        #endregion
    }
}
