using CleanTest.Framework.Drivers.ApiDriver.Adapters;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;

namespace CleanTest.Framework.Factories;

public static class ApiDriverFactory
{
    /// <summary>
    /// Creates an instance of an API driver adapter based on the specified base URL and optional authenticator.
    /// </summary>
    /// <param name="baseUrl">The base URL for the API.</param>
    /// <param name="authenticator">An optional MicrosoftAuthenticator for authentication.</param>
    /// <returns>An instance of IApiDriverAdapter.</returns>
    public static IApiDriverAdapter Create(string baseUrl)
    {
        string apiClientType = "RestSharp";
        
        return apiClientType.ToLower() switch
        {
            "httpclient" => new HttpClientAdapter(baseUrl),
            "restsharp" => new RestSharpAdapter(baseUrl),
            _ => throw new ArgumentException($"Unsupported API client type: {apiClientType}")
        };
    }
}