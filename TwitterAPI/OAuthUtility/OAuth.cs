using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

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
        private static UrlBank urlBank = new UrlBank();

        private static OAuthBase oauth = new OAuthBase();

        private static string timestamp;
        private static string nonce;

		private static readonly string REQUEST_TOKNE_URL = "https://api.twitter.com/oauth/request_token";
		private static readonly string AUTHORIZE_URL = "https://api.twitter.com/oauth/authorize";
		private static readonly string ACCESS_TOKEN_URL = "https://api.twitter.com/oauth/access_token";

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

			Uri reqUrl = new Uri(REQUEST_TOKNE_URL);

			// シグネチャを生成
			string signature = oauth.GenerateSignature(reqUrl,
				ConsumerKey, ConsumerSecret, null, null, "GET", timestamp, nonce,
				OAuthBase.SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedReqParam);

			string reqTokenUrl = normalizedUrl + "?" + normalizedReqParam
				+ "&oauth_signature=" + Method.UrlEncode(signature);

			HttpWebRequest req = WebRequest.Create(reqTokenUrl) as HttpWebRequest;
			req.Proxy = Core.Config.Proxy;

			try
			{
				WebResponse res = req.GetResponse();
				StreamReader reader = new StreamReader(res.GetResponseStream());

				string str = reader.ReadToEnd();
				OAuthTokenClass token = new OAuthTokenClass(str);

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
            string signature = oauth.GenerateSignature(new Uri(REQUEST_TOKNE_URL),
                ConsumerKey, ConsumerSecret, Token, TokenSecret, "GET", timestamp, nonce,
                OAuthBase.SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedReqParam);

            // AccessToken用のURLを生成
            string accessTokenUrl = string.Format("{0}?{1}&oauth_signature={2}&oauth_verifier={3}",
                ACCESS_TOKEN_URL, normalizedReqParam, UrlEncode(signature), Verifier);

            try
            {
                // レスポンスの取得
                WebRequest req = WebRequest.Create(accessTokenUrl);
                req.Proxy = Core.Config.Proxy;
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
                    RequestUrl = string.Format(UrlBank.PATTERN_AUTHORIZE, OAuthToken, OAuthTokenSecret);
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
