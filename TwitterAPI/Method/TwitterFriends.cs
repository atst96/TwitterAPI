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

		public static TwitterResponse<FriendsIds> FriendsIds(OAuthTokens tokens, int Cursor = 0)
		{
			return new TwitterResponse<FriendsIds>(Method.Get(UrlBank.FriendsIds, tokens));
		}

		public static TwitterResponse<FriendsIds> FollowersIds(OAuthTokens tokens, int Cursor = 0)
		{
			return new TwitterResponse<FriendsIds>(Method.Get(UrlBank.FollowersIds, tokens));
		}

		public static TwitterResponse<TwitterUser> Create(OAuthTokens tokens, decimal UserId, bool Follow = true)
		{
			var data = string.Format("user_id={0}", UserId, Follow ? "true" : "false");
			return new TwitterResponse<TwitterUser>(Method.Post(UrlBank.FriendshipsCreate, tokens, null, "application/x-www-form-urlencoded", data, Encoding.UTF8.GetBytes(data)));
		}

		public static TwitterResponse<TwitterUser> Create(OAuthTokens tokens, string ScreenName, bool Follow = true)
		{
			var data = string.Format("screen_name={0}", ScreenName, Follow ? "true" : "false");
			return new TwitterResponse<TwitterUser>(Method.Post(UrlBank.FriendshipsCreate, tokens, null, "application/x-www-form-urlencoded", data, Encoding.UTF8.GetBytes(data)));
		}

		public static TwitterResponse<TwitterUser> Destroy(OAuthTokens tokens, decimal UserId)
		{
			var data = string.Format("user_id={0}", UserId);
			return new TwitterResponse<TwitterUser>(Method.Post(UrlBank.FriendshipsDestroy, tokens, null, "application/x-www-form-urlencoded", data, Encoding.UTF8.GetBytes(data)));
		}

		public static TwitterResponse<TwitterUser> Destroy(OAuthTokens tokens, string ScreenName)
		{
			var data = string.Format("screen_name={0}", ScreenName);
			return new TwitterResponse<TwitterUser>(Method.Post(UrlBank.FriendshipsDestroy, tokens, null, "application/x-www-form-urlencoded", data, Encoding.UTF8.GetBytes(data)));
		}

		public static TwitterResponse<TwitterUserCollection> FollowersList(OAuthTokens tokens)
		{
			return new TwitterResponse<TwitterUserCollection>(Method.Get(UrlBank.FollowersList, tokens));
		}

		public static TwitterResponse<TwitterUserCollection> Lookup(OAuthTokens tokens, List<string> ScreenNames)
		{
			var url = UrlBank.FriendshipsLookup + "?screen_name=" + string.Join(",", ScreenNames);
			return new TwitterResponse<TwitterUserCollection>(Method.Get(url, tokens, null));
		}

		public static TwitterResponse<TwitterUserCollection> Lookup(OAuthTokens tokens, List<decimal> UserIds)
		{
			var url = UrlBank.FriendshipsLookup + "?user_id=" + string.Join(",", UserIds);
			return new TwitterResponse<TwitterUserCollection>(Method.Get(url, tokens, null));
		}
    }

	public class FriendsIds : ParameterClass
	{
		[Parameters("previous_cursor")]
		public int PreviousCursor { get; set; }

		[Parameters("ids")]
		public List<decimal> IDs { get; set; }

		[Parameters("previous_cursor_str")]
		public string StringPreviousCursor { get; set; }

		[Parameters("next_cursor")]
		public int NextCursor { get; set; }

		[Parameters("next_cursor_str")]
		public string StringNextCursor { get; set; }
	}
}
