using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Xml.Serialization;

namespace TwitterAPI
{

    public class TwitterUpdateMediaService
    {
        private static OAuthBase oauth = new OAuthBase();

        private static string Provider_Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
        private static string TwitPic_Url = "http://api.twitpic.com/2/upload.json";
        private static string TwitPicRealm = "http://api.twitpic.com/";
        private static string ImgLy_Url = "http://img.ly/api/2/upload.json";
        private static string TwitterRealm = "http://api.twitter.com/";
        private static string TwipplePhoto_Url = "http://p.twipple.jp/api/upload2";
        private static string YFrog_Url = "https://yfrog.com/api/xauth_upload";

        private static string GenerateHeader(OAuthTokens tokens, string url,
            out string normalizedUrl, out string normalizedReqParam)
        {
            var uri = new Uri(url);

            var nonce = oauth.GenerateNonce();
            var timestamp = oauth.GenerateTimeStamp();
            var signature = oauth.GenerateSignature(uri,
                tokens.ConsumerKey, tokens.ConsumerSecret, tokens.AccessToken, tokens.AccessTokenSecret,
                "GET", timestamp, nonce, out normalizedUrl, out normalizedReqParam);

            var oauthSignaturePattern = "OAuth realm=\"{0:s}\", oauth_consumer_key=\"{1:s}\","
                + " oauth_signature_method=\"HMAC-SHA1\","
                + " oauth_token=\"{2:s}\", oauth_timestamp=\"{3:s}\", oauth_nonce=\"{4:s}\","
                + " oauth_version=\"1.0\", oauth_signature=\"{5:s}\"";

            return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                oauthSignaturePattern,
                TwitPicRealm, tokens.ConsumerKey, tokens.AccessToken,
                timestamp, nonce, HttpUtility.UrlEncode(signature));
        }

