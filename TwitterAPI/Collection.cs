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

	[DataContract]
	public class TwitterSettings
	{
		[DataMember(Name = "always_use_https")]
		public bool AlwaysUseHttps { get; set; }

		[DataMember(Name = "discoverable_by_email")]
		public bool DiscoverableByEmail { get; set; }

		[DataMember(Name = "geo_enabled")]
		public bool IsGeoEnabled { get; set; }

		[DataMember(Name = "language")]
		public string Language { get; set; }

		[DataMember(Name = "protected")]
		public bool Protected { get; set; }

		[DataMember(Name = "screen_name")]
		public string ScreenName { get; set; }

		[DataMember(Name = "show_all_inline_media")]
		public bool ShowAllInlineMedia { get; set; }

		[DataMember(Name = "time_zone")]
		public TimeZone TimeZone { get; set; }

		[DataMember(Name = "trend_location")]
		public List<TwitterTrendWoeId> TrendeLocation { get; set; }

		[DataMember(Name = "use_cookie_personalization")]
		public bool UseCookiePersonalization { get; set; }
	}

	[DataContract]
	public class SleepTime
	{
		[DataMember(Name = "enabled")]
		public bool? IsEnabled { get; set; }

		[DataMember(Name = "end_time")]
		public string EndTime { get; set; }

		[DataMember(Name = "start_time")]
		public string StartTime { get; set; }
	}

	[DataContract]
	public class TimeZone
	{
		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "tzinfo_name")]
		public string TzinfoName { get; set; }

		[DataMember(Name = "utc_offset")]
		public int UtcOffset { get; set; }
	}
}
