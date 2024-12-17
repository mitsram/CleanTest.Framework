using CleanTest.Framework.WebDriver;

public class SeleniumFrameHandler : IFrameHandler
{
    private readonly SeleniumWebDriverAdapter _driverAdapter;
    private readonly string _frameLocator;

    public SeleniumFrameHandler(SeleniumWebDriverAdapter driverAdapter, string frameLocator)
    {
        _driverAdapter = driverAdapter;
        _frameLocator = frameLocator;
    }

    public IWebElementAdapter FindElementById(string id) =>
        _driverAdapter.FindElementById($"#{_frameLocator} #{id}");

    public IWebElementAdapter FindElementByClassName(string className) =>
        _driverAdapter.FindElementByClassName($"#{_frameLocator} .{className}");

    // Implement other methods as needed
}