        /// <summary>
        /// TwitPicに画像を投稿します
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="tweet"></param>
        /// <param name="fileName"></param>
        /// <param name="ApiKey"></param>
        /// <returns></returns>
        public static TwitterResponse<UpdateImageJsonReponse> UpdateTwitPic(OAuthTokens tokens, string tweet, string fileName,
            string ApiKey)
        {

            var encoding = Encoding.GetEncoding("iso-8859-1");

            var boundary = Guid.NewGuid().ToString();

            string bodyHead = string.Format("--{0}", boundary);
            string bodyFoot = string.Format("--{0}--", boundary);


            string FileContentType = "";
            switch (Path.GetExtension(fileName))
            {
                case ".png":
                    FileContentType = "image/png";
                    break;
            }

            StringBuilder contents = new StringBuilder();


            string fileHeader = string.Format("Content-Disposition: file; name=\"{0:s}\";"
                    + " filename=\"{1:s}\"", "media", Path.GetFileName(fileName));

            string fileData = FileToString(fileName, encoding);

            contents.AppendLine(bodyHead);
            contents.AppendLine(fileHeader);
            contents.AppendLine(string.Format("Content-Type: {0:s}", FileContentType));
            contents.AppendLine();
            contents.AppendLine(fileData);

            contents.AppendLine(bodyHead);
            contents.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0:s}\"", "key"));
            contents.AppendLine();
            contents.AppendLine(ApiKey);

            contents.AppendLine(bodyHead);
            contents.AppendLine("Content-Disposition: form-data; name=\"message\"");
            contents.AppendLine();
            contents.AppendLine(encoding.GetString(Encoding.UTF8.GetBytes(tweet)));


            contents.AppendLine(bodyFoot);


            byte[] bytes = encoding.GetBytes(contents.ToString());


            var req = OAuthEchoUpdate(tokens, bytes, boundary, TwitPicRealm, Provider_Url, TwitPic_Url);

            var response = new TwitterResponse<UpdateImageJsonReponse>();

            if (req.GetType() == typeof(HttpWebResponse))
            {
                try
                {
                    var res = (WebResponse)req;
                    var reader = new StreamReader(res.GetResponseStream());
                    response.ResponseObject = JsonConvert.DeserializeObject<UpdateImageJsonReponse>(reader.ReadToEnd());
                    reader.Close();
                    response.Result = StatusResult.Success;
                    response.RequestUrl = res.ResponseUri.ToString();
                }
                catch(Exception)
                {
                    response.Result = StatusResult.ParseError;
                }

            }
            else if (response.GetType() == typeof(WebException))
            {
                var ex = (WebException)req;
                var reader = new StreamReader(ex.Response.GetResponseStream());
                string res = reader.ReadToEnd();
                reader.Dispose();

                var obj = (JObject)JsonConvert.DeserializeObject(res);
                var errors = obj.SelectToken("errors", false);
                if (errors != null)
                {
                    if (errors.Type == JTokenType.String)
                    {
                        response.Error = new TwitterError();
                        response.Error.Message = errors.ToString();
                    }
                    else if (errors.Type == JTokenType.Array)
                    {
                        response.Error = new TwitterError();
                        response.Error = JsonConvert.DeserializeObject<List<TwitterError>>(errors.ToString())[0];
                    }
                    response.Result = Method.GetStatusResult(ex.Status);
                }
            }

            return response;
        }


        /// <summary>
        /// img.lyに画像を投稿します
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="tweet"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static TwitterResponse<UpdateImageJsonReponse> UpdateImgLy(OAuthTokens tokens, string tweet, string fileName)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");

            var boundary = Guid.NewGuid().ToString();

            var bodyHead = string.Format("--{0}", boundary);
            var bodyFoot = string.Format("--{0}--", boundary);


            var FileContentType = "";
            switch (Path.GetExtension(fileName))
            {
                case ".png":
                    FileContentType = "image/png";
                    break;
            }

            var contents = new StringBuilder();


            var fileHeader = string.Format("Content-Disposition: file; name=\"{0:s}\";"
                    + " filename=\"{1:s}\"", "media", Path.GetFileName(fileName));

            var fileData = FileToString(fileName, encoding);

            contents.AppendLine(bodyHead);
            contents.AppendLine(fileHeader);
            contents.AppendLine(string.Format("Content-Type: {0:s}", FileContentType));
            contents.AppendLine();
            contents.AppendLine(fileData);

            contents.AppendLine(bodyHead);
            contents.AppendLine("Content-Disposition: form-data; name=\"message\"");
            contents.AppendLine();
            contents.AppendLine(encoding.GetString(Encoding.UTF8.GetBytes(tweet)));


            contents.AppendLine(bodyFoot);

            System.Diagnostics.Debug.WriteLine(contents.ToString());

            var bytes = encoding.GetBytes(contents.ToString());


            var req = OAuthEchoUpdate(tokens, bytes, boundary, TwitterRealm, Provider_Url, ImgLy_Url);

            var response = new TwitterResponse<UpdateImageJsonReponse>();

            if (req.GetType() == typeof(HttpWebResponse))
            {
                try
                {
                    var res = (WebResponse)req;
                    var reader = new StreamReader(res.GetResponseStream());
                    response.ResponseObject = JsonConvert.DeserializeObject<UpdateImageJsonReponse>(reader.ReadToEnd());
                    reader.Close();
                    response.Result = StatusResult.Success;
                    response.RequestUrl = res.ResponseUri.ToString();
                }
                catch
                {
                    response.Result = StatusResult.ParseError;
                }

            }
            else if (response.GetType() == typeof(WebException))
            {
                var ex = (WebException)req;
                var reader = new StreamReader(ex.Response.GetResponseStream());
                string res = reader.ReadToEnd();
                reader.Dispose();

                var obj = (JObject)JsonConvert.DeserializeObject(res);
                var errors = obj.SelectToken("errors", false);
                if (errors != null)
                {
                    if (errors.Type == JTokenType.String)
                    {
                        response.Error = new TwitterError();
                        response.Error.Message = errors.ToString();
                    }
                    else if (errors.Type == JTokenType.Array)
                    {
                        response.Error = new TwitterError();
                        response.Error = JsonConvert.DeserializeObject<List<TwitterError>>(errors.ToString())[0];
                    }
                    response.Result = Method.GetStatusResult(ex.Status);
                }
            }

            return response;
        }


        /// <summary>
        /// ついっぷるフォトに画像を投稿します
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="tweet"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static TwitterResponse<UpdateImageXmlResponse> UpdateTwipplePhoto(OAuthTokens tokens, string fileName)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");

            var boundary = Guid.NewGuid().ToString();

            string normalizedUrl, normalizedReqParam;
            var signature = GenerateHeader(tokens, Provider_Url, out normalizedUrl, out normalizedReqParam);

            var bodyHead = string.Format("--{0}", boundary);
            var bodyFoot = string.Format("--{0}--", boundary);


            var FileContentType = "";
            switch (Path.GetExtension(fileName))
            {
                case ".png":
                    FileContentType = "image/png";
                    break;
            }

            var contents = new StringBuilder();


            var fileHeader = string.Format("Content-Disposition: file; name=\"{0:s}\";"
                    + " filename=\"{1:s}\"", "media", Path.GetFileName(fileName));

            var fileData = FileToString(fileName, encoding);

            contents.AppendLine(bodyHead);

            contents.AppendLine(fileHeader);
            contents.AppendLine(string.Format("Content-Type: {0:s}", FileContentType));
            contents.AppendLine();
            contents.AppendLine(fileData);

            contents.AppendLine(bodyFoot);


            System.Diagnostics.Debug.WriteLine(contents.ToString());

            var bytes = encoding.GetBytes(contents.ToString());


            var req = OAuthEchoUpdate(tokens, bytes, boundary, TwitterRealm, Provider_Url, TwipplePhoto_Url);

            var response = new TwitterResponse<UpdateImageXmlResponse>();

            if (req.GetType() == typeof(HttpWebResponse))
            {
                try
                {
                    var res = (WebResponse)req;
                    var reader = new StreamReader(res.GetResponseStream());
                    var result = reader.ReadToEnd();
                    reader.Dispose();

                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(result)))
                    {
                        try
                        {
                            if (result.Contains("<rsp stat=\"ok\">"))
                            {
                                var serializer = new XmlSerializer(typeof(UpdateImageXmlResponse));
                                response.ResponseObject = (UpdateImageXmlResponse)serializer.Deserialize(stream);
                            }
                            else if (result.Contains("<rsp stat=\"fail\">"))
                            {
                                var serializer = new XmlSerializer(typeof(UpdateImageXmlError));
                                var error = (UpdateImageXmlError)serializer.Deserialize(stream);
                                response.Error = new TwitterError();
                                response.Error.Message = error.Error.Message;
                                response.Error.ErrorCode = error.Error.Code;
                                switch (response.Error.ErrorCode)
                                {
                                    case 1001:
                                        response.Result = StatusResult.Unauthorized;
                                        break;
                                    case 1002:
                                        response.Result = StatusResult.FileNotFound;
                                        break;
                                    default:
                                        response.Result = StatusResult.Unknown;
                                        break;
                                }
                            }
                            else  response.Result = StatusResult.ParseError;

                        } catch(Exception ex) { response.Result = StatusResult.ParseError;
                        }
                    }
                }
                catch(Exception)
                {
                    response.Result = StatusResult.ParseError;
                }

            }
            else if (req.GetType() == typeof(WebException))
            {
                var ex = (WebException)req;
                response.Result = Method.GetStatusResult(ex.Status);
            }

            return response;
        }


        /// <summary>
        /// yfrogに画像を投稿します
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="fileName"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static TwitterResponse<UpdateImageJsonResponse2> UpdateYFrog(OAuthTokens tokens, string fileName, string apiKey)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");

            var boundary = Guid.NewGuid().ToString();

            string normalizedUrl, normalizedReqParam;
            var signature = GenerateHeader(tokens, Provider_Url, out normalizedUrl, out normalizedReqParam);

            var bodyHead = string.Format("--{0}", boundary);
            var bodyFoot = string.Format("--{0}--", boundary);

            var FileContentType = "";
            switch (Path.GetExtension(fileName))
            {
                case ".png":
                    FileContentType = "image/png";
                    break;
            }

            var contents = new StringBuilder();

            var fileHeader = string.Format("Content-Disposition: file; name=\"{0:s}\";"
                    + " filename=\"{1:s}\"", "media", Path.GetFileName(fileName));

            var fileData = FileToString(fileName, encoding);

            contents.AppendLine(bodyHead);

            contents.AppendLine(fileHeader);
            contents.AppendLine(string.Format("Content-Type: {0:s}", FileContentType));
            contents.AppendLine();
            contents.AppendLine(fileData);

            contents.AppendLine(bodyHead);
            contents.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0:s}\"", "key"));
            contents.AppendLine();
            contents.AppendLine(apiKey);

            contents.AppendLine(bodyFoot);


            var bytes = encoding.GetBytes(contents.ToString());


            var req = OAuthEchoUpdate(tokens, bytes, boundary, TwitterRealm, Provider_Url, YFrog_Url);

            var response = new TwitterResponse<UpdateImageJsonResponse2>();

            if (req.GetType() == typeof(HttpWebResponse))
            {
                var res = (WebResponse)req;
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string str = reader.ReadToEnd();
                reader.Dispose();

                try
                {
                    JObject obj = (JObject)JsonConvert.DeserializeObject(str);
                    var rsp = obj.SelectToken("rsp", false);
                    if (rsp != null)
                    {
                        response.ResponseObject = JsonConvert.DeserializeObject<UpdateImageJsonResponse2>(rsp.ToString());
                        reader.Close();
                        response.Result = StatusResult.Success;
                        response.RequestUrl = res.ResponseUri.ToString();
                    }
                }
                catch(Exception ex)
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(UpdateImageXmlError));
                        var stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
                        var error = (UpdateImageXmlError)serializer.Deserialize(stream);
                        stream.Close();
                        response.Error.ErrorCode = (int)error.Error.Code;
                        response.Error.Message = error.Error.Message;
                        switch (response.Error.ErrorCode)
                        {
                            case 1001:
                                response.Result = StatusResult.Unauthorized;
                                break;
                            case 1002:
                                response.Result = StatusResult.FileNotFound;
                                break;
                            default:
                                response.Result = StatusResult.Unknown;
                                break;
                        }
                    }
                    catch
                    {
                        response.Result = StatusResult.ParseError;
                    }
                }
            }
            else if (req.GetType() == typeof(WebException))
            {
                var ex = (WebException)req;
                response.Result = Method.GetStatusResult(ex.Status);
            }

            return response;
        }

        /// <summary>
        /// ファイルを文字列にエンコーディングして返します
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>文字列</returns>
        public static string FileToString(string fileName, Encoding encoding)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] imgBytes = new byte[fs.Length];
            fs.Read(imgBytes, 0, imgBytes.Length);
            string fileData = encoding.GetString(imgBytes);
            fs.Close();
            return fileData;
        }

        private static object OAuthEchoUpdate(OAuthTokens tokens, byte[] postBody, string boundary, string realm_url, string provider_url, string upload_url)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");

            /*-------------------- OAuth認証 --------------------*/

            string normalizedUrl, normalizedReqParam;

            var nonce = oauth.GenerateNonce();
            var timestamp = oauth.GenerateTimeStamp();
            var signature = oauth.GenerateSignature(new Uri(provider_url),
                tokens.ConsumerKey, tokens.ConsumerSecret, tokens.AccessToken, tokens.AccessTokenSecret, "GET",
                timestamp, nonce, out normalizedUrl, out normalizedReqParam);

            var oauthHeader = string.Format("OAuth realm=\"{0:s}\", oauth_consumer_key=\"{1:s}\","
                + " oauth_signature_method=\"HMAC-SHA1\","
                + " oauth_token=\"{2:s}\", oauth_timestamp=\"{3:s}\", oauth_nonce=\"{4:s}\","
                + " oauth_version=\"1.0\", oauth_signature=\"{5:s}\"",
            realm_url, tokens.ConsumerKey, tokens.AccessToken, timestamp, nonce, HttpUtility.UrlEncode(signature));


            /*-------------------- リクエストの生成 --------------------*/

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(upload_url);
            req.Proxy = null;
            req.Headers.Add("X-Auth-Service-Provider", provider_url);
            req.Headers.Add("X-Verify-Credentials-Authorization", oauthHeader);
            req.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            req.Method = "POST";
            req.ServicePoint.Expect100Continue = false;


            /*-------------------- POST内容の書き込み --------------------*/

            using (var writer = req.GetRequestStream())
                writer.Write(postBody, 0, postBody.Length);

            try
            {
                return req.GetResponse();
            }
            catch (WebException ex)
            {
                var reader = new StreamReader(ex.Response.GetResponseStream());
                reader.Dispose();
                return ex;
            }
        }
    
    }

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class ServiceNameAttribute : Attribute
	{
		private string name;

		public ServiceNameAttribute(string name)
		{
			this.name = name;
		}

		public static string GetName(Enum value)
		{
			Type enumType = value.GetType();
			string ename =  Enum.GetName(enumType, value);
			ServiceNameAttribute[] attrs = (ServiceNameAttribute[])enumType.GetField(ename)
				.GetCustomAttributes(typeof(ServiceNameAttribute), false);
			return attrs[0].name;
		}
	}

    public enum ImageUpdateService
    {
        [ServiceName("Twitter公式")]
        Twitter,

		[ServiceName("Twitpic")]
        TwitPic,

		[ServiceName("img.ly")]
        img_ly,

		[ServiceName("yfrog Twitter")]
        yfrog,

		[ServiceName("ついっぷるフォト")]
        TwipplePhoto,
    }

    
}
