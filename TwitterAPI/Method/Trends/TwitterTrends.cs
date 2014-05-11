using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TwitterAPI
{
    public abstract partial class TwitterTrends
    {
        private static string Trends_Available_Url = "https://api.twitter.com/1.1/trends/available.json";

        private static string Trends_Place_Url = "https://api.twitter.com/1.1/trends/place.json";

        public static TwitterResponse<TwitterTrendWoeIdCollection> Available(OAuthTokens tokens)
        {
            if(tokens == null) throw new ArgumentNullException("Tokens");

            var res = Method.Get(Trends_Available_Url, tokens);

            
            TwitterResponse<TwitterTrendWoeIdCollection> status = new TwitterResponse<TwitterTrendWoeIdCollection>(res);

            if (res.Result == StatusResult.Success)
            {
                try
                {
                    status.ResponseObject = JsonConvert.DeserializeObject<TwitterTrendWoeIdCollection>(res.ResponseStream);
                }
                catch
                {
                    status.ResponseObject = null;
                    status.Result = StatusResult.ParseError;
                }
            }

            return status;
        }

        public static TwitterResponse<TwitterTrendCollection> Place(OAuthTokens tokens, decimal id)
        {
            if (tokens == null) throw new ArgumentNullException("Tokens");

            var res = Method.Get(Trends_Place_Url, tokens, new TwitterTrendOptions { Id = id });

            return new TwitterResponse<TwitterTrendCollection>(res);
        }

        public class TwitterTrendOptions : ParameterClass
        {
            [Parameters("id")]
            public decimal Id { get; set; }
        }
    }
}
