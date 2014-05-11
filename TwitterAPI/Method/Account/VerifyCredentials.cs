using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    using Newtonsoft.Json;

    public abstract partial class TwitterAccount
    {
        public static TwitterResponse<TwitterUser> VerifyCredentials(OAuthTokens tokens)
        {
            var result = Method.Get(Url_Verify_Credentials, tokens);

            return new TwitterResponse<TwitterUser>(result);
        }
    }
}
