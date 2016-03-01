using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Newtonsoft.Json;
using System.Web.Services;
using System.IO;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Collections;
using System.Data;



namespace AngularCartAdmin
{

    //<%@ Page Language = "vb" AutoEventWireup="false" Codebehind="MyForm.aspx.vb" Inherits="Proj.MyForm" ValidateRequest="false"%>
    /// <summary>
    /// Summary description for crud
    /// </summary>
    public class crud : IHttpHandler
    {
        string methodname = string.Empty;
        string callbackmethodname = string.Empty;
        string parameter = string.Empty;

        private static readonly string connACartLocal = String.Format(GetConnectionString("localCartConnectionString"), Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data"));
        private static readonly string connACartRemote = GetConnectionString("remoteCartConnectionString");
        
        Product dbProduct = new Product();
        [ConfigurationPropertyAttribute("requestPathInvalidCharacters", DefaultValue = "<,>,*,%,&,:,\\,?")]

        public string RequestPathInvalidCharacters { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string _ac = context.Request.Params["ac"];
            string _cn = context.Request.Params["cn"];

            //$.getJSON("/crud.ashx?ac=save2file&cn=products")

            if (_cn.ToLower() == "local")
            {
                _cn = connACartLocal;
            }
            else if (_cn.ToLower() == "remote")
            {
                _cn = connACartRemote;
            }
            else if (_cn.ToLower() == "products")
            {
                //_cn = connACartRemote;
            }

            switch (_ac.ToLower())
            {
                case "getproducts":
                    List<Product> _result = GetProducts(context, _cn);
                    context.Response.Write(ToJson(_result));
                    break;
                case "save2file":
                    string s = Save2File(context);
                    context.Response.Write(ToJson(s));
                    break;
                case "update":
                    //"/crud.ashx?ac=update&cn=id"
                    string s2 = Update(context, _cn);
                    context.Response.Write(ToJson(s2));
                    break;
                case "getbyid":
                    //context.Response.Write(GetById(int.Parse(context.Request.Params["Id"])));
                    break;
                case "insert":
                    context.Response.Write(Insert(context));
                    break;
                case "delete":
                    //context.Response.Write(Delete(int.Parse(context.Request.Params["Id"])));
                    string sDelete = Delete(context, _cn);
                    context.Response.Write(ToJson(sDelete));
                    break;
            }
        }

        protected string ToJson(object target)
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            return s.Serialize(target);
        }

        public List<Product> Deserialize<T>(HttpContext context)
        {
            //read the json string
            string jsonData = new StreamReader(context.Request.InputStream).ReadToEnd();

            //cast to specified objectType
            //var obj = (T)new JavaScriptSerializer().Deserialize<T>(jsonData);
            var obj = new JavaScriptSerializer().Deserialize<List<Product>>(jsonData);

            //return the object
            return obj;
        }

        public Product Deserialize2<Product>(HttpContext context)
        {
            //read the json string
            string jsonData = new StreamReader(context.Request.InputStream).ReadToEnd();

            //cast to specified objectType
            //var obj = new JavaScriptSerializer().Deserialize<T>(jsonData);
            var obj = new JavaScriptSerializer().Deserialize<Product>(jsonData);

            //return the object
            return obj;
        }

