using System;
using System.IO;
using System.Text;
using Core;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web;

namespace TwitterAPI
{
    [DataContract]
    public class TwitterStatus
    {
        #region "Properties"
        [DataMember(Name = "created_at")]
        private string c_date { get; set; }

        public DateTime CreateDate
        {
            get
            {
                return DateTime.ParseExact(c_date, "ddd MMM dd HH':'mm':'ss zz'00' yyyy",
                    System.Globalization.DateTimeFormatInfo.InvariantInfo,
                    System.Globalization.DateTimeStyles.None);
            }
            set { CreateDate = value; }
        }

        /// <summary>
        /// ツイートID
        /// </summary>
        [DataMember(Name = "id")]
        public decimal Id { get; set; }

        /// <summary>
        /// ツイートID(文字列型)
        /// </summary>
        [DataMember(Name = "id_str")]
        public string StringId { get; set; }

        /// <summary>
        /// ツイート内容
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// クライアント
        /// </summary>
        [DataMember(Name = "source")]
        public string Source { get; set; }

        [DataMember(Name = "truncated")]
        public bool Truncated { get; set; }

        [DataMember(Name = "in_reply_to_status_id")]
        public decimal? InReplyToStatusId { get; set; }

        [DataMember(Name = "in_reply_to_status_id_str")]
        public string StringInReplyToStatusId { get; set; }

        [DataMember(Name = "in_reply_to_user_id")]
        public decimal? InReplyToUserId { get; set; }

        [DataMember(Name = "in_reply_to_user_id_str")]
        public string StringInReplyToUsrId { get; set; }

