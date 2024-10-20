using System.Text.Json;
using RestSharp;

namespace Shoop24.Administration.Services.Interfaces;

public class ResolverBase
{
    public Dictionary<string, string> Values => new Dictionary<string, string>()
    {
        { "asd", "" }
    };

    private RestClient _client;
    /// <summary>
    /// returns the RestClient for all Resolvers. Entrance to be continued with equest
    /// </summary>
    /// <returns>RestClient</returns>
    public RestClient GetClient()
    {
        if(_client == null)
        {
            _client = new RestClient();
        }
        return _client;
    }
    /// <summary>
    /// Executes an async request and returns the result as a T object if successful or throws an exception if not
    /// </summary>
    /// <typeparam name="T">Custom Type. Necsessary to be Serializeable with given Object / Model</typeparam>
    /// <param name="value">Value as object or whatelse. Serializeable with the response</param>
    /// <param name="request">Build the RestRequest (GEt, POST, aso)</param>
    /// <returns>Task<list type=""> awaitable</list></returns>
    /// <exception cref="Exception"></exception>
    public async Task<T?> ReturnAsyncResult<T>(RestRequest request)
    {
        if(_client == null)
            throw new Exception("Client is not initialized");
        var result = await _client.ExecuteAsync(request);
        if(result.IsSuccessful)
        {
            if (result.Content != null) return JsonSerializer.Deserialize<T>(result.Content);
            else
            {
                throw new Exception("Content is null!!");
            }
        }
        else
        {
            throw new Exception(result.ErrorMessage);
        }
    }

    /// <summary>
    /// represents the same operation only sync
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public T? ReturnResult<T>(RestRequest request)
    {
        return ReturnAsyncResult<T>(request).GetAwaiter().GetResult();
    }
}