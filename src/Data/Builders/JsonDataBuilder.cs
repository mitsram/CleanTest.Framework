using System.Text.Json;
using CleanTest.Framework.Data.Entities;

namespace CleanTest.Framework.Data;

public class JsonDataBuilder
{
    private readonly SectionOneBuilder _sectionOneBuilder = SectionOneBuilder.Empty();
    private readonly SectionTwoBuilder _sectionTwoBuilder = SectionTwoBuilder.Empty();
    private const string ResourceNameSpace = "CleanTest.Data.Json";
    
    private JsonDataBuilder()
    {

    }

    public static JsonDataBuilder Empty() => new();

    public static JsonDataBuilder CreateFromJson()
    {
        return CreateFromJson(ResourceNameSpace + "Payload.json");
    }

    public JsonDataBuilder WithSectionOne(Action<SectionOneBuilder> action)
    {
        action(_sectionOneBuilder);
        return this;
    }
    
    public JsonDataBuilder WithSectionTwo(Action<SectionTwoBuilder> action)
    {
        action(_sectionTwoBuilder);
        return this;
    }

    public JsonData Build()
    {
        return new JsonData
        {
            SectionOne = _sectionOneBuilder.Build(),
            SectionTwo = _sectionTwoBuilder.Build()
        };
    }

    // Create from JSON file
    private static JsonDataBuilder CreateFromJson(string resourcePath)
    {
        var builder = new JsonDataBuilder();
        var json = LoadFromJson<JsonData>(resourcePath);

        return builder
            .WithSectionOne(sectionOne => sectionOne
                .WithName("New one name")
                .WithDescription("New one description"))
            .WithSectionTwo(sectionTwo => sectionTwo
                .WithName("New two name")
                .WithDescription("New two description"));
    }

    private static T LoadFromJson<T>(string resourcePath)
    {
        using var stream = typeof(T).Assembly
            .GetManifestResourceStream(resourcePath);

        if (stream == null) throw new InvalidOperationException($"JSON resource not found: {resourcePath}");

        using var reader = new StreamReader(stream);
        var jsonConent = reader.ReadToEnd();
        
        return JsonSerializer.Deserialize<T>(jsonConent) ?? throw new InvalidOperationException($"JSON resource could not be deserialized: {resourcePath}");
    }

}