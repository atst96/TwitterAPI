using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TwitterAPI
{
    public abstract partial class TwitterFriendships
    {
		private static string FRIENDS_IDS_URL = "https://api.twitter.com/1.1/friends/ids.json";
		private static string FOLLOWERS_IDS_URL = "https://api.twitter.com/1.1/followers/ids.json";

		public static TwitterResponse<FriendsIds> FriendsIds(OAuthTokens tokens, int Cursor = 0)
		{
			var res = Method.Get(FRIENDS_IDS_URL, tokens);

			return new TwitterResponse<FriendsIds>(res);
		}

		public static TwitterResponse<FriendsIds> FollowersIds(OAuthTokens tokens, int Cursor = 0)
		{
			var res = Method.Get(FOLLOWERS_IDS_URL, tokens);

			return new TwitterResponse<FriendsIds>(res);
		}
    }

	[DataContract]
	public class FriendsIds
	{
		[DataMember(Name = "previous_cursor")]
		public int PreviousCursor { get; set; }

		[DataMember(Name = "ids")]
		public List<decimal> IDs { get; set; }

		[DataMember(Name = "previous_cursor_str")]
		public string StringPreviousCursor { get; set; }

		[DataMember(Name = "next_cursor")]
		public int NextCursor { get; set; }

		[DataMember(Name = "next_cursor_str")]
		public string StringNextCursor { get; set; }
	}
}
