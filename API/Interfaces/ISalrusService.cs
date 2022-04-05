namespace API.Interfaces
{
    public interface ISalrusService
    {
        HttpClient GetHttpClient();

        string GetFullUrl(string baseUrl, Object dto);
    }
}