        public static List<Product> GetProducts(HttpContext context, string cn)
        {
            List<Product> list = new List<Product>();
            const string SQL = "SELECT  [productid]                                                         " +
                                "       ,[sku]                                                              " +
                                "       ,[productname]                                                      " +
                                "       ,[storeid]                                                          " +
                                "       ,[categoryname]                                                     " +
                                "       ,[header]                                                           " +
                                "       ,[shortdesc]                                                        " +
                                "       ,[description]                                                      " +
                                "       ,[link]                                                             " +
                                "       ,[linktext]                                                         " +
                                "       ,[imageurl]                                                         " +
                                "       ,[imagename]                                                        " +
                                "       ,[carousel]                                                         " +
                                "       ,[carousel_caption]                                                 " +
                                "       ,[tube]                                                             " +
                                "       ,[videoid]                                                          " +
                                "       ,[showvideo]                                                        " +
                                "       ,[unitprice]                                                        " +
                                "       ,[saleprice]                                                        " +
                                "       ,[unitsinstock]                                                     " +
                                "       ,[unitsonorder]                                                     " +
                                "       ,[reorderlevel]                                                     " +
                                "       ,[expecteddate]                                                     " +
                                "       ,[discontinued]                                                     " +
                                "       ,[notes]                                                            " +
                                "       ,[faux]                                                             " +
                                "       ,[sortorder]                                                        " +
                                " FROM [Product] WHERE ([storeid] = '{0}') " +
                                " ORDER BY [sortorder]";

            List <string> products = new List<string>();

            using (IDbConnection cnn = CreateConnection(cn))
            {
                using (IDbCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = string.Format(SQL, "7cc6cb94-0938-4675-b84e-6b97ada53978");

                    using (IDataReader rdr = cmd.ExecuteReader())
                    {
                        SmartDataReader sdr = new SmartDataReader(rdr);

                        while (sdr.Read())
                        {
                            Product f = new Product();
                            f.productid = sdr.GetString("productid");
                            f.sku = sdr.GetString("sku");
                            f.productname = sdr.GetString("productname");
                            f.storeid = sdr.GetString("storeid");
                            f.categoryname = sdr.GetString("categoryname");
                            f.header = sdr.GetString("header");
                            f.shortdesc = sdr.GetString("shortdesc");
                            f.description = sdr.GetString("description");
                            f.link = sdr.GetString("link");
                            f.linktext = sdr.GetString("linktext");
                            f.imageurl = sdr.GetString("imageurl");
                            f.imagename = sdr.GetString("imagename");
                            f.carousel = sdr.GetBoolean("carousel");
                            f.carousel_caption = sdr.GetString("carousel_caption");
                            f.tube = sdr.GetString("tube");
                            f.videoid = sdr.GetString("videoid");
                            f.showvideo = sdr.GetBoolean("showvideo");
                            f.unitprice = sdr.GetFloat("unitprice");
                            f.saleprice = sdr.GetFloat("saleprice");
                            f.unitsinstock = sdr.GetInt32("unitsinstock");
                            f.unitsonorder = sdr.GetInt32("unitsonorder");
                            f.reorderlevel = sdr.GetInt32("reorderlevel");
                            f.expecteddate = sdr.GetDateTime("expecteddate");
                            f.discontinued = sdr.GetBoolean("discontinued");
                            f.notes = sdr.GetString("notes");
                            f.faux = sdr.GetBoolean("faux");
                            f.sortorder = sdr.GetInt32("sortorder");
                            //f.publishedDate = sdr.GetDateTime("publishedDate");
                            //f.rank = sdr.GetInt32("rank");
                            //f.beginDate = sdr.GetDateTime("beginDate");
                            //f.endDate = sdr.GetDateTime("endDate");
                            list.Add(f);
                        }
                    }
                }
            }

            if (list.Count == 0)
            {
                return null;
            }

            return list;

        }

