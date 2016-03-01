using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml;
using System.Text;

namespace AngularCartAdmin
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string _dir = context.Request.Params["dir"];

            if ((_dir == null) || (_dir == "undefined"))
            {
                _dir = "ac_products/";
            }
            else
            {
                _dir = "ac_products/" + _dir + "/";
            }

            var result = "0";
            try
            {
                if (context.Request.Files.Count > 0)
                {
                    //string path, actual_path;
                    //context.Response.ContentType = "text/plain";
                    //HttpPostedFile uploadFiles = context.Request.Files["Filedata"];
                    //string pathToSave = HttpContext.Current.Server.MapPath("~/UploadFiles/") + uploadFiles.FileName;
                    //uploadFiles.SaveAs(pathToSave);

                    HttpPostedFile file = null;

                    for (int i = 0; i < context.Request.Files.Count; i++)
                    {
                        file = context.Request.Files[i];
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            //var path = Path.Combine("uploads", fileName);
                            string savepath = context.Server.MapPath(_dir);
                            if (!Directory.Exists(savepath))
                                Directory.CreateDirectory(savepath);

                            file.SaveAs(savepath + fileName);
                            result = "1";
                        }
                    }

                }
            }
            catch { }
            context.Response.Write(result);
        }


        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    ProcessRequest();
        //}
        //public void ProcessRequest(HttpContext context)
        //{
        //    //context.Response.ContentType = "text/plain";
        //    //context.Response.Write("Hello World");

        //    Response.Expires = -1;
        //    try
        //    {
        //        //var db = new mypicksDBDataContext();
        //        HttpPostedFile postedFile = Request.Files["photos"];
        //        string savepath = Server.MapPath("uploads/images/");
        //        string filename = postedFile.FileName;
        //        if (!Directory.Exists(savepath))
        //            Directory.CreateDirectory(savepath);
        //        postedFile.SaveAs(savepath + filename);
        //        Response.Write("upload");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.ToString());

        //    }
        //}    


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }


    //public class AjaxFileUploader : IHttpHandler
    //{
    //    public void ProcessRequest(HttpContext context)
    //    {
    //        if (context.Request.Files.Count > 0)
    //        {
    //            string path = context.Server.MapPath("~/Files");
    //            if (!Directory.Exists(path))
    //                Directory.CreateDirectory(path);
    //            var file = context.Request.Files[0];
    //            string fileName;
    //            if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
    //            {
    //                string[] files = file.FileName.Split(new char[] { '\\' });
    //                fileName = files[files.Length - 1];
    //            }
    //            else
    //            {
    //                fileName = file.FileName;
    //            }
    //            string strFileName = fileName;
    //            fileName = Path.Combine(path, fileName);
    //            file.SaveAs(fileName);
    //            string msg = "{";
    //            msg += string.Format("error:'{0}',\n", string.Empty);
    //            msg += string.Format("msg:'{0}',\n", strFileName);
    //            msg += string.Format("path:'{0}'", path.Replace("\\", "?"));
    //            msg += "}";
    //            context.Response.Write(msg);
    //        }
    //    }
    //    public bool IsReusable
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //    }
    //}

}