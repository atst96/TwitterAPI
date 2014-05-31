using Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using TwitterAPI;

namespace TwitterAPI
{

    public delegate void InitUserStreamCallback(FriendsList status);
    public delegate void StatusCreatedCallback(TwitterStatus status);
    public delegate void StatusDeletedCallback(TwitterDeletedStatus status);
    public delegate void DirectMessageCreatedCallback(TwitterDirectMessage status);
    public delegate void DirectMessageDeletedCallback(TwitterDeletedStatus status);
    public delegate void EventCallback(StreamEventStatus status);
    public delegate void StreamStoppedCallback(StopRequest status);
    public delegate void StreamAccept(string json);

    public class UserStreamOptions : ParameterClass
    {
        /// <summary>
        /// Withを設定します
        /// </summary>
		[Parameters("with")]
        public string With { get; set; }

        /// <summary>
        /// Trackを設定します
        /// </summary>
		[Parameters("track")]
		public TwitterStreamTrackOption Track { get; set; }

		/// <summary>
		/// フォローユーザーの活動状況をリアルタイムに取得します。
		/// </summary>
		[Parameters("include_followings_activity")]
		public bool? IncludeFollowingsActivity { get; set; }

        /// <summary>
        /// リプライの種類を設定します
        /// </summary>
		[Parameters("replies")]
		public RepliesType Replies { get; set; }

        /// <summary>
        /// 位置情報を設定します
        /// </summary>
		[Parameters("locations")]
		public TwitterLocationProperty Location { get; set; }

        /// <summary>
        /// stringify_friends_idsを設定します
        /// </summary>
		[Parameters("stringify_friends_ids")]
        public bool? StringifyFriendsIds { get; set; }
    }

    public class FilterStreamOptions
    {
        /// <summary>
        /// この文字が含まれているツイートを取得します
        /// </summary>
        public List<string> Track;

        /// <summary>
        /// 位置情報を指定します
        /// { 緯度 , 経度 }
        /// </summary>
        public double[] Location = new double[2];
    }

    public class TwitterStream : IDisposable
    {
        private static OAuthBase oauth = new OAuthBase();

        private static readonly string filter_url = "https://stream.twitter.com/1.1/statuses/filter.json";

        private DirectMessageCreatedCallback directMessageCreatedCallback;
        private DirectMessageDeletedCallback directMessageDeletedCallback;
        private EventCallback eventCallback;
        private InitUserStreamCallback friendCallback;
        private StatusCreatedCallback statusCreatedCallback;
        private StatusDeletedCallback statusDeletedCallback;
        private StreamStoppedCallback streamStoppedCallback;

        OAuthTokens tokens;
        string UserAgent = "";

        /// <summary>
        /// OAuthTokens
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="userAgent">ユーザーエージェント</param>
        public TwitterStream(OAuthTokens tokens, string userAgent)
        {
            if (tokens == null) throw new ArgumentNullException("Tokens");
                this.tokens = tokens;
                if (string.IsNullOrWhiteSpace(userAgent)) this.UserAgent = "TwitterAPI";
                else this.UserAgent = userAgent;
        }

        public Dictionary<string, string> FilterStreamOptionsToDiecionary(FilterStreamOptions options)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (options.Track.Count > 0)
            {
                string res = "";
                for (int i = 0; i < options.Track.Count; i++)
                {
                    if (i == 0) res += options.Track[i];
                    else res += "," + options.Track[i];
                }
                result.Add("track", res);
            }

            if (options.Location[0] > 0 && options.Location[1] > 0)
                result.Add("locations", string.Format("{0},{1}", options.Location[0], options.Location[1]));

            return result;
        }

        HttpWebRequest request;

        private bool StreamStop = true;

