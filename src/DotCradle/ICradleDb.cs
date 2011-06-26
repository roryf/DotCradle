namespace DotCradle
{
    public interface ICradleDb
    {
        string Name { get; }
        bool Exists();
        string Create();
        string All();
        string Info();
        string Put(string id, string doc);
        string Post(string doc);
        string Delete(string id, string rev);
    }
}