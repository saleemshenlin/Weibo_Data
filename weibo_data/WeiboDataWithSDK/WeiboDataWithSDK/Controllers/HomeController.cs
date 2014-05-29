using NetDimension.Weibo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiboDataWithSDK.Models;

namespace WeiboDataWithSDK.Controllers
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
        public ActionResult Index()
        {
            ViewBag.Message = InitWeiboOAuth();
            ViewBag.User = LoadUserInfo();
            //LoadPoiUesr();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
        /// 米亚罗 B2094757D16FAAFA4392 530
        /// 米亚罗自然保护区 B2094653DB64ABFD499C
        /// 
        /// </summary>
        private void LoadPoiUesr()
        {

            if (Sina == null)
            {
                Sina = new Client(OAUTH);
            }
            dynamic json = Sina.API.Dynamic.Place.POIUsers("B2094757D16FAAFA4392", 50, 1, false);
            //IEnumerable<NetDimension.Weibo.Entities.user.Entity> users = json;
            if (json.IsDefined("total_number"))
            {
                int totalNumber = int.Parse(json.total_number);
            }
            if (json.IsDefined("users"))
            {
                foreach (var user in json.users)
                {
                    User userEntity = new User();
                    userEntity.ID = user.id;
                    userEntity.IDStr = user.idstr;
                    userEntity.Name = user.name;
                    userEntity.Province = user.province;
                    userEntity.City = user.city;
                    userEntity.Location = user.location;
                    userEntity.ProfileImageUrl = user.profile_image_url;
                    userEntity.Gender = user.gender;
                    userEntity.CreatedAt = user.created_at;
                    if (user.IsDefined("status"))
                    {
                        Status statusEntity = new Status();
                        statusEntity.CreatedAt = user.status.created_at;
                        statusEntity.ID = user.status.id;
                        statusEntity.Text = user.status.text;
                        statusEntity.Source = user.status.source;
                        statusEntity.ThumbnailPictureUrl = user.status.thumbnail_pic;
                        statusEntity.MiddleSizePictureUrl = user.status.bmiddle_pic;
                        statusEntity.OriginalPictureUrl = user.status.original_pic;
                        statusEntity.Long = 102.806778f;
                        statusEntity.Lat = 31.657766f;
                        statusEntity.RepostsCount = int.Parse(user.status.reposts_count);
                        statusEntity.CommentsCount = int.Parse(user.status.comments_count);
                        statusEntity.AttitudeCount = int.Parse(user.status.attitudes_count);
                        statusEntity.User = userEntity;
                    }
                }
            }
            //return users;
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