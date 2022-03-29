namespace API.Interfaces
{
    public interface IWBService
    {
        string WbKeyOld { get; set; }
        string WbKey2 { get; set; }
        string GetWBKey();
        HttpClient GetHttpClient2();
        HttpClient GetHttpClientOld();
        string GetFullUrl(string baseUrl, Object dto, bool isOld = false);
    }
}