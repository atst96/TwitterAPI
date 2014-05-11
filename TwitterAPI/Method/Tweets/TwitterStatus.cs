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
            return TwitterStatusCommand.Update(Tweet, tokens, options);
        }


        /// <summary>
        /// ツイートと画像の投稿
        /// </summary>
        /// <param name="Tweet">ツイート内容</param>
        /// <param name="fileName">画像ファイル名</param>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="options">オプション</param>
        /// <returns></returns>
        public static TwitterResponse<TwitterStatus> UpdateWithMedia
            (string Tweet, string fileName, OAuthTokens tokens, StatusUpdateOptions options = null)
        {
            return TwitterStatusCommand.UpdateWithMedia(Tweet, fileName, tokens, options);
        }

		public static TwitterResponse<TwitterStatus> UpdateWithMedia
			(string Tweet, byte[] content, OAuthTokens tokens, StatusUpdateOptions options = null)
		{
			return TwitterStatusCommand.UpdateWithMedia(Tweet, content, tokens, options);
		}

        /// <summary>
        /// ツイートの削除
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="Id">StatudId</param>
        public static TwitterResponse<TwitterStatus> Destroy(OAuthTokens tokens, decimal Id)
        {
            return TwitterStatusCommand.Destroy(tokens, Id);
        }


        /// <summary>
        /// リツイートする
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="id">StatusId</param>
        public static TwitterResponse<TwitterStatus> Retweet(OAuthTokens tokens, decimal id)
        {
            return TwitterStatusCommand.Retweet(id, tokens);
        }


        /// <summary>
        /// リツイートの取消
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="id">RetweetedStatusId</param>
        /// <returns></returns>
        public static TwitterResponse<TwitterStatus> DestroyRetweet(OAuthTokens tokens, decimal id)
        {
            return TwitterStatusCommand.DestroyRetweet(id, tokens);
        }


    }

    public class TwitterStatusCollection : List<TwitterStatus> { }
}
