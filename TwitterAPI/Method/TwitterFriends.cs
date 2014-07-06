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

		/// <summary>
		/// リツイートを非表示にしているユーザー一覧を表示します
		/// </summary>
		public static TwitterResponse<List<decimal>> NoRetweetsIds(OAuthTokens tokens)
		{
			return new TwitterResponse<List<decimal>>(Method.Get(string.Format("{0}?stringify_ids=false", UrlBank.FriendshipsNoRetweetsIds), tokens));
		}

		public static TwitterResponse<FriendsIds> FriendsIds(OAuthTokens tokens, int Cursor = 0)
		{
			return new TwitterResponse<FriendsIds>(Method.Get(UrlBank.FriendsIds, tokens));
		}

		public static TwitterResponse<FriendsIds> FollowersIds(OAuthTokens tokens, int Cursor = 0)
		{
			return new TwitterResponse<FriendsIds>(Method.Get(UrlBank.FollowersIds, tokens));
		}

		public static TwitterResponse<UserIds> Incoming(OAuthTokens tokens, CursorOption option = null)
		{
			return new TwitterResponse<UserIds>(Method.Get(string.Format("{0}?stringify_ids=false", UrlBank.FriendshipsIncoming), tokens));
		}

		public static TwitterResponse<UserIds> Outgoing(OAuthTokens tokens, CursorOption option = null)
		{
			return new TwitterResponse<UserIds>(Method.Get(string.Format("{0}?stringify_ids=false", UrlBank.FriendshiptsOutgoing), tokens));
		}


		/// <summary>
		/// フォローする
		/// </summary>
		/// <param name="tokens">トークン</param>
		/// <param name="UserId">ユーザID</param>
		/// <param name="Follow">フォロー通知の有無</param>
		public static TwitterResponse<TwitterUser> Create(OAuthTokens tokens, decimal UserId, bool Follow = true)
		{
			var data = string.Format("user_id={0}&follow={1}", UserId, Follow.ToString().ToLower());
			return new TwitterResponse<TwitterUser>(Method.Post(string.Format("{0}?{1}", UrlBank.FriendshipsCreate, data), tokens, null, "application/x-www-form-urlencoded", null, null));
		}


		/// <summary>
		/// フォローする
		/// </summary>
		/// <param name="tokens">トークン</param>
		/// <param name="UserId">ScreenName</param>
		/// <param name="Follow">フォロー通知の有無</param>
		public static TwitterResponse<TwitterUser> Create(OAuthTokens tokens, string ScreenName, bool Follow = true)
		{
			var data = string.Format("screen_name={0}&follow={1}", ScreenName, Follow.ToString().ToLower());
			return new TwitterResponse<TwitterUser>(Method.Post(string.Format("{0}?{1}", UrlBank.FriendshipsCreate, data), tokens, null, "application/x-www-form-urlencoded", null, null));
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

		public static TwitterResponse<TwitterRelationship> Update(OAuthTokens tokens, FriendshipsUpdateOption option)
		{
			return new TwitterResponse<TwitterRelationship>(Method.Post(UrlBank.FriendshipsUpdate, tokens, option, "application/x-www-form-urlencoded", null, null));
		}

		public static TwitterResponse<TwitterRelationship> Show(OAuthTokens tokens, FriendshipShowOption option)
		{
			return new TwitterResponse<TwitterRelationship>(Method.Get(UrlBank.FriendshipsShow, tokens, option));
		}

		public static TwitterResponse<TwitterUserCollection> FriendsList(OAuthTokens tokens)
		{
			return new TwitterResponse<TwitterUserCollection>(Method.Get(UrlBank.FriendsList, tokens));
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

	public class FriendshipsUpdateOption : ParameterClass
	{
		[Parameters("screen_name")]
		public string ScreenName { get; set; }

		[Parameters("user_id")]
		public decimal? UserId { get; set; }

		[Parameters("device")]
		public bool? Device { get; set; }

		[Parameters("retweets")]
		public bool? Retweets { get; set; }
	}

	public class FriendshipShowOption : ParameterClass
	{
		[Parameters("source_id")]
		public decimal? SourceId { get; set; }

		[Parameters("source_screen_name")]
		public string SourceScreenName { get; set; }

		[Parameters("target_id")]
		public decimal? TargetId { get; set; }

		[Parameters("target_screen_name")]
		public string TargetScreenName { get; set; }
	}

	[DataContract]
	public class TwitterRelationship
	{
		[DataMember(Name = "target")]
		public _Target Target { get; set; }

		[DataMember(Name = "source")]
		public _Source Source { get; set; }


		[DataContract]
		public class _Target
		{
			[DataMember(Name = "target")]
			public string StringId { get; set; }

			[DataMember(Name = "id")]
			public decimal Id { get; set; }

			[DataMember(Name = "followed_by")]
			public bool FollowedBy { get; set; }

			[DataMember(Name = "screen_name")]
			public string ScreenName { get; set; }

			[DataMember(Name = "following")]
			public bool Following { get; set; }
		}

		[DataContract]
		public class _Source : _Target
		{
			[DataMember(Name = "can_dm")]
			public bool? CanDM { get; set; }

			[DataMember(Name = "blocking")]
			public bool? Blocking { get; set; }

			[DataMember(Name = "muting")]
			public bool? Muting { get; set; }

			[DataMember(Name = "all_replies")]
			public bool? AllReplies { get; set; }

			[DataMember(Name = "want_retweets")]
			public bool? WantRetweets { get; set; }

			[DataMember(Name = "marked_spam")]
			public bool? MarkedSpam { get; set; }

			[DataMember(Name = "notifications_enabled")]
			public bool? NotificationsEnabled { get; set; }
		}
	}


}
