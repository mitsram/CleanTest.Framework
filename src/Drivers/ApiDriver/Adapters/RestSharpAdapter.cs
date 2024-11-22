using RestSharp;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;

namespace CleanTest.Framework.Drivers.ApiDriver.Adapters;

public sealed class RestSharpAdapter : IApiDriverAdapter
{
    private readonly RestClient _client;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestSharpAdapter"/> class with the specified base URL.
    /// </summary>
    /// <param name="baseUrl">The base URL for the API to which requests will be sent.</param>
    public RestSharpAdapter(string baseUrl)
    {
        _client = new RestClient(baseUrl);
    }

    /// <summary>
    /// Sends an asynchronous HTTP request to the specified endpoint.
    /// </summary>
    /// <param name="method">The HTTP method to use (e.g., GET, POST, PUT, DELETE).</param>
    /// <param name="endpoint">The API endpoint to which the request is sent.</param>
    /// <param name="body">An optional object to be serialized as the request body. Default is null.</param>
    /// <param name="headers">An optional dictionary of headers to include in the request. Default is null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the <see cref="ApiResponse"/> result.
    /// The <see cref="ApiResponse"/> includes the status code, content, and headers of the response.
    /// </returns>
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

    /// <summary>
    /// Disposes of the resources used by the <see cref="RestSharpAdapter"/> class.
    /// This method is called to release unmanaged resources and perform other cleanup operations.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the resources used by the <see cref="RestSharpAdapter"/> class.
    /// This method is called by the Dispose method and can be overridden in derived classes.
    /// </summary>
    /// <param name="disposing">A boolean indicating whether the method was called directly or by the garbage collector.</param>
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
