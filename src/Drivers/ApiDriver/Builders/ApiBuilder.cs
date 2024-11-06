using System.Text;
using System.Text.Json;

namespace CleanTest.Framework.Drivers.ApiDriver.Builders;

public class ApiBuilder
{
    private readonly string _endpoint;
    private readonly Dictionary<string, string> _headers = new();
    private object? _body;
    private readonly HttpClient _httpClient;

    private ApiBuilder(string endpoint)
    {
        _endpoint = endpoint;
        _httpClient = new HttpClient();
    }

    public static ApiBuilder Endpoint(string endpoint)
    {
        return new ApiBuilder(endpoint);
    }

    public ApiBuilder WithHeaders(Action<Dictionary<string, string>> configureHeaders)
    {
        configureHeaders(_headers);
        return this;
    }

    public ApiBuilder WithBody<T>(T body)
    {
        _body = body;
        return this;
    }

    public async Task<HttpResponseMessage> ExecutePost()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);

            // Add headers
            foreach (var header in _headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            // Add body if exists
            if (_body != null)
            {
                var json = JsonSerializer.Serialize(_body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"API request failed: {ex.Message}", ex);
        }
    }
}