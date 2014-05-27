using NetDimension.Weibo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using weibo_data.Dals;
using weibo_data.Models;

namespace weibo_data.Controllers
{
    public class HomeController : Controller
    {
        public static string APPKEY = "1663244227";
        public static string APPSECRET = "5cedafd36f790630c49775d7e56e741a";
        public static string RETURNURL = "https://api.weibo.com/oauth2/default.html";
        public static OAuth OAUTH;
        public static string WEIBO_NAME = "saleemshenlin@gmail.com";
        public static string PASSWORD = "1qaz2wsx";
        Client Sina = null;
        string UserID = string.Empty;
        /// <summary>
        /// 连接数据库
        /// </summary>
        private WeiboBigDataContext db = new WeiboBigDataContext();
        public ActionResult Index()
        {
            //ReadFolder();
            ViewBag.Message = InitWeiboOAuth();
            ViewBag.User = LoadUserInfo();
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
            int num = 0;//计数
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
                                db.WeiboFromBigDatas.Add(weibo);
                                db.SaveChanges();
                                num += 1;
                                Console.Write("num:" + num);
                                stringLine = sr.ReadLine();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
                finally
                {
                    sr.Close();
                }
            }
        }

        /// <summary>
        /// 初始化Weibo
        /// </summary>
        /// <returns></returns>
        private string InitWeiboOAuth()
        {
            if (Session["AccessToken"] == null)
            {
                OAUTH = new OAuth(APPKEY, APPSECRET, RETURNURL);
                if (!OAUTH.ClientLogin(WEIBO_NAME, PASSWORD))
                {
                    return "授权登录失败，请重试。";
                }
                else
                {
                    Session["AccessToken"] = OAUTH.AccessToken;
                    Sina = new Client(OAUTH);
                    UserID = Sina.API.Entity.Account.GetUID();
                    return OAUTH.AccessToken;
                }
            }
            else
            {
                return (string)Session["AccessToken"];
            }
        }
        /// <summary>
        /// place/pois/users
        /// 米亚罗自然保护区 P01Q5057WTO
        /// 米亚罗自然保护区 P01Q5057J2X
        /// </summary>
        private List<NetDimension.Weibo.Entities.user.Entity> LoadPoiUesr()
        {
            IEnumerable<NetDimension.Weibo.Entities.user.Entity> json = Sina.API.Dynamic.Place.POIUsers("P01Q5057WTO", 50, 1,false);
            List<NetDimension.Weibo.Entities.user.Entity> ds = new List<NetDimension.Weibo.Entities.user.Entity>();
            return ds;
        }
        /// <summary>
        /// 获取用户信息，我们来个直接把JSON写到页面的方法和下面的方法区别下
        /// </summary>
        /// <returns>JSON</returns>
        public string LoadUserInfo()
        {
            NetDimension.Weibo.Entities.user.Entity user = Sina.API.Entity.Users.Show(UserID, null);

            string result = user.ToString();

            return string.Format("{0}", result);
        }

    }
}