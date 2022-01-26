namespace API.Interfaces
{
    public interface IWBService
    {
        string WbKey1 { get; set; }
        string WbKey2 { get; set; }
        string GetWBKey();
        HttpClient getHttpClient2();
    }
}