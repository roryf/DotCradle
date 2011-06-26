namespace DotCradle
{
    public interface ICradleConnection
    {
        string Databases();
        string Uuids(int count);
        string Info();
        string Config();
        ICradleDb Database(string name);
    }
}