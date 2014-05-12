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

		private static string Trends_Closest_Url = "https://api.twitter.com/1.1/trends/closest.json";

        public static TwitterResponse<TwitterTrendWoeIdCollection> Available(OAuthTokens tokens)
        {
            return new TwitterResponse<TwitterTrendWoeIdCollection>(Method.Get(Trends_Available_Url, tokens));
        }

        public static TwitterResponse<TwitterTrendCollection> Place(OAuthTokens tokens, decimal id)
        {
            return new TwitterResponse<TwitterTrendCollection>(Method.Get(Trends_Place_Url, tokens, new TwitterTrendPlaceOptions { Id = id }));
        }

		public static TwitterResponse<TwitterTrendCollection> Closest(OAuthTokens tokens, decimal Lat, decimal Long)
		{
			return new TwitterResponse<TwitterTrendCollection>(Method.Get(Trends_Closest_Url, tokens, new TwitterTrendClosestOptions { Lat = Lat, Long = Long }));
		}

        public class TwitterTrendPlaceOptions : ParameterClass
        {
            [Parameters("id")]
            public decimal Id { get; set; }
        }

		public class TwitterTrendClosestOptions : ParameterClass
		{
			[Parameters("lat")]
			public decimal Lat { get; set; }

			[Parameters("long")]
			public decimal Long { get; set; }
		}
    }
}
