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

        private string RawRequest(string method, string path)
        {
            return RawRequest(method, path, new Dictionary<string, string>());
        }

        private string RawRequest(string method, string path, IDictionary<string, string> urlParams)
        {
            return RawRequest(method, path, urlParams, null);
        }

        private string RawRequest(string method, string path, IDictionary<string, string> urlParams, string data)
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

            var response = request.GetResponse();
            var responseData = response.GetResponseStream().ReadToEnd();
            return Encoding.UTF8.GetString(responseData, 0, responseData.Length);
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

        public string Databases()
        {
            return RawRequest("get", "_all_dbs");
        }

        public string Uuids(int count)
        {
            return RawRequest("get", "_uuids", new Dictionary<string, string> {{"count", count.ToString()}});
        }

        public string Info()
        {
            return RawRequest("get", "/");
        }

        public string Config()
        {
            return RawRequest("get", "_config");
        }

        public ICradleDb Database(string name)
        {
            return new CradleDb(name, this);
        }
    }
}