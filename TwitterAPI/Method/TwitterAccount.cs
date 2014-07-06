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
			[Parameters("name", ParameterMethodType.POST)]
			public string Name { get; set; }

			[Parameters("url", ParameterMethodType.POST)]
			public string Url { get; set; }

			[Parameters("location", ParameterMethodType.POST)]
			public string Location { get; set; }

			[Parameters("description", ParameterMethodType.POST)]
			public string Description { get; set; }

			[Parameters("include_entities", ParameterMethodType.POST)]
			public bool? IncludeEntities { get; set; }

			[Parameters("skip_status", ParameterMethodType.POST)]
			public bool? SkipStatus { get; set; }
		}
	}
}
