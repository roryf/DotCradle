using DotCradle.Impl;
using Machine.Specifications;

namespace DotCradle.Specs.Connection
{
    public class CradleConnectionSpecs
    {
        [Subject(typeof(CradleConnection), "listing available databases")]
        public class when_databases_exist
        {
            private static CradleConnection conn;
            private static string databases;

            Establish context = () =>
                                {
                                    conn = new CradleConnection(CradleConnectionOptions.Localhost);
                                    conn.Database("foobar").Create();
                                };

            Because of = () => databases = conn.Databases();

            It should_return_list_of_databases = () => databases.ShouldContain("foobar");
        }

        [Subject(typeof(CradleConnection), "uuids")]
        public class when_getting_uuids_from_server
        {
            private static CradleConnection conn;
            private static string result;

            Establish context = () => conn  =new CradleConnection(CradleConnectionOptions.Localhost);

            Because of = () => result = conn.Uuids(1);

            It should_get_response = () => result.Length.ShouldBeGreaterThan(1);
        }

        [Subject(typeof(CradleConnection), "server info")]
        public class when_getting_server_info
        {
            private static CradleConnection conn;
            private static string result;

            Establish context = () => conn = new CradleConnection(CradleConnectionOptions.Localhost);

            Because of = () => result = conn.Info();

            It should_get_response = () => result.Length.ShouldBeGreaterThan(1);

            It should_contain_welcome_message = () => result.ShouldContain("Welcome");
        }

        [Subject(typeof(CradleConnection), "server config")]
        public class when_getting_server_config
        {
            private static CradleConnection conn;
            private static string result;

            Establish context = () => conn = new CradleConnection(CradleConnectionOptions.Localhost);

            Because of = () => result = conn.Config();

            It should_get_response = () => result.Length.ShouldBeGreaterThan(1);
        }

        [Subject(typeof(CradleConnection), "database")]
        public class when_getting_database
        {
            private static CradleConnection conn;
            private static ICradleDb result;

            Establish context = () => conn = new CradleConnection(CradleConnectionOptions.Localhost);

            Because of = () => result = conn.Database("foobar");

            It should_return_datbase_with_correct_name = () => result.Name.ShouldEqual("foobar");
        }
    }
}