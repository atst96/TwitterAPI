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

			Dictionary<string, string> dic = new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(option.Name)) dic.Add("name", option.Name);
			if (!string.IsNullOrEmpty(option.Url)) dic.Add("url", option.Url);
			if (!string.IsNullOrEmpty(option.Location)) dic.Add("Location",option.Location);
			if (!string.IsNullOrEmpty(option.Description)) dic.Add("description", option.Description);
			if (option.IncludeEntities == true) dic.Add("include_entities", "true");
			List<string> param = new List<string>();
			foreach (var d in dic) param.Add(string.Format("{0}={1}", d.Key, d.Value));

			var result = Method.Post(Update_Profile_Url, tokens, "", "application/x-www-form-urlencoded", dic);

			return new TwitterResponse<TwitterUser>(result);
		}

		public class UpdateProfileOption : ParameterClass
		{
            [Parameters("name","POST")]
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
