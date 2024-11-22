using CleanTest.Framework.Drivers.ApiDriver.Adapters;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;

namespace CleanTest.Framework.Factories;

public static class ApiDriverFactory
{
    /// <summary>
    /// Creates an instance of an API driver adapter based on the specified base URL and optional client type.
    /// If the client type is null or empty, it defaults to "RestSharp".
    /// </summary>
    /// <param name="baseUrl">The base URL for the API.</param>
    /// <param name="clientType">An optional string specifying the type of API client to create. Defaults to "RestSharp".</param>
    /// <returns>An instance of <see cref="IApiDriverAdapter"/> corresponding to the specified client type.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified client type is unsupported.</exception>
    public static IApiDriverAdapter Create(string baseUrl, string clientType = "RestSharp")
    {
        return clientType.ToLower() switch
        {
            "httpclient" => new HttpClientAdapter(baseUrl),
            "restsharp" => new RestSharpAdapter(baseUrl),
            _ => throw new ArgumentException($"Unsupported API client type: {clientType}")
        };
    }
}