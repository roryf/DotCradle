using System.Collections.Generic;

namespace DotCradle
{
    public interface ICradleConnection
    {
        CradleResponse Get(string path);
        CradleResponse Get(string path, IDictionary<string, string> urlParams);
        CradleResponse Post(string path, string data);
        CradleResponse Put(string path);
        CradleResponse Put(string path, string data);
        CradleResponse Head(string path);
        CradleResponse Delete(string path);
        string Databases();
        string Uuids(int count);
        string Info();
        string Config();
        ICradleDb Database(string name);
    }
}