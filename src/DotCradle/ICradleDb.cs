namespace DotCradle
{
    public interface ICradleDb
    {
        string Name { get; }
        bool Exists();
        string Create();
        string Destroy();
        string All();
        string Info();
        string Get(string id);
        string Get(string id, string rev);
        string Head(string id);
        string Put(string id, string doc);
        string Post(string doc);
        string Delete(string id, string rev);
    }
}