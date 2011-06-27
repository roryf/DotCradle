using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DotCradle.Extensions;

namespace DotCradle.Impl
{
    public class CradleConnection : ICradleConnection
    {
        private readonly CradleConnectionOptions _options;

        public CradleConnection(CradleConnectionOptions options)
        {
            _options = options;
        }

        private CradleResponse RawRequest(string method, string path)
        {
            return RawRequest(method, path, new Dictionary<string, string>());
        }

        private CradleResponse RawRequest(string method, string path, string data)
        {
            return RawRequest(method, path, new Dictionary<string, string>(), data);
        }

        private CradleResponse RawRequest(string method, string path, IDictionary<string, string> urlParams, string data = null)
        {
            path = (string.IsNullOrWhiteSpace(path) ? "/" : path).Replace(new Regex(@"/https?:\/\//"), "").Replace(new Regex(@"/\/{2,}/"), "");
            if (path[0] != '/')
            {
                path = '/' + path;
            }

            var requestUri = BuildRequestUri(path, urlParams);

            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = method.ToUpper();
            request.UserAgent = string.Format(CultureInfo.InvariantCulture, "DotCradle/{0}",
                                                  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            if (!string.IsNullOrWhiteSpace(data))
            {
                var byteArray = Encoding.UTF8.GetBytes(data);
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
                    urlParams.Select((kvp) => string.Join("=", new[] {kvp.Key, HttpUtility.UrlEncode(kvp.Value)}))
                    );
                url = string.Format("{0}?{1}", url, paramsAsString);
            }

            return new Uri(url);
        }

        public CradleResponse Get(string path)
        {
            return RawRequest("get", path);
        }

        public CradleResponse Get(string path, IDictionary<string, string> urlParams)
        {
            return RawRequest("get", path, urlParams);
        }

        public CradleResponse Post(string path, string data)
        {
            return RawRequest("post", path, data);
        }

        public CradleResponse Put(string path)
        {
            return RawRequest("put", path);
        }

        public CradleResponse Put(string path, string data)
        {
            return RawRequest("put", path, data);
        }

        public CradleResponse Head(string path)
        {
            return RawRequest("head", path);
        }

        public CradleResponse Delete(string path)
        {
            return RawRequest("delete", path);
        }

        public string Databases()
        {
            return Get("_all_dbs").Data;
        }

        public string Uuids(int count)
        {
            return Get("_uuids", new Dictionary<string, string> { { "count", count.ToString() } }).Data;
        }

        public string Info()
        {
            return Get("/").Data;
        }

        public string Config()
        {
            return Get("_config").Data;
        }

        public ICradleDb Database(string name)
        {
            return new CradleDb(name, this);
        }
    }
}