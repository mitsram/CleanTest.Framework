using System.Text;
using System.Text.Json;
using System.Net.Http;

namespace CleanTest.Framework.Drivers.ApiDriver.Builders;

public class ApiBuilder
{
    private readonly string _endpoint;
    private readonly Dictionary<string, string> _headers = new();
    private object? _body;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiBuilder"/> class with the specified endpoint.
    /// </summary>
    /// <param name="endpoint">The API endpoint to be used for requests.</param>
    private ApiBuilder(string endpoint)
    {
        _endpoint = endpoint;
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiBuilder"/> class with the specified endpoint.
    /// </summary>
    /// <param name="endpoint">The API endpoint to be used for requests.</param>
    /// <returns>A new instance of <see cref="ApiBuilder"/>.</returns>
    public static ApiBuilder Endpoint(string endpoint)
    {
        return new ApiBuilder(endpoint);
    }

    /// <summary>
    /// Configures the headers for the API request.
    /// </summary>
    /// <param name="configureHeaders">An action to configure the headers.</param>
    /// <returns>The current instance of <see cref="ApiBuilder"/> for method chaining.</returns>
    public ApiBuilder WithHeaders(Action<Dictionary<string, string>> configureHeaders)
    {
        configureHeaders(_headers);
        return this;
    }

    /// <summary>
    /// Sets the body of the API request.
    /// </summary>
    /// <typeparam name="T">The type of the body object.</typeparam>
    /// <param name="body">The body object to be sent with the request.</param>
    /// <returns>The current instance of <see cref="ApiBuilder"/> for method chaining.</returns>
    public ApiBuilder WithBody<T>(T body)
    {
        _body = body;
        return this;
    }

    /// <summary>
    /// Executes the HTTP POST request to the specified endpoint.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing the <see cref="HttpResponseMessage"/> result.</returns>
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