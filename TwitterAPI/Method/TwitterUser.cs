using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TwitterAPI
{
	[DataContract]
	public class TwitterUser
	{
		[DataMember(Name = "id")]
		public decimal Id { get; set; }

		[DataMember(Name = "id_str")]
		public string StringId { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "screen_name")]
		public string ScreenName { get; set; }

		[DataMember(Name = "location")]
		public string Location { get; set; }

		[DataMember(Name = "description")]
		public string Description { get; set; }

		[DataMember(Name = "url")]
		public string Url { get; set; }

		[DataMember(Name = "entities", IsRequired = false)]
		public user_entities Entities { get; set; }

		[DataContract]
		public class user_entities
		{
			[DataMember(Name = "url")]
			public TwitterUser_Urls Url { get; set; }

			[DataMember(Name = "description")]
			public TwitterUser_Urls Description { get; set; }

			[DataContract]
			public class TwitterUser_Urls
			{
				[DataMember(Name = "urls")]
				public List<TwitterStatus.TweetEntities.Entities_Urls> Urls { get; set; }
			}
		}

		[DataMember(Name = "protected")]
		public bool Protected { get; set; }

		[DataMember(Name = "followers_count")]
		public int FollowersCount { get; set; }

		[DataMember(Name = "friends_count")]
		public int FriendsCount { get; set; }

		[DataMember(Name = "listed_count")]
		public int? ListedCount { get; set; }

		[DataMember(Name = "created_at")]
		private string created_at { get; set; }

		public DateTime CreateDate { get { return DateTime.ParseExact(created_at, "ddd MMM dd HH':'mm':'ss zz'00' yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None); } }

		[DataMember(Name = "favourites_count")]
		public decimal FavoritesCount { get; set; }

		[DataMember(Name = "utc_offset")]
		public string UtcOffset { get; set; }

		[DataMember(Name = "time_zone")]
		public string TimeZone { get; set; }

		[DataMember(Name = "geo_enabled")]
		public bool GetEnabled { get; set; }

		[DataMember(Name = "verified")]
		public bool Verified { get; set; }

		[DataMember(Name = "statuses_count")]
		public decimal StatusCount { get; set; }

		[DataMember(Name = "lang")]
		public string Language { get; set; }

		[DataMember(Name = "contributors_enabled")]
		public bool ContributorsEnabled { get; set; }

		[DataMember(Name = "is_translator")]
		public bool IsTranslator { get; set; }

		[DataMember(Name = "profile_background_color")]
		private string profile_background_color { get; set; }

		public Color ProfileBackgroundColor { get { return ColorTranslator.FromHtml("#" + profile_background_color); } }

		[DataMember(Name = "profile_background_image_url")]
		public string ProfileBackgroundImageUrl { get; set; }

		[DataMember(Name = "profile_background_image_url_https")]
		public string ProfileBackgroundImageUrl_Https { get; set; }

		[DataMember(Name = "profile_background_tile")]
		public bool ProfileBackgroundTile { get; set; }

		[DataMember(Name = "profile_image_url")]
		public string ProfileImageUrl { get; set; }

		[DataMember(Name = "profile_image_url_https")]
		public string ProfileImageUrl_Https { get; set; }

		[DataMember(Name = "profile_banner_url")]
		public string ProfileBannerUrl { get; set; }

		[DataMember(Name = "profile_link_color")]
		private string profile_link_color { get; set; }

		public Color ProfileLinkColor { get { return ColorTranslator.FromHtml("#" + profile_link_color); } }

		[DataMember(Name = "profile_sidebar_border_color")]
		private string profile_sidebar_border_color { get; set; }

		public Color ProfileSidebarBorderColor { get { return ColorTranslator.FromHtml("#" + profile_sidebar_border_color); } }

		[DataMember(Name = "profile_sidebar_fill_color")]
		private string profile_sidebar_fill_color { get; set; }

		public Color ProfileSidebarFillColor { get { return ColorTranslator.FromHtml("#" + profile_sidebar_fill_color); } }

		[DataMember(Name = "profile_text_color")]
		private string profile_text_color { get; set; }

		public Color ProfileTextColor { get { return ColorTranslator.FromHtml("#" + profile_text_color); } }

		[DataMember(Name = "profile_use_background_image")]
		public bool ProfileUseBackgroundImag { get; set; }

		[DataMember(Name = "default_profile")]
		public bool DefaultProfile { get; set; }

		[DataMember(Name = "default_profile_image")]
		public bool DefaultProfileImage { get; set; }

		[DataMember(Name = "following")]
		public bool? Following { get; set; }

		[DataMember(Name = "follow_request_sent")]
		public bool? FollowRequestSend { get; set; }

		[DataMember(Name = "notifications")]
		public bool? Notifications { get; set; }

		//public static 

		public static TwitterResponse<TwitterUsers> BlocksList(OAuthTokens tokens, UserListOption option = null)
		{
			return new TwitterResponse<TwitterUsers>(Method.Get(UrlBank.BlocksList, tokens, option));
		}

		public static TwitterResponse<UserIds> BlocksIds(OAuthTokens tokens, CursorOption option = null)
		{
			return new TwitterResponse<UserIds>(Method.Get(string.Format("{0}?stringify_ids=false", UrlBank.BlocksIds), tokens, option));
		}

		public static TwitterResponse<TwitterUser> Show(OAuthTokens tokens, UsersShowOption option)
		{
			return new TwitterResponse<TwitterUser>(Method.Get(UrlBank.UsersShow, tokens, option));
		}

		public static TwitterResponse<UserIds> MutesIds(OAuthTokens tokens, CursorOption option = null)
		{
			return new TwitterResponse<UserIds>(Method.Get(UrlBank.MutesUsersIds, tokens, option));
		}

		public static TwitterResponse<TwitterUsers> MutesList(OAuthTokens tokens, UserListOption option = null)
		{
			return new TwitterResponse<TwitterUsers>(Method.Get(UrlBank.MutesUsersList, tokens, option));
		}
	}

	[DataContract]
	public class TwitterUsers
	{
		[Parameters("previous_cursor")]
		public int PreviousCursor { get; set; }

		[Parameters("users")]
		public List<TwitterUser> Users { get; set; }

		[Parameters("previous_cursor_str")]
		public string StringPreviousCursor { get; set; }

		[Parameters("next_cursor")]
		public int NextCursor { get; set; }

		[Parameters("next_cursor_str")]
		public string StringNextCursor { get; set; }

	}

	public class UserListOption : ParameterClass
	{
		[Parameters("include_entities")]
		public bool? IncludeEntities { get; set; }

		[Parameters("skip_status")]
		public bool? SkipStatus { get; set; }

		[Parameters("cursor")]
		public int? Cursor { get; set; }
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
}
