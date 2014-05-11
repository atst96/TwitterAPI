using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TwitterAPI;

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
            return TwitterDirectMessageCommand.DirectMessage(tokens, options);
        }

        public static TwitterResponse<TwitterDirectMessageCollection> DirectMessageSent(OAuthTokens tokens, DirectMessageOptions options)
        {
            return TwitterDirectMessageCommand.DirectMessageSent(tokens, options);
        }
    }

    public class TwitterDirectMessageCollection : List<TwitterDirectMessage> { }
}
