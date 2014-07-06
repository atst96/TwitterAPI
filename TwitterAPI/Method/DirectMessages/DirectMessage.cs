using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TwitterAPI;
using System.Net;
using Core;
using System.Security.Cryptography;
using System.IO;

namespace TwitterAPI
{
	[DataContract]
	public class TwitterDirectMessage
	{
		/// <summary>
		/// ダイレクトメッセージのID
		/// </summary>
		[DataMember(Name = "id")]
		public decimal Id { get; set; }

		[DataMember(Name = "created_at")]
		private string created_at { get; set; }

		public DateTime CreateDate
		{
			get
			{
				return DateTime.ParseExact(created_at, "ddd MMM dd HH':'mm':'ss zz'00' yyyy",
					System.Globalization.DateTimeFormatInfo.InvariantInfo,
					System.Globalization.DateTimeStyles.None);
			}
		}

		/// <summary>
		/// ダイレクトメッセージのID(文字列)
		/// </summary>
		[DataMember(Name = "id_str")]
		public string StringId { get; set; }

		/// <summary>
		/// ダイレクトメッセージの受信者
		/// </summary>
		[DataMember(Name = "recipient")]
		public TwitterUser Recipient { get; set; }

		/// <summary>
		/// ダイレクトメッセージの送信者
		/// </summary>
		[DataMember(Name = "sender")]
		public TwitterUser Sender { get; set; }

		/// <summary>
		/// ダイレクトメッセージの内容
		/// </summary>
		[DataMember(Name = "text")]
		public string Text { get; set; }


		[DataMember(Name = "sender_id")]
		public decimal SenderId { get; set; }

		[DataMember(Name = "sender_id_str")]
		public string SenderStringId { get; set; }

		[DataMember(Name = "sender_screen_name")]
		public string SenderScreenName { get; set; }

		[DataMember(Name = "recipient_id")]
		public decimal RecipientId { get; set; }

		[DataMember(Name = "recipient_id_str")]
		public string RecipientStringId { get; set; }

		[DataMember(Name = "recipient_screen_name")]
		public string RecipientScreenName { get; set; }

		[DataMember(Name = "entities")]
		public TwitterStatus.TweetEntities Entities { get; set; }


		public static TwitterResponse<TwitterDirectMessageCollection> DirectMessage(OAuthTokens tokens, DirectMessageOptions options)
		{
			return new TwitterResponse<TwitterDirectMessageCollection>(Method.Get(UrlBank.DirectMessages, tokens, options));
		}

		public static TwitterResponse<TwitterDirectMessageCollection> DirectMessageSent(OAuthTokens tokens, DirectMessageOptions options)
		{
			return new TwitterResponse<TwitterDirectMessageCollection>(Method.Get(UrlBank.DirectMessagesSent, tokens, options));
		}

		public static TwitterResponse<TwitterDirectMessage> Show(OAuthTokens tokens, decimal id)
		{
			return new TwitterResponse<TwitterDirectMessage>(Method.Get(string.Format(UrlBank.DirectMessageShow, id), tokens, null));
		}

		public static TwitterResponse<TwitterDirectMessage> Destroy(OAuthTokens tokens, decimal id, bool IncludeEntities = false)
		{
			return new TwitterResponse<TwitterDirectMessage>(Method.GenerateResponseResult(Method.GenerateWebRequest(string.Format("{0}?id={1}", UrlBank.DirectMessagesDestroy, id), WebMethod.POST, tokens, null, "application/x-www-form-urlencoded", null, null)));
		}

		public static TwitterResponse<TwitterDirectMessage> New(OAuthTokens tokens, string ScreenName, string Text)
		{
			string data = string.Format("text={0}&screen_name={1}", Uri.EscapeDataString(Text), Uri.EscapeDataString(ScreenName));

			return new TwitterResponse<TwitterDirectMessage>(Method.GenerateResponseResult(Method.GenerateWebRequest("https://api.twitter.com/1.1/direct_messages/new.json?" + data, WebMethod.POST, tokens, null, "application/x-www-form-urlencoded", null, null)));
		}

		public static TwitterResponse<TwitterDirectMessage> New(OAuthTokens tokens, decimal UserId, string Text)
		{
			string data = string.Format("text={0}&screen_name={1}", Uri.EscapeDataString(Text), UserId);

			return new TwitterResponse<TwitterDirectMessage>(Method.GenerateResponseResult(Method.GenerateWebRequest("https://api.twitter.com/1.1/direct_messages/new.json?" + data, WebMethod.POST, tokens, null, "application/x-www-form-urlencoded", null, null)));
		}

		private class DMDestroyOption : ParameterClass
		{
			[Parameters("id",  ParameterMethodType.POSTData)]
			public decimal Id { get; set; }

			[Parameters("include_entities")]
			public bool? IncludeEntities { get; set; }
		}
	}

	public class TwitterDirectMessageCollection : List<TwitterDirectMessage> { }
}