        [DataMember(Name = "in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }

        [DataMember(Name = "user")]
        public TwitterUser User { get; set; }

        [DataMember(Name = "geo", IsRequired = false)]
        public TwitterGeo Geo { get; set; }

        [DataContract]
        public class TwitterGeo
        {
            [DataMember(Name = "type")]
            public string Type { get; set; }

            [DataMember(Name = "coordinates")]
            private double[] Coordinates { get; set; }

            public double Lat { get { return Coordinates[0]; } }

            public double Long { get { return Coordinates[1]; } }
        }

        [DataMember(Name = "coordinates", IsRequired = false)]
        public TwitterGeo Cordinates { get; set; }

        //[DataMember(Name = "place", IsRequired = false)]
        //public string Place { get; set; }

        //[DataMember(Name = "contributors", IsRequired = false)]
        //public bool? Contributors { get; set; }

        [DataMember(Name = "retweeted_status", IsRequired = false)]
        public TwitterStatus RetweetedStatus { get; set; }

        [DataMember(Name = "retweet_count")]
        public decimal RetweetCount { get; set; }

        [DataMember(Name = "favorite_count")]
        public decimal FavoriteCount { get; set; }

        [DataMember(Name = "entities")]
        public TweetEntities Entities { get; set; }

        [DataContract]
        public class TweetEntities
        {
            [DataMember(Name = "hashtags")]
            public List<Entities_Hashtags> Hashtags { get; set; }

            [DataContract]
            public class Entities_Hashtags
            {
                [DataMember(Name = "text")]
                public string Text { get; set; }

                [DataMember(Name = "indices")]
                private int[] Indices { get; set; }

                public int IndexStart { get { return Indices[0]; } }

                public int IndexEnd { get { return Indices[1]; } }
            }

            [DataMember(Name = "symbols")]
            public List<Entities_Symbols> Symbols { get; set; }

            [DataContract]
            public class Entities_Symbols
            {
                [DataMember(Name = "text")]
                public string Text { get; set; }

                [DataMember(Name = "indices")]
                private int[] Indices { get; set; }

                public int IndexStart { get { return Indices[0]; } }
                public int IndexEnd { get { return Indices[1]; } }
            }


            [DataMember(Name = "urls")]
            public List<Entities_Urls> Urls { get; set; }

            [DataContract]
            public class Entities_Urls
            {
                [DataMember(Name = "url")]
                public string Url { get; set; }

                [DataMember(Name = "display_url")]
                public string DisplayUrl { get; set; }

                [DataMember(Name = "expanded_url")]
                public string ExpandedUrl { get; set; }

                [DataMember(Name = "indices")]
                private int[] Indices { get; set; }

                public int IndexStart { get { return Indices[0]; } }
                public int IndexEnd { get { return Indices[1]; } }
            }


            [DataMember(Name = "user_mentions")]
            public List<Entities_UserMentions> UserMentions { get; set; }

            [DataContract]
            public class Entities_UserMentions
            {
                [DataMember(Name = "name")]
                public string Name { get; set; }

                [DataMember(Name = "id")]
                public decimal? Id { get; set; }

                [DataMember(Name = "indices")]
                private int[] indices { get; set; }

                public int IndexStart { get { return indices[0]; } }
                public int IndexEnd { get { return indices[1]; } }
            }

            [DataMember(Name = "media", IsRequired = false)]
            public List<Entities_Media> Media { get; set; }

            [DataContract]
            public class Entities_Media
            {
                [DataMember(Name = "id")]
                public decimal Id { get; set; }

                [DataMember(Name = "id_str")]
                public string StringId { get; set; }

                [DataMember(Name = "media_url")]
                public string MediaUrl { get; set; }

                [DataMember(Name = "media_url_https")]
                public string MediaHttpsUrl { get; set; }

                [DataMember(Name = "url")]
                public string Url { get; set; }

                [DataMember(Name = "display_url")]
                public string DisplayUrl { get; set; }

                [DataMember(Name = "expanded_url")]
                public string ExpandedUrl { get; set; }

                [DataMember(Name = "sizes")]
                public MediaSizes Sizes { get; set; }

                [DataContract]
                public class MediaSizes
                {
                    [DataMember(Name = "large")]
                    public MediaSize Large { get; set; }

                    [DataMember(Name = "medium")]
                    public MediaSize Medium { get; set; }

                    [DataMember(Name = "small")]
                    public MediaSize Small { get; set; }

                    [DataMember(Name = "thumb")]
                    public MediaSize Thumb { get; set; }

                    [DataContract]
                    public class MediaSize
                    {
                        [DataMember(Name = "w")]
                        public int Width { get; set; }

                        [DataMember(Name = "resize")]
                        public string ResizeMode { get; set; }

                        [DataMember(Name = "h")]
                        public int Height { get; set; }
                    }
                }

                [DataMember(Name = "type")]
                public string MediaType { get; set; }

                [DataMember(Name = "indices")]
                private int[] Indices { get; set; }

                public int IndexStart { get { return Indices[0]; } }

                public int IndexEnd { get { return Indices[1]; } }
            }
        }

        [DataMember(Name = "favorited")]
        public bool Favorited { get; set; }

        [DataMember(Name = "retweeted")]
        public bool Retweeted { get; set; }

        [DataMember(Name = "lang")]
        public string Language { get; set; }
        #endregion


        /// <summary>
        /// ツイートの投稿
        /// </summary>
        /// <param name="Tweet">ツイート内容</param>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="options">オプション</param>
        /// <returns></returns>
        public static TwitterResponse<TwitterStatus> Update(string Tweet, OAuthTokens tokens, StatusUpdateOptions options = null)
        {
			var url = UrlBank.StatusesUpdate;

			var data = "status=" + Method.UrlEncode(Tweet);
			return new TwitterResponse<TwitterStatus>(Method.GenerateResponseResult(Method.GenerateWebRequest(url, WebMethod.POST, tokens, options, "application/x-www-form-urlencoded", data, UTF8Encoding.UTF8.GetBytes(data))));
        }


        /// <summary>
        /// ツイートと画像の投稿
        /// </summary>
        /// <param name="Tweet">ツイート内容</param>
        /// <param name="fileName">画像データ(バイト配列)</param>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="options">オプション</param>
        /// <returns></returns>
		public static TwitterResponse<TwitterStatus> UpdateWithMedia
			(string Tweet, byte[] content, OAuthTokens tokens, StatusUpdateOptions options = null)
		{
			var url = UrlBank.StatusesUpdateWithMedia;

			var boundary = Guid.NewGuid().ToString();

			var header = string.Format("--{0}", boundary);
			var footer = string.Format("--{0}--", boundary);

			var ContentType = "multipart/form-data;boundary=" + boundary;

			/*--------------------  送信するデータの生成  --------------------*/

			var encoding = Encoding.GetEncoding("iso-8859-1");

			var fileData = encoding.GetString(content);


			var builder = new StringBuilder();

			// ヘッダーを書き込む
			builder.AppendLine(header);
			builder.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "status"));
			builder.AppendLine();
			builder.AppendLine();

			// 投稿内容をUTF-8で書き込む(日本語文字化け回避)
			builder.AppendLine(encoding.GetString(Encoding.GetEncoding("UTF-8").GetBytes(Tweet)));

			// ファイル情報、フッターを書き込む
			builder.AppendLine();
			builder.AppendLine(header);
			builder.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "media[]", string.Format("image-{0}.png", DateTime.Now.ToString("yyyyMMddHHmmss"))));
			builder.AppendLine("Content-Type: application/octet-stream");
			builder.AppendLine();
			builder.AppendLine(fileData);
			builder.AppendLine(footer);

