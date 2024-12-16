public class FrameLocator : IFrameLocator
{
    private readonly PlaywrightWebDriverAdapter _driverAdapter;
    private readonly string _frameName;

    public FrameLocator(PlaywrightWebDriverAdapter driverAdapter, string frameName)
    {
        _driverAdapter = driverAdapter;
        _frameName = frameName;
    }

    private string PrefixWithFrame(string selector) => $"#{_frameName} {selector}";

    public IWebElementAdapter FindElementById(string id) =>
        _driverAdapter.FindElementById(PrefixWithFrame($"#{id}"));

    public IWebElementAdapter FindElementByClassName(string className) =>
        _driverAdapter.FindElementByClassName(PrefixWithFrame($".{className}"));

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className) =>
        _driverAdapter.FindElementsByClassName(PrefixWithFrame($".{className}"));

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector) =>
        _driverAdapter.FindElementsByCssSelector(PrefixWithFrame(cssSelector));

    public IWebElementAdapter FindElementByXPath(string xpath) =>
        _driverAdapter.FindElementByXPath(PrefixWithFrame(xpath));

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath) =>
        _driverAdapter.FindElementsByXPath(PrefixWithFrame(xpath));

    public IWebElementAdapter FindElementByPlaceholder(string placeholder) =>
        _driverAdapter.FindElementByPlaceholder(PrefixWithFrame($"[placeholder='{placeholder}']"));

    public IWebElementAdapter FindElementByRole(string role) =>
        _driverAdapter.FindElementByRole(PrefixWithFrame($"[role='{role}']"));

    public IWebElementAdapter FindElementByLabel(string label) =>
        _driverAdapter.FindElementByLabel(PrefixWithFrame($"label:has-text('{label}') + *"));

    // Add other methods as needed
}
