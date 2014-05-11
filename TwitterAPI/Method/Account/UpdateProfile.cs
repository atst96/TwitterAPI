using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace TwitterAPI
{
	public abstract partial class TwitterAccount
	{
		private static string Update_Profile_Url = "https://api.twitter.com/1.1/account/update_profile.json";

		public static TwitterResponse<TwitterUser> UpdateProfile(OAuthTokens tokens, UpdateProfileOption option)
		{
			return new TwitterResponse<TwitterUser>(Method.Post(Update_Profile_Url, tokens, option, "application/x-www-form-urlencoded", null, null, null));
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
