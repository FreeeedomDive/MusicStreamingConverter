using Newtonsoft.Json;
using RestSharp;

namespace MusicSearch.Client.Extensions;

public static class RestResponseExtensions
{
    private static void ThrowIfNotSuccessful(this RestResponse restResponse)
    {
        if (!restResponse.IsSuccessful)
        {
            throw new Exception(restResponse.Content ?? "Server returned unsuccessful response");
        }
    }

    public static T TryDeserialize<T>(this RestResponse restResponse)
    {
        restResponse.ThrowIfNotSuccessful();
        if (restResponse.Content == null)
        {
            throw new Exception("Content is null");
        }

        try
        {
            var response = JsonConvert.DeserializeObject<T>(restResponse.Content);
            if (response == null)
            {
                throw new Exception($"Can not deserialize response as {typeof(T).Name}");
            }

            return response;
        }
        catch
        {
            throw new Exception($"Can not deserialize response as {typeof(T).Name}");
        }
    }
}