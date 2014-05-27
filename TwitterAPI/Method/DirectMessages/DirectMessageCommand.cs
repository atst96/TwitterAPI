using System.Collections.Generic;

using Newtonsoft.Json;


namespace TwitterAPI
{
    public class TwitterDirectMessageCommand
    {

        public static TwitterResponse<TwitterDirectMessageCollection> DirectMessage(OAuthTokens tokens, DirectMessageOptions Options = null)
        {
            return new TwitterResponse<TwitterDirectMessageCollection>(Method.Get(UrlBank.DirectMessages, tokens, Options));
        }

        public static TwitterResponse<TwitterDirectMessageCollection> DirectMessageSent(OAuthTokens tokens, DirectMessageOptions Options = null)
        {
            return new TwitterResponse<TwitterDirectMessageCollection>(Method.Get(UrlBank.DirectMessagesSent, tokens, Options));
        }

		//public static TwitterResponse<TwitterDirectMessage> New(OAuthTokens tokens, )
    }


	/*public class DirectMessageNewOption : ParameterClass {

		[Parameters("text")]
	}*/
}
