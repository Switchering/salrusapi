namespace API.Interfaces
{
    public interface IWBService
    {
        string WbKey1 { get; set; }
        string WbKey2 { get; set; }
        string GetWBKey();
        HttpClient GetHttpClient2();
        string GetFullUrl(string baseUrl, Object dto );
    }
}