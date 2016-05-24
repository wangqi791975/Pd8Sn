using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Com.Panduo.Common;
using Com.Panduo.Entity.Payment;
using Com.Panduo.Service.Payment.PayInfo;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.ServiceConst;
using Com.Panduo.ServiceImpl.Product.Solr;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    [TestFixture]
    public class ToolsTest
    { 
        private static string ReplaceDataDotSpaceRegexFormat = @"(\d+)\.+(\w+?)";
        private static string ReplaceNumberSpaceRegexFormat = @"(\d+)\s+(\d+?)";
        private static string ReplaceUnitSpaceRegexFormat = @"(\d+)\s*(\w+)";
        private static string ReplaceMuliSpaceRegexFormat = @"(x+)(\d+?)";

        /// <summary>
        /// 过滤掉URL中特殊字符为合法参数
        /// </summary>
        /// <param name="paramValue">URL或者URL参数</param>
        /// <returns></returns>
        public static string FilterUrl(string paramValue)
        {
            if (paramValue.IsNullOrEmpty())
                return string.Empty;

            //替换数字之间的空格
            paramValue = Regex.Replace(paramValue, ReplaceNumberSpaceRegexFormat, "$1-$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //替换x数字格式为-x-数字的空格
            paramValue = Regex.Replace(paramValue, ReplaceMuliSpaceRegexFormat, "-$1-$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //替换数字之间的小数点
            paramValue = Regex.Replace(paramValue, ReplaceDataDotSpaceRegexFormat, "$1$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //替换单位的空格
            paramValue = Regex.Replace(paramValue, ReplaceUnitSpaceRegexFormat, "$1$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //替换特殊字符
            paramValue = Regex.Replace(paramValue, "([/]{1,2})|([-!！~`·&*#%^……《<>》{}（()）？?×'‘’,，.。:：\\s\\u005C\"【】\\[[\\]]+)", "-");

            //替换最后一个-
            return paramValue.TrimEnd('-').ToLower();
        }

        

        
        [Test]
        public void TestE()
        {
            var _encryptKey = "560AF6B1";
            var data = @"http://127.0.0.1/Order/RequestDownloadImage?orderDetailId=1&imageName=Z00177A.jpg";
            var encryptStr = CryptHelper.EncryptDes(data, _encryptKey);
            var decryptStr = CryptHelper.DecryptDes("ZAvCpEIHyPzOsm/vwp0IJ2ngzA3AzbBFDyzdeE+CnJoeYbWIHRBJvJQaTwkCl79Bf0siF5w+wOYKjKmMK/4bXWGYYGxyvYG4XNhPxXrLmy9FtmiRqbObTA==", _encryptKey);

            Console.WriteLine("原始数据:{0}", data);
            Console.WriteLine("加密数据:{0}", encryptStr);
            Console.WriteLine("解密数据:{0}", decryptStr);

            _encryptKey = "abc123456";
            encryptStr = CryptHelper.EncryptDes(data);
            decryptStr = CryptHelper.DecryptDes(encryptStr);

            Console.WriteLine("原始数据:{0}", data);
            Console.WriteLine("加密数据:{0}", encryptStr);
            Console.WriteLine("解密数据:{0}", decryptStr);

        }

        [Test]
        public void Testmd5()
        {
            var input = "0112345";
            var phpMd5 = "4ad4bdb7082864cf4cb5329e935c7b36"; 
            var md5Net = input.ToMd5();

            Console.WriteLine("PHP:{0}", phpMd5);
            Console.WriteLine("Net:{0}", md5Net);
        }


        [Test]
        public void TestFilterUrl()
        {
            var data = "(Grade B) Synthetic Jade Beads Round Green 8mm Dia,38cm（15\"） long,5 Strands(Approx：45PCs/Strand)";

            Console.WriteLine("{0}:{1}", "原始字符串", data);
            Console.WriteLine("{0}:{1}", "替换后字符串", FilterUrl(data));

            var data2 = "1,3,4,5,6";
            var list = data2.Split<int>(",");
            var list2 = data2.Split<decimal>(",");
            var list3 = data2.Split<float>("[,:;]");
        }

         [Test]
        public void TestShowText()
        {  
            var discount = 0.925M;
            discount = 1 - discount;
            Console.WriteLine(string.Format("{0:0.##}", Math.Abs(discount * 100)) + "% off");

            discount = 0.9251M;
            discount = 1 - discount;
            Console.WriteLine(string.Format("{0:0.##}", Math.Abs(discount * 100)) + "% off");


            discount = 0.9251765562M;
            discount = 1 - discount;
            Console.WriteLine(string.Format("{0:0.##}", Math.Abs(discount * 100)) + "% off");



            discount = 0.92M;
            discount = 1 - discount;
            Console.WriteLine(string.Format("{0:0.##}", Math.Abs(discount * 100)) + "% off");


            discount = 0.8M;
            discount = 1 - discount;
            Console.WriteLine(string.Format("{0:0.##}", Math.Abs(discount * 100)) + "% off");


            discount = 5M;
            discount = 1 - discount;
            Console.WriteLine(string.Format("{0:0.##}", Math.Abs(discount * 100)) + "% off");


            discount = 1M;
            discount = 1 - discount;
            Console.WriteLine(string.Format("{0:0.##}", Math.Abs(discount * 100)) + "% off");



            discount = 0M;
            discount = 1 - discount;
            Console.WriteLine(string.Format("{0:0.##}", Math.Abs(discount * 100)) + "% off");

        }
       
        [Test]
        public void TestReplace()
        {
            var data = "http://www.baidu.com";
            var scheme = "https";

            var url = Regex.Replace(data, "^(http[s]?)", scheme);

            Console.WriteLine(url);

            data = "https://www.163.com";
            scheme = "http";

            url = Regex.Replace(data, "^(http[s]?)", scheme);

            Console.WriteLine(url);
        }

        [Test]
        public void TestPox()
        {
            string PO_BOX_PATTERN = "^.*(hc|p).*box.*$";
            string address = "jofdhcBOxdfsfd";

            var isPObox = address.IsMatch(PO_BOX_PATTERN);
            Console.WriteLine(isPObox);

        }


        [Test]
        public void TestSplit()
        {
            string str = "11|A,113;9:42";
            var val1 = str.Split("|,:;");
            var val11 = str.SplitRegex("[|,:;]");
            IList<string> val2 = str.Split(new[] {'|',',',';',':'});

            foreach (var s in val1)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("-----------------------");
            foreach (var s in val11)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("-----------------------");

            foreach (var s in val2)
            {
                Console.WriteLine(s);
            }

        }

        [Test]
        public void TestDec()
        {
            var val = 25M*1.00M;
            var decimalval = Math.Round(val, 2);
            Console.WriteLine(decimalval);
            decimalval = Math.Round(25.00M, 2);
            Console.WriteLine(decimalval);
            decimalval = Math.Round(25.125487M, 2);
            Console.WriteLine(decimalval);
            decimalval = Math.Round(25.125487M, 0);
            Console.WriteLine(decimalval);
            decimalval = Math.Round(25M, 0);
            Console.WriteLine(decimalval);
            decimalval = decimal.Round(25M*1.00M, 2, MidpointRounding.AwayFromZero);
            Console.WriteLine(decimalval);
        }


        [Test]
        public void TestAA()
        {
            string aaa = @"D:\A";
            string bb = @"A\Mailtemp";

            var isAbsAaa = Directory.Exists(aaa);
            var isAbsBB = Directory.Exists(bb);
            Console.WriteLine(isAbsAaa);

            Console.WriteLine(isAbsBB);
        }


        [Test]
        public void TestUrlPathHelper()
        {
            var qustring = "http://localhost:53258/product/index?id=1970&mode=quick&p1=11&p2=222&p4=333";
            //var qustring = "";//"http://www.8seasons.com/wholesale-wood-buttons-c-691_2271.html?pcount=3&disp_order=30&p1=7924&p2=341&p3=175";
            var prefix = "p";

            var list = UrlPathHelper.GetQueryStringParamList<int>(qustring, prefix);

            var qs = UrlPathHelper.GetQueryString(list, prefix);


        }

        [Test]
        public void TesSolrPageRateHelper()
        {

            var rate = SolrPageRateHelper.GetDataRate(2, SolrRateType.BestSeller);
            var count = SolrPageRateHelper.GetDataCount(1, 90, SolrRateType.Promotion);
        }

        [Test]
        public void Test222()
        {

            var v1 =  string.Format("{0}{1}{2}", "USD ", 14M, string.Empty);
            var v2 = string.Format("{0}{1}{2}", "USD ", 14.00M, string.Empty);
            var v3 = string.Format("{0}{1:0.000}{2}", "USD ", 14M, string.Empty);
            var v4 = string.Format("{0}{1:0.000}{2}", "USD ", 14.00M, string.Empty);
        }



        [Test]
        public void TesSolrRateHelper()
        {

            var count = SolrRateHelper.GetDataCount(ProductSearchAreaType.MixProduct);

            var count2 = SolrRateHelper.GetDataCount(ProductSearchAreaType.Promotion);

            var count3 = SolrRateHelper.GetDataCount(ProductSearchAreaType.NormalArea);
        }

        [Test]
        public void TesProductSolrService()
        {
            var solrParam = new SolrQueryParam
            {
                //Keyword = "beads",
                //ProductIds = new[] { 23, 123, 213, 21312, 3122, 111 },
                //IgnoreProductIds = new[] { 888, 1111, 222, 444, 555, 666 },
                //Skus = new[] { "B00001", "YB00021", "CB00001", "YK90093" },
                //JoinDateFrom = DateTime.Now.AddDays(-30),
                //JoinDateTo = DateTime.Now.AddDays(-10),
                //SalePriceMinFrom = 5.50M,
                //SalePriceMinTo = 11.0M,
                //IsPromotionOn = true,
                //PromotionId = 13,
                //PromotionDiscountFrom = 0.5M,
                //PromotionDiscountTo = null,
                //ProductAreaId = 23,
                //IsInStock = true,
                //CategoryId = 123,
                //PropertyValueIds = new[] { 12, 523, 126, 422 },
                //PropertyValueGroupIds = new[] { 98, 143 },
                AreaType = ProductSearchAreaType.BestMatch, 
                IsStatisticsPropertyValue = true,
                IsStatisticsCategory = true,
                Sorts = new[]
                 {
                    ProductSorterCriteria.JoinDateNewToOld,
                    ProductSorterCriteria.LastModifyDate
                 }
            };
            ProductSolrService.SearchProduct(1, 10, solrParam);

        }

        [Test]
        public void TestMemcached()
        {
            var product = new Product
            {
                ProductId = 1,
                CategoryId = 11,
                ProductCode = "B12345",
                IsHot = false,
                CostPrice = 123.22M
            };

            var products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    CategoryId = 11,
                    ProductCode = "B12345",
                    IsHot = false,
                    CostPrice = 123.22M
                },
                new Product
                {
                    ProductId = 1,
                    CategoryId = 11,
                    ProductCode = "B12345",
                    IsHot = false,
                    CostPrice = 123.22M
                }
            };

            //下面是设置数据
            //单个产品
            MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, 1), product);

            //单个多语种产品信息,假设英语的语种ID = 1
            MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, 1, 1), product);

            //单个多语种产品信息,假设英语的语种Code = EN
            MemcachedHelper.Instance.Set(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, 1, "EN"), product);

            //所有产品信息 
            MemcachedHelper.Instance.Set(MemcachedConst.ToAllKey(MemcachedConst.KEY_PRODUCT), products);

            //所有德语产品信息 ,假设德语的语种ID = 2
            MemcachedHelper.Instance.Set(MemcachedConst.ToAllKey(MemcachedConst.KEY_PRODUCT, 2), products);

            //所有德语产品信息 ,假设德语的语种Code = DE
            MemcachedHelper.Instance.Set(MemcachedConst.ToAllKey(MemcachedConst.KEY_PRODUCT, "DE"), products);

            //下面是获取数据
            var pd = MemcachedHelper.Instance.Get<Product>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, 1));

            var pd2 = MemcachedHelper.Instance.Get<Product>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, 1, 1));

            var pd3 = MemcachedHelper.Instance.Get<Product>(MemcachedConst.ToObjectKey(MemcachedConst.KEY_PRODUCT, 1, "EN"));

            var pds1 = MemcachedHelper.Instance.Get<IList<Product>>(MemcachedConst.ToAllKey(MemcachedConst.KEY_PRODUCT), new List<Product>());

            var pds2 = MemcachedHelper.Instance.Get<IList<Product>>(MemcachedConst.ToAllKey(MemcachedConst.KEY_PRODUCT, 2), new List<Product>());

            var pds3 = MemcachedHelper.Instance.Get<IList<Product>>(MemcachedConst.ToAllKey(MemcachedConst.KEY_PRODUCT, "DE"), new List<Product>());

        }

        [Test]
        public void TestMemcached2()
        {
            var hashtable = MemcachedHelper.Instance.Stats();
            foreach (var item in hashtable)
            {

            }
        }


        [Test]
        public void TestMemcached4()
        {
            var vo = new PaypalInfo();
            vo.IsExpressCheckOut = true;

            var po = new PaymentLogPaypalPo();
            ObjectHelper.CopyProperties(vo, po, new[] { "" });

        }

        [Test]
        public void TestMemcached3()
        {
            var map1 = MemcachedHelper.Instance.GetAllSlabIds();

            var map = MemcachedHelper.Instance.GetAllCacheKeys();
        }

        [Test]
        public void TestGenerateCreditCard()
        {
            var visa = GenerateCreditCardNumber("Visa");
            var discover = GenerateCreditCardNumber("Discover");
            var masterCard = GenerateCreditCardNumber("MasterCard");
            var amex = GenerateCreditCardNumber("Amex");

            Console.WriteLine("visa:{0}", visa);
            Console.WriteLine("discover:{0}", discover);
            Console.WriteLine("masterCard:{0}", masterCard);
            Console.WriteLine("amex:{0}", amex);

        }

        public static string GenerateCreditCardNumber(string cardType)
        {
            int[] cc_number = new int[16];
            int cc_len = 16;
            int start = 0;
            Random r = new Random();
            switch (cardType)
            {
                case "Visa":
                    cc_number[start++] = 4;
                    break;
                case "Discover":
                    cc_number[start++] = 6;
                    cc_number[start++] = 0;
                    cc_number[start++] = 1;
                    cc_number[start++] = 1;
                    break;
                case "MasterCard":
                    cc_number[start++] = 5;
                    cc_number[start++] = (int)Math.Floor(r.NextDouble() * 5) + 1;
                    break;
                case "Amex":
                    cc_number = new int[15];
                    cc_number[start++] = 3;
                    cc_number[start++] = r.Next(2) == 1 ? 7 : 4;
                    cc_len = 15;
                    break;
            }

            for (int i = start; i < (cc_len - 1); i++)
            {
                cc_number[i] = (int)Math.Floor(r.NextDouble() * 10);
            }

            int sum = 0;
            for (int j = 0; j < (cc_len - 1); j++)
            {
                int digit = cc_number[j];
                if ((j & 1) == (cc_len & 1)) digit *= 2;
                if (digit > 9) digit -= 9;
                sum += digit;
            }

            int[] check_digit = new int[] { 0, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            cc_number[cc_len - 1] = check_digit[sum % 10];

            string result = string.Empty;
            foreach (int digit in cc_number)
            {
                result += digit;
            }
            return result;
        }

        [Test]
        public void EmailTemplate()
        {
            var template = @"<html><head>
    <meta content='text/html; charset=utf-8' http-equiv='Content-Type'>
    <title></title>
    <link type='text/css' rel='stylesheet' href='/Css/base_1.0_1.1.5.css'>
    <script type='text/javascript' src='/Js/base_1.0_1.2.js'></script>
    <script type='text/javascript' src='/Js/8seasons_1.0_1.3.js'></script>
</head>
<body>
     
<div id='header'>
    <img border='0' src='http://www.baidu.com/img/bdlogo.gif' alt='' title=''>
    头部  
</div>

    <div class='Content'>
        <div class='MainContent'>
    <div class='Banner'>
        <div class='PageBanner'>
            <div class='BgBannerMenu'>
                <ul>
                    <li><a href='#'>Hi,[$name$]</a></li> 
                    <li><a href='#'>[$new_arrail$]</a></li> 
                    <li><a href='' class='aaa'>Mid-autumn-Day</a></li> 
                    <li><a href='' class='aaa'>NewYear</a></li> 
                    <li><a id='lnkChristmas' href='/Christmas-Day.html?CategoryName=aaaa&amp;ProductName=fff&amp;pid=123' class='aaa'>Christmas-Day</a></li> 
                    <li><a id='lnkObj' href='/aaaa-fff-123.html' class='aaa'>Click Here</a></li> 
                </ul>
            </div>
        </div> 
        <div class='Clear'>
        </div>
    </div> 
</div> 
<div class='Clear'>
</div>

    </div>
    <div class='WarpFooter'>
</div>
<div id='Footer'>
     尾部
</div> 
    


</body></html>";

            var bb = MailTemplateHelper.RemoveTag(template, "[$name$]");
            var aa = MailTemplateHelper.GetTagValue(template, "[$name$]");
            var cc = MailTemplateHelper.ReplaceTagValue(template, @"(\[\$\*?\$\])", "jack");
            var dd = MailTemplateHelper.ReplaceTemplateTag(template, new Dictionary<string, string>
            {
                {"[$name$]","jack"},
                {"[$new_arrail$]","What New?"}
            });

            Console.WriteLine(aa);
            Console.WriteLine("-------------End GetTagValue-----------------");
            Console.WriteLine(bb);
            Console.WriteLine("-------------End RemoveTag-----------------");
            Console.WriteLine(cc);
            Console.WriteLine("-------------End ReplaceTagValue--------------");
            Console.WriteLine(dd);
        }
    }
}
