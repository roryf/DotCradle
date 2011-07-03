using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DotCradle.Extensions;

namespace DotCradle.Impl
{
    public class CradleRequest
    {
        private string _httpMethod;
        private string _path;
        private string _data;
        private IDictionary<string, string> _urlParams;
        private CradleConnectionOptions _options;

        public static CradleRequest WithOptions(CradleConnectionOptions options)
        {
            var request = new CradleRequest();
            request._options = options;
            return request;
        }

        public CradleRequest WithHttpMethod(string method)
        {
            _httpMethod = method;
            return this;
        }

        public CradleRequest WithPath(string path)
        {
            _path = path;
            return this;
        }

        public CradleRequest WithPostData(string data)
        {
            _data = data;
            return this;
        }

        public CradleRequest WithUrlParameters(IDictionary<string, string> urlParams)
        {
            _urlParams = urlParams;
            return this;
        }

        public CradleResponse Execute()
        {
            var path = (string.IsNullOrWhiteSpace(_path) ? "/" : _path).Replace(new Regex(@"/https?:\/\//"), "").Replace(new Regex(@"/\/{2,}/"), "");
            if (path[0] != '/')
            {
                path = '/' + path;
            }

            var requestUri = BuildRequestUri(path, _urlParams);

            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = _httpMethod.ToUpper();
            request.UserAgent = string.Format(CultureInfo.InvariantCulture, "DotCradle/{0}",
                                                  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            if (!string.IsNullOrWhiteSpace(_data))
            {
                var byteArray = Encoding.UTF8.GetBytes(_data);
                var dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                request.ContentLength = byteArray.Length;
                request.ContentType = "application/json";
            }
            else
            {
                request.ContentLength = 0;
            }

            var webResponse = (HttpWebResponse)request.GetResponse();
            var responseData = webResponse.GetResponseStream().ReadToEnd();
            var content = Encoding.UTF8.GetString(responseData, 0, responseData.Length);


            var response = new CradleResponse
            {
                Data = content,
                Headers = GetHeaders(webResponse.Headers),
                StatusCode = webResponse.StatusCode
            };
            return response;
        }

        private IDictionary<string, string> GetHeaders(WebHeaderCollection webHeaders)
        {
            var headers = new Dictionary<string, string>();
            foreach (var key in webHeaders.AllKeys)
            {
                headers[key] = webHeaders[key];
            }
            return headers;
        }

        private Uri BuildRequestUri(string path, IDictionary<string, string> urlParams)
        {
            var url = string.Format("{0}://{1}:{2}{3}", _options.Secure ? "https" : "http", _options.Host, _options.Port, path);

            if (urlParams.Count > 0)
            {
                var paramsAsString = string.Join(
                    "&",
                    urlParams.Select((kvp) => string.Join("=", new[] { kvp.Key, HttpUtility.UrlEncode(kvp.Value) }))
                    );
                url = string.Format("{0}?{1}", url, paramsAsString);
            }

            return new Uri(url);
        }

        public const string HttpGet = "GET";
        public const string HttpPost = "POST";
        public const string HttpPut = "PUT";
        public const string HttpHead = "HEAD";
        public const string HttpInfo = "INFO";
        public const string HttpDelete = "DELETE";
    }
}