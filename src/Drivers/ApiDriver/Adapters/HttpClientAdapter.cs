using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;

namespace CleanTest.Framework.Drivers.ApiDriver.Adapters;

public sealed class HttpClientAdapter : IApiDriverAdapter
{
    private readonly HttpClient _client;
    private bool _disposed;

    
    public HttpClientAdapter(string baseUrl)
    {
        // Ensure base URL preserves path segments
        if (!baseUrl.EndsWith("/"))
        {
            baseUrl += "/";
        }
        _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }
    
    public async Task<ApiResponse> SendRequestAsync(string method, string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        // Trim leading slash from endpoint to prevent path override
        endpoint = endpoint.TrimStart('/');
        
        // Validate and construct full URL
        var fullUri = new Uri(_client.BaseAddress, endpoint);
        
        // Debug log to verify full URL
        Console.WriteLine($"Constructed full URL: {fullUri}");

        if (!Uri.IsWellFormedUriString(fullUri.ToString(), UriKind.Absolute))
        {
            throw new ArgumentException($"Invalid URL combination: BaseAddress={_client.BaseAddress}, Endpoint={endpoint}");
        }

        var request = new HttpRequestMessage(new HttpMethod(method), fullUri);

        // Debug log to verify URL construction
        Console.WriteLine($"Sending {method} request to: {fullUri}");

        if (body != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        }

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        var response = await _client.SendAsync(request);
        
        Console.WriteLine(request);

        return new ApiResponse
        {
            StatusCode = (int)response.StatusCode,
            Content = await response.Content.ReadAsStringAsync(),
            Headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))
                .Concat(response.Content.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value)))
                .ToDictionary(h => h.Key, h => h.Value)
        };
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _client?.Dispose();
            }
            _disposed = true;
        }
    }
}
