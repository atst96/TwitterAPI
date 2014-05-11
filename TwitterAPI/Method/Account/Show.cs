using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    using Newtonsoft.Json;

    public abstract partial class TwitterAccount
    {

        public class UsersShowOption : ParameterClass
        {
            [Parameters("user_id")]
            public decimal? UserId { get; set; }

            [Parameters("screen_name")]
            public string ScreenName { get; set; }

            [Parameters("include_entities")]
            public bool? IncludeEntities { get; set; }
        }

		public static TwitterResponse<TwitterUser> Show(OAuthTokens tokens, UsersShowOption option)
		{
			var result = Method.Get(Url_Users_Show, tokens, option);

			return new TwitterResponse<TwitterUser>(result);
		}
    }
}
