using RestSharp;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;

namespace CleanTest.Framework.Drivers.ApiDriver.Adapters;

public sealed class RestSharpAdapter : IApiDriverAdapter
{
    private readonly RestClient _client;
    private bool _disposed;

    public RestSharpAdapter(string baseUrl)
    {
        _client = new RestClient(baseUrl);
    }

    public async Task<ApiResponse> SendRequestAsync(string method, string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        var requestMethod = Enum.TryParse<Method>(method, true, out var parsedMethod) ? parsedMethod : Method.Post;
        var request = new RestRequest(endpoint, requestMethod);

        if (body != null)
        {
            request.AddJsonBody(body);
        }

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
        }

        var response = await _client.ExecuteAsync(request);

        return new ApiResponse
        {
            StatusCode = (int)response.StatusCode,
            Content = response.Content,
            Headers = response.Headers?.ToDictionary(h => h.Name, h => h.Value?.ToString()) ?? new Dictionary<string, string>()
        };
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _client?.Dispose();
        }
        _disposed = true;
    }
}
