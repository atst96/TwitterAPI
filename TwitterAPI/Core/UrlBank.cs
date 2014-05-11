using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    public class UrlBank
    {
		/*public static readonly string REQUEST_TOKNE_URL = "https://api.twitter.com/oauth/request_token";
		public static readonly string AUTHORIZE_URL = "https://api.twitter.com/oauth/authorize";
		public static readonly string ACCESS_TOKEN_URL = "https://api.twitter.com/oauth/access_token";*/

        // タイムライン
        public static readonly string HOME_TIMELINE = "https://api.twitter.com/1.1/statuses/home_timeline.json";
        public static readonly string MENTIONS_TIMELINE = "https://api.twitter.com/1.1/statuses/mentions_timeline.json";

        // DirectMessage
        public static readonly string DIRECT_MESSAGE = "https://api.twitter.com/1.1/direct_messages.json";
        public static readonly string DIRECT_MESSAGE_SHOW = "https://api.twitter.com/1.1/direct_messages/show.json";

        public static readonly string SEARCH_TWEETS = "https://api.twitter.com/1.1/search/tweets.json";

        // Straming
        public static readonly string STREAMING_USER = "https://userstream.twitter.com/1.1/user.json";

        //
        

        // Favorites
        public static readonly string FAVORITES_CREATE = "https://api.twitter.com/1.1/favorites/create.json";

        public static readonly string PATTERN_AUTHORIZE = "https://api.twitter.com/oauth/authorize?oauth_token={0}&oauth_token_secret={1}";
    }
}
