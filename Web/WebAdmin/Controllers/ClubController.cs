using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Service.Customer.Club;
using Com.Panduo.Service.Payment.PayInfo;
using Com.Panduo.Web.Common;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;

namespace Com.Panduo.Web.Controllers
{
    public class ClubController : Controller
    {
        #region Club
        [HttpGet]
        public ActionResult ClubCustomer()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ClubCustomerList(string customerEmail, string clubManager, int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<ClubCustomerSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<ClubCustomerSorterCriteria>>();
            if (!customerEmail.IsNullOrEmpty())
                searchCriteria.Add(ClubCustomerSearchCriteria.CustomerEmail, customerEmail);
            if (!clubManager.IsNullOrEmpty() && clubManager != "0")
                searchCriteria.Add(ClubCustomerSearchCriteria.ClubManager, clubManager);
            var clubCustomer = ServiceFactory.ClubService.FindClubCustomerView(page, pageSize, searchCriteria, sorterCriteria);
            ViewBag.Page = page;
            return View(clubCustomer);
        }

        [HttpGet]
        public ActionResult ClubCustomerDetail(int id, int page)
        {
            ViewBag.Page = page;
            var clubCustomerView = ServiceFactory.ClubService.GetClubCustomerView(id);
            var paypalInfo = ServiceFactory.PaymentService.GetPaypalInfo(clubCustomerView.PayLogId);
            ViewBag.ClubPaymengInfo = paypalInfo.IsNullOrEmpty() ? new PaypalInfo() : paypalInfo;
            return View(clubCustomerView);
        }

        [HttpGet]
        public ActionResult ClubCustomerEdit(int id)
        {
            var clubCustomerView = ServiceFactory.ClubService.GetClubCustomerView(id);
            return View(clubCustomerView);
        }

        [HttpPost]
        public ActionResult ClubCustomerEdit(int id, int customerManagerId)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            ServiceFactory.ClubService.SetClubCustomerManager(id, customerManagerId, 9999);
            return Json(hashtable);
        }

        [HttpGet]
        public FileResult ExportExcel(string customerEmail, string clubManager)
        {
            const int page = 1;
            const int pageSize = 100000;
            var searchCriteria = new Dictionary<ClubCustomerSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<ClubCustomerSorterCriteria>>();
            if (!customerEmail.IsNullOrEmpty())
                searchCriteria.Add(ClubCustomerSearchCriteria.CustomerEmail, customerEmail);
            if (!clubManager.IsNullOrEmpty() && clubManager != "0")
                searchCriteria.Add(ClubCustomerSearchCriteria.ClubManager, clubManager);

            var list = ServiceFactory.ClubService.FindClubCustomerView(page, pageSize, searchCriteria, sorterCriteria).Data;

            var stream = GenerateExcel(list).GetBuffer();
            var file = "Packing list - " + ServiceFactory.ConfigureService.SiteLanguageCode.ToLower() + ".xls";
            return File(stream, "application/vnd.ms-excel", file);
        }
        #endregion

        #region Club黑名单

        [HttpGet]
        public ActionResult ClubBlackList()
        {
            string clubBlackList = "";
            clubBlackList = ServiceFactory.ClubService.GetAllClubBlackList().Aggregate(clubBlackList, (current, m) => current + m.CustomerEmail + ",");
            if (!clubBlackList.IsEmpty())
            {
                clubBlackList = clubBlackList.Substring(0, clubBlackList.Length - 1);
            }
            ViewBag.ClubBlackList = clubBlackList;
            return View();
        }

