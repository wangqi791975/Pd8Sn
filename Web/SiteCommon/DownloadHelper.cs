using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Com.Panduo.Web.Common
{
    public static class DownloadHelper
    {

        public static byte[] ReadFileData(string fileName)
        {
            byte[] pReadByte = new byte[0];
            FileStream pFileStream = null;
            BinaryReader br = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(pFileStream);
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                pReadByte = br.ReadBytes((int)br.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (br != null)
                {
                    br.Close();
                }
                if (pFileStream != null)
                {
                    pFileStream.Close();
                    pFileStream.Dispose();
                }
            }
        }



        public static void DownloadByFile(string sourceFilePath, string saveFileName)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Buffer = true;
            //response.Charset = "GB2312";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(saveFileName));
            response.ContentEncoding = System.Text.Encoding.UTF8;
            //response.ContentType = "application/octet-stream";
            response.ContentType = GetContentType(saveFileName);
            response.WriteFile(sourceFilePath);
            response.End();
        }

        public static void DownloadByByte(byte[] data, string saveFileName)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Buffer = true;
            //response.Charset = "GB2312";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(saveFileName));
            // response.ContentType = "application/octet-stream";
            response.ContentType = GetContentType(saveFileName);
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.BinaryWrite(data);
            response.AppendHeader("Content-Length", data.Length.ToString());
            response.End();
        }

        private static string GetContentType(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".zip")
            {
                return "application/zip";
            }
            else if (ext == ".xls" || ext == ".xlsx")
            {
                return "application/x-excel";
            }
            else if (ext == ".doc" || ext == ".docx")
            {
                return "application/msword";
            }
            else if (ext == ".pdf")
            {
                return "application/pdf";
            }
            else if (ext == ".jpg" || ext == ".jpeg")
            {
                return "image/jpeg";
            }
            else if (ext == ".gif")
            {
                return "image/gif";
            }
            else if (ext == ".png")
            {
                return "image/png";
            }
            return "text/html";
        }
    }
}
