using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace TwitterAPI
{
    public abstract partial class TwitterFavorites
    {
        public static TwitterResponse<TwitterStatus> Create(OAuthTokens tokens, decimal Id, bool IncludeEntities = true)
        {
            if (tokens == null) throw new ArgumentNullException("tokens");
            if (Id < 1) throw new ArgumentNullException("StatusId");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", Id.ToString());
            if (IncludeEntities == true) dic.Add("include_entities", IncludeEntities.ToString());

            var result = Method.Post(UrlBank.FAVORITES_CREATE, tokens, "", "", dic);

            return new TwitterResponse<TwitterStatus>(result);
        }
    }


}
