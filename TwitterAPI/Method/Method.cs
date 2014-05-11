using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

using TwitterAPI;
using Core;

using System.IO;
using System.Security.Cryptography;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace TwitterAPI
{
	public enum WebMethod
	{
		POST,
		GET,
	}

    public abstract class Method
    {
        private static OAuthBase oauth = new OAuthBase();
        /// <summary>
        /// TwitterAPIをMETHOD=GETで取得する
        /// </summary>
        /// <param name="url">APIのURL</param>
        /// <param name="tokens">OAuthToken</param>
        /// <param name="param">パラメータ(Dictionary)</param>
        /// <returns>RequestResult</returns>
        public static ResponseResult Get(string url, OAuthTokens tokens, ParameterClass param = null)
        {
			return GenerateResponseResult(GenerateWebRequest(url, WebMethod.GET, tokens, param, null, null, null, null));
        }

		public static HttpWebRequest GenerateWebRequest(string url, WebMethod method,OAuthTokens tokens, ParameterClass parameters,string contentType, string postHeader, byte[] data, WebProxy proxy)
		{
			OAuthBase oauth = new OAuthBase();

			string addr = url, timestamp = oauth.GenerateTimeStamp(), nonce = oauth.GenerateNonce(), normalizedUrl, normalizedReqParams, signature;

			if(parameters != null)
			{
				string param = parameters.GenerateGetParameters();
				if (!string.IsNullOrEmpty(param)) addr += param;

				System.Windows.Forms.MessageBox.Show(addr);
			}

			HttpWebRequest req = null;
			
			// シグネチャ

			if (method == WebMethod.GET)
			{
				signature = oauth.GenerateSignature(new Uri(addr), tokens.ConsumerKey, tokens.ConsumerSecret, tokens.AccessToken, tokens.AccessTokenSecret, "GET", timestamp, nonce, out normalizedUrl, out normalizedReqParams);
				req = HttpWebRequest.Create(string.Format("{0}?{1}&oauth_signature={2}", normalizedUrl, normalizedReqParams, Uri.EscapeDataString(signature))) as HttpWebRequest;

				if (parameters != null)
				{
					string param = parameters.GenerateParameters("bearer");
					if (!string.IsNullOrEmpty(param))
						req.Headers.Add(HttpRequestHeader.Authorization, param);
				}
			}
			else if (method == WebMethod.POST)
			{
				req = HttpWebRequest.Create(addr) as HttpWebRequest;

				string signatureBase = oauth.GenerateSignatureBase(new Uri(addr), tokens.ConsumerKey, tokens.AccessToken, tokens.AccessTokenSecret, "POST", timestamp, nonce, "HMAC-SHA1", out normalizedUrl, out normalizedReqParams) + (!string.IsNullOrEmpty(postHeader) ? Uri.EscapeDataString("&" + postHeader) : ""),
					compositKey = string.Concat(Uri.EscapeDataString(tokens.ConsumerSecret), "&", Uri.EscapeDataString(tokens.AccessTokenSecret)), oauthSignature;
				using (HMACSHA1 hasher = new HMACSHA1(UTF8Encoding.UTF8.GetBytes(compositKey))) oauthSignature = Convert.ToBase64String(hasher.ComputeHash(UTF8Encoding.UTF8.GetBytes(signatureBase)));

				signature = string.Format("OAuth {0}\", oauth_signature=\"{1}\"", normalizedReqParams.Replace("=", "=\"").Replace("&", "\", "), Uri.EscapeDataString(oauthSignature));
				req.Headers.Add(HttpRequestHeader.Authorization, signature);
				req.Method = "POST";
				if (!string.IsNullOrEmpty(contentType)) req.ContentType = contentType;

				if (data != null) using (Stream stream = req.GetRequestStream()) stream.Write(data, 0, data.Length);
			}

			ServicePointManager.Expect100Continue = false;
			//req.Proxy = proxy;

			return req;
		}

		public static ResponseResult GenerateResponseResult(HttpWebRequest request)
		{
			var req = request;

			ResponseResult result = new ResponseResult();

			try
			{
				WebResponse res = req.GetResponse();
				using (StreamReader reader = new StreamReader(res.GetResponseStream()))
					result.ResponseStream = reader.ReadToEnd();
				result.Result = StatusResult.Success;
				result.AccessLevel = WebHeaderToAccessLevel(res.Headers);
				result.RateLimited = new RateLimited(res.Headers[XRateLimitLimit], res.Headers[XRateLimitRemaining], res.Headers[XRateLimitRemaining]);
				result.Url = res.ResponseUri.ToString();
				res.Close();
			}
			catch (Exception exception)
			{
				if (exception is WebException)
				{
					var ex = exception as WebException;

					result.Result = GetStatusResult(ex.Status);
					result.ResponseStream = null;
					using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
					{
						System.Windows.Forms.MessageBox.Show(ex.Response.Headers.ToString());
						JObject obj = (JObject)JsonConvert.DeserializeObject(reader.ReadToEnd());
						var errors = obj.SelectToken("errors", false);
						if (errors != null)
						{
							if (errors.Type == JTokenType.String)
							{
								result.Error = new TwitterError();
								result.Error.Message = errors.ToString();
							}
							else if (errors.Type == JTokenType.Array)
							{
								result.Error = new TwitterError();
								result.Error = JsonConvert.DeserializeObject<List<TwitterError>>(errors.ToString())[0];
							}

						}
					}
					result.AccessLevel = WebHeaderToAccessLevel(ex.Response.Headers);
					result.RateLimited = new RateLimited(ex.Response.Headers[XRateLimitLimit], ex.Response.Headers[XRateLimitRemaining], ex.Response.Headers[XRateLimitRemaining]);
					result.Url = ex.Response.ResponseUri.ToString();
				}
				else
				{
					result.Result = StatusResult.Unknown;
				}
			}

			return result;
		}


        private static bool IsNumeric(object o)
        {
            return o is sbyte || o is byte || o is short || o is ushort || o is int || o is uint ||
                o is long || o is long || o is ulong || o is float || o is double || o is decimal;
        }


		public static ResponseResult xAuthOAuthGet(string url, OAuthTokens tokens, string username, string password, Dictionary<string, string> param = null, string mode = "client_auth")
		{
			OAuthBase oauth = new OAuthBase();

			// タイムスタンプの生成
			string timestamp = oauth.GenerateTimeStamp();

			// nonceの生成
			string nonce = oauth.GenerateNonce();

			string normalizedUrl, normalizedReqParams;

			// DictionaryをURLのパラメータに変換
			if (param != null)
			{
				url += ConvertMethod.DictionaryToParams(param);
			}

			// シグネチャの生成
			string signature = oauth.GenerateSignature(new Uri(url),
				tokens.ConsumerKey, tokens.ConsumerSecret, tokens.AccessToken, tokens.AccessTokenSecret,
				"GET", timestamp, nonce, out normalizedUrl, out normalizedReqParams);

			string requestUrl = string.Format("{0}?{1}&oauth_signature={2}",
				normalizedUrl, normalizedReqParams, Uri.EscapeDataString(signature));

			WebRequest req = WebRequest.Create(requestUrl);

			ResponseResult result = new ResponseResult();

			try
			{
				WebResponse res = req.GetResponse();
				StreamReader reader = new StreamReader(res.GetResponseStream());
				result.ResponseStream = reader.ReadToEnd();
				result.Result = StatusResult.Success;

				result.AccessLevel = StringToLevelEnum(res.Headers["x-access-level"]);
				result.RateLimited = new RateLimited(res.Headers["x-rate-limit-limit"],
					res.Headers["x-rate-limig-remaining"], res.Headers["x-rate-limit-reset"]);

				result.Url = res.ResponseUri.ToString();

				res.Close();
			}
			catch (WebException ex)
			{
				result.Result = GetStatusResult(ex.Status);
				result.ResponseStream = null;
				using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
				{
					System.Windows.Forms.MessageBox.Show(ex.Response.Headers.ToString());
					JObject obj = (JObject)JsonConvert.DeserializeObject(reader.ReadToEnd());
					var errors = obj.SelectToken("errors", false);
					if (errors != null)
					{
						if (errors.Type == JTokenType.String)
						{
							result.Error = new TwitterError();
							result.Error.Message = errors.ToString();
						}
						else if (errors.Type == JTokenType.Array)
						{
							result.Error = new TwitterError();
							result.Error = JsonConvert.DeserializeObject<List<TwitterError>>(errors.ToString())[0];
						}

					}
				}
				result.AccessLevel = StringToLevelEnum(ex.Response.Headers["x-access-level"]);
				result.RateLimited = new RateLimited(ex.Response.Headers["x-rate-limit-limit"],
					ex.Response.Headers["x-rate-limit-remaining"], ex.Response.Headers["x-rate-limit-reset"]);
				result.Url = ex.Response.ResponseUri.ToString();

			}

			return result;
		}

		public static ResponseResult Post(string url, OAuthTokens tokens, ParameterClass parameters, string contentType, string postHeader, byte[] data, WebProxy proxy)
		{
			return GenerateResponseResult(GenerateWebRequest(url, WebMethod.POST, tokens, parameters, contentType, postHeader, data, proxy));
		}

		public static ResponseResult PostText(string reqUrl, OAuthTokens tokens, string data, ParameterClass param)
		{
			var req = GenerateWebRequest(reqUrl, WebMethod.POST, tokens, param, null,null, null, null);
			if (!string.IsNullOrEmpty(data)) req.Headers[HttpRequestHeader.Authorization] += Uri.EscapeDataString("&" + data);
			System.Windows.Forms.MessageBox.Show(req.Headers[HttpRequestHeader.Authorization]);
			return GenerateResponseResult(req);
		}

        public static StatusResult GetStatusResult(WebExceptionStatus status)
        {
            StatusResult result = new StatusResult();
            switch ((HttpStatusCode)status)
            {
                case HttpStatusCode.OK:
                    result = StatusResult.Success;
                    break;
                case HttpStatusCode.BadRequest:
                    result = StatusResult.BadRequest;
                    break;
                case (HttpStatusCode)420:
                    result = StatusResult.RateLimited;
                    break;
                case HttpStatusCode.Unauthorized:
                    result = StatusResult.Unauthorized;
                    break;
                case HttpStatusCode.NotFound:
                    result = StatusResult.FileNotFound;
                    break;
                case HttpStatusCode.ProxyAuthenticationRequired:
                    result = StatusResult.ProxyAuthenticationRequired;
                    break;
                case HttpStatusCode.RequestTimeout:
                    result = StatusResult.RequestTimeout;
                    break;
                case HttpStatusCode.Forbidden:
                    result = StatusResult.Unauthorized;
                    break;
                default:
                    result = StatusResult.Unknown;
                    break;
            }
            return result;
        }

		private static readonly string XAccessLevel = "x-access-level";

		private static readonly string XRateLimitLimit = "x-rate-limit-limit";
		private static readonly string XRateLimitRemaining = "x-rate-limig-remaining";
		private static readonly string XRateLimitReset = "x-rate-limig-remaining";

        /// <summary>
        /// AccessLevelの文字列をEnumに変換します
        /// </summary>
        /// <param name="AccessLevel"></param>
        /// <returns></returns>
        public static AccessLevel StringToLevelEnum(string accessLevel)
        {
            if (accessLevel == "read") return AccessLevel.Read;
            else if (accessLevel == "read-write") return AccessLevel.ReadWrite;
            else if (accessLevel == "read-write-directmessages") return AccessLevel.ReadWriteDirectMessages;
            else return AccessLevel.Unknown;
        }

		public static AccessLevel WebHeaderToAccessLevel(WebHeaderCollection headers)
		{
			if (headers[XAccessLevel] == "read") return AccessLevel.Read;
			else if (headers[XAccessLevel] == "read-write") return AccessLevel.ReadWrite;
			else if (headers[XAccessLevel] == "read-write-directmessages") return AccessLevel.ReadWriteDirectMessages;
			else return AccessLevel.Unknown;
		}

        /// <summary>
        /// Urlエンコードします
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            string result = Uri.EscapeDataString(str)
            .Replace(Uri.EscapeDataString("@"), "@")
            .Replace(@"*", @"%2A") // %2A = *
            .Replace("!", @"%21") // %21 = !
            .Replace("#", @"%23") // %23 = #
            .Replace("'", @"%27") // %27 = '
            .Replace("(", @"%28") // %28 = (
            .Replace(")", @"%29"); // %29 = )

            return result;
        }


        public abstract class ConvertMethod
        {
            public static string DictionaryToParams(Dictionary<string, string> dic)
            {
                string param = "";
                foreach (var values in dic.Select((v, i) => new { v, i }))
                {
                    if (values.i == 0) param += string.Format("?{0}={1}", values.v.Key, UrlEncode(values.v.Value));
                    else param += string.Format("&{0}={1}", values.v.Key, UrlEncode(values.v.Value));
                }
                return param;
            }
        }
    }


    public enum ContentType
    {
        Application_WWW_Form_UrlEncoded,
        Multipart_FormData
    }

    /// <summary>
    /// アクセスレベル
    /// </summary>
    public enum AccessLevel
    {
        Read,
        ReadWrite,
        ReadWriteDirectMessages,
        Unknown
    }


    /// <summary>
    /// レスポンスリザルト
    /// </summary>
    public class ResponseResult
    {
        public AccessLevel AccessLevel { get; set; }
        public StatusResult Result { get; set; }
        public RateLimited RateLimited { get; set; }
        public TwitterError Error { get; set; }
        public string Url { get; set; }
        public string ResponseStream { get; set; }
    }

    /// <summary>
    /// パラメータ属性(初期値: Method=GET, UrlEncode=false)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Struct, Inherited = true)]
    public class Parameters : Attribute
    {
        public Parameters() { }

        public Parameters(string ParameterName)
        {
            this.ParmaterName = ParameterName;
        }

        public Parameters(string ParameterName, string Method)
        {
            this.ParmaterName = ParameterName;
            this.Method = Method;
        }

        public Parameters(string ParameterName, string Method, bool UrlEncode)
        {
            this.ParmaterName = ParameterName;
            this.Method = Method;
            this.UrlEncode = UrlEncode;
        }

        /// <summary>
        /// パラメータ名
        /// </summary>
        public string ParmaterName { get; set; }

        /// <summary>
        /// メソッド(GET/POST)
        /// </summary>
        public string Method = "GET";

        /// <summary>
        /// Urlエンコードを行うかどうか
        /// </summary>
        public bool UrlEncode = false;
    }


    public class ParameterClass
    {
        /// <summary>
        /// プロパティと属性からGETパラメータを生成します
        /// </summary>
        /// <returns></returns>
        public string GenerateGetParameters()
        {
            List<string> param = new List<string>();

            Type type = this.GetType();

            MemberInfo[] members = type.GetMembers(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (MemberInfo m in members)
            {
                if (m.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = type.GetProperty(m.Name);
                    Attribute attr = Attribute.GetCustomAttribute(m, typeof(Parameters));
                    var attribute = attr as Parameters;
                    if (attribute != null && !string.IsNullOrEmpty(attribute.ParmaterName))
                    {
                        if (attribute.Method.ToLower() == "get")
                            if (property.GetValue(this, null) != null)
                                param.Add(string.Format("{0}={1}", attribute.ParmaterName, property.GetValue(this, null)));
                    }
                    else
                    {
                        if (property.GetValue(this, null) != null)
                            param.Add(string.Format("{0}={1}", m.Name, property.GetValue(this, null).ToString()));
                    }
                }
            }
            return param.Count > 0 ? "?" + string.Join("&", param) : string.Empty;
        }

		public string GenerateParameters(string paramName)
		{
			List<string> param = new List<string>();

			Type type = this.GetType();

			MemberInfo[] members = type.GetMembers(
				BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
			foreach (MemberInfo m in members)
			{
				if (m.MemberType == MemberTypes.Property)
				{
					PropertyInfo property = type.GetProperty(m.Name);
					Attribute attr = Attribute.GetCustomAttribute(m, typeof(Parameters));
					var attribute = attr as Parameters;
					if (attribute != null && !string.IsNullOrEmpty(attribute.ParmaterName))
					{
						if (attribute.Method.ToLower() == paramName.ToLower())
							if (property.GetValue(this, null) != null)
								param.Add(string.Format("{0}={1}", attribute.ParmaterName, property.GetValue(this, null)));
					}
					else
					{
						if (property.GetValue(this, null) != null)
							param.Add(string.Format("{0}={1}", m.Name, property.GetValue(this, null).ToString()));
					}
				}
			}
			return param.Count > 0 ? "?" + string.Join("&", param) : string.Empty;
		}

        /// <summary>
        /// ポロパティと属性からPOSTパラメータを抽出します
        /// </summary>
        /// <returns></returns>
        public string GeneratePostParameters()
        {
            List<string> param = new List<string>();

            Type type = this.GetType();

            MemberInfo[] members = type.GetMembers(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (MemberInfo m in members)
            {
                if (m.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = type.GetProperty(m.Name);
                    Attribute attr = Attribute.GetCustomAttribute(m, typeof(Parameters));
                    var attribute = attr as Parameters;
                    if (attribute != null)
                    {
                        if (attribute.Method.ToLower() == "post")
                            if (property.GetValue(this, null) != null)
                                param.Add(string.Format("{0}={1}", attribute.ParmaterName, property.GetValue(this, null).GetType().ToString()));
                    }
                }
            }
            return param.Count > 0 ? "?" + string.Join("&", param) : string.Empty;
        }
    }
}
