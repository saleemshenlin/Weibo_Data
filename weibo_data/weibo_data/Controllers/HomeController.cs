using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using weibo_data.Models;

namespace weibo_data.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = ReadFolder().ToString();
            ReadFolder();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /// <summary>
        /// 读取文件夹中的文件名
        /// </summary>
        private void ReadFolder()
        {
            List<string> result = new List<string>();
            string url = Server.MapPath("data");
            DirectoryInfo TheFolder = new DirectoryInfo(url);
            if (TheFolder != null)
            {
                FileInfo[] Folders = TheFolder.GetFiles();
                foreach (FileInfo fileinfo in Folders)
                {
                    result.Add(fileinfo.Name);
                }
            }
            ReadJsonFromFile(result);
        }

        private void ReadJsonFromFile(List<string> files)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            foreach (string filename in files)
            {
                string fileUrl = Server.MapPath("data/" + filename);
                //读取文件
                StreamReader sr = new StreamReader(fileUrl, System.Text.Encoding.UTF8);
                try
                {
                    if (sr != null)
                    {
                        string stringLine = sr.ReadLine();
                        WeiboFromBigData weibo = new WeiboFromBigData();
                        if (stringLine != null)
                        {
                            string result = "";
                            while (stringLine != null)
                            {
                                result = stringLine.Substring(1, stringLine.Length - 2);
                                weibo = JsonConvert.DeserializeObject<WeiboFromBigData>(result);
                                stringLine = sr.ReadLine();
                            }
                        }
                    }
                }
                finally
                {
                    sr.Close();
                }
            }
        }
    }
}