        [HttpPost]
        public ActionResult ClubBlackList(string blacklists)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };
            if (!blacklists.IsNullOrEmpty())
            {
                var clubBlackArr = blacklists.Split(",").Select(m => m.Trim());

                var clubBlackLists = new List<ClubBlackList>();
                string invalidEmails = "";
                //过滤无用邮箱
                foreach (string blackEmail in clubBlackArr)
                {
                    if (!ServiceFactory.CustomerService.GetCustomerByEmail(blackEmail).IsNullOrEmpty())
                    {
                        if (!ServiceFactory.ClubService.GetClubBlackList(blackEmail).IsNullOrEmpty())
                        {
                            ServiceFactory.ClubService.DeleteClubBlackList(blackEmail);
                        }
                        clubBlackLists.Add(new ClubBlackList { CustomerEmail = blackEmail });
                    }
                    else
                    {
                        invalidEmails += blackEmail + ",";
                    }
                }
                if (!invalidEmails.IsEmpty())
                {
                    invalidEmails = invalidEmails.Substring(0, invalidEmails.Length - 1);
                    hashtable["msg"] = "您输入的邮箱" + invalidEmails + "在系统中不存在!";
                    hashtable["error"] = true;
                }

                ServiceFactory.ClubService.SetClubBlackList(clubBlackLists);
                string clubBlackList = "";
                clubBlackList = ServiceFactory.ClubService.GetAllClubBlackList().Aggregate(clubBlackList, (current, m) => current + m.CustomerEmail + ",");
                if (!clubBlackList.IsEmpty())
                {
                    clubBlackList = clubBlackList.Substring(0, clubBlackList.Length - 1);
                }
                hashtable["blacklist"] = clubBlackList;
            }
            else
            {
                hashtable["msg"] = "客户邮箱不允许为空";
                hashtable["error"] = true;
            }
            return Json(hashtable);
        }

        #endregion

        #region Manager
        [HttpGet]
        public ActionResult Manager()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManagerEdit(int id)
        {
            var customerManager = ServiceFactory.CustomerService.GetCustomerManager(id);
            if (customerManager.IsNullOrEmpty())
                customerManager = new CustomerManager();
            return View(customerManager);
        }

        [HttpPost]
        public ActionResult ManagerEdit(CustomerManager customerManager)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false }, { "getlist", true } };

            HttpPostedFileBase file = Request.Files["avatar"];

            ServiceFactory.CustomerService.EditCustomerManager(customerManager);

            return Json(hashtable);
        }

        [HttpGet]
        public ActionResult ManagerList(string name, int page = 1, int pageSize = 20)
        {
            var searchCriteria = new Dictionary<ManagerSearchCriteria, object>();
            if (!name.IsNullOrEmpty())
            {
                searchCriteria.Add(ManagerSearchCriteria.Name, name);
            }
            return View(ServiceFactory.CustomerService.FindAllManager(page, pageSize, searchCriteria, null));
        }

        [HttpPost]
        public ActionResult DeleteManager(int id)
        {
            var hashtable = new Hashtable { { "msg", string.Empty }, { "error", false } };

            ServiceFactory.CustomerService.DeleteCustomerManager(id);

            return Json(hashtable);
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 加载Excel
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private IWorkbook LoadExcel(string fullPath)
        {
            var file = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var hssfworkbook = new HSSFWorkbook(file);
            file.Close();
            file.Dispose();
            return hssfworkbook;
        }


        private MemoryStream WriteToStream(IWorkbook workbook)
        {
            var file = new MemoryStream();
            workbook.Write(file);
            file.Close();
            file.Dispose();
            return file;
        }

        private MemoryStream GenerateExcel(IList<ClubCustomerView> sourceList)
        {
            var workBook = LoadExcel(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "8seasonsclub.xls"));
            var sheet = workBook.GetSheet("club客户收款汇总-年-月");

            var rowCount = sourceList.Count;
            const int columnCount = 6;
            var templetRow = sheet.GetRow(1);
            for (var i = 2; i < rowCount + 2; i++)
            {
                var row = sheet.CreateRow(i);

                row.Height = templetRow.Height;

                for (var j = 0; j < columnCount; j++)
                {
                    var column = row.CreateCell(j);
                    var orginalCell = templetRow.GetCell(j);
                    column.CellStyle = CopyCellStyle(workBook, orginalCell.CellStyle);//行样式
                }
            }

            //设置行信息
            var positionIndex = 2;

            #region 设置默认超连接的样式，蓝色带下划线
            ICellStyle hlinkStyle = workBook.CreateCellStyle();
            IFont hlinkFont = workBook.CreateFont();
            hlinkFont.Underline = FontUnderlineType.Single;
            hlinkFont.Color = HSSFColor.Blue.Index;
            hlinkStyle.SetFont(hlinkFont);
            hlinkStyle.Alignment = HorizontalAlignment.Center;
            hlinkStyle.VerticalAlignment = VerticalAlignment.Center;
            #endregion

            foreach (var item in sourceList)
            {
                sheet.GetRow(positionIndex).GetCell(0).SetCellValue(item.CustomerId);
                sheet.GetRow(positionIndex).GetCell(1).SetCellValue(item.Fee.ToString(CultureInfo.InvariantCulture));
                sheet.GetRow(positionIndex).GetCell(2).SetCellValue(item.BeginDate);
                sheet.GetRow(positionIndex).GetCell(3).SetCellValue(item.ManagerName);
                sheet.GetRow(positionIndex).GetCell(4).SetCellValue(item.TransactionId);
                sheet.GetRow(positionIndex).GetCell(5).SetCellValue(item.Website);
                positionIndex++;
            }
            return WriteToStream(workBook);
        }
        private static ICellStyle CopyCellStyle(IWorkbook wb, ICellStyle orginalCellStyle)
        {
            var style = wb.CreateCellStyle();
            style.CloneStyleFrom(orginalCellStyle);

            return style;
        }
        #endregion
    }
}
