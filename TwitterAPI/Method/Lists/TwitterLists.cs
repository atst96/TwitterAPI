using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TwitterAPI
{

	[DataContract]
	public abstract class TwitterList
	{
		private static readonly string List_Url = "https://api.twitter.com/1.1/lists/list.json";
		private static readonly string List_Statuses = "https://api.twitter.com/1.1/lists/statuses.json";

		[DataMember(Name = "created_at")]
		private string created_at { get; set; }

		public DateTime CreateDate { get { return DateTime.ParseExact(created_at, "ddd MMM dd HH':'mm':'ss zz'00' yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None); } }

		[DataMember(Name = "description")]
		public string Description { get; set; }

		[DataMember(Name = "following")]
		public bool Following { get; set; }

		[DataMember(Name = "full_name")]
		public string FullName { get; set; }

		[DataMember(Name = "id")]
		public decimal Id { get; set; }

		[DataMember(Name = "id_str")]
		public string StringId { get; set; }

		[DataMember(Name = "member_count")]
		public int CountOfMembers { get; set; }

		[DataMember(Name = "Mode")]
		public ListMode Mode { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "slug")]
		public string Slug { get; set; }

		[DataMember(Name = "subscriver_count")]
		public int CountOfSubscriver { get; set; }

		[DataMember(Name = "uri")]
		public string Uri { get; set; }

		[DataMember(Name = "user")]
		public TwitterUser User { get; set; }

		public static TwitterResponse<TwitterListCollection> List(OAuthTokens tokens, UserOptions option = null)
		{
			return new TwitterResponse<TwitterListCollection>(Method.Get(List_Url, tokens, option));
		}

		public static TwitterResponse<TwitterStatusCollection> Statuses(OAuthTokens tokens, ListsStatusesOptions option)
		{
			return new TwitterResponse<TwitterStatusCollection>(Method.Get(List_Statuses, tokens, option));
		}
	}

	public class UserOptions : ParameterClass
	{
		[Parameters("user_id")]
		decimal? UserId { get; set; }

		[Parameters("screen_name")]
		string ScreenName { get; set; }
	}

	public class ListsStatusesOptions : ParameterClass
	{
		[Parameters("list_id")]
		public decimal ListId { get; set; }

		[Parameters("slug")]
		public string Slug { get; set; }

		[Parameters("owner_screen_name")]
		public string OwnerScreenName { get; set; }

		[Parameters("owner_id")]
		public decimal? OwnerId { get; set; }

		[Parameters("since_id")]
		public decimal? SinceId { get; set; }

		[Parameters("max_id")]
		public decimal? MaxId { get; set; }

		[Parameters("count")]
		public int? Count { get; set; }

		[Parameters("include_entities")]
		public bool? IncludeEntities { get; set; }

		[Parameters("include_rts")]
		public bool? IncludeRetweets { get; set; }
	}
}
