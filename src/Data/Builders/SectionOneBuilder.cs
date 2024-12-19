using CleanTest.Framework.Data.Entities;

namespace CleanTest.Framework.Data;

public class SectionOneBuilder
{
    private string _name;
    private string _description;

    private SectionOneBuilder()
    {
    }

    public static SectionOneBuilder Empty() => new();

    public SectionOneBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public SectionOneBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public SectionOne Build()
    {
        return new SectionOne()
        {
            Name = _name,
            Description = _description
        };
    }
}