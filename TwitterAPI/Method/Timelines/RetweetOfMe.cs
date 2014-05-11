using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    public static partial class TwitterTimeline
    {
        /// <summary>
        /// MentionsTimelineの取得
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <returns>TwitterStatusCollection</returns>
        public static TwitterResponse<TwitterStatusCollection> RetweetOfMe(OAuthTokens tokens)
        {
            return GetTimeline(Url_Retweet_Of_Me, tokens);
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
    }
}
