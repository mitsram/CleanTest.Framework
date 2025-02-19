using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;

namespace CleanTest.Framework.Drivers.ApiDriver.Adapters;

public sealed class PlaywrightApiAdapter : IApiDriverAdapter
{
    private IPlaywright _playwright;
    private IAPIRequestContext _apiRequestContext;
    private bool _disposed;

    public PlaywrightApiAdapter(string baseUrl)
    {
        // Preserve entire base URL including path segments
        var uriBuilder = new UriBuilder(baseUrl);
        uriBuilder.Path = uriBuilder.Path.TrimEnd('/');
        var formattedBaseUrl = uriBuilder.ToString();

        _playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        _apiRequestContext = _playwright.APIRequest.NewContextAsync(new()
        {
            BaseURL = formattedBaseUrl
        }).GetAwaiter().GetResult();
    }

    public async Task<ApiResponse> SendRequestAsync(string method, string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        // Handle path combination correctly
        var fullPath = endpoint.StartsWith("/")
            ? endpoint
            : $"/{endpoint}";        

        IAPIResponse response = method.ToUpper() switch
        {
            "GET" => await _apiRequestContext.GetAsync(fullPath, new() { Headers = headers }),
            "POST" => await _apiRequestContext.PostAsync(fullPath, new() { DataObject = body, Headers = headers }),
            "PUT" => await _apiRequestContext.PutAsync(fullPath, new() { DataObject = body, Headers = headers }),
            "DELETE" => await _apiRequestContext.DeleteAsync(fullPath, new() { Headers = headers }),
            "PATCH" => await _apiRequestContext.PatchAsync(fullPath, new() { DataObject = body, Headers = headers }),
            _ => throw new ArgumentException($"Unsupported HTTP method: {method}")
        };

        return new ApiResponse
        {
            StatusCode = response.Status,
            Content = await response.TextAsync(),
            Headers = response.Headers
        };
    }

    public void Dispose()
    {
        if (_disposed) return;
        
        _apiRequestContext?.DisposeAsync().GetAwaiter().GetResult();
        _playwright?.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
