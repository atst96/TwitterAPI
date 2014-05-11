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
            if (string.IsNullOrEmpty(Url)) throw new ArgumentNullException("Url");
            else if (tokens == null) throw new ArgumentNullException("Tokens");
            else
            {
                ResponseResult result;
                if (options != null) result = Method.Get(Url, tokens, options);
                else result = Method.Get(UrlBank.HOME_TIMELINE, tokens);

                return new TwitterResponse<TwitterStatusCollection>(result);
            }
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
