using System;
using DotCradle.Impl;
using NUnit.Framework;

namespace DotCradle.Tests
{
    [TestFixture]
    public class when_checking_if_db_exists : SpecificationBase
    {
        private ICradleConnection conn;
        private ICradleDb db;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
        }

        public override void AfterEach()
        {
            db.Destroy();
        }

        public override void Because()
        {
            db = conn.Database("foobar");
        }

        [Test]
        public void should_return_false()
        {
            db.Exists().ShouldBeFalse();
        }
    }

    [TestFixture]
    public class when_creating_db : SpecificationBase
    {
        private ICradleConnection conn;
        private ICradleDb db;
        private string result;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
            db = conn.Database("foobar");
        }

        public override void AfterEach()
        {
            db.Destroy();
        }

        public override void Because()
        {
            result = db.Create();
        }

        [Test]
        public void should_get_response()
        {
            result.Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void db_should_exist()
        {
            db.Exists().ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_destroying_db : SpecificationBase
    {
        private ICradleConnection conn;
        private ICradleDb db;
        private string result;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
            conn.Put("/foobar");
            db = conn.Database("foobar");
        }

        public override void Because()
        {
            result = db.Destroy();
        }

        [Test]
        public void should_get_response()
        {
            result.Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void db_should_not_exist()
        {
            db.Exists().ShouldBeFalse();
        }
    }

    [TestFixture]
    public class when_getting_db_info : SpecificationBase
    {
        private ICradleConnection conn;
        private ICradleDb db;
        private string result;

        public override void Given()
        {
            conn = new CradleConnection(CradleConnectionOptions.Localhost);
            db = conn.Database("foobar");
            db.Create();
        }

        public override void AfterEach()
        {
            db.Destroy();
        }

        public override void Because()
        {
            result = db.Info();
        }

        [Test]
        public void should_get_respnse()
        {
            result.Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void should_get_respnse_for_correct_db()
        {
            result.ShouldContain("foobar");
        }
    }
}