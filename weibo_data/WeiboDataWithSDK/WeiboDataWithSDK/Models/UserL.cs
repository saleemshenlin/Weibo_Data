using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeiboDataWithSDK.Models
{
    public class UserL
    {
        [JsonProperty(PropertyName = "id")]
        [Key]
        public string ID { get; internal set; }
        /// <summary>
        /// 用户UID(字符型)
        /// </summary>
        [JsonProperty(PropertyName = "idstr")]
        public string IDStr { get; internal set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; internal set; }
        /// <summary>
        /// 友好显示名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; internal set; }
        /// <summary>
        /// 用户所在地区ID
        /// </summary>
        [JsonProperty(PropertyName = "province")]
        public string Province { get; internal set; }
        /// <summary>
        /// 用户所在城市ID
        /// </summary>
        [JsonProperty(PropertyName = "city")]
        public string City { get; internal set; }
        /// <summary>
        /// 用户所在地
        /// </summary>
        [JsonProperty(PropertyName = "location")]
        public string Location { get; internal set; }
        /// <summary>
        /// 用户描述
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; internal set; }
        /// <summary>
        /// 用户头像地址
        /// </summary>
        [JsonProperty(PropertyName = "profile_image_url")]
        public string ProfileImageUrl { get; internal set; }
        /// <summary>
        /// 性别，m：男、f：女、n：未知
        /// </summary>
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; internal set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; internal set; }
        /// <summary>
        /// 微博认证用户的类型
        /// </summary>
        [JsonProperty(PropertyName = "verified_type")]
        public string VerifiedType { get; internal set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [JsonProperty(PropertyName = "remark")]
        public string Remark { get; internal set; }
        /// <summary>
        /// 是否允许所有人对我的微博进行评论
        /// </summary>
        [JsonProperty(PropertyName = "allow_all_comment")]
        public bool AllowAllComment { get; internal set; }
        /// <summary>
        /// 用户大头像地址
        /// </summary>
        [JsonProperty(PropertyName = "avatar_large")]
        public string AvatarLarge { get; internal set; }

        public virtual ICollection<StatusL> Status { get; internal set; }
    }
}