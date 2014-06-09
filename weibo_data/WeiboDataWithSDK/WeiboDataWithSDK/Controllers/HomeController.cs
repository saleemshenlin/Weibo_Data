using NetDimension.Weibo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using weibo_data.Dals;
using WeiboDataWithSDK.Models;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Xml;

namespace WeiboDataWithSDK.Controllers
{
    public class HomeController : Controller
    {
        //daqingAPP
        public static string APPKEY = "3745111922";
        public static string APPSECRET = "8bb53cfe8d623fb1001d7c990b88f168";
        //Yamba_微博
        //public static string APPKEY = "1663244227";//3745111922
        //public static string APPSECRET = "5cedafd36f790630c49775d7e56e741a";//8bb53cfe8d623fb1001d7c990b88f168
        public static string RETURNURL = "https://api.weibo.com/oauth2/default.html";
        public static OAuth OAUTH;
        public static string WEIBO_NAME = "saleemshenlin@gmail.com";
        public static string PASSWORD = "1qaz2wsx";
        static Client Sina = null;
        string UserID = string.Empty;
        private WeiboBigDataContext db = new WeiboBigDataContext();
        public ActionResult Index()
        {
            ViewBag.Message = InitWeiboOAuth();
            ViewBag.User = LoadUserInfo();
            //LoadPoiUesr();
            //LoadNearbyUser(103.119773f, 31.684551f);//103.119773 31.684551
            //LoadPoiTimeLine("B2094653DB64A1F4409D", 31.48252f, 103.19954f); //B2094653DB64A1F4409D 103.19954 31.48252
            DataBaseToExcel();
            //ReadFolder();

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
        /// 米亚罗 B2094757D16FAAFA4392 530 31.657766465 102.80677515
        /// 米亚罗自然保护区 B2094653DB64ABFD499C 193 31.65477 102.810272
        /// 古尔沟温泉山庄  B2094653DB64A1FB469C 31.49766 102.97848
        /// 毕棚沟旅游景区 B2094653DB64A0FA459E 31.40819 102.98744
        /// 毕棚沟 B2094751D569AAFA409E 103.071091 31.402451
        /// 娜姆湖 B2094653DB64A0FA4899 102.95245 31.31969
        /// 桃坪羌寨 B2094757D16CAAF4469C  103.45722781 31.554634941
        /// 桃坪寨  B2094653DB64A1F8489D 103.46205 31.57092
        /// 白空寺 B2094653DB64ABFE459D 103.45392 31.60256
        /// 甘堡藏家 B2094653DB64A1F4409D 103.19954 31.48252
        /// </summary>
        private void LoadPoiUesr(string poiid)
        {
            int times = 0;
            if (Sina == null)
            {
                Sina = new Client(OAUTH);
            }
            dynamic json = Sina.API.Dynamic.Place.POIUsers(poiid, 50, 1, false);
            //IEnumerable<NetDimension.Weibo.Entities.user.Entity> users = json;
            if (json.IsDefined("total_number"))
            {
                int totalNumber = int.Parse(json.total_number);
                times = totalNumber / 50 + 1;
            }
            LoadPoiUserReadJson(json);
            for (int i = 2; i <= times; i++)
            {
                dynamic newjson = Sina.API.Dynamic.Place.POIUsers(poiid, 50, i, false);
                LoadPoiUserReadJson(newjson);
            }
            //return users;
        }
        private void LoadPoiUserReadJson(dynamic json)
        {
            try
            {
                if (json.IsDefined("users"))
                {
                    foreach (var user in json.users)
                    {
                        var isInUserDb = db.UserDb.Find(user.id);
                        if (isInUserDb == null)
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
                            db.UserDb.Add(userEntity);
                            db.SaveChanges();
                            if (user.IsDefined("status"))
                            {
                                var isInStatusDb = db.StatusDb.Find(user.status.id);
                                if (isInStatusDb == null)
                                {
                                    Status statusEntity = new Status();
                                    statusEntity.CreatedAt = user.status.created_at;
                                    statusEntity.ID = user.status.id;
                                    statusEntity.Text = user.status.text;
                                    if (statusEntity.Text.IndexOf("此微博已被作者删除") <= -1)
                                    {
                                        statusEntity.Source = user.status.source;
                                        if (user.status.IsDefined("thumbnail_pic"))
                                        {
                                            statusEntity.ThumbnailPictureUrl = user.status.thumbnail_pic;
                                            statusEntity.MiddleSizePictureUrl = user.status.bmiddle_pic;
                                            statusEntity.OriginalPictureUrl = user.status.original_pic;
                                        }
                                        statusEntity.Long = 103.19954f;// 
                                        statusEntity.Lat = 31.48252f;
                                        statusEntity.RepostsCount = int.Parse(user.status.reposts_count);
                                        statusEntity.CommentsCount = int.Parse(user.status.comments_count);
                                        statusEntity.AttitudeCount = int.Parse(user.status.attitudes_count);
                                        statusEntity.User = userEntity;
                                        db.StatusDb.Add(statusEntity);

                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {

            }
        }
        /// <summary>
        /// place/nearby/users
        ///米亚罗 B2094757D16FAAFA4392 530 31.657766465 102.80677515
        ///米亚罗自然保护区 B2094653DB64ABFD499C 193 31.65477 102.810272
        ///古尔沟温泉山庄  B2094653DB64A1FB469C 31.49766 102.97848
        ///毕棚沟旅游景区 B2094653DB64A0FA459E 31.40819 102.98744
        ///毕棚沟 B2094751D569AAFA409E 103.071091 31.402451
        ///娜姆湖 B2094653DB64A0FA4899 102.95245 31.31969
        ///桃坪羌寨 B2094757D16CAAF4469C  103.45722781 31.554634941
        ///桃坪寨  B2094653DB64A1F8489D 103.46205 31.57092
        ///白空寺 B2094653DB64ABFE459D 103.45392 31.60256
        ///甘堡藏家 B2094653DB64A1F4409D 103.19954 31.48252
        /// </summary>
        private void LoadNearbyUser(float lon, float lat)
        {
            int times = 0;
            if (Sina == null)
            {
                Sina = new Client(OAUTH);
            }
            dynamic json = Sina.API.Dynamic.Place.NearByUsers(lat, lon, 3000, 50, 1);
            //IEnumerable<NetDimension.Weibo.Entities.user.Entity> users = json;
            if (json.IsDefined("total_number"))
            {
                int totalNumber = int.Parse(json.total_number);
                times = totalNumber / 50 + 1;
            }
            LoadNearbyUserReadJson(json);
            for (int i = 2; i <= times; i++)
            {
                dynamic newjson = Sina.API.Dynamic.Place.NearByUsers(lat, lon, 3000, 50, i);
                LoadNearbyUserReadJson(newjson);
            }
            //return users;
        }
        private void LoadNearbyUserReadJson(dynamic json)
        {
            try
            {
                if (json.IsDefined("users"))
                {
                    foreach (var user in json.users)
                    {
                        var isInUserDb = db.UserDb.Find(user.id);
                        if (isInUserDb == null)
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
                            db.UserDb.Add(userEntity);
                            db.SaveChanges();
                            if (user.IsDefined("status"))
                            {
                                var isInStatusDb = db.StatusDb.Find(user.status.id);
                                if (isInStatusDb == null)
                                {
                                    Status statusEntity = new Status();
                                    statusEntity.CreatedAt = user.status.created_at;
                                    statusEntity.ID = user.status.id;
                                    statusEntity.Text = user.status.text;
                                    if (statusEntity.Text.IndexOf("此微博已被作者删除") <= -1)
                                    {
                                        statusEntity.Source = user.status.source;
                                        if (user.status.IsDefined("thumbnail_pic"))
                                        {
                                            statusEntity.ThumbnailPictureUrl = user.status.thumbnail_pic;
                                            statusEntity.MiddleSizePictureUrl = user.status.bmiddle_pic;
                                            statusEntity.OriginalPictureUrl = user.status.original_pic;
                                        }
                                        statusEntity.Long = float.Parse(user.lon);// 
                                        statusEntity.Lat = float.Parse(user.lat);
                                        statusEntity.RepostsCount = int.Parse(user.status.reposts_count);
                                        statusEntity.CommentsCount = int.Parse(user.status.comments_count);
                                        statusEntity.AttitudeCount = int.Parse(user.status.attitudes_count);
                                        statusEntity.User = userEntity;
                                        db.StatusDb.Add(statusEntity);

                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {

            }
        }
        /// <summary>
        /// place/poi_timeline
        ///米亚罗 B2094757D16FAAFA4392 530 31.657766465 102.80677515
        ///米亚罗自然保护区 B2094653DB64ABFD499C 193 31.65477 102.810272
        ///古尔沟温泉山庄  B2094653DB64A1FB469C 31.49766 102.97848
        ///毕棚沟旅游景区 B2094653DB64A0FA459E 31.40819 102.98744
        ///毕棚沟 B2094751D569AAFA409E 103.071091 31.402451
        ///娜姆湖 B2094653DB64A0FA4899 102.95245 31.31969
        ///桃坪羌寨 B2094757D16CAAF4469C  103.45722781 31.554634941
        ///桃坪寨  B2094653DB64A1F8489D 103.46205 31.57092
        ///白空寺 B2094653DB64ABFE459D 103.45392 31.60256
        ///甘堡藏家 B2094653DB64A1F4409D 103.19954 31.48252
        /// </summary>
        /// <param name="poiid"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        private void LoadPoiTimeLine(string poiid, float lat, float lon)
        {
            int times = 0;
            if (Sina == null)
            {
                Sina = new Client(OAUTH);
            }
            dynamic json = Sina.API.Dynamic.Place.POITimeline(poiid, "0", "0", 50, 1, false);
            //IEnumerable<NetDimension.Weibo.Entities.user.Entity> users = json;
            if (json.IsDefined("total_number"))
            {
                int totalNumber = int.Parse(json.total_number);
                times = totalNumber / 50 + 1;
            }
            LoadPoiTimeLineReadJson(json, lat, lon);
            for (int i = 2; i <= times; i++)
            {
                dynamic newjson = Sina.API.Dynamic.Place.POITimeline(poiid, "0", "0", 50, i, false);
                LoadPoiTimeLineReadJson(newjson, lon, lat);
            }
        }
        private void LoadPoiTimeLineReadJson(dynamic json, float lat, float lon)
        {
            try
            {
                if (json.IsDefined("statuses"))
                {
                    foreach (var status in json.statuses)
                    {
                        var isInStatusDb = db.StatusDb.Find(status.id);
                        if (isInStatusDb == null)
                        {
                            Status statusEntity = new Status();
                            statusEntity.CreatedAt = status.created_at;
                            statusEntity.ID = status.id;
                            statusEntity.Text = status.text;
                            if (statusEntity.Text.IndexOf("此微博已被作者删除") <= -1 && statusEntity.Text.IndexOf("此微博已被删除") <= -1)
                            {
                                User userEntity = new User();
                                userEntity.ID = status.user.id;
                                userEntity.IDStr = status.user.idstr;
                                userEntity.Name = status.user.name;
                                userEntity.Province = status.user.province;
                                userEntity.City = status.user.city;
                                userEntity.Location = status.user.location;
                                userEntity.ProfileImageUrl = status.user.profile_image_url;
                                userEntity.Gender = status.user.gender;
                                userEntity.CreatedAt = status.user.created_at;
                                var isInUserDb = db.UserDb.Find(status.user.id);
                                if (isInUserDb == null)
                                {
                                    db.UserDb.Add(userEntity);
                                    db.SaveChanges();
                                }
                                statusEntity.Source = status.source;
                                if (status.IsDefined("thumbnail_pic"))
                                {
                                    statusEntity.ThumbnailPictureUrl = status.thumbnail_pic;
                                    statusEntity.MiddleSizePictureUrl = status.bmiddle_pic;
                                    statusEntity.OriginalPictureUrl = status.original_pic;
                                }
                                if (status.geo != null)
                                {
                                    statusEntity.Long = (float)status.geo.coordinates[1];
                                    statusEntity.Lat = (float)status.geo.coordinates[0];
                                }
                                else
                                {
                                    statusEntity.Long = lon;
                                    statusEntity.Lat = lat;
                                }
                                statusEntity.RepostsCount = int.Parse(status.reposts_count);
                                statusEntity.CommentsCount = int.Parse(status.comments_count);
                                statusEntity.AttitudeCount = int.Parse(status.attitudes_count);
                                statusEntity.UserID = userEntity.ID;
                                db.StatusDb.Add(statusEntity);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            finally
            {

            }
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
        /// <summary>
        /// 根据id调用Status/Show
        /// </summary>
        /// <param name="statusid"></param>
        private void LoadStatusShow(string statusid)
        {
            if (Sina == null)
            {
                Sina = new Client(OAUTH);
            }
            dynamic json = Sina.API.Dynamic.Statuses.Show(statusid);
            try
            {
                if (json != null)
                {
                    var isInHarzardsDb = db.HarzardsDb.Find(json.id);
                    if (isInHarzardsDb == null)
                    {
                        Hazards hazardsEntity = new Hazards();
                        hazardsEntity.CreatedAt = json.created_at;
                        hazardsEntity.ID = json.id;
                        hazardsEntity.Text = json.text;
                        if (hazardsEntity.Text.IndexOf("此微博已被作者删除") <= -1 && hazardsEntity.Text.IndexOf("此微博已被删除") <= -1)
                        {
                            User userEntity = new User();
                            userEntity.ID = json.user.id;
                            userEntity.IDStr = json.user.idstr;
                            userEntity.Name = json.user.name;
                            userEntity.Province = json.user.province;
                            userEntity.City = json.user.city;
                            userEntity.Location = json.user.location;
                            userEntity.ProfileImageUrl = json.user.profile_image_url;
                            userEntity.Gender = json.user.gender;
                            userEntity.CreatedAt = json.user.created_at;
                            var isInUserDb = db.UserDb.Find(json.user.id);
                            if (isInUserDb == null)
                            {
                                db.UserDb.Add(userEntity);
                                db.SaveChanges();
                            }
                            hazardsEntity.Source = json.source;
                            if (json.IsDefined("thumbnail_pic"))
                            {
                                hazardsEntity.ThumbnailPictureUrl = json.thumbnail_pic;
                                hazardsEntity.MiddleSizePictureUrl = json.bmiddle_pic;
                                hazardsEntity.OriginalPictureUrl = json.original_pic;
                            }
                            if (json.geo != null)
                            {
                                hazardsEntity.Long = (float)json.geo.coordinates[1];
                                hazardsEntity.Lat = (float)json.geo.coordinates[0];
                            }
                            //else
                            //{
                            //    statusEntity.Long = 0f;
                            //    statusEntity.Lat = 0f;
                            //}
                            hazardsEntity.RepostsCount = int.Parse(json.reposts_count);
                            hazardsEntity.CommentsCount = int.Parse(json.comments_count);
                            hazardsEntity.AttitudeCount = int.Parse(json.attitudes_count);
                            hazardsEntity.UserID = userEntity.ID;
                            db.HarzardsDb.Add(hazardsEntity);
                            db.SaveChanges();
                        }
                    }

                }
            }
            finally
            {

            }
        }

        /// <summary>
        /// 读取抓取数据文件夹中的文件名
        /// </summary>
        private void ReadFolder()
        {
            List<string> result = new List<string>();
            string url = Server.MapPath("Hazard_Data");
            DirectoryInfo TheFolder = new DirectoryInfo(url);
            if (TheFolder != null)
            {
                FileInfo[] Folders = TheFolder.GetFiles();
                foreach (FileInfo fileinfo in Folders)
                {
                    result.Add(fileinfo.Name);
                }
            }
            ReadXMLFromFile(result);
        }
        /// <summary>
        /// 读取XML中的微博id
        /// </summary>
        /// <param name="result"></param>
        private void ReadXMLFromFile(List<string> result)
        {
            foreach (string filename in result)
            {
                string fileUrl = Server.MapPath("Hazard_Data/" + filename);
                XmlDocument doc = new XmlDocument();
                doc.Load(fileUrl);
                XmlNode extraction = doc.SelectSingleNode("extraction");
                try
                {
                    XmlNode lixian = extraction.SelectSingleNode("lixian");
                    XmlNodeList items = lixian.ChildNodes;
                    foreach (XmlNode item in items)
                    {
                        XmlNode mid = item.SelectSingleNode("mid");
                        string weiboMid = mid.InnerText;
                        //调用weibo api
                        LoadStatusShow(weiboMid);
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
                finally
                {
                    //sr.Close();
                }
            }
        }


        private void DataBaseToExcel()
        {
            //List<Status> statuses = db.StatusDb.ToList();
            //List<User> users = db.UserDb.ToList();
            List<Hazards> hazards = db.HarzardsDb.ToList();
            User user = new User();
            Status status = new Status();
            Hazards hazard = new Hazards();
            PropertyInfo[] userInfo = user.GetType().GetProperties();
            PropertyInfo[] statusInfo = status.GetType().GetProperties();
            PropertyInfo[] hazardInfo = hazard.GetType().GetProperties();
            object oOpt = System.Reflection.Missing.Value; //for optional arguments
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks oWBs = oXL.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook oWB = oWBs.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Worksheets[1];

            //outputRows is a List<List<object>>
            int numberOfRows = hazards.Count(); //users.Count;
            int numberOfColumns = hazardInfo.Count();//userInfo.Count();

            Microsoft.Office.Interop.Excel.Range oRng;
            for (int i = 0; i < numberOfColumns; i++)
            {
                oSheet.Cells[1, i + 1] = hazardInfo[i].Name;
                oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[1, i + 1];
                oRng.Interior.ColorIndex = 15;
            }

            int row = 0;
            foreach (var newstatus in hazards)
            {
                //if (row < 6000)
                //{
                //    row++;
                //    continue;
                //}
                for (int col = 0; col < numberOfColumns; col++)
                {
                    try
                    {
                        if (!hazardInfo[col].Name.Equals("User"))
                        {
                            oSheet.Cells[row + 2, col + 1] = hazardInfo[col].GetValue(newstatus, null);
                        }
                    }
                    catch
                    {
                        oSheet.Cells[row + 2, col + 1] = "null";
                    }
                }
                row++;
            }


            oXL.Visible = true;
        }

    }
}