			return new TwitterResponse<TwitterStatus>(Method.GenerateResponseResult(Method.GenerateWebRequest(url, WebMethod.POST, tokens, options, ContentType, null, encoding.GetBytes(builder.ToString()))));
		}


		/// <summary>
		/// ツイートを取得
		/// </summary>
		/// <param name="id"></param>
		/// <param name="tokens"></param>
		/// <returns></returns>
		public static TwitterResponse<TwitterStatus> Show(decimal id, OAuthTokens tokens)
		{
			return new TwitterResponse<TwitterStatus>(Method.Get(string.Format(UrlBank.StatusesShow, id), tokens));
		}

        /// <summary>
        /// ツイートの削除
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="Id">StatudId</param>
        public static TwitterResponse<TwitterStatus> Destroy(OAuthTokens tokens, decimal Id)
        {
			return new TwitterResponse<TwitterStatus>(Method.Post(string.Format(UrlBank.StatusesDestroy, Id), tokens, null, null, null, null));
        }


        /// <summary>
        /// リツイートする
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="id">StatusId</param>
        public static TwitterResponse<TwitterStatus> Retweet(OAuthTokens tokens, decimal id)
        {
			return new TwitterResponse<TwitterStatus>(Method.Post(string.Format(UrlBank.StatusesRetweet, id), tokens, null, null, null, null));
        }


        /// <summary>
        /// リツイートの取消
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="id">RetweetedStatusId</param>
        /// <returns></returns>
        public static TwitterResponse<TwitterStatus> DestroyRetweet(OAuthTokens tokens, decimal id)
        {
			return new TwitterResponse<TwitterStatus>(Method.Post(string.Format(UrlBank.StatusesDestroy, id), tokens, null, null, null, null));
        }

		public static TwitterResponse<TwitterStatusCollection> Retweets(OAuthTokens tokens, decimal StatusId, RetweetsOption option = null)
		{
			return new TwitterResponse<TwitterStatusCollection>(Method.Get(string.Format(UrlBank.StatusesRetweets, StatusId), tokens, option));
		}

		public static TwitterResponse<UserIds> Retweeters(OAuthTokens tokens, decimal StatusId, CursorOption option = null)
		{
			return new TwitterResponse<UserIds>(Method.Get(string.Format("{0}?id={1}&stringify_ids=false", UrlBank.StatusesRetweeters, StatusId), tokens, null));
		}

		public class StatusesShowOption : ParameterClass
		{
			[Parameters("id")]
			public decimal Id { get; set; }

			[Parameters("trim_user")]
			public bool? TrimUser { get; set; }

			[Parameters("include_entities")]
			public bool? IncludeEntities { get; set; }

			[Parameters("include_my_retweet")]
			public bool? IncludeMyRetweet { get; set; }
		}

		public class RetweetsOption : ParameterClass
		{
			[Parameters("count")]
			public int? Count { get; set; }

			[Parameters("trim_user")]
			public bool? TrimUser { get; set; }
		}

		public class OembedOption : ParameterClass
		{
			[Parameters("maxwidth")]
			public int? MaxWidth { get; set; }

			[Parameters("hide_media")]
			public bool? HideMedia { get; set; }

			[Parameters("hide_thread")]
			public bool? HideThread { get; set; }

			[Parameters("omit_script")]
			public bool? OmitScript { get; set; }

			[Parameters("related")]
			public StringArray Related { get; set; }

			[Parameters("lang")]
			public string Language { get; set; }
		}
    }

    public class TwitterStatusCollection : List<TwitterStatus> { }
}
