using System;

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

        public bool Exists()
        {
            throw new NotImplementedException();
        }

        public string Create()
        {
            throw new NotImplementedException();
        }

        public string Destroy()
        {
            throw new NotImplementedException();
        }

        public string All()
        {
            throw new NotImplementedException();
        }

        public string Info()
        {
            throw new NotImplementedException();
        }

        public string Get(string id)
        {
            throw new NotImplementedException();
        }

        public string Get(string id, string rev)
        {
            throw new NotImplementedException();
        }

        public string Head(string id)
        {
            throw new NotImplementedException();
        }

        public string Put(string id, string doc)
        {
            throw new NotImplementedException();
        }

        public string Post(string doc)
        {
            throw new NotImplementedException();
        }

        public string Delete(string id, string rev)
        {
            throw new NotImplementedException();
        }
    }
}