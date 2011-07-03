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

        private CradleResponse Request(string method, string path)
        {
            return Request(method, path, new Dictionary<string, string>());
        }

        private CradleResponse Request(string method, string path, string data)
        {
            return Request(method, path, new Dictionary<string, string>(), data);
        }

        private CradleResponse Request(string method, string path, IDictionary<string, string> urlParams, string data = null)
        {
            return
                CradleRequest.WithOptions(_options).WithHttpVerb(method).WithPath(path).WithUrlParameters(urlParams).
                    WithPostData(data).Execute();
        }

        public CradleResponse Get(string path)
        {
            return Request(CradleRequest.HttpGet, path);
        }

        public CradleResponse Get(string path, IDictionary<string, string> urlParams)
        {
            return Request(CradleRequest.HttpGet, path, urlParams);
        }

        public CradleResponse Post(string path, string data)
        {
            return Request(CradleRequest.HttpPost, path, data);
        }

        public CradleResponse Put(string path)
        {
            return Request(CradleRequest.HttpPut, path);
        }

        public CradleResponse Put(string path, string data)
        {
            return Request(CradleRequest.HttpPut, path, data);
        }

        public CradleResponse Head(string path)
        {
            return Request(CradleRequest.HttpHead, path);
        }

        public CradleResponse Delete(string path)
        {
            return Request(CradleRequest.HttpDelete, path);
        }

        public CradleResponse Delete(string path, IDictionary<string, string> urlParams)
        {
            return Request(CradleRequest.HttpDelete, path, urlParams);
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