namespace ForeignExchange.Interfaces
{
    using SQLite.Net.Interop;

    public interface IConfig
    {
        string DirectoryDB { get; }

        ISQLitePlatform PlatForm { get; }
    }
}
