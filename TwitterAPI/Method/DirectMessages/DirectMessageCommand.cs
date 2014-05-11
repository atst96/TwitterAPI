using System.Collections.Generic;

using Newtonsoft.Json;


namespace TwitterAPI
{
    public class TwitterDirectMessageCommand
    {

        private static string DirectMessage_Sent_Url = "https://api.twitter.com/1.1/direct_messages/sent.json";

        public static TwitterResponse<TwitterDirectMessageCollection> DirectMessage(OAuthTokens tokens, DirectMessageOptions Options = null)
        {
            return new TwitterResponse<TwitterDirectMessageCollection>(Method.Get(UrlBank.DIRECT_MESSAGE, tokens, Options));
        }

        public static TwitterResponse<TwitterDirectMessageCollection> DirectMessageSent(OAuthTokens tokens, DirectMessageOptions Options = null)
        {
            return new TwitterResponse<TwitterDirectMessageCollection>(Method.Get(DirectMessage_Sent_Url, tokens, Options));
        }

		//public static TwitterResponse<TwitterDirectMessage> New(OAuthTokens tokens, )
    }


	/*public class DirectMessageNewOption : ParameterClass {

		[Parameters("text")]
	}*/
}
