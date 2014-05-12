using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    public abstract partial class TwitterAccount
    {
        /*----------/  URLs  /----------*/
        private static string Url_Verify_Credentials = "https://api.twitter.com/1.1/account/verify_credentials.json";

		private static string Url_Users_Show = "https://api.twitter.com/1.1/users/show.json";

		private static string Update_Profile_Url = "https://api.twitter.com/1.1/account/update_profile.json";


		public static TwitterResponse<TwitterUser> VerifyCredentials(OAuthTokens tokens)
		{
			return new TwitterResponse<TwitterUser>(Method.Get(Url_Verify_Credentials, tokens));
		}

		public static TwitterResponse<TwitterUser> UpdateProfile(OAuthTokens tokens, UpdateProfileOption option)
		{
			return new TwitterResponse<TwitterUser>(Method.Post(Update_Profile_Url, tokens, option, "application/x-www-form-urlencoded", null, null));
		}

		public static TwitterResponse<TwitterUser> Show(OAuthTokens tokens, UsersShowOption option)
		{
			return new TwitterResponse<TwitterUser>(Method.Get(Url_Users_Show, tokens, option));
		}

		public class UsersShowOption : ParameterClass
		{
			[Parameters("user_id")]
			public decimal? UserId { get; set; }

			[Parameters("screen_name")]
			public string ScreenName { get; set; }

			[Parameters("include_entities")]
			public bool? IncludeEntities { get; set; }
		}

		public class UpdateProfileOption : ParameterClass
		{
			[Parameters("name", "POST")]
			public string Name { get; set; }

			[Parameters("url", "POST")]
			public string Url { get; set; }

			[Parameters("location", "POST")]
			public string Location { get; set; }

			[Parameters("description", "POST")]
			public string Description { get; set; }

			[Parameters("include_entities", "POST")]
			public bool? IncludeEntities { get; set; }

			[Parameters("skip_status", "POST")]
			public bool? SkipStatus { get; set; }
		}
	}
}
