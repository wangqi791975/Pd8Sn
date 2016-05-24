
using Com.Panduo.Service;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.ServiceImpl;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    /// <summary>
    /// ServiceConfigTest 的摘要说明
    /// </summary>
    [TestFixture]
    public class ServiceConfigTest :SpringTest
    {
        [Test]
        public void TestLang()
        {

            var isLoadCacheInit = ServiceConfig.IsLoadCacheInit;

            var lang = ServiceFactory.ConfigureService.SiteLanguageCode;
            var langId = ServiceFactory.ConfigureService.SiteLanguageId;

            //var service = ServiceFactory.AdminUserService; 

            var conn = SqlHelper.CONN_STRING;
        }
    }
}
