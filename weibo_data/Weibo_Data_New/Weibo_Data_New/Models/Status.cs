using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Weibo_Data_New.Models
{
    public class Status
    {
        [JsonProperty(PropertyName = "created_at")]
		public string CreatedAt { get; internal set; }
		[JsonProperty(PropertyName = "id")]
		[Key]
        public string ID { get; internal set; }
		[JsonProperty(PropertyName = "text")]
		public string Text { get; internal set; }
		[JsonProperty(PropertyName = "source")]
        public string Source { get; internal set; }
        [JsonProperty(PropertyName = "thumbnail_pic")]
        public string ThumbnailPictureUrl { get; internal set; }
        [JsonProperty(PropertyName = "bmiddle_pic")]
        public string MiddleSizePictureUrl { get; internal set; }
        [JsonProperty(PropertyName = "original_pic")]
        public string OriginalPictureUrl { get; internal set; }
        public int RepostsCount { get; internal set; }
		[JsonProperty(PropertyName = "comments_count")]
		public int CommentsCount { get; internal set; }
		[JsonProperty(PropertyName = "attitudes_count")]
        public int AttitudeCount { get; internal set; }

        public float Long { get; internal set; }
        public float Lat { get; internal set; }
        public virtual User User { get; internal set; }
        [ForeignKey("User")]
        public string UserID { get; internal set; }
    }
}