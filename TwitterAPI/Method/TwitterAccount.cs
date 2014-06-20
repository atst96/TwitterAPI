using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    public abstract partial class TwitterAccount
    {

		public static TwitterResponse<TwitterSettings> Settings(OAuthTokens tokens)
		{
			return new TwitterResponse<TwitterSettings>(Method.Get(UrlBank.AccountSettings, tokens));
		}

		public static TwitterResponse<TwitterUser> VerifyCredentials(OAuthTokens tokens)
		{
			return new TwitterResponse<TwitterUser>(Method.Get(UrlBank.AccountVerifyCredentails, tokens));
		}

		public static TwitterResponse<TwitterUser> UpdateProfile(OAuthTokens tokens, UpdateProfileOption option)
		{
			return new TwitterResponse<TwitterUser>(Method.Post(UrlBank.AccountUpdateProfile, tokens, option, "application/x-www-form-urlencoded", null, null));
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
