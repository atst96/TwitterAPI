﻿using System;
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

		public static TwitterResponse<TwitterUserCollection> FollowersList(OAuthTokens tokens)
		{
			return new TwitterResponse<TwitterUserCollection>(Method.Get(UrlBank.FollowersList, tokens));
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
