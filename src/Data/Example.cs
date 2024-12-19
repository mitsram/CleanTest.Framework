namespace CleanTest.Framework.Data;

public class Example
{
    public void GenerateDataFromJson()
    {
        var jsonMessage = JsonDataBuilder
            .CreateFromJson()
            .WithSectionOne(o => o
                .WithName("Name")
                .WithDescription("Description"))
            .WithSectionTwo(t => t
                .WithName("Name")
                .WithDescription("Description"))
            .Build();
    }
}