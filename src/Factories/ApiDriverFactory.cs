using CleanTest.Framework.Drivers.ApiDriver.Adapters;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;
using CleanTest.Framework.Drivers.WebDriver.Enums;

namespace CleanTest.Framework.Factories;

public static class ApiDriverFactory
{    
    public static IApiDriverAdapter Create(string baseUrl, ApiDriverType apiDriverType)
    {
        return apiDriverType switch
        {
            ApiDriverType.Playwright => new PlaywrightApiAdapter(baseUrl),
            ApiDriverType.RestSharp => new RestSharpAdapter(baseUrl),            
            _ => throw new ArgumentException($"Unsupported API client type: {apiDriverType}")
        };
    }
}