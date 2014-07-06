using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace TwitterAPI
{

    public class RateLimited
    {
        public RateLimited() { }

        public RateLimited(string LimitLimit, string LimitRemaining, string LimitReset)
        {
            int pars;
			if (int.TryParse(LimitRemaining, out pars)) Limit = int.Parse(LimitRemaining);
            if (int.TryParse(LimitLimit, out pars)) Max = int.Parse(LimitLimit);
			if (!string.IsNullOrEmpty(LimitReset))
			{
				ResetDate = DateTime.Now;
				ResetDate.AddSeconds(double.Parse(LimitReset));
				ResetDate = System.TimeZone.CurrentTimeZone.ToLocalTime(ResetDate);
			}
        }

        public int Limit { get; private set; }
        public int Max { get; private set; }
        public DateTime ResetDate { get; private set; }
    }

    public class TwitterResponse<T>
    {
        public T ResponseObject { get; set; }

        public StatusResult Result { get; set; }

        public string RequestUrl { get; set; }

        internal OAuthTokens Tokens { get; set; }

        public RateLimited RateLimited { get; set; }

        public AccessLevel AccessLevel { get; set; }

        public TwitterError Error { get; set; }

		private string Source { get; set; }

		public override string ToString()
		{
			return Source;
		}

        public TwitterResponse() { }

        public TwitterResponse(ResponseResult res, bool SetObject = true)
        {
            if (res != null)
            {
                this.Result = res.Result;
                this.RequestUrl = res.Url;
                this.RateLimited = res.RateLimited;
                this.AccessLevel = res.AccessLevel;
                this.Error = res.Error;
                Source = res.ResponseStream;

                if (SetObject)
                {
                    if (res.Result == StatusResult.Success)
                    {
                        try { this.ResponseObject = JsonConvert.DeserializeObject<T>(res.ResponseStream); }
                        catch(Exception) { this.Result = StatusResult.ParseError; }
                    }
				}
            }
        }
    }

}
