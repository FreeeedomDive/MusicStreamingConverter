using RestSharp;

namespace RestClient;

public static class RestClientBuilder
{
    public static RestSharp.RestClient BuildRestClient(string url, bool validateSsl = true)
    {
        var restClientOptions = new RestClientOptions
        {
            BaseUrl = new Uri(url)
        };
        if (!validateSsl)
        {
            restClientOptions.RemoteCertificateValidationCallback = (_, _, _, _) => true;
        }
        return new RestSharp.RestClient(restClientOptions);
    }
}