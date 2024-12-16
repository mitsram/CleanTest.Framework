using CleanTest.Framework.WebDriver.Interfaces;

public class FrameLocator
{
    private readonly IFrameHandler _frameHandler;

    public FrameLocator(IFrameHandler frameHandler)
    {
        _frameHandler = frameHandler;
    }

    public IWebElementAdapter FindElementById(string id) =>
        _frameHandler.FindElementById(id);

    public IWebElementAdapter FindElementByClassName(string className) =>
        _frameHandler.FindElementByClassName(className);

    // Add other methods as needed
}
