using CleanTest.Framework.Drivers.ApiDriver.Adapters;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;

namespace CleanTest.Framework.Factories;

public static class ApiDriverFactory
{    
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