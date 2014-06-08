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
			var data = string.Format("id={0}", id);
			var __data__ = UTF8Encoding.UTF8.GetBytes(data);

			#region test
			var oauth = new OAuthBase();

			string addr = UrlBank.DirectMessagesDestroy, timestamp = oauth.GenerateTimeStamp(), nonce = oauth.GenerateNonce(), normalizedUrl, normalizedReqParams, signature;


			HttpWebRequest req = null;

			// シグネチャ

			req = HttpWebRequest.Create(addr) as HttpWebRequest;

			string signatureBase = oauth.GenerateSignatureBase(new Uri(addr), tokens.ConsumerKey, tokens.AccessToken, tokens.AccessTokenSecret, "POST", timestamp, nonce, "HMAC-SHA1", out normalizedUrl, out normalizedReqParams)/* + (!string.IsNullOrEmpty(data) ? Uri.EscapeDataString("&" + data) : "")*/,
				compositKey = string.Concat(Uri.EscapeDataString(tokens.ConsumerSecret), "&", Uri.EscapeDataString(tokens.AccessTokenSecret)), oauthSignature;
			using (var hasher = new HMACSHA1(UTF8Encoding.UTF8.GetBytes(compositKey))) oauthSignature = Convert.ToBase64String(hasher.ComputeHash(UTF8Encoding.UTF8.GetBytes(signatureBase)));

			signature = string.Format("OAuth {0}\", oauth_signature=\"{1}\"", normalizedReqParams.Replace("=", "=\"").Replace("&", "\", "), Uri.EscapeDataString(oauthSignature));
			req.ContentType = "application/x-www-form-urlencoded";
			req.Method = "POST";
			req.Headers.Add(HttpRequestHeader.Authorization, signature);

			if (data != null) using (Stream stream = req.GetRequestStream()) stream.Write(__data__, 0, __data__.Length);



			ServicePointManager.Expect100Continue = false;

			#endregion

			return new TwitterResponse<TwitterDirectMessage>(Method.GenerateResponseResult(req));

			/*var option = new DMDestroyOption { Id = id/*, IncludeEntities = IncludeEntities };
			var data = string.Format("id={0}", id);
			System.Diagnostics.Debug.WriteLine(data);
			return new TwitterResponse<TwitterDirectMessage>(Method.Post("https://api.twitter.com/1.1/direct_messages/destroy.json?id=" + id
				, tokens, null, "application/x-www-form-urlencoded", data, UTF8Encoding.UTF8.GetBytes(data)));*/
		}

		public static TwitterResponse<TwitterDirectMessage> New(OAuthTokens tokens, string ScreenName, string Text)
		{
			string data = string.Format("text={0}&screen_name={1}", Method.UrlEncode(Text), ScreenName);
			System.Diagnostics.Debug.WriteLine(data);
			return new TwitterResponse<TwitterDirectMessage>(Method.GenerateResponseResult(Method.GenerateWebRequest(
				"https://api.twitter.com/1.1/direct_messages/new.json", WebMethod.POST, tokens, null, "application/x-www-form-urlencoded", data, UTF8Encoding.UTF8.GetBytes(data))));
		}

		private class DMDestroyOption : ParameterClass
		{
			[Parameters("id", "PostData")]
			public decimal Id { get; set; }

			[Parameters("include_entities")]
			public bool? IncludeEntities { get; set; }
		}
	}

	public class TwitterDirectMessageCollection : List<TwitterDirectMessage> { }
}
