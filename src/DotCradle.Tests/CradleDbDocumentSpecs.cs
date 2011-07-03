using System;
using DotCradle.Impl;
using NUnit.Framework;

namespace DotCradle.Tests
{
    [TestFixture]
    public class when_getting_all_documents : SpecificationBase
    {
        private string result;
        private ICradleDb db;

        public override void Given()
        {
            var conn = new CradleConnection(CradleConnectionOptions.Localhost);
            db = conn.Database("foobar");
            db.Create();
        }

        public override void AfterEach()
        {
            db.Destroy();
        }

        public override void Because()
        {
            result = db.All();
        }

        [Test]
        public void should_get_response()
        {
            result.Length.ShouldBeGreaterThan(0);
        }
    }

    [TestFixture]
    public class when_getting_doc_by_id : SpecificationBase
    {
        private string result;
        private ICradleDb db;

        public override void Given()
        {
            var conn = new CradleConnection(CradleConnectionOptions.Localhost);
            db = conn.Database("foobar");
            db.Create();
            conn.Put("/foobar/testdoc", "{\"name\": \"yermaw\"}");
        }

        public override void AfterEach()
        {
            db.Destroy();
        }

        public override void Because()
        {
            result = db.Get("testdoc");
        }

        [Test]
        public void should_get_response()
        {
            result.Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void should_get_correct_doc()
        {
            result.ShouldContain("yermaw");
        }
    }
}