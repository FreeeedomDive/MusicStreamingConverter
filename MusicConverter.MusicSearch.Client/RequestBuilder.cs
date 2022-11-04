using RestSharp;

namespace MusicConverter.MusicSearch.Client;

public static class RequestBuilder
{
    public static RestRequest BuildRequest(string route, string endpoint, string query, int skip, int take)
    {
        var request = new RestRequest($"{route}/{endpoint}/find");
        request.AddQueryParameter("query", query);
        request.AddQueryParameter("skip", skip);
        request.AddQueryParameter("take", take);

        return request;
    }
    
    public static RestRequest BuildRequest(string route, string endpoint, string id)
    {
        return new RestRequest($"{route}/{endpoint}/{id}");
    }
}