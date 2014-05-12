using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TwitterAPI;
using Core;
using Newtonsoft.Json;

namespace TwitterAPI
{
    public static partial class TwitterTimeline
    {

        private static string Url_Mentions_Timeline = "https://api.twitter.com/1.1/statuses/mentions_timeline.json";

        private static string Url_Home_Timeline = "https://api.twitter.com/1.1/statuses/home_timeline.json";

        private static string Url_User_Timeline = "https://api.twitter.com/1.1/statuses/user_timeline.json";

        private static string Url_Retweet_Of_Me = "https://api.twitter.com/1.1/statuses/retweets_of_me.json";



        private static TwitterResponse<TwitterStatusCollection> GetTimeline(string Url, OAuthTokens tokens, TimelineOptions options = null)
        {
                return new TwitterResponse<TwitterStatusCollection>(Method.Get(Url, tokens, options));
        }

		/// <summary>
		/// UserTimelineの取得
		/// </summary>
		/// <param name="tokens">OAuthTokens</param>
		/// <param name="options">TimelineOptions</param>
		/// <returns>TwitterStatusCollection</returns>
		public static TwitterResponse<TwitterStatusCollection> UserTimeline(OAuthTokens tokens, TimelineOptions options = null)
		{
			return GetTimeline(Url_User_Timeline, tokens, options);
		}

		/// <summary>
		/// MentionsTimelineの取得
		/// </summary>
		/// <param name="tokens">OAuthTokens</param>
		/// <param name="options">TimelineOptions</param>
		/// <returns>TwitterStatusCollection</returns>
		public static TwitterResponse<TwitterStatusCollection> RetweetOfMe(OAuthTokens tokens, TimelineOptions options = null)
		{
			return GetTimeline(Url_Retweet_Of_Me, tokens, options);
		}

		/// <summary>
		/// MentionsTimelineの取得
		/// </summary>
		/// <param name="tokens">OAuthTokens</param>
		/// <param name="options">TimelineOptions</param>
		/// <returns>TwitterStatusCollection</returns>
		public static TwitterResponse<TwitterStatusCollection> MentionsTimeline(OAuthTokens tokens, TimelineOptions options = null)
		{
			return GetTimeline(Url_Mentions_Timeline, tokens, options);
		}

		/// <summary>
		/// HomeTimelineの取得
		/// </summary>
		/// <param name="tokens">OAuthTokens</param>
		/// <param name="options">TimelineOptions</param>
		/// <returns>TwitterStatusCollection</returns>
		public static TwitterResponse<TwitterStatusCollection> HomeTimeline(OAuthTokens tokens, TimelineOptions options = null)
		{
			return GetTimeline(Url_Home_Timeline, tokens, options);
		}


        public class TimelineOptions : ParameterClass
        {

            /// <summary>
            /// 取得するツイート数
            /// </summary>
            [Parameters("count")]
            public int? Count { get; set; }


            /// <summary>
            /// このTweetID以降のつぶやきを取得する
            /// </summary>
            [Parameters("since_id")]
            public decimal? SinceId { get; set; }


            /// <summary>
            /// このTweetID以前のつぶやきを取得する
            /// </summary>
            [Parameters("max_id")]
            public decimal? MaxId { get; set; }

            /// <summary>
            /// ユーザ情報を取得する
            /// </summary>
            [Parameters("trim_user")]
            public bool? TrimUser { get; set; }

            [Parameters("exclude_replies")]
            public bool? ExcludeReplies { get; set; }

            [Parameters("contributor_details")]
            public bool? ContributorDatails { get; set; }

            [Parameters("include_entities")]
            public bool? IncludeEntities { get; set; }
        }
        
    }
}
