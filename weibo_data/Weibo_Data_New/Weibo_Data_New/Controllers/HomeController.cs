using NetDimension.Weibo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Weibo_Data_New.Controllers
{
    public class HomeController : Controller
    {
        public static string APPKEY = "1663244227";
        public static string APPSECRET = "5cedafd36f790630c49775d7e56e741a";
        public static string RETURNURL = "https://api.weibo.com/oauth2/default.html";
        public static OAuth OAUTH;
        public static string WEIBO_NAME = "saleemshenlin@gmail.com";
        public static string PASSWORD = "1qaz2wsx";
        static Client Sina = null;
        string UserID = string.Empty;
        
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Message = InitWeiboOAuth();
            ViewBag.User = LoadUserInfo();
            LoadPoiUesr();
            return View();
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Home/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
        /// 米亚罗 B2094757D16FAAFA4392
        /// 米亚罗自然保护区 B2094653DB64ABFD499C
        /// 
        /// </summary>
        private List<NetDimension.Weibo.Entities.user.Entity> LoadPoiUesr()
        {
            //NetDimension.Weibo.Interface.Dynamic.PlaceInterface place = new NetDimension.Weibo.Interface.Dynamic.PlaceInterface(Sina);
            dynamic json = Sina.API.Dynamic.Place.POIUsers("B2094757D16FAAFA4392", 50, 1, false);
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
