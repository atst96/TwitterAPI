using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitterAPI
{
    public abstract partial class TwitterFriendships
    {
        private static string Followers_List = "https://api.twitter.com/1.1/followers/list.json";

        public static TwitterResponse<TwitterUserCollection> FollowersList(OAuthTokens tokens)
        {
            var res = Method.Get(Followers_List, tokens);

            return new TwitterResponse<TwitterUserCollection>(res);
        }

    }
}
