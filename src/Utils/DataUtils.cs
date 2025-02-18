using System.Text.Json;

namespace CleanTest.Framework.Utils;

public class DataUtils
{
    public static T LoadFromJson<T, TBuilder>(string resourcePath)
    {
        using var stream = typeof(TBuilder).Assembly
            .GetManifestResourceStream(resourcePath);
        
        if (stream == null)
        {
            throw new InvalidOperationException(
                $"JSON resource not found: {resourcePath}");
        }

        using var reader = new StreamReader(stream);
        var jsonContent = reader.ReadToEnd();
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        return JsonSerializer.Deserialize<T>(jsonContent, options) 
            ?? throw new InvalidOperationException($"Failed to deserialize {typeof(T).Name}");
    }

}