
namespace TwitterAPI
{
    public static partial class TwitterTimeline
    {
        /// <summary>
        /// HomeTimelineの取得
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <returns>TwitterStatusCollection</returns>
        public static TwitterResponse<TwitterStatusCollection> HomeTimeline(OAuthTokens tokens)
        {
            return GetTimeline(Url_Home_Timeline, tokens);
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
    }
}
