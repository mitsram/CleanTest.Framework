using CleanTest.Framework.Data.Entities;

namespace CleanTest.Framework.Data;

public class SectionTwoBuilder
{
    private string _name;
    private string _description;

    private SectionTwoBuilder()
    {
    }

    public static SectionTwoBuilder Empty() => new();

    public SectionTwoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public SectionTwoBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public SectionTwo Build()
    {
        return new SectionTwo()
        {
            Name = _name,
            Description = _description
        };
    }
}