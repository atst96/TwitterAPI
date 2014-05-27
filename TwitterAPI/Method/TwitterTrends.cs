using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TwitterAPI
{
    public abstract partial class TwitterTrends
    {

        public static TwitterResponse<TwitterTrendWoeIdCollection> Available(OAuthTokens tokens)
        {
            return new TwitterResponse<TwitterTrendWoeIdCollection>(Method.Get(UrlBank.TrendsAvailable, tokens));
        }

        public static TwitterResponse<TwitterTrendCollection> Place(OAuthTokens tokens, decimal id)
        {
            return new TwitterResponse<TwitterTrendCollection>(Method.Get(UrlBank.TrendsPlace, tokens, new TwitterTrendPlaceOptions { Id = id }));
        }

		public static TwitterResponse<TwitterTrendCollection> Closest(OAuthTokens tokens, decimal Lat, decimal Long)
		{
			return new TwitterResponse<TwitterTrendCollection>(Method.Get(UrlBank.TrendsClosest, tokens, new TwitterTrendClosestOptions { Lat = Lat, Long = Long }));
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
