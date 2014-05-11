
namespace TwitterAPI
{
    public static partial class TwitterTimeline
    {
        /// <summary>
        /// MentionsTimelineの取得
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <returns>TwitterStatusCollection</returns>
        public static TwitterResponse<TwitterStatusCollection> MentionsTimeline(OAuthTokens tokens)
        {
            return GetTimeline(Url_Mentions_Timeline, tokens);
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
    }
}
