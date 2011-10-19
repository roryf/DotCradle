using System.Net;
using DotCradle.Impl;
using Machine.Specifications;

namespace DotCradle.Specs.Db
{
    public class CheckingForExistence
    {
        [Subject(typeof (CradleDb), "checking for existence")]
        public class when_database_does_not_exist
        {
            private static CradleConnection conn;
            private static CradleDb db;
            private static bool result;

            Establish context = () => 
                { 
                    conn = new CradleConnection(CradleConnectionOptions.Localhost);
                    db = conn.Database("foobar") as CradleDb;
                };

            Because of = () => result = db.Exists();

            It should_return_false = () => result.ShouldBeFalse();
        }

        [Subject(typeof(CradleDb), "checking for existence")]
        public class when_database_exists
        {
            private static CradleConnection conn;
            private static CradleDb db;
            private static bool result;

            Establish context = () =>
            {
                conn = new CradleConnection(CradleConnectionOptions.Localhost);
                db = conn.Database("foobar") as CradleDb;
                db.Create();
            };

            Because of = () => result = db.Exists();

            It should_return_true = () => result.ShouldBeTrue();
        }
    }

    public class CreatingDatabase
    {
        [Subject(typeof(CradleDb), "creating")]
        public class when_database_does_not_exist
        {
            private static CradleConnection conn;
            private static CradleDb db;
            private static string result;
            private static bool exists;

            Establish context = () =>
                {
                    conn = new CradleConnection(CradleConnectionOptions.Localhost);
                    db = conn.Database("foobar") as CradleDb;
                };

            Because of = () =>
                {
                    result = db.Create();
                    exists = conn.Head("/foobar").StatusCode == HttpStatusCode.OK;
                };

            It should_get_response = () => result.Length.ShouldBeGreaterThan(0);

            It should_create_database = () => exists.ShouldBeTrue();
        }
    }

    public class DestroyingDatabase
    {
        [Subject(typeof(CradleDb), "destroying")]
        public class when_database_exists
        {
            private static CradleConnection conn;
            private static CradleDb db;
            private static string result;
            private static bool exists;

            Establish context = () =>
                                        {
                                            conn = new CradleConnection(CradleConnectionOptions.Localhost);
                                            conn.Put("/foobar");
                                            db = conn.Database("foobar") as CradleDb;
                                        };

            Because of = () =>
                                 {
                                     result = db.Destroy();
                                     exists = conn.Head("/foobar").StatusCode == HttpStatusCode.OK;
                                 };

            It should_get_response = () => result.Length.ShouldBeGreaterThan(0);

            It should_destroy_database = () => exists.ShouldBeFalse();
        }
    }

    public class DatabaseInfo
    {
        [Subject(typeof(CradleDb), "database info")]
        public class when_database_exists
        {
            private static CradleConnection conn;
            private static CradleDb db;
            private static string result;

            Establish context = () =>
            {
                conn = new CradleConnection(CradleConnectionOptions.Localhost);
                conn.Put("/foobar");
                db = conn.Database("foobar") as CradleDb;
            };

            Because of = () =>
            {
                result = db.Info();
            };

            It should_get_response = () => result.Length.ShouldBeGreaterThan(0);

            It should_get_response_for_correct_database = () => result.ShouldContain("foobar");
        }
    }

    public class Documents
    {
        [Subject(typeof(CradleDb), "getting all documents")]
        public class when_documents_exist
        {
            private static CradleConnection conn;
            private static CradleDb db;
            private static string result;

            Establish context = () =>
            {
                conn = new CradleConnection(CradleConnectionOptions.Localhost);
                conn.Put("/foobar");
                conn.Put("/foobar/yermaw", @"{""name"": ""foobar""}");
                db = conn.Database("foobar") as CradleDb;
            };

            Because of = () =>
            {
                result = db.All();
            };

            It should_get_response = () => result.Length.ShouldBeGreaterThan(0);

            It should_contain_document_in_response = () => result.ShouldContain(@"""id"":""yermaw""");
        }

        [Subject(typeof(CradleDb), "getting document by id")]
        public class when_document_exists
        {
            private static CradleConnection conn;
            private static CradleDb db;
            private static string result;

            Establish context = () =>
            {
                conn = new CradleConnection(CradleConnectionOptions.Localhost);
                conn.Put("/foobar");
                conn.Put("/foobar/yermaw", @"{""name"": ""foobar""}");
                db = conn.Database("foobar") as CradleDb;
            };

            Because of = () =>
            {
                result = db.Get("yermaw");
            };

            It should_get_response = () => result.Length.ShouldBeGreaterThan(0);

            It should_get_correct_doc = () => result.ShouldContain(@"""name"":""foobar""");
        }
    }
}