
namespace TwitterAPI
{
    public static partial class TwitterTimeline
    {
        /// <summary>
        /// UserTimelineの取得
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <returns>TwitterStatusCollection</returns>
        public static TwitterResponse<TwitterStatusCollection> UserTimeline(OAuthTokens tokens)
        {
            return GetTimeline(Url_User_Timeline, tokens);
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
    }
}
