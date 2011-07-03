using System;
using DotCradle.Impl;
using NUnit.Framework;

namespace DotCradle.Tests
{
    [TestFixture]
    public class when_getting_list_of_databases : SpecificationBase
    {
        private ICradleConnection conn;
        private string dbs;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
            conn.Database("foobar").Create();
        }

        public override void AfterEach()
        {
            conn.Database("foobar").Destroy();
        }

        public override void Because()
        {
            dbs = conn.Databases();
        }

        [Test]
        public void should_get_response()
        {
            dbs.Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void should_contain_db_name()
        {
            dbs.ShouldContain("foobar");
        }
    }

    [TestFixture]
    public class when_getting_list_of_uuids : SpecificationBase
    {
        private ICradleConnection conn;
        private string result;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
        }

        public override void Because()
        {
            result = conn.Uuids(1);
        }

        [Test]
        public void should_get_response()
        {
            result.Length.ShouldBeGreaterThan(0);
        }
    }

    [TestFixture]
    public class when_getting_server_info : SpecificationBase
    {
        private ICradleConnection conn;
        private string result;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
        }

        public override void Because()
        {
            result = conn.Info();
        }

        [Test]
        public void should_get_response()
        {
            result.Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void should_contain_expected_message()
        {
            result.ShouldContain("Welcome");
        }
    }

    [TestFixture]
    public class when_getting_server_config : SpecificationBase
    {
        private ICradleConnection conn;
        private string result;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
        }

        public override void Because()
        {
            result = conn.Config();
        }

        [Test]
        public void should_get_response()
        {
            result.Length.ShouldBeGreaterThan(0);
        }
    }

    [TestFixture]
    public class when_getting_database : SpecificationBase
    {
        private ICradleConnection conn;
        private ICradleDb result;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
        }

        public override void Because()
        {
            result = conn.Database("foobar");
        }

        [Test]
        public void should_have_correct_name()
        {
            result.Name.ShouldEqual("foobar");
        }
    }
}