        public bool IsStreaming { get { return !StreamStop; } }

        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            EndStream();
            directMessageCreatedCallback = null;
            directMessageDeletedCallback = null;
            eventCallback = null;
            friendCallback = null;
            statusCreatedCallback = null;
            statusDeletedCallback = null;
            streamStoppedCallback = null;
        }


        /// <summary>
        /// UserStreamを開始します
        /// </summary>
        /// <param name="friendCallback"></param>
        /// <param name="streamStoppedCallback"></param>
        /// <param name="statusCreatedCallback"></param>
        /// <param name="statusDeletedCallback"></param>
        /// <param name="eventCallback"></param>
        /// <param name="directMessageCreatedCallback"></param>
        /// <param name="directMessageDeletedCallback"></param>
        public IAsyncResult StartUserStream(UserStreamOptions option,
            InitUserStreamCallback friendCallback, StreamStoppedCallback streamStoppedCallback,
            StatusCreatedCallback statusCreatedCallback, StatusDeletedCallback statusDeletedCallback,
            EventCallback eventCallback, DirectMessageCreatedCallback directMessageCreatedCallback,
            DirectMessageDeletedCallback directMessageDeletedCallback)
        {
            this.friendCallback = friendCallback;
            this.streamStoppedCallback = streamStoppedCallback;
            this.statusCreatedCallback = statusCreatedCallback;
            this.statusDeletedCallback = statusDeletedCallback;
            this.eventCallback = eventCallback;
            this.directMessageCreatedCallback = directMessageCreatedCallback;
            this.directMessageDeletedCallback = directMessageDeletedCallback;

			request = Method.GenerateWebRequest(UrlBank.UserStream, WebMethod.GET, tokens, option, null, null, null);
			request.UserAgent = UserAgent;

            StreamStop = false;
            return request.BeginGetResponse(StreamCallback, request);
        }


        /// <summary>
        /// FilterStreamを開始します
        /// </summary>
        /// <param name="friendCallback"></param>
        /// <param name="streamStoppedCallback"></param>
        /// <param name="statusCreatedCallback"></param>
        /// <param name="statusDeletedCallback"></param>
        /// <param name="eventCallback"></param>
        /// <param name="directMessageCreatedCallback"></param>
        /// <param name="directMessageDeletedCallback"></param>
        public IAsyncResult StartFilterStream(
            InitUserStreamCallback friendCallback, StreamStoppedCallback streamStoppedCallback,
            StatusCreatedCallback statusCreatedCallback, StatusDeletedCallback statusDeletedCallback,
            EventCallback eventCallback, DirectMessageCreatedCallback directMessageCreatedCallback,
            DirectMessageDeletedCallback directMessageDeletedCallback, FilterStreamOptions options)
        {
            this.friendCallback = friendCallback;
            this.streamStoppedCallback = streamStoppedCallback;
            this.statusCreatedCallback = statusCreatedCallback;
            this.statusDeletedCallback = statusDeletedCallback;
            this.eventCallback = eventCallback;
            this.directMessageCreatedCallback = directMessageCreatedCallback;
            this.directMessageDeletedCallback = directMessageDeletedCallback;

            request = PostRequest(filter_url, tokens, UserAgent, FilterStreamOptionsToDiecionary(options));

            StreamStop = false;
            return request.BeginGetResponse(StreamCallback, request);
        }



        private void StreamCallback(IAsyncResult result)
        {

            var req = result.AsyncState as WebRequest;
            HttpWebResponse res = null;
            try
            {
                res = req.EndGetResponse(result) as HttpWebResponse;
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        try
                        {
                            while (!StreamStop && !reader.EndOfStream)
                            {
                                var data = reader.ReadLine();

								ThreadPool.QueueUserWorkItem(_ =>
								{
									if (!StreamStop && !string.IsNullOrEmpty(data))
									{
										if (Regex.IsMatch(data, @"""event""\s*:\s*\""(.+?)""") && eventCallback != null)
										{
											// Events

											JObject obj = (JObject)JsonConvert.DeserializeObject(data);
											var events = obj.SelectToken("event", false);
											if (eventCallback != null)
											{
												var _event = JsonConvert.DeserializeObject<StreamEventStatus>(data);

												var targetobject = obj.SelectToken("target_object", false);

												/*TwitterStatus _targetObject = null;
												TwitterList listObject = null;*/

												if (targetobject != null)
												{

													if (targetobject.SelectToken("subscriver_couont", false) != null)
													{
														//System.Windows.Forms.MessageBox.Show(targetobject.ToString());
														//_targetObject = JsonConvert.DeserializeObject<TwitterStatus>(targetobject.ToString());
													}
													else if (events.ToString().Contains("list_"))
													{
														//listObject = JsonConvert.DeserializeObject<TwitterList>(targetobject.ToString());
														_event.TargetObject_List = JsonConvert.DeserializeObject<TwitterList>(targetobject.ToString());
													}
													else if (targetobject.SelectToken("user", false) != null)
													{
														//_targetObject = JsonConvert.DeserializeObject<TwitterStatus>(targetobject.ToString());
														_event.TargetObject = JsonConvert.DeserializeObject<TwitterStatus>(targetobject.ToString());
													}

												}

												//_event.TargetObject = _targetObject;
												//_event.TargetObject_List = listObject;
												this.eventCallback(_event);
												_event = null;
												targetobject = null;
												obj = null;
												events = null;
											}
										}
										else if (Regex.IsMatch(data, @"""user""\s*:\s*\{") && Regex.IsMatch(data, @"""entities""\s*:\s*\{") && !Regex.IsMatch(data, @"""event""\s*:\s*""") && statusCreatedCallback != null)
										{
											// Status
											var _data = JsonConvert.DeserializeObject<TwitterStatus>(data);
											statusCreatedCallback(_data);
											_data = null;
										}
										else if (Regex.IsMatch(data, @"^\{\s*""delete"":") && (statusDeletedCallback != null || directMessageDeletedCallback != null))
										{
											var delete = ((JObject)JsonConvert.DeserializeObject(data)).SelectToken("delete", false);
											if (Regex.IsMatch(delete.ToString(), @"""status""\s*:\s*\{"))
											{
												// Delete Status
												if (statusDeletedCallback != null)
												{
													var _data = JsonConvert.DeserializeObject<TwitterDeletedStatus>(delete.SelectToken("status", false).ToString());
													statusDeletedCallback(_data);
													_data = null;
												}
											}
											else if (Regex.IsMatch(delete.ToString(), @"""direct_message""\s*:\s*\{"))
											{
												// Delete DirectMessage
												if (directMessageDeletedCallback != null)
												{
													var _data = (JsonConvert.DeserializeObject<TwitterDeletedStatus>(delete.SelectToken("direct_message", false).ToString()));
													directMessageDeletedCallback(_data);
													_data = null;
												}
											}
										}
										else if (Regex.IsMatch(data, @"^\{\s*""direct_message"":") && directMessageCreatedCallback != null)
										{
											// DirectMessage
											var _data = JsonConvert.DeserializeObject<TwitterDirectMessage>(((JObject)JsonConvert.DeserializeObject(data)).SelectToken("direct_message", false).ToString());
											directMessageCreatedCallback(_data);
											_data = null;
										}
										else if (Regex.IsMatch(data, @"^\{\s*""friends"":") && friendCallback != null)
										{
											// FriendIds
											var _data = JsonConvert.DeserializeObject<FriendsList>(data);
											friendCallback(_data);
										}

										// JObject obj = (JObject)JsonConvert.DeserializeObject(data);
										// System.Diagnostics.Debug.WriteLine(data);

										/*var friends = obj.SelectToken("friends", false);
										if (friends != null)
										{

											if (friendCallback != null && friends.HasValues)
											{
												friendCallback(JsonConvert.DeserializeObject<FriendsList>(data));
											}
										}*/

										/*var delete = obj.SelectToken("delete", false);
										if (delete != null)
										{
											var deletedStatus = delete.SelectToken("status", false);
											if (deletedStatus != null)
											{
												if (statusDeletedCallback != null && deletedStatus.HasValues)
												{
													statusDeletedCallback(JsonConvert.DeserializeObject<TwitterDeletedStatus>(deletedStatus.ToString()));
												}
											}

											//var deletedDirectMessage = delete.SelectToken("direct_message", false);

										}*/


										

										/*var user = obj.SelectToken("user", false);
										if (user != null)
										{
											if (statusCreatedCallback != null && user.HasValues)
											{
												statusCreatedCallback(JsonConvert.DeserializeObject<TwitterStatus>(data));
											}
										}*/

										/*var directMessage = obj.SelectToken("direct_message", false);
										if (directMessage != null)
										{
											if (directMessageCreatedCallback != null && directMessage.HasValues)
											{
												directMessageCreatedCallback(JsonConvert.DeserializeObject<TwitterDirectMessage>(directMessage.ToString()));
											}
										}*/
									}
									data = null;
								});
                            }


                            reader.Close();
                            OnStreamStopped(StreamStop ? StopRequest.StoppedByRequest : StopRequest.WebConnectionFailed);
                        }
                        catch (Exception ex)
						{
                            reader.Close();
                            OnStreamStopped(StreamStop ? StopRequest.StoppedByRequest : StopRequest.WebConnectionFailed);
                        }
                    }

                }

            }
            catch (WebException ex)
            {
                res = ex.Response as HttpWebResponse;
                if (res != null)
                {
                    switch (res.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            OnStreamStopped(StopRequest.Unauthorized);
                            break;
                        case HttpStatusCode.Forbidden:
                            OnStreamStopped(StopRequest.Forriden);
                            break;
                        case HttpStatusCode.NotAcceptable:
                            OnStreamStopped(StopRequest.NotFound);
                            break;
                        case HttpStatusCode.RequestEntityTooLarge:
                            OnStreamStopped(StopRequest.RequestEntityTooLarge);
                            break;
                        case (HttpStatusCode)420:
                            OnStreamStopped(StopRequest.RateLimited);
                            break;
                        case HttpStatusCode.InternalServerError:
                            OnStreamStopped(StopRequest.InternalServerError);
                            break;
                        case HttpStatusCode.ServiceUnavailable:
                            OnStreamStopped(StopRequest.ServiceUnavailable);
                            break;
                        default:
                            OnStreamStopped(StopRequest.Unknown);
                            break;
                    }
                }
                else
                {
                    OnStreamStopped(StopRequest.Unknown);
                }
            }
            catch (Exception)
            {
                OnStreamStopped(StopRequest.WebConnectionFailed);
            }
            finally
            {
                req.Abort();
                if (res != null) res.Close();
                request = null;
            }

        }
        

        private void OnStreamStopped(StopRequest exeption)
        {
            if (streamStoppedCallback != null)
                streamStoppedCallback(exeption);
        }


        /// <summary>
        /// ストリーミングを停止します
        /// </summary>
        public void EndStream()
        {
            StreamStop = true;
            if (request != null)
            {
                request.Abort();
                request = null;
            }
        }

        private static HttpWebRequest GetRequest(string url, OAuthTokens tokens, string UserAgent)
        {
            string normalizedUrl, normalizedReqParam;

            string timestamp = oauth.GenerateTimeStamp();
            string nonce = oauth.GenerateNonce();

            // シグネチャの生成
            string signature = oauth.GenerateSignature(new Uri(url),
                tokens.ConsumerKey, tokens.ConsumerSecret, tokens.AccessToken, tokens.AccessTokenSecret,
                "GET", timestamp, nonce, OAuthBase.SignatureTypes.HMACSHA1,
                out normalizedUrl, out normalizedReqParam);

            string requestUrl = string.Format("{0}?{1}&oauth_signature={2}",
                normalizedUrl, normalizedReqParam, HttpUtility.UrlEncode(signature));

            var req = WebRequest.Create(requestUrl) as HttpWebRequest;
            req.UserAgent = UserAgent;
            req.UseDefaultCredentials = true;
            req.Proxy = null;

            if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 2)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            return req;
        }


        private static HttpWebRequest PostRequest(string url, OAuthTokens tokens, string UserAgent, Dictionary<string,string> paramt = null)
        {
            string nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            string timestamp = oauth.GenerateTimeStamp();            

            string normalizedUrl, normalizedReqParam;

            string signatureBase = oauth.GenerateSignatureBase(new Uri(url), tokens.ConsumerKey, tokens.AccessToken, tokens.ConsumerSecret,
                "POST", timestamp, nonce, "HMAC-SHA1", out normalizedUrl, out normalizedReqParam);

            string param = "";
            if (param != null)
            {
                param = Method.ConvertMethod.DictionaryToParams(paramt);
                if (!string.IsNullOrEmpty(param))
                {
                    param = Regex.Replace(param, @"^\?", "");

                    normalizedUrl += "?" + param;
                    signatureBase += Uri.EscapeDataString("&" + param);
                }
            }

            var conpositeKey = string.Concat(Uri.EscapeDataString(tokens.ConsumerSecret),
                "&", Uri.EscapeDataString(tokens.AccessTokenSecret));
            string oauth_signature = "";
            using (HMACSHA1 hasher = new HMACSHA1(UTF8Encoding.UTF8.GetBytes(conpositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(UTF8Encoding.UTF8.GetBytes(signatureBase)));
            }

            string signature = "OAuth " + normalizedReqParam;
            signature = signature.Replace("=", "=\"").Replace("&", "\", ");
            signature += "\", oauth_signature=\"" + Uri.EscapeDataString(oauth_signature);

            ServicePointManager.Expect100Continue = false;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(normalizedUrl);
            req.Headers.Add("Authorization", signature);
            req.Proxy = null;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.PreAuthenticate = true;
            req.AllowWriteStreamBuffering = true;
            req.UserAgent = UserAgent;

            if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 2)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            return req;
        }

        
    }

    public enum StopRequest
    {
        StoppedByRequest,
        WebConnectionFailed,
        Unauthorized,
        Forriden,
        NotFound,
        NotAcceptable,
        RequestEntityTooLarge,
        RateLimited,
        InternalServerError,
        ServiceUnavailable,
        Unknown
    }

    
}