        public string Update(HttpContext context, string cn)
        {
            string s1 = string.Empty;
            string _return = string.Empty;

            string jsonData = new StreamReader(context.Request.InputStream).ReadToEnd();

            JavaScriptSerializer jss = new JavaScriptSerializer();

            var objProduct = jss.Deserialize<Product>(jsonData);

            //"{\n    \"productid\": \"F5D189EC-2020-46BE-B4DE-82C53A43F088\",\n    \"sku\": \"DJI_Phantom_1_1_1_Quadcopter_GoPro_Mount\",\n    \"productname\": \"DJI Phantom 1.1.1 Quadcopter with GoPro Mount\",\n    \"storeid\": \"7cc6cb94-0938-4675-b84e-6b97ada53978\",\n    \"categoryname\": \"drone\",\n    \"header\": \"DJI Phantom 1.1.1 Quadcopter with GoPro Mount\",\n    \"shortdesc\": \"Includes FREE<br />▪ &nbsp;Watson 4-Hour Rapid Charger with 4 AA NiMH Rechargeable Batteries (2300mAh)&nbsp;<br />▪ &nbsp;Watson RC LiPo Battery with XT60 Connector for DJI Phantom Quadcopter\",\n    \"description\": \"MFR # CP.PT.000036<br />▪ &nbsp;Watson 4-Hour Rapid Charger with 4 AA NiMH Rechargeable Batteries (2300mAh)&nbsp;<br />▪ &nbsp;Watson RC LiPo Battery with XT60 Connector for DJI Phantom Quadcopter<br />▪ &nbsp;8\\\" Self-Tightening Propeller Blades<br />▪ &nbsp;Add Exciting Aerial Shots to Your Movies<br />▪ &nbsp;Mount is Compatible with GoPro Cameras<br />▪ &nbsp;Integrated GPS Flight Control<br />▪ &nbsp;Naza-M V2 Autopilot with Failsafe<br />▪ &nbsp;Max Horizontal Flight Speed of 10m/s<br />▪ &nbsp;Max Vertical Flight Speed of 6m/s<br />▪ &nbsp;Intelligent Orientation Control (IOC)<br />▪ &nbsp;Supports Dual Flight Control Modes<br />▪ &nbsp;LED Indicators &amp; Low Voltage Protection\",\n    \"link\": \"\",\n    \"linktext\": \"xxx\",\n    \"imageurl\": null,\n    \"imagename\": \"DJI_Phantom_1_1_1_Quadcopter_GoPro_Mount.png\",\n    \"carousel\": true,\n    \"carousel_caption\": \"DJI Phantom Quadcopter with GoPro Mount\",\n    \"tube\": \"youtube\",\n    \"videoid\": \"\",\n    \"showvideo\": false,\n    \"unitprice\": 349,\n    \"saleprice\": 0,\n    \"unitsinstock\": null,\n    \"unitsonorder\": null,\n    \"reorderlevel\": null,\n    \"expecteddate\": null,\n    \"discontinued\": null,\n    \"notes\": \"\",\n    \"faux\": null,\n    \"sortorder\": 1\n}"

            if (objProduct != null)
            {
                Hashtable htParams = new Hashtable();
                htParams.Add("@strProductID", objProduct.productid);
                htParams.Add("@sku", objProduct.sku);
                htParams.Add("@productname", objProduct.productname);
                htParams.Add("@strStoreID", objProduct.storeid);
                htParams.Add("@categoryname", objProduct.categoryname);
                htParams.Add("@header", objProduct.header);
                htParams.Add("@shortdesc", objProduct.shortdesc);
                htParams.Add("@description", objProduct.description);
                htParams.Add("@link", objProduct.link);
                htParams.Add("@linktext", objProduct.linktext);
                htParams.Add("@imageurl", objProduct.imageurl);
                htParams.Add("@imagename", objProduct.imagename);
                htParams.Add("@carousel", objProduct.carousel);
                htParams.Add("@carousel_caption", objProduct.carousel_caption);
                htParams.Add("@tube", objProduct.tube);
                htParams.Add("@videoid", objProduct.videoid);
                htParams.Add("@showvideo", objProduct.showvideo);
                htParams.Add("@unitprice", objProduct.unitprice);
                htParams.Add("@saleprice", objProduct.saleprice);
                htParams.Add("@unitsinstock", objProduct.unitsinstock);
                htParams.Add("@unitsonorder", objProduct.unitsonorder);
                htParams.Add("@reorderlevel", objProduct.reorderlevel);
                htParams.Add("@expecteddate", objProduct.expecteddate);
                htParams.Add("@discontinued", objProduct.discontinued);
                htParams.Add("@notes", objProduct.notes);
                htParams.Add("@faux", objProduct.faux);
                htParams.Add("@sortorder", objProduct.sortorder);

                QuickDL.SqlUtil sqlRun = new QuickDL.SqlUtil();
                sqlRun.ZConnStr = cn;

                try
                {
                    int iReturn = sqlRun.ExecSPReturnIntValue("usp_ProductUpdate", htParams);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            else
            {
                //context.Response.Write("Sorry something goes wrong.");
            }

            return "success!";
        }

        public static string Save2File(HttpContext context)
        {
            string s1 = string.Empty;
            string _return = string.Empty;

            //set the content type, you can either return plain text, html text, or json or other type    
            context.Response.ContentType = "text/plain";
            var result = "error: failed to save to text file!";
            try
            {
                //string jsonData = new StreamReader(context.Request.InputStream).ReadToEnd();
                var fileName = "products.js";

                //string targetDirectory = Path.Combine(context.Request.PhysicalApplicationPath, context.Request[\"ac_products\"].Replace(\"/\",\"\"));
                string targetDirectory = context.Server.MapPath("ac_products/");
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                //string targetFilePath = Path.Combine(targetDirectory, file.FileName);	
                string targetFilePath = targetDirectory + fileName;

                long nLen = context.Request.InputStream.Length;
                if (nLen > 10)
                {
                    using (Stream file = File.Create(targetFilePath))
                    {
                        CopyStream(context.Request.InputStream, file);
                    }
                    result = "success: saved to text file!";
                }
                else
                {
                    result = "Can't save EMPTY FILE!";
                }
            }
            catch { }

            //context.Response.Write(result);
            return result;
        }


        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        /// <summary>
        /// This function will take httpcontext object and will read the input stream
        /// It will use the built in JavascriptSerializer framework to deserialize object based given T object type value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        //public T Deserialize<T>(HttpContext context)
        //{
        //    //read the json string
        //    string jsonData = new StreamReader(context.Request.InputStream).ReadToEnd();

        //    //cast to specified objectType
        //    var obj = (T)new JavaScriptSerializer().Deserialize<T>(jsonData);

        //    //return the object
        //    return obj;
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////

        public string Delete(HttpContext context, string cn)
        {
            string s1 = string.Empty;
            string _return = string.Empty;

            string jsonData = new StreamReader(context.Request.InputStream).ReadToEnd();

            JavaScriptSerializer jss = new JavaScriptSerializer();

            var objProduct = jss.Deserialize<Product>(jsonData);

            //"{\n    \"productid\": \"F5D189EC-2020-46BE-B4DE-82C53A43F088\",\n    \"sku\": \"DJI_Phantom_1_1_1_Quadcopter_GoPro_Mount\",\n    \"productname\": \"DJI Phantom 1.1.1 Quadcopter with GoPro Mount\",\n    \"storeid\": \"7cc6cb94-0938-4675-b84e-6b97ada53978\",\n    \"categoryname\": \"drone\",\n    \"header\": \"DJI Phantom 1.1.1 Quadcopter with GoPro Mount\",\n    \"shortdesc\": \"Includes FREE<br />▪ &nbsp;Watson 4-Hour Rapid Charger with 4 AA NiMH Rechargeable Batteries (2300mAh)&nbsp;<br />▪ &nbsp;Watson RC LiPo Battery with XT60 Connector for DJI Phantom Quadcopter\",\n    \"description\": \"MFR # CP.PT.000036<br />▪ &nbsp;Watson 4-Hour Rapid Charger with 4 AA NiMH Rechargeable Batteries (2300mAh)&nbsp;<br />▪ &nbsp;Watson RC LiPo Battery with XT60 Connector for DJI Phantom Quadcopter<br />▪ &nbsp;8\\\" Self-Tightening Propeller Blades<br />▪ &nbsp;Add Exciting Aerial Shots to Your Movies<br />▪ &nbsp;Mount is Compatible with GoPro Cameras<br />▪ &nbsp;Integrated GPS Flight Control<br />▪ &nbsp;Naza-M V2 Autopilot with Failsafe<br />▪ &nbsp;Max Horizontal Flight Speed of 10m/s<br />▪ &nbsp;Max Vertical Flight Speed of 6m/s<br />▪ &nbsp;Intelligent Orientation Control (IOC)<br />▪ &nbsp;Supports Dual Flight Control Modes<br />▪ &nbsp;LED Indicators &amp; Low Voltage Protection\",\n    \"link\": \"\",\n    \"linktext\": \"xxx\",\n    \"imageurl\": null,\n    \"imagename\": \"DJI_Phantom_1_1_1_Quadcopter_GoPro_Mount.png\",\n    \"carousel\": true,\n    \"carousel_caption\": \"DJI Phantom Quadcopter with GoPro Mount\",\n    \"tube\": \"youtube\",\n    \"videoid\": \"\",\n    \"showvideo\": false,\n    \"unitprice\": 349,\n    \"saleprice\": 0,\n    \"unitsinstock\": null,\n    \"unitsonorder\": null,\n    \"reorderlevel\": null,\n    \"expecteddate\": null,\n    \"discontinued\": null,\n    \"notes\": \"\",\n    \"faux\": null,\n    \"sortorder\": 1\n}"

            if (objProduct != null)
            {
                Hashtable htParams = new Hashtable();
                htParams.Add("@strProductID", objProduct.productid);

                QuickDL.SqlUtil sqlRun = new QuickDL.SqlUtil();
                sqlRun.ZConnStr = cn;

                try
                {
                    int iReturn = sqlRun.ExecSPReturnIntValue("usp_ProductDelete", htParams);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            else
            {
                //context.Response.Write("Sorry something goes wrong.");
            }

            return "deleted!";
        }

        public string Insert(HttpContext context)
        {
            string s1 = string.Empty;
            string _return = string.Empty;

            //deserialize the object
            List<Product> objProduct = Deserialize<List<Product>>(context);
            //now we print out the value, we check if it is null or not
            if (objProduct != null)
            {
                Hashtable htParams = new Hashtable();
                //htParams.Add("@productid", objProduct[0].productid);
                htParams.Add("@sku", objProduct[0].sku);
                htParams.Add("@productname", objProduct[0].productname);
                htParams.Add("@strStoreID", objProduct[0].storeid);
                htParams.Add("@categoryname", objProduct[0].categoryname);
                htParams.Add("@header", objProduct[0].header);
                htParams.Add("@shortdesc", objProduct[0].shortdesc);
                htParams.Add("@description", objProduct[0].description);
                htParams.Add("@link", objProduct[0].link);
                htParams.Add("@linktext", objProduct[0].linktext);
                htParams.Add("@imageurl", objProduct[0].imageurl);
                htParams.Add("@imagename", objProduct[0].imagename);
                htParams.Add("@carousel", objProduct[0].carousel);
                htParams.Add("@carousel_caption", objProduct[0].carousel_caption);
                htParams.Add("@tube", objProduct[0].tube);
                htParams.Add("@videoid", objProduct[0].videoid);
                htParams.Add("@showvideo", objProduct[0].showvideo);
                htParams.Add("@unitprice", objProduct[0].unitprice);
                htParams.Add("@saleprice", objProduct[0].saleprice);
                htParams.Add("@unitsinstock", objProduct[0].unitsinstock);
                htParams.Add("@unitsonorder", objProduct[0].unitsonorder);
                htParams.Add("@reorderlevel", objProduct[0].reorderlevel);
                htParams.Add("@expecteddate", objProduct[0].expecteddate);
                htParams.Add("@discontinued", objProduct[0].discontinued);
                htParams.Add("@notes", objProduct[0].notes);
                htParams.Add("@faux", objProduct[0].faux);
                htParams.Add("@sortorder", objProduct[0].sortorder);

                QuickDL.SqlUtil sqlRun = new QuickDL.SqlUtil();
                sqlRun.ZConnStr = ConfigurationManager.ConnectionStrings["CartConnectionString"].ConnectionString;

                try
                {
                    int iReturn = sqlRun.ExecSPReturnIntValue("usp_ProductAdd", htParams);
                }
                catch(Exception ex)
                {
                    string s = ex.Message;
                }
            }
            else
            {
                //context.Response.Write("Sorry something goes wrong.");
            }

            //try
            //{
            //    Employee emp = newEmployee
            //        {
            //        FirstName = context.Request.Params["FirstName"],
            //        LastName = context.Request.Params["LastName"],
            //        Dob = Convert.ToDateTime(context.Request.Params["Dob"]),
            //        Country = context.Request.Params["Country"],
            //        Address = context.Request.Params["Address"]
            //    };
            //    dbEmployee.Employees.Add(emp);
            //    dbEmployee.SaveChanges();

            //    response.IsSuccess = true;
            //    response.Data = emp;
            //    response.Message = "Employee inserted successfully!";
            //    response.CallBack = callbackmethodname;
            //}
            //catch (Exception ex)
            //{
            //    response.IsSuccess = false;
            //    response.Message = ex.Message;
            //}

            //returnJsonConvert.SerializeObject(response);

            return "success!";
        }

        //publicstring Update(HttpContext context)
        //{
        //    JsonResponse response = newJsonResponse();

        //    try
        //    {
        //        int id = int.Parse(context.Request.Params["Id"]);

        //        Employee emp = dbEmployee.Employees.FirstOrDefault(m => m.Id == id);


        //        emp.FirstName = context.Request.Params["FirstName"];
        //        emp.LastName = context.Request.Params["LastName"];
        //        emp.Dob = Convert.ToDateTime(context.Request.Params["Dob"]);
        //        emp.Country = context.Request.Params["Country"];
        //        emp.Address = context.Request.Params["Address"];

        //        dbEmployee.SaveChanges();

        //        response.IsSuccess = true;
        //        response.Data = emp;
        //        response.Message = "Employee updated successfully!";
        //        response.CallBack = callbackmethodname;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //    }

        //    returnJsonConvert.SerializeObject(response);
        //}

        //publicstring Delete(int id)
        //{
        //    JsonResponse response = newJsonResponse();

        //    try
        //    {
        //        Employee emp = dbEmployee.Employees.FirstOrDefault(m => m.Id == id);
        //        if (emp != null)
        //        {
        //            dbEmployee.Employees.Remove(emp);
        //            dbEmployee.SaveChanges();
        //            response.IsSuccess = true;
        //            response.CallBack = callbackmethodname;
        //            response.Data = "Employee Deleted successfully!";
        //            response.Message = "Employee Deleted successfully!";
        //        }
        //        else
        //        {
        //            response.IsSuccess = false;
        //            response.Message = "Employee not exist!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //    }

        //    returnJsonConvert.SerializeObject(response);
        //}

        //publicstring GetEmployees()
        //{
        //    JsonResponse response = newJsonResponse();

        //    try
        //    {
        //        IEnumerable<Employee> employees = dbEmployee.Employees.ToList();
        //        response.IsSuccess = true;
        //        response.CallBack = callbackmethodname;
        //        response.Data = employees;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //        response.IsSuccess = false;
        //    }

        //    returnJsonConvert.SerializeObject(response);
        //}

        //publicstring GetById(int id)
        //{
        //    JsonResponse response = newJsonResponse();

        //    try
        //    {
        //        Employee emp = dbEmployee.Employees.FirstOrDefault(m => m.Id == id);
        //        response.IsSuccess = true;
        //        response.CallBack = callbackmethodname;
        //        response.Data = emp;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //    }

        //    returnJsonConvert.SerializeObject(response);
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private static IDbConnection CreateConnection(string strConn)
        {
            IDbConnection cnn = new System.Data.SqlClient.SqlConnection(strConn);
            cnn.Open();

            return cnn;
        }

        private static string GetConnectionString(string strConnName)
        {
            string _connString = string.Empty;

            try
            {
                _connString = ConfigurationManager.ConnectionStrings[strConnName].ConnectionString;
            }
            catch (Exception ex)
            {
                _connString = string.Empty;
            }

            return _connString;
        }

    }


    public class Product
    {
        public string productid { get; set; }
        public string sku { get; set; }
        public string productname { get; set; }
        public string storeid { get; set; }
        public string categoryname { get; set; }
        public string header { get; set; }
        public string shortdesc { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string linktext { get; set; }
        public string imageurl { get; set; }
        public string imagename { get; set; }
        public bool? carousel { get; set; }
        public string carousel_caption { get; set; }
        public string tube { get; set; }
        public string videoid { get; set; }
        public bool? showvideo { get; set; }
        public float? unitprice { get; set; }
        public float? saleprice { get; set; }
        public int? unitsinstock { get; set; }
        public int? unitsonorder { get; set; }
        public int? reorderlevel { get; set; }
        public DateTime? expecteddate { get; set; }
        public bool? discontinued { get; set; }
        public string notes { get; set; }
        public bool? faux { get; set; }
        public int? sortorder { get; set; }

    }



}