using System;
using System.Collections.Generic;
using System.Net;

namespace DotCradle.Impl
{
    public class CradleDb : ICradleDb
    {
        private readonly string _name;
        private readonly ICradleConnection _connection;

        public CradleDb(string name, ICradleConnection connection)
        {
            _name = name;
            _connection = connection;
        }

        public string Name { get { return _name; }}

        private string GetPath(string path)
        {
            return string.Join("/", _name, path);
        }

        public bool Exists()
        {
            return _connection.Head(GetPath("/")).StatusCode == HttpStatusCode.OK;
        }

        public string Create()
        {
            return _connection.Put(GetPath("/")).Data;
        }

        public string Destroy()
        {
            return _connection.Delete(GetPath("/")).Data;
        }

        public string All()
        {
            return _connection.Get(GetPath("/_all_docs")).Data;
        }

        public string Info()
        {
            return _connection.Get(GetPath("/")).Data;
        }

        public string Get(string id)
        {
            return _connection.Get(GetPath("/" + id)).Data;
        }

        public string Get(string id, string rev)
        {
            return _connection.Get(GetPath("/" + id), new Dictionary<string, string> {{"rev", rev}}).Data;
        }

        public string Head(string id)
        {
            return _connection.Head(GetPath("/" + id)).Data;
        }

        public string Put(string id, string doc)
        {
            return _connection.Put(GetPath("/" + id), doc).Data;
        }

        public string Post(string doc)
        {
            return _connection.Post(GetPath("/"), doc).Data;
        }

        public string Delete(string id, string rev)
        {
            return _connection.Delete(GetPath("/" + id), new Dictionary<string, string> { { "rev", rev } }).Data;
        }
    }
}