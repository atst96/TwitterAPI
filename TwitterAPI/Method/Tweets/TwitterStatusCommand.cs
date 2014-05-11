using Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TwitterAPI
{
    public abstract class TwitterStatusCommand
    {
        private static OAuthBase oauth = new OAuthBase();

        private static readonly string StatusUpdateUrl = "https://api.twitter.com/1.1/statuses/update.json";
        private static readonly string StatuUpdateWithMediaUrl = "https://api.twitter.com/1.1/statuses/update_with_media.json";
        private static readonly string RetweetUrl = "https://api.twitter.com/1.1/statuses/retweet/{0}.json";
        private static readonly string DestroyRetweetUrl = "https://api.twitter.com/1.1/statuses/destroy/{0}.json";


        /// <summary>
        /// ツイートを投稿します。
        /// </summary>
        /// <param name="tweet">ツイート内容</param>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="options">オプション</param>
        public static TwitterResponse<TwitterStatus> Update(string tweet, OAuthTokens tokens, StatusUpdateOptions options = null)
        {
            string url = StatusUpdateUrl;
            if (options != null) url += Method.ConvertMethod.DictionaryToParams(StatusUpdateOptionsToDictionary(options));


            string postData = "status=" + Method.UrlEncode(tweet);
            var res = Method.Post(url, tokens, postData);

            return new TwitterResponse<TwitterStatus>(res);
        }


        /// <summary>
        /// 画像付きでツイートを投稿します。
        /// </summary>
        /// <param name="tweet">ツイート内容</param>
        /// <param name="fileName">添付画像</param>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="options">オプション</param>
        public static TwitterResponse<TwitterStatus> UpdateWithMedia(string tweet, string fileName, OAuthTokens tokens, StatusUpdateOptions options = null)
        {
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullException("fileName");
			}
			else
			{
				string url = StatuUpdateWithMediaUrl;
				if (options != null) url += Method.ConvertMethod.DictionaryToParams(StatusUpdateOptionsToDictionary(options));


				string boundary = Guid.NewGuid().ToString();

				string header = string.Format("--{0}", boundary);
				string footer = string.Format("--{0}--", boundary);

				string ContentType = "multipart/form-data;boundary=" + boundary;

				/*--------------------  送信するデータの生成  --------------------*/

				var encoding = Encoding.GetEncoding("iso-8859-1");

				FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				byte[] imgBytes = new byte[fs.Length];
				fs.Read(imgBytes, 0, imgBytes.Length);
				string fileData = encoding.GetString(imgBytes);
				fs.Close();


				StringBuilder builder = new StringBuilder();

				// ヘッダーを書き込む
				builder.AppendLine(header);
				builder.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "status"));
				builder.AppendLine();
				builder.AppendLine();

				// 投稿内容をUTF-8で書き込む(日本語文字化け回避)
				builder.AppendLine(encoding.GetString(Encoding.GetEncoding("UTF-8").GetBytes(tweet)));

				// ファイル情報、フッターを書き込む
				builder.AppendLine();
				builder.AppendLine(header);
				builder.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"",
					"media[]", Path.GetFileName(fileName)));
				builder.AppendLine("Content-Type: application/octet-stream");
				builder.AppendLine();
				builder.AppendLine(fileData);
				builder.AppendLine(footer);

				var result = Method.UploadPost(url, tokens, encoding.GetBytes(builder.ToString()), ContentType);

				TwitterResponse<TwitterStatus> status = new TwitterResponse<TwitterStatus>();

				if (result.GetType() == typeof(HttpWebResponse))
				{
					var res = (WebResponse)result;

					StreamReader reader = new StreamReader(res.GetResponseStream());
					string response = reader.ReadToEnd();
					reader.Dispose();

					try
					{
						status.ResponseObject = JsonConvert.DeserializeObject<TwitterStatus>(response);
						status.Result = StatusResult.Success;
					}
					catch { status.Result = StatusResult.ParseError; }

					status.AccessLevel = Method.StringToLevelEnum(res.Headers["x-access-level"]);
					status.RateLimited = new RateLimited(res.Headers["x-rate-limit-limit"],
					res.Headers["x-rate-limit-remaining"], res.Headers["x-rate-limit-reset"]);

					status.RequestUrl = res.ResponseUri.ToString();
				}
				else if (result.GetType() == typeof(WebException))
				{
					var res = (WebException)result;

					StreamReader reader = new StreamReader(res.Response.GetResponseStream());
					string response = reader.ReadToEnd();
					reader.Dispose();

					status.Result = Method.GetStatusResult(res.Status);
					status.ResponseObject = null;

					JObject obj = (JObject)JsonConvert.DeserializeObject(response);
					var errors = obj.SelectToken("errors", false);
					if (errors != null)
					{
						if (errors.Type == JTokenType.String)
						{
							status.Error = new TwitterError();
							status.Error.Message = errors.ToString();
						}
						else if (errors.Type == JTokenType.Array)
						{
							status.Error = new TwitterError();
							status.Error = JsonConvert.DeserializeObject<List<TwitterError>>(errors.ToString())[0];
						}
					}

					status.AccessLevel = Method.StringToLevelEnum(res.Response.Headers["x-access-level"]);
					status.RateLimited = new RateLimited(res.Response.Headers["x-rate-limit-limit"],
					res.Response.Headers["x-rate-limit-remaining"], res.Response.Headers["x-rate-limit-reset"]);
					status.RequestUrl = res.Response.ResponseUri.ToString();
				}

				return status;
			}
        }

		/// <summary>
		/// 画像付きでツイートを投稿します。
		/// </summary>
		/// <param name="tweet">ツイート内容</param>
		/// <param name="content">画像のバイト配列</param>
		/// <param name="tokens">OAuthTokens</param>
		/// <param name="options">オプション</param>
		public static TwitterResponse<TwitterStatus> UpdateWithMedia(string tweet, byte[] content, OAuthTokens tokens, StatusUpdateOptions options = null)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			else
			{
				string url = StatuUpdateWithMediaUrl;
				if (options != null) url += Method.ConvertMethod.DictionaryToParams(StatusUpdateOptionsToDictionary(options));


				string boundary = Guid.NewGuid().ToString();

				string header = string.Format("--{0}", boundary);
				string footer = string.Format("--{0}--", boundary);

				string ContentType = "multipart/form-data;boundary=" + boundary;

				/*--------------------  送信するデータの生成  --------------------*/

				var encoding = Encoding.GetEncoding("iso-8859-1");

				string fileData = encoding.GetString(content);


				StringBuilder builder = new StringBuilder();

				// ヘッダーを書き込む
				builder.AppendLine(header);
				builder.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "status"));
				builder.AppendLine();
				builder.AppendLine();

				// 投稿内容をUTF-8で書き込む(日本語文字化け回避)
				builder.AppendLine(encoding.GetString(Encoding.GetEncoding("UTF-8").GetBytes(tweet)));

				// ファイル情報、フッターを書き込む
				builder.AppendLine();
				builder.AppendLine(header);
				builder.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "media[]", string.Format("image-{0}.png", DateTime.Now.ToString("yyyyMMddHHmmss"))));
				builder.AppendLine("Content-Type: application/octet-stream");
				builder.AppendLine();
				builder.AppendLine(fileData);
				builder.AppendLine(footer);

				var result = Method.UploadPost(url, tokens, encoding.GetBytes(builder.ToString()), ContentType);

				TwitterResponse<TwitterStatus> status = new TwitterResponse<TwitterStatus>();

				if (result is HttpWebResponse)
				{
					var res = (WebResponse)result;

					StreamReader reader = new StreamReader(res.GetResponseStream());
					string response = reader.ReadToEnd();
					reader.Dispose();

					try
					{
						status.ResponseObject = JsonConvert.DeserializeObject<TwitterStatus>(response);
						status.Result = StatusResult.Success;
					}
					catch { status.Result = StatusResult.ParseError; }

					status.AccessLevel = Method.StringToLevelEnum(res.Headers["x-access-level"]);
					status.RateLimited = new RateLimited(res.Headers["x-rate-limit-limit"],
					res.Headers["x-rate-limit-remaining"], res.Headers["x-rate-limit-reset"]);

					status.RequestUrl = res.ResponseUri.ToString();
				}
				else if (result.GetType() == typeof(WebException))
				{
					var res = (WebException)result;

					StreamReader reader = new StreamReader(res.Response.GetResponseStream());
					string response = reader.ReadToEnd();
					reader.Dispose();

					status.Result = Method.GetStatusResult(res.Status);
					status.ResponseObject = null;

					JObject obj = (JObject)JsonConvert.DeserializeObject(response);
					var errors = obj.SelectToken("errors", false);
					if (errors != null)
					{
						if (errors.Type == JTokenType.String)
						{
							status.Error = new TwitterError();
							status.Error.Message = errors.ToString();
						}
						else if (errors.Type == JTokenType.Array)
						{
							status.Error = new TwitterError();
							status.Error = JsonConvert.DeserializeObject<List<TwitterError>>(errors.ToString())[0];
						}
					}

					status.AccessLevel = Method.StringToLevelEnum(res.Response.Headers["x-access-level"]);
					status.RateLimited = new RateLimited(res.Response.Headers["x-rate-limit-limit"],
					res.Response.Headers["x-rate-limit-remaining"], res.Response.Headers["x-rate-limit-reset"]);
					status.RequestUrl = res.Response.ResponseUri.ToString();
				}

				return status;
			}
		}


        /// <summary>
        /// ツイートを削除します。
        /// </summary>
        /// <param name="tokens">OAuthTokens</param>
        /// <param name="Id">StatusId</param>
        /// <returns></returns>
        public static TwitterResponse<TwitterStatus> Destroy(OAuthTokens tokens, decimal Id)
        {
            if (tokens == null) throw new ArgumentNullException("Tokens");
            if (Id < 1) throw new ArgumentNullException("Id");

            string Url = string.Format("https://api.twitter.com/1.1/statuses/destroy/{0}.json", Id);

            var response = Method.Post(Url, tokens, null, null);

            TwitterResponse<TwitterStatus> status = new TwitterResponse<TwitterStatus>(response);

            if (response.Result == StatusResult.Success)
            {
                try { status.ResponseObject = JsonConvert.DeserializeObject<TwitterStatus>(response.ResponseStream); }
                catch (Exception) { status.Result = StatusResult.ParseError; }
            }

            return status;
        }


        /// <summary>
        /// 指定したツイートをリツイートします。
        /// </summary>
        /// <param name="id">StatusId</param>
        /// <param name="tokens">OAuthTokens</param>
        public static TwitterResponse<TwitterStatus> Retweet(decimal id, OAuthTokens tokens)
        {
            if (id < 1) throw new ArgumentNullException("Id");
            else if (tokens == null) throw new ArgumentNullException("Tokens");
            else
            {
                string url = string.Format(RetweetUrl, id);

                var result = Method.Post(url, tokens, null);

                TwitterResponse<TwitterStatus> status = new TwitterResponse<TwitterStatus>(result);
                if (result.Result == StatusResult.Success)
                {
                    try { status.ResponseObject = JsonConvert.DeserializeObject<TwitterStatus>(result.ResponseStream); }
                    catch (Exception)
                    {
                        status.ResponseObject = null;
                        status.Result = StatusResult.ParseError;
                    }
                }
                return status;
            }
        }


        /// <summary>
        /// リツイートを取り消します。
        /// </summary>
        /// <param name="id">StatusId</param>
        /// <param name="tokens">OAuthTokens</param>
        public static TwitterResponse<TwitterStatus> DestroyRetweet(decimal id, OAuthTokens tokens)
        {
            if (id < 1) throw new ArgumentNullException("Id");
            else if (tokens == null) throw new ArgumentNullException("Tokens");
            else
            {
                string url = string.Format(DestroyRetweetUrl, id);

                var result = Method.Post(url, tokens, null);

                TwitterResponse<TwitterStatus> status = new TwitterResponse<TwitterStatus>(result);
                if (result.Result == StatusResult.Success)
                {
                    try { status.ResponseObject = JsonConvert.DeserializeObject<TwitterStatus>(result.ResponseStream); }
                    catch (Exception)
                    {
                        status.Result = StatusResult.ParseError;
                    }
                }
                return status;
            }
        }


        /// <summary>
        /// StatusUpdateOptionsをDictionaryに変換
        /// </summary>
        private static Dictionary<string, string> StatusUpdateOptionsToDictionary(StatusUpdateOptions option)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (option.InReplyToStatusId > 0) dic.Add("in_reply_to_status_id", option.InReplyToStatusId.ToString());
            if (option.Lat > 0) dic.Add("lat", option.Lat.ToString());
            if (option.Long > 0) dic.Add("long", option.Long.ToString());
            if (option.PlaceId != null) dic.Add("place_id", option.PlaceId);
            if (option.DisplayCoordinates != null) dic.Add("display_coordinates", option.DisplayCoordinates.ToString());
            return dic;
        }


    }
}
