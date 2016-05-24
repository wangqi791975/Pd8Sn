//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ConfigureServiceTest.cs
//创 建 人：罗海明
//创建时间：2014/12/22 13:40:40 
//功能说明：站点配置服务单元测试
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Text.RegularExpressions;
using Com.Panduo.Service;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.Service.SiteConfigure;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class ConfigureServiceTest : SpringTest
    {

        #region 重写
        protected override void OnSetUp()
        {

        }

        protected override void OnTearDown()
        {

        }

        /// <summary>
        /// 站点配置测试数据初始化（整个测试过程中，只调用一次）
        /// </summary>
        public void TestFixtureSetup()
        {
            Console.WriteLine("==========Test ConfigureService Begin===========");
            TestDataService.ClearConfigData();
            TestDataService.LanguageInit();
            TestDataService.CurrencyInit();
            TestDataService.IpWhiteBlackListInit();
            TestDataService.CountryContinentInit();
            TestDataService.AddressFormatInit();
            TestDataService.CountryInit();
            TestDataService.CountryProvinceInit();
            TestDataService.CountryCityInit();
            TestDataService.CountryCityDescriptionInit();
            TestDataService.CountryProvinceDescriptionInit();
            TestDataService.CountryDescriptionInit();
        }

        public void TestFixtureTearDown()
        {
            TestDataService.ClearConfigData();
            Console.WriteLine("==========Test ConfigureService End===========");
        }
        #endregion

        #region 黑白名单管理
        #region TestAddBlackList
        [Test]
        public void TestAddBlackList1()
        {
            var back = new BlackList
            {
                IpAddress = "192.168.1.100",
                CreateId = 000,
                CreateTime = DateTime.Now,
                ModifyTime = DateTime.Now
            };
            ServiceFactory.ConfigureService.AddBlackList(back);
        }

        [Test]
        public void TestAddBlackList2()
        {
            var back = new BlackList
            {
                IpAddress = "192.168.1.101",
                CreateId = 1,
            };
            ServiceFactory.ConfigureService.AddBlackList(back);
        }

        [Test]
        public void TestAddBlackList3()
        {
            Assert.That(
                delegate
                {
                    var back = new BlackList
                    {
                        IpAddress = "192.168.1.200",
                        CreateId = 100,
                    };
                    ServiceFactory.ConfigureService.AddBlackList(back);
                },
                    Throws.TypeOf<BussinessException>()
                );
        }
        #endregion

        #region TestGetBlackList
        [Test]
        public void TestGetBlackList1()
        {
            var aa = ServiceFactory.ConfigureService.GetBlackList().Count;
            Assert.AreEqual(true, aa > 0);
        }

        [Test]
        public void TestGetBlackList2()
        {
            bool flag = false;
            var aa = ServiceFactory.ConfigureService.GetBlackList();
            foreach (var blackList in aa)
            {
                if ("192.168.1.200".Equals(blackList.IpAddress))
                {
                    flag = true;
                    break;
                }
            }
            Assert.AreEqual(true, flag);
        }
        #endregion

        #region TestDeleteBlackListById
        [Test]
        public void TestDeleteBlackListById1()
        {
            ServiceFactory.ConfigureService.DeleteBlackListById(1);
        }

        [Test]
        public void TestDeleteBlackListById2()
        {
            ServiceFactory.ConfigureService.DeleteBlackListById(2);
        }
        #endregion

        #region TestAddWhiteList
        [Test]
        public void TestAddWhiteList1()
        {
            Assert.DoesNotThrow(delegate
            {
                var back = new WhiteList
                {
                    IpAddress = "192.168.1.111",
                    CreateId = 000,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now
                };
                ServiceFactory.ConfigureService.AddWhiteList(back);
            });
        }
        [Test]
        public void TestAddWhiteList4()
        {
            var back = new WhiteList
            {
                IpAddress = "192.168.1.100",
                CreateId = 000,
                CreateTime = DateTime.Now
            };
            ServiceFactory.ConfigureService.AddWhiteList(back);
        }

        [Test]
        public void TestAddWhiteList2()
        {
            var back = new WhiteList
            {
                IpAddress = "192.168.1.101",
                CreateId = 000,
            };
            ServiceFactory.ConfigureService.AddWhiteList(back);
        }

        [Test]
        public void TestAddWhiteList3()
        {
            //Assert.Throws<BussinessException>(
            //    delegate
            //    {
            //        var back = new WhiteList
            //        {
            //            IpAddress = "192.168.1.200",
            //            CreateId = 200,
            //        };
            //        ServiceFactory.ConfigureService.AddWhiteList(back);
            //    });
            Assert.That(
                delegate
                {
                    var back = new WhiteList
                    {
                        IpAddress = "192.168.1.200",
                        CreateId = 200,
                    };
                    ServiceFactory.ConfigureService.AddWhiteList(back);
                },
              Throws.TypeOf<BussinessException>());
        }
        #endregion

        #region TestGetWhiteList
        [Test]
        public void TestGetWhiteList1()
        {
            var aa = ServiceFactory.ConfigureService.GetWhiteList().Count;
            Assert.AreEqual(true, aa > 0);
        }

        [Test]
        public void TestGetWhiteList2()
        {
            bool flag = false;
            var aa = ServiceFactory.ConfigureService.GetWhiteList();
            foreach (var whiteList in aa)
            {
                if ("192.168.1.200".Equals(whiteList.IpAddress))
                {
                    flag = true;
                    break;
                }
            }
            Assert.AreEqual(true, flag);
        }
        #endregion

        #region TestDeleteWhiteListById
        [Test]
        public void TestDeleteWhiteListById()
        {
            ServiceFactory.ConfigureService.DeleteWhiteListById(3);
        }
        #endregion
        #endregion

        #region TestGetAllValidLanguage
        [Test]
        public void TestGetAllValidLanguage1()
        {
            var c = ServiceFactory.ConfigureService.GetAllValidLanguage().Count;
            Assert.AreEqual(7, c);
        }

        [Test]
        public void TestGetAllValidLanguage2()
        {
            bool flag = false;
            var c = ServiceFactory.ConfigureService.GetAllValidLanguage();
            foreach (var language in c)
            {
                if ("it".Equals(language.LanguageCode))
                {
                    flag = true;
                }
            }
            Assert.AreEqual(true, flag);
        }
        #endregion

        #region TestGetContact
        [Test]
        public void TestGetContact1()
        {
            var c = ServiceFactory.ConfigureService.GetContact();
            Assert.AreEqual(true, c.MailBox.Equals("8seasons@panduo.com.cn") && c.Skype.Equals("panduo@panduo.com.cn") && c.Telephone.Equals("132566565566"));
        }

        [Test]
        public void TestGetContact2()
        {
            ServiceConfig.SiteContactMailBox = "test@panduo.com";
            ServiceConfig.SiteContactSkype = "panduo@skype.com";
            ServiceConfig.SiteContactTelephone = "1234567890";
            var c = ServiceFactory.ConfigureService.GetContact();
            Assert.AreEqual(true, c.MailBox.Equals("test@panduo.com") && c.Skype.Equals("panduo@skype.com") && c.Telephone.Equals("1234567890"));
        }
        #endregion


        [Test]
        [Category("Country")]
        public void TestGetAllCountry()
        {
            int c = ServiceFactory.ConfigureService.GetAllCountry().Count;
            Assert.AreEqual(true, c == 241);
        }

        [Test]
        [Category("Country")]
        public void TestGetAllValidCountry()
        {
            int c = ServiceFactory.ConfigureService.GetAllValidCountry().Count;
            Assert.AreEqual(true, c == 241);
        }

        [Test]
        [Category("Country")]
        public void TestGetCommonCountry()
        {
            int c = ServiceFactory.ConfigureService.GetCommonCountry().Count;
            Assert.AreEqual(1, c);
        }

        [Test]
        [Category("Country")]
        public void TestSetCommonCountry()
        {
            bool f = false;
            ServiceFactory.ConfigureService.SetCommonCountry(44, true, 10);
            var c = ServiceFactory.ConfigureService.GetCommonCountry();
            foreach (var country in c)
            {
                if (country.CountryId == 44 && country.IsCommonCountry == true && country.DisplayOrder == 10)
                {
                    f = true;
                    break;
                }
            }
            ServiceFactory.ConfigureService.SetCommonCountry(44, false, 0);//还原
            Assert.AreEqual(true, f);
        }


        [Test]
        [TestCase(245)]
        [TestCase(200)]
        [TestCase(50)]
        [Category("Country")]
        public void TestSetCountryHidden(int countryId)
        {
            int begin = ServiceFactory.ConfigureService.GetAllValidCountry().Count;
            ServiceFactory.ConfigureService.SetCountryHidden(countryId, false);
            int end = ServiceFactory.ConfigureService.GetAllValidCountry().Count;
            ServiceFactory.ConfigureService.SetCountryHidden(countryId, true);
            Assert.AreEqual(begin, end + 1);
        }



        [Test]
        [Category("Country")]
        public void TestGetAllCountryLanguages()
        {
            int c = ServiceFactory.ConfigureService.GetAllCountryLanguages().Count;
            Assert.AreEqual(true, c > 0);
        }


        [Test]
        [TestCase("61.135.169.125")]
        [TestCase("37.61.54.158")]
        [Category("Country")]
        public void TestGetCountrySimpleCode2ByIp(string ipAddress)
        {
            var c = ServiceFactory.ConfigureService.GetCountrySimpleCode2ByIp(ipAddress);
            Assert.AreEqual(2, c.Length);
        }

        #region TestGetCountryLanguages
        [Test]
        public void TestGetCountryLanguages1()
        {
            int c = ServiceFactory.ConfigureService.GetCountryLanguages(1).Count;
            Assert.AreEqual(true, c == 7);
        }

        [Test]
        public void TestGetCountryLanguages2()
        {
            int c = ServiceFactory.ConfigureService.GetCountryLanguages(2).Count;
            Assert.AreEqual(true, c == 7);
        }

        [Test]
        public void TestGetCountryLanguages3()
        {
            int c = ServiceFactory.ConfigureService.GetCountryLanguages(1000).Count;
            Assert.AreEqual(true, c == 0);
        }
        #endregion

        #region TestGetCountryLanguage
        [Test]
        public void TestGetCountryLanguage1()
        {
            var c = ServiceFactory.ConfigureService.GetCountryLanguage(2, 1);
            Assert.AreEqual(true, "Albania".Equals(c.CountryName));
        }

        [Test]
        public void TestGetCountryLanguage2()
        {
            var c = ServiceFactory.ConfigureService.GetCountryLanguage(1, 7);
            Assert.AreEqual(true, "Afghanistan".Equals(c.CountryName));
        }

        [Test]
        public void TestGetCountryLanguage3()
        {
            var c = ServiceFactory.ConfigureService.GetCountryLanguage(1, 8);
            Assert.AreEqual(null, c);
        }
        #endregion


        [Test]
        [Category("Country")]
        [TestCase(1, "FUCK")]
        [TestCase(14, "FUCK Again")]
        [TestCase(32, "￥￥￥￥￥￥￥￥￥￥￥￥￥￥￥￥$$$$$$$$$$$$$$")]
        [TestCase(88, "*******************************************************************************在长就爆掉了")]
        public void TestSetCountryAddressFormat1(int countryId, string format)
        {
            string old = ServiceFactory.ConfigureService.GetCountryAddressFormat(countryId);//原始数据
            ServiceFactory.ConfigureService.SetCountryAddressFormat(countryId, format);
            string c = ServiceFactory.ConfigureService.GetCountryAddressFormat(countryId);
            ServiceFactory.ConfigureService.SetCountryAddressFormat(countryId, old);//恢复数据
            Assert.AreEqual(format, c);
        }

        #region TestGetCountryAddressFormats
        [Test]
        [Category("Country")]
        public void TestGetCountryAddressFormats1()
        {
            string c = ServiceFactory.ConfigureService.GetCountryAddressFormat(1);
            Assert.AreEqual("$firstname $lastname$cr$streets$cr$city, $postcode$cr$statecomma$country", c);
        }

        [Test]
        [Category("Country")]
        public void TestGetCountryAddressFormats2()
        {
            string c = ServiceFactory.ConfigureService.GetCountryAddressFormat(14);
            Assert.AreEqual("$firstname $lastname$cr$streets$cr$postcode $city$cr$country", c);
        }
        #endregion


        [Test]
        public void TestGetAllProvinceByCountryId()
        {
            int c = ServiceFactory.ConfigureService.GetAllProvinceByCountryId(1).Count;
            Assert.AreEqual(true, c == 2);
        }

        [Test]
        public void TestGetAllProvinceLanguages()
        {
            int c = ServiceFactory.ConfigureService.GetAllProvinceLanguages().Count;
            Assert.AreEqual(true, c == 14);
        }

        #region TestGetProvinceLanguages
        [Test]
        public void TestGetProvinceLanguages1()
        {
            int c = ServiceFactory.ConfigureService.GetProvinceLanguages(1).Count;
            Assert.AreEqual(true, c == 7);
        }

        [Test]
        public void TestGetProvinceLanguages2()
        {
            int c = ServiceFactory.ConfigureService.GetProvinceLanguages(2).Count;
            Assert.AreEqual(true, c == 7);
        }

        [Test]
        public void TestGetProvinceLanguages3()
        {
            int c = ServiceFactory.ConfigureService.GetProvinceLanguages(100).Count;
            Assert.AreEqual(0, c);
        }
        #endregion

        #region TestGetProvinceLanguage
        [Test]
        public void TestGetProvinceLanguage1()
        {
            var c = ServiceFactory.ConfigureService.GetProvinceLanguage(1, 7);
            Assert.AreEqual(true, "it name".Equals(c.ProvinceName));
        }


        [Test]
        public void TestGetProvinceLanguage2()
        {
            var c = ServiceFactory.ConfigureService.GetProvinceLanguage(1, 6);
            Assert.AreEqual(true, "jp name".Equals(c.ProvinceName));
        }

        [Test]
        public void TestGetProvinceLanguage3()
        {
            var c = ServiceFactory.ConfigureService.GetProvinceLanguage(1, 9);
            Assert.AreEqual(null, c);
        }
        #endregion

        #region TestGetAllCityByProvinceId
        [Test]
        public void TestGetAllCityByProvinceId1()
        {
            int c = ServiceFactory.ConfigureService.GetAllCityByProvinceId(1).Count;
            Assert.AreEqual(true, c == 2);
        }

        [Test]
        public void TestGetAllCityByProvinceId2()
        {
            int c = ServiceFactory.ConfigureService.GetAllCityByProvinceId(30).Count;
            Assert.AreEqual(true, c == 0);
        }
        #endregion

        #region TestGetCityLanguages
        [Test]
        public void TestGetCityLanguages1()
        {
            int c = ServiceFactory.ConfigureService.GetCityLanguages(1).Count;
            Assert.AreEqual(true, c == 7);
        }


        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void TestGetCityLanguages2(int cityId)
        {
            int c = ServiceFactory.ConfigureService.GetCityLanguages(cityId).Count;
            Assert.AreEqual(true, c == 7);
        }

        [Test]
        [TestCase(100)]
        [TestCase(200)]
        public void TestGetCityLanguages3(int cityId)
        {
            int c = ServiceFactory.ConfigureService.GetCityLanguages(cityId).Count;
            Assert.AreEqual(0, c);
        }
        #endregion

        #region TestGetCityLanguage
        [Test]
        public void TestGetCityLanguage1()
        {
            var c = ServiceFactory.ConfigureService.GetCityLanguage(2, 1);
            Assert.AreEqual("111", c.CityName);
        }

        [Test]
        public void TestGetCityLanguage2()
        {
            var c = ServiceFactory.ConfigureService.GetCityLanguage(2, 6);
            Assert.AreEqual("2666", c.CityName);
        }

        [Test]
        public void TestGetCityLanguage3()
        {
            var c = ServiceFactory.ConfigureService.GetCityLanguage(4, 8);
            Assert.AreEqual(null, c);
        }

        [Test]
        public void TestIsCountryHighRisk()
        {
            var c = ServiceFactory.ConfigureService.IsCountryHighRisk(5);
            Console.WriteLine(c);
        }
        #endregion

        [Test]
        public void TestIsIpAddressLimit()
        {
            bool c = ServiceFactory.ConfigureService.IsIpAddressLimit;
            Assert.AreEqual(true, c);
        }


        #region TestGetCountryBySimpleCode2
        [Test]
        [Category("Country")]
        public void TestGetCountryBySimpleCode21()
        {
            var c = ServiceFactory.ConfigureService.GetCountryBySimpleCode2("AO");
            Assert.AreEqual(true, "Angola".Equals(c.CountryName));
        }

        [Test]
        [Category("Country")]
        public void TestGetCountryBySimpleCode22()
        {
            var c = ServiceFactory.ConfigureService.GetCountryBySimpleCode2("XX");
            Assert.AreEqual(null, c);
        }

        [Test]
        [Category("Country")]
        public void TestGetCountryBySimpleCode23()
        {
            var c = ServiceFactory.ConfigureService.GetCountryBySimpleCode2("US");
            Assert.AreEqual(true, "United States".Equals(c.CountryName) && c.IsCommonCountry == true && c.IsDisplay == true && c.CountryId == 223);
        }

        [Test]
        public void TestGetCountryByIp()
        {
            var country = ServiceFactory.ConfigureService.GetCountryByIp("101.68.66.206");
            Console.WriteLine(country.CountryName);

            country = ServiceFactory.ConfigureService.GetCountryByIp("");
            Console.WriteLine(country.CountryName);
        }

        #endregion

        [Test]
        [Category("Currency")]
        public void TestGetAllCurrencies()
        {
            int c = ServiceFactory.ConfigureService.GetAllCurrencies().Count;
            Assert.AreEqual(8, c);
        }

        [Test]
        [Category("Currency")]
        public void TestGetAllValidCurrencies()
        {
            int start = ServiceFactory.ConfigureService.GetAllValidCurrencies().Count;
            ServiceFactory.ConfigureService.SetCurrencyStatusById(10, false);
            int end = ServiceFactory.ConfigureService.GetAllValidCurrencies().Count;
            ServiceFactory.ConfigureService.SetCurrencyStatusById(10, true);
            Assert.AreEqual(start, end + 1);
        }

        [Test]
        [TestCase(8)]
        [TestCase(1)]
        [Category("Currency")]
        public void TestSetCurrencyStatusById(int currencyId)
        {
            int start = ServiceFactory.ConfigureService.GetAllCurrencies().Count;
            ServiceFactory.ConfigureService.SetCurrencyStatusById(currencyId, false);
            int end = ServiceFactory.ConfigureService.GetAllValidCurrencies().Count;
            ServiceFactory.ConfigureService.SetCurrencyStatusById(currencyId, true);
            Assert.AreEqual(start, end + 1);
        }

        [Test]
        [Category("Currency")]
        public void TestGetSingleCurrencyRate1()
        {
            var c = ServiceFactory.ConfigureService.GetSingleCurrencyRate(1);
            Assert.AreEqual(1.00000000, c);
        }

        [Test]
        [Category("Currency")]
        public void TestGetSingleCurrencyRate2()
        {
            var c = ServiceFactory.ConfigureService.GetSingleCurrencyRate("EUR");
            Assert.AreEqual(0.80000001, c);
        }

        #region TestSetSearchKeyword
        [Test]
        [Category("SearchKeyword")]
        public void TestSetSearchKeyword1()
        {
            var s = new SearchKeyword()
            {
                KeywordName = "test",
                KeywordUrl = "http://wwww.8seasons.com",
                DisplayOrder = 10,
                LanguageId = 1,
                KeywordType = KeywordType.InBox,
            };
            ServiceFactory.ConfigureService.SetSearchKeyword(s);
        }

        [Test]
        [Category("SearchKeyword")]
        public void TestSetSearchKeyword2()
        {
            var s = new SearchKeyword()
            {
                KeywordName = "test bottom",
                KeywordUrl = "http://wwww.8seasons.com",
                DisplayOrder = 12,
                LanguageId = 2,
                KeywordType = KeywordType.Bottom,
            };
            ServiceFactory.ConfigureService.SetSearchKeyword(s);
        }
        #endregion


        [Test]
        [Category("SearchKeyword")]
        [TestCase(KeywordType.InBox)]
        [TestCase(KeywordType.Bottom)]
        public void TestGetSearchKeywordByType1(KeywordType type)
        {
            var c = ServiceFactory.ConfigureService.GetSearchKeywordByType(type).Count;
            Assert.AreEqual(true, c == 1);
        }

        [Test]
        public void GetConfig()
        {
            var a = ServiceFactory.ConfigureService.GetConfig("");
        }

        [Test]
        public void TetPromotion()
        {
            var isPromotion = ServiceFactory.ConfigureService.IsPromotion;
            ServiceFactory.ConfigureService.IsPromotion = true;
            isPromotion = ServiceFactory.ConfigureService.IsPromotion;

            var promotionBegin = ServiceFactory.ConfigureService.PromotionDateBegin;
            ServiceFactory.ConfigureService.PromotionDateBegin = DateTime.Now.AddDays(2);
            promotionBegin = ServiceFactory.ConfigureService.PromotionDateBegin;

            var promotionEnd = ServiceFactory.ConfigureService.PromotionDateEnd;
            ServiceFactory.ConfigureService.PromotionDateEnd = DateTime.Now.AddDays(20);
            promotionEnd = ServiceFactory.ConfigureService.PromotionDateEnd;
        }

        [Test]
        public void TestGetRemoteCurrencies()
        {
            var currencies = ServiceFactory.ConfigureService.GetAllRemoteCurrencies();
            foreach (var currency in currencies)
            {
                Console.WriteLine(string.Format("ChineseName:{0}, CurrencyCode:{1}, ExchangeRate:{2}, ExchangeRateRemote:{3}", currency.ChineseName, currency.CurrencyCode, currency.ExchangeRate, currency.ExchangeRateRemote));
                Console.WriteLine("-------");
            }
        }

        [Test]
        public void TestRegex()
        {
            string source = "\n\t\t\t\t\t \n\t\t\t\t\t  \n                    <tr>\n                    \t<td>美元</td>\n                        <td>620.12</td>\n                        <td>615.15</td>\n                        <td>622.6</td>\n                        <td>622.6</td>\n                        <td>614.07</td>\n                        <td>2015-04-14</td>\n                        <td>14:47:31</td>\n                    ";
            string sPattern = @"<td.?>(.+?)</td>";
            var mc = Regex.Matches(source, sPattern);
            Console.WriteLine(mc[0].Groups[1].Value);
        }
    }
}
