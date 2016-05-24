<%@ WebHandler Language="C#" Class="CutAvatarHandler" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using Resources;

public class CutAvatarHandler : IHttpHandler, IRequiresSessionState
{

    [WebMethod(EnableSession = true)]
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Charset = "utf-8";
        System.IO.Stream stream = null;
        System.Drawing.Image originalImg = null;   //原图
        System.Drawing.Image thumbImg = null;      //缩放图       


        try
        {
            int minWidth = 75;   //最小宽度
            int minHeight = 75;  //最小高度
            int maxWidth = 300;  //最大宽度
            int maxHeight = 300;  //最大高度

            string resultTip = string.Empty;  //返回信息

            HttpPostedFile file = context.Request.Files["Filedata"];      //上传文件      

            string uploadPath = HttpContext.Current.Server.MapPath("/ImagesAvator/Temp");  //得到上传路径

            string lastImgUrl = @context.Request.Params["LastImgUrl"];

            if (!string.IsNullOrEmpty(lastImgUrl))
            {
                PubClass.FileDel(HttpContext.Current.Server.MapPath(lastImgUrl));
            }

            if (file != null)
            {
                if (!System.IO.Directory.Exists(uploadPath))
                {
                    System.IO.Directory.CreateDirectory(uploadPath);
                }

                string ext = System.IO.Path.GetExtension(file.FileName).ToLower();   //上传文件的后缀（小写）

                if (ext == ".jpg" || ext == ".png")
                {
                    string flag = "ThumbNail" + DateTime.Now.ToFileTime() + ext;

                    string uploadFilePath = uploadPath + "\\" + flag;   //缩放图文件路径

                    stream = file.InputStream;

                    originalImg = System.Drawing.Image.FromStream(stream);

                    if (originalImg.Width > minWidth && originalImg.Height > minHeight)
                    {
                        thumbImg = PubClass.GetThumbNailImage(originalImg, maxWidth, maxHeight);  //按宽、高缩放

                        if (thumbImg.Width > minWidth && thumbImg.Height > minWidth)
                        {
                            thumbImg.Save(uploadFilePath);

                            resultTip = "/ImagesAvator/Temp" + "\\" + flag + "$" + thumbImg.Width + "$" + thumbImg.Height;
                        }
                        else
                        {
                            resultTip = Lang.ErrorImageFormat;
                        }
                    }
                    else
                    {
                        resultTip = Lang.ErrorImageSize + minWidth + "*" + minHeight;
                    }
                }
            }
            else
            {
                resultTip = Lang.ErrorImageEmpty;
            }

            context.Response.Write(resultTip);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            if (originalImg != null)
            {
                originalImg.Dispose();
            }

            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }

            if (thumbImg != null)
            {
                thumbImg.Dispose();
            }

            GC.Collect();
        }


    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}