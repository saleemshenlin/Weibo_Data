using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace weibo_data.Models
{
    /// <summary>
    /// 从BigData抓取来的weibo的实体类
    /// </summary>
    public class WeiboFromBigData
    {
        /// <summary>
        /// WeiboFromBigData 的 id 主键
        /// </summary>
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        /// <summary>
        /// //被评论的微博ID，当本条微博为AAA微博的评论微博时，该字段为AAA对应的微博ID。当本条微博为原创微博或转发微博时，该字段为空。
        /// </summary>
        [JsonProperty(PropertyName = "beCommentWeiboId")]
        string CommentWeiboId { get; set; }

        /// <summary>
        /// //被转发的微博ID，当本条微博为AAA微博的转发微博时，该字段为AAA对应的微博ID。当本条微博为原创微博或评论微博时，该字段为空。当转发微博的源微博被删除时，该字段为-1。
        /// </summary>
        [JsonProperty(PropertyName = "beForwardWeiboId")]
        string ForwardWeiboId { get; set; }
        /// <summary>
        /// //当前微博的评论数
        /// </summary>
        [JsonProperty(PropertyName = "commentCount")]
        string CommentCount { get; set; }
        /// <summary>
        /// 微博内容
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        string Content { get; set; }
        /// <summary>
        /// //微博创建时间
        /// </summary>
        [JsonProperty(PropertyName = "createTime")]
        string CreateTime { get; set; }
        /// <summary>
        /// //赞的数目
        /// </summary>
        [JsonProperty(PropertyName = "praiseCount")]
        string PraiseCount { get; set; }

        /// <summary>
        /// //转发数
        /// </summary>
        [JsonProperty(PropertyName = "reportCount")]
        string ReportCount { get; set; }
        /// <summary>
        /// //微博来源
        /// </summary>
        [JsonProperty(PropertyName = "source")]
        string Source { get; set; }

        /// <summary>
        /// //微博创建者
        /// </summary>
        [JsonProperty(PropertyName = "souuserIdrce")]
        string UserId { get; set; }

        /// <summary>
        /// //微博id
        /// </summary>
        [JsonProperty(PropertyName = "weiboId")]
        string WeiboId { get; set; }
        /// <summary>
        /// //微博对应的url
        /// </summary>
        [JsonProperty(PropertyName = "weiboUrl")]
        string WeiboUrl { get; set; }
    }
}