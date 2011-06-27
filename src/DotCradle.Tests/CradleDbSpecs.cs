using System;
using DotCradle.Impl;
using NUnit.Framework;

namespace DotCradle.Tests
{
    [TestFixture]
    public class CradleDbSpecs : SpecificationBase
    {
        private CradleConnection _connection;

        public override void Because()
        {
            _connection = new CradleConnection(CradleConnectionOptions.Localhost);
        }

        private CradleDb Db(string name)
        {
            return new CradleDb(name, _connection);
        }

        [Test]
        public void Can_check_if_db_exists()
        {
            Db("foobar").Exists().ShouldBeTrue();
        }

        [Test]
        public void Can_create_db()
        {
            Db("notfound").Create().Length.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Can_destroy_db()
        {
            Db("foobar").Destroy().Length.ShouldBeGreaterThan(0);
            Db("foobar").Exists().ShouldBeFalse();
        }
    }
}