using CleanTest.Framework.Drivers.WebDriver.Adapters;
using CleanTest.Framework.WebDriver.Interfaces;

public class PlaywrightFrameHandler : IFrameHandler
{
    private readonly PlaywrightWebDriverAdapter _driverAdapter;
    private readonly string _frameName;

    public PlaywrightFrameHandler(PlaywrightWebDriverAdapter driverAdapter, string frameName)
    {
        _driverAdapter = driverAdapter;
        _frameName = frameName;
    }

    public IWebElementAdapter FindElementById(string id) =>
        _driverAdapter.FindElementById($"#{_frameName} #{id}");

    public IWebElementAdapter FindElementByClassName(string className) =>
        _driverAdapter.FindElementByClassName($"#{_frameName} .{className}");

    // Implement other methods as needed
} 