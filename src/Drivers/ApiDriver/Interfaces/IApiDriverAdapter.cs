using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanTest.Framework.Drivers.ApiDriver.Interfaces;

public interface IApiDriverAdapter : IDisposable
{
    Task<ApiResponse> SendRequestAsync(string method, string endpoint, object? body = null, Dictionary<string, string>? headers = null);
}

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string? Content { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
}

