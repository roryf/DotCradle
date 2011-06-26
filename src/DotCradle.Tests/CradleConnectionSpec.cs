using System;
using DotCradle.Impl;
using NUnit.Framework;

namespace DotCradle.Tests
{
    [TestFixture]
    public class CradleConnectionSpec : SpecificationBase
    {
        private ICradleConnection _connection;

        public override void Because()
        {
            _connection = new CradleConnection(CradleConnectionOptions.Localhost);
        }

        [Test]
        public void Can_get_list_of_databases()
        {
            _connection.Databases().Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Can_get_uuids()
        {
            _connection.Uuids(1).Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Can_get_info()
        {
            var info = _connection.Info();
            info.Length.ShouldBeGreaterThan(0);
            info.ShouldContain("Welcome");
        }

        [Test]
        public void Can_get_config()
        {
            _connection.Config().Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Can_get_db()
        {
            var db = _connection.Database("foobar");
            db.ShouldNotBeNull();
            db.Name.ShouldEqual("foobar");
        }
    }
}