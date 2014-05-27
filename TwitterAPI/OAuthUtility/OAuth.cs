using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitterAPI
{
    public partial class OAuthTokens
    {
        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string AccessToken { get; private set; }
        public string AccessTokenSecret { get; private set; }

        public OAuthTokens() { }

        public OAuthTokens(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            ConsumerKey = consumerKey; ConsumerSecret = consumerSecret;
            AccessToken = accessToken; AccessTokenSecret = accessTokenSecret;
        }
    }

    public partial class OAuthUtility
    {
        private static OAuthBase oauth = new OAuthBase();

        private static string timestamp;
        private static string nonce;

		/// <summary>
		/// 認証用URLの取得
		/// </summary>
		/// <param name="ConsumerKey">ConsumerKey</param>
		/// <param name="ConsumerSecret">ConsumerSecret</param>
		/// <returns>認証用のURL</returns>
		public static OAuthTokenClass GetRequestUrl(string ConsumerKey, string ConsumerSecret)
		{
			// Nonceの生成
			nonce = oauth.GenerateNonce();

			// TimeStampの生成(Unix)
			timestamp = oauth.GenerateTimeStamp();

			string normalizedUrl, normalizedReqParam;

			var reqUrl = new Uri(UrlBank.OAuthRequestToken);

			// シグネチャを生成
			string signature = oauth.GenerateSignature(reqUrl,
				ConsumerKey, ConsumerSecret, null, null, "GET", timestamp, nonce,
				OAuthBase.SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedReqParam);

			string reqTokenUrl = normalizedUrl + "?" + normalizedReqParam
				+ "&oauth_signature=" + Method.UrlEncode(signature);

			var req = WebRequest.Create(reqTokenUrl) as HttpWebRequest;

			try
			{
				var res = req.GetResponse();
				var reader = new StreamReader(res.GetResponseStream());

				string str = reader.ReadToEnd();
				var token = new OAuthTokenClass(str);

				res.Close(); reader.Close();

				return token;

			}
			catch (WebException ex)
			{
				/*using (StreamReader rdr = new StreamReader(ex.Response.GetResponseStream()))
					System.Windows.Forms.MessageBox.Show(rdr.ReadToEnd());
				throw ex;*/
				return null;
			}
		}

		public static TwitterResponse<string> GetBearer(string consumerKey, string consumerSecret)
		{
			string url = "https://api.twitter.com/oauth2/token";

			string token_credential = string.Format("{0}:{1}",
				HttpUtility.UrlEncode(consumerKey), HttpUtility.UrlEncode(consumerSecret));
			string credentail = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(token_credential));

			byte[] data = Encoding.UTF8.GetBytes("grant_type=client_credentials");

			HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
			req.Proxy = null;
			req.Method = "POST";
			req.Headers.Add("Authorization", credentail);
			req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
			req.ContentLength = data.Length;

			using (Stream stream = req.GetRequestStream())
				stream.Write(data, 0, data.Length);

			var res = new TwitterResponse<string>(Method.GenerateResponseResult(req), false);

			if (res.Result == StatusResult.Success)
			{
				var json = JsonConvert.DeserializeObject(res.ToString());
				if (((JObject)json).SelectToken("token_type", false) != null)
					if (((JObject)json).SelectToken("token_type", false).ToString().ToLower() == "bearer")
					res.ResponseObject = ((JObject)json).SelectToken("access_token").ToString();
			}
			return res;
		}

        /// <summary>
        /// AccessTokenを取得
        /// </summary>
        /// <param name="ConsumerKey">ConsumerKey</param>
        /// <param name="ConsumerSecret">ConsumerSecret</param>
        /// <param name="Token">AccessToken</param>
        /// <param name="TokenSecret">AccessTokenSecret</param>
        /// <param name="Verifier">PINコード</param>
        /// <returns>OAuthTokenResponse</returns>
        public static OAuthTokenResponse GetAccessToken(string ConsumerKey, string ConsumerSecret, string Token, string TokenSecret, string Verifier)
        {
            string normalizedUrl, normalizedReqParam;

            // シグネチャを生成
            string signature = oauth.GenerateSignature(new Uri(UrlBank.OAuthRequestToken),
                ConsumerKey, ConsumerSecret, Token, TokenSecret, "GET", timestamp, nonce,
                OAuthBase.SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedReqParam);

            // AccessToken用のURLを生成
            string accessTokenUrl = string.Format("{0}?{1}&oauth_signature={2}&oauth_verifier={3}",
                UrlBank.OAuthAccessToken, normalizedReqParam, UrlEncode(signature), Verifier);

            try
            {
                // レスポンスの取得
                WebRequest req = WebRequest.Create(accessTokenUrl);
                WebResponse res = req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("Shift_JIS"));

                // (独自クラス) レスポンスの文字列からOAuth情報を取得
                OAuthTokenResponse response = new OAuthTokenResponse(sr.ReadToEnd());

                res.Close(); sr.Close();

                return response;
            }
            catch (WebException)
            {
                return null;
            }
        }

        /// <summary>
        /// URLエンコード
        /// </summary>
        /// <param name="Url">エンコードするURL</param>
        private static string UrlEncode(string Url) { return System.Web.HttpUtility.UrlEncode(Url); }

        /// <summary>
        /// OAuthToken取得用クラス
        /// </summary>
        public class OAuthTokenClass
        {
            /// <summary>
            /// OAuthTokenを取得
            /// </summary>
            /// <param name="RequestToken">リクエストトークンURL</param>
            public OAuthTokenClass(string RequestToken)
            {
                string TokenPattern = "oauth_token=(.*?)&oauth_token_secret=(.*?)&oauth_callback_confirmed=(true|false)";
                if (Regex.IsMatch(RequestToken, TokenPattern))
                {
                    OAuthToken = Regex.Replace(RequestToken, TokenPattern, "$1");
                    OAuthTokenSecret = Regex.Replace(RequestToken, TokenPattern, "$2");
                    if (Regex.Replace(RequestToken, TokenPattern, "$3") == "true") OAuthCallbackConfirmed = true;
                    else OAuthCallbackConfirmed = false;
                    RequestUrl = string.Format("{0}?oauth_token={1}&oauth_token_secret={2}", UrlBank.OAuthAuthorize, OAuthToken, OAuthTokenSecret);
                }
            }

            public string OAuthToken { get; private set; }
            public string OAuthTokenSecret { get; private set; }
            public bool OAuthCallbackConfirmed { get; private set; }
            public string RequestUrl { get; private set; }
        }

        /// <summary>
        /// AccessToken取得用クラス
        /// </summary>
        public class OAuthTokenResponse
        {
            public OAuthTokenResponse() { }

            /// <summary>
            /// AccessTokenを取得
            /// </summary>
            /// <param name="AccessTokenUrl">アクセストークン</param>
            public OAuthTokenResponse(string AccessTokenStr)
            {
                string[] param = AccessTokenStr.Split('&');
                Dictionary<string, string> dics = new Dictionary<string, string>();

                foreach (string pr in param) {
                    string[] prm = pr.Split('=');
                    dics.Add(prm[0],prm[1]);
                }

                this.AccessToken = dics["oauth_token"];
                this.AccessTokenSecret = dics["oauth_token_secret"];
                this.UserId = decimal.Parse(dics["user_id"]);
                this.ScreenName = dics["screen_name"];
            }

            public string AccessToken { get; private set; }
            public string AccessTokenSecret { get; private set; }
            public decimal UserId { get; private set; }
            public string ScreenName { get; private set; }
        }
    }
}
