using DotCradle.Impl;
using Machine.Specifications;

namespace DotCradle.Specs
{
    public class DeleteFoobarDatabaseAfterEachSpec : ICleanupAfterEveryContextInAssembly
    {
        public void AfterContextCleanup()
        {
            var conn = new CradleConnection(CradleConnectionOptions.Localhost);
            conn.Delete("/foobar");
        }
    }
}