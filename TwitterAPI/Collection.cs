using System;
using System.Drawing;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
    }

    [DataContract]
    public class TwitterUserCollection : List<TwitterUser> { }    

    [DataContract]
    public class TwitterSearchCollection
    {
        [DataMember(Name = "statuses")]
        public List<TwitterStatus> Statuses { get; set; }

        [DataMember(Name = "search_metadata")]
        public MetaData SearchMetadata { get; set; }

        [JsonObject]
        public class MetaData
        {
            [DataMember(Name = "max_id")]
            public decimal MaxId { get; set; }

            [DataMember(Name = "since_id")]
            public decimal SinceId { get; set; }

            [DataMember(Name = "refresh_url")]
            public string RefreshUrl { get; set; }

            [DataMember(Name = "next_results")]
            public string NextResults { get; set; }

            [DataMember(Name = "couont")]
            public int Count { get; set; }

            [DataMember(Name = "complated_in")]
            public double ComplatedIn { get; set; }

            [DataMember(Name = "since_id_str")]
            public string StringSinceId { get; set; }

            [DataMember(Name = "query")]
            public string Query { get; set; }

            [DataMember(Name = "max_id_str")]
            public string StringMaxId { get; set; }
        }
    }

    [DataContract(Name = "delete")]
    public class TwitterDeletedStatus
    {
        [DataMember(Name = "id")]
        public decimal Id { get; set; }

        [DataMember(Name = "user_id")]
        public decimal UserId { get; set; }

        [DataMember(Name = "id_str")]
        public string StringId { get; set; }

        [DataMember(Name = "user_id_str")]
        public string StringUserId { get; set; }
    }

    [DataContract]
    public class FriendsList
    {
        [DataMember(Name = "friends", IsRequired = false)]
        public List<decimal> Friends { get; set; }
    }

	[DataContract]
    public class StreamEventStatus
    {
		[DataMember(Name = "event")]
        public StreamEventType EventType { get; set; }

		[DataMember(Name = "source")]
		public TwitterUser Source { get; set; }

		[DataMember(Name = "target")]
        public TwitterUser Target { get; set; }

        public TwitterStatus TargetObject { get; set; }
		public TwitterList TargetObject_List { get; set; }

		[DataMember(Name = "created_at")]
		private string created_at { get; set; }

		public DateTime CreateDate
		{
			get
			{
				return DateTime.ParseExact(created_at, "ddd MMM dd HH':'mm':'ss zz'00' yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None);
			}
		}
    }


    [DataContract]
    public class UpdateImageJsonReponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "width")]
        public long Width { get; set; }

        [DataMember(Name = "height")]
        public long Height { get; set; }

        [DataMember(Name = "size")]
        public long Size { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "timestamp")]
        public string TimeStamp { get; set; }

        [DataMember(Name = "user")]
        public ImageUser User { get; set; }

        [DataContract]
        public class ImageUser
        {
            [DataMember(Name = "id")]
            public decimal Id { get; set; }

            [DataMember(Name = "screen_name")]
            public string ScreenName { get; set; }
        }
    }

    [DataContract]
    public class UpdateImageJsonResponse2
    {
        [DataMember(Name = "stat")]
        public string Status { get; set; }

        [DataMember(Name = "mediaid")]
        public string MediaId { get; set; }

        [DataMember(Name = "mediaurl")]
        public string MediaUrl { get; set; }
    }


    [XmlRoot("rsp")]
    public class UpdateImageXmlError
    {
        [XmlAttribute("stat")]
        public string Status { get; set; }

        [XmlElement("err")]
        public ErrMsg Error { get; set; }

        public class ErrMsg
        {
            [XmlAttribute("code")]
            public long Code { get; set; }

            [XmlAttribute("msg")]
            public string Message { get; set; }
        }
    }

    [XmlRoot("rsp")]
    public class UpdateImageXmlResponse
    {
        [XmlAttribute("stat")]
        public string Status { get; set; }

        [XmlElement("mediaid")]
        public string MediaId { get; set; }

        [XmlElement("mediaurl")]
        public string MediaUrl { get; set; }
    }

	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
    public enum StreamEventType
    {
        Unknown,

        [EnumMember(Value = "block")]
        Block,

		[EnumMember(Value = "unblock")]
        UnBlock,

		[EnumMember(Value = "favorite")]
        Favorite,

		[EnumMember(Value = "unfavorite")]
        UnFavorite,

		[EnumMember(Value = "follow")]
        Follow,

		[EnumMember(Value = "unfollow")]
        UnFollow,

		[EnumMember(Value = "list_member_added")]
        ListMemberAdded,

		[EnumMember(Value = "list_member_removed")]
        ListMemberRemoved,

		[EnumMember(Value = "list_user_subscribed")]
        ListUserSubscribed,

		[EnumMember(Value = "list_user_unsubscribed")]
        ListUserUnsubscribed,

		[EnumMember(Value = "list_created")]
        ListCreated,

		[EnumMember(Value = "list_updated")]
        ListUpdated,

		[EnumMember(Value = "list_destroyed")]
        ListDestroyed,

		[EnumMember(Value = "user_update")]
        UserUpdated,

		[EnumMember(Value = "access_revoked")]
        AccessRevoked,
    }

    [DataContract]
    public class TwitterTrendWoeId
    {
        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "countryCode")]
        public string CountryCode { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "parentid")]
        public double ParentId { get; set; }

        [DataMember(Name = "placeType")]
        public TrendsType Type { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "woeid")]
        public decimal WoeId { get; set; }

        [DataContract]
        public class TrendsType
        {
            [DataMember(Name = "code")]
            public int Code { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }
        }
        
    }

    public class TwitterTrendWoeIdCollection : List<TwitterTrendWoeId> { }

    [DataContract]
    public class TwitterTrend
    {
        [DataMember(Name = "events")]
        public string Events { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "promoted_content")]
        public string PromotedContent { get; set; }

        [DataMember(Name = "query")]
        public string Query { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }

    [DataContract]
    public class TwitterTrendLocations
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "woeid")]
        public decimal WoeId { get; set; }
    }

    [DataContract]
    public class TwitterTrendCollection
    {
        [DataMember(Name = "as_of")]
        public string AsOf { get; set; }

		[DataMember(Name = "created_at")]
		private string created_at { get; set; }

		public DateTime CreateDate { get { return DateTime.ParseExact(created_at, "ddd MMM dd HH':'mm':'ss zz'00' yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None); } }

        [DataMember(Name = "trends")]
        public List<TwitterTrend> Trends { get; set; }

        [DataMember(Name = "locations")]
        public List<TwitterTrendLocations> Locations { get; set; }
    }

	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ListMode
	{
		[EnumMember(Value = "public")]
		Public,

		[EnumMember(Value = "private")]
		Private,
	}

	public class TwitterListCollection : List<TwitterList> { }
}
