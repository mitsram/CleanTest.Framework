using CleanTest.Framework.Drivers.WebDriver.Adapters.Elements;
using CleanTest.Framework.Drivers.WebDriver.Interfaces;
using Microsoft.Playwright;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters;

public class PlaywrightWebDriverAdapter : IWebDriverAdapter
{
    private readonly IPage _page;
    private IFrameLocator? _currentFrame;
    
    public PlaywrightWebDriverAdapter(IPage page)
    {
        _page = page;
    }

    public string GetCurrentUrl() => _page.Url;

    public void NavigateToUrl(string url) => _page.GotoAsync(url).GetAwaiter().GetResult();

    public IWebElementAdapter FindElementById(string id) =>
        new PlaywrightWebElementAdapter(GetLocator($"#{id}"));

    public IWebElementAdapter FindElementByClassName(string className) =>
        new PlaywrightWebElementAdapter(GetLocator($".{className}"));

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className) =>
        GetLocator($".{className}").AllAsync().GetAwaiter().GetResult()
            .Select(e => new PlaywrightWebElementAdapter(GetLocator($".{className}")))
            .ToList();        

    public IWebElementAdapter FindElementByCssSelector(string cssSelector) =>
        new PlaywrightWebElementAdapter(GetLocator(cssSelector));
    
    public IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector) =>
        GetLocator(cssSelector).AllAsync().GetAwaiter().GetResult()
            .Select(e => new PlaywrightWebElementAdapter(GetLocator(cssSelector)))
            .ToList();        

    public IWebElementAdapter FindElementByXPath(string xpath) => 
        new PlaywrightWebElementAdapter(GetLocator(xpath));

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath) =>
        GetLocator(xpath).AllAsync().GetAwaiter().GetResult()
            .Select(e => new PlaywrightWebElementAdapter(GetLocator(xpath)))
            .ToList();        

    public IWebElementAdapter FindElementByTagName(string tagName) =>
        new PlaywrightWebElementAdapter(GetLocator(tagName));
    
    public IWebElementAdapter FindElementByText(string text)
    {
        if (_currentFrame != null)
        {
            return new PlaywrightWebElementAdapter(_currentFrame.GetByText(text, new FrameLocatorGetByTextOptions { Exact = true }));
        }
        return new PlaywrightWebElementAdapter(_page.GetByText(text, new PageGetByTextOptions { Exact = true }));
    }

    public IWebElementAdapter FindElementByPlaceholder(string placeholder)
    {
        if (_currentFrame != null)
        {
            return new PlaywrightWebElementAdapter(_currentFrame.GetByPlaceholder(placeholder));
        }
        return new PlaywrightWebElementAdapter(_page.GetByPlaceholder(placeholder));
    }

    public IWebElementAdapter FindElementByRole(AriaRole role, string? name = null, PageGetByRoleOptions? pageOptions = null)
    {
        if (_currentFrame != null)
        {
            var frameOptions = new FrameLocatorGetByRoleOptions();
            if (!string.IsNullOrEmpty(name))
            {
                frameOptions.Name = name;
            }
            var locator = _currentFrame.GetByRole(role, frameOptions);
            return new PlaywrightWebElementAdapter(locator);
        }

        pageOptions ??= new PageGetByRoleOptions();
        if (!string.IsNullOrEmpty(name))
        {
            pageOptions.Name = name;
        }
        
        return new PlaywrightWebElementAdapter(_page.GetByRole(role, pageOptions));
    }

    public IWebElementAdapter FindElementByRole(string role, string? name = null)
    {
        if (!Enum.TryParse<AriaRole>(role, true, out var ariaRole))
        {
            throw new ArgumentException($"Invalid ARIA role: {role}", nameof(role));
        }

        if (_currentFrame != null)
        {
            var frameOptions = new FrameLocatorGetByRoleOptions();
            if (!string.IsNullOrEmpty(name))
            {
                frameOptions.Name = name;
            }
            var locator = _currentFrame.GetByRole(ariaRole, frameOptions);
            return new PlaywrightWebElementAdapter(locator);
        }

        var pageOptions = new PageGetByRoleOptions();
        if (!string.IsNullOrEmpty(name))
        {
            pageOptions.Name = name;
        }
        
        return new PlaywrightWebElementAdapter(_page.GetByRole(ariaRole, pageOptions));
    }

    public IWebElementAdapter FindElementByLabel(string label)
    {
        if (_currentFrame != null)
        {
            return new PlaywrightWebElementAdapter(_currentFrame.GetByLabel(label, new FrameLocatorGetByLabelOptions { Exact = true }));
        }
        return new PlaywrightWebElementAdapter(_page.GetByLabel(label, new PageGetByLabelOptions { Exact = true }));
    }

    public IWebElementAdapter FindElementByTitle(string title)
    {
        if (_currentFrame != null)
        {
            return new PlaywrightWebElementAdapter(_currentFrame.GetByTitle(title));
        }
        return new PlaywrightWebElementAdapter(_page.GetByTitle(title));
    }

    public IWebElementAdapter WaitAndFindElementByXPath(string xpath, int timeoutInSeconds = 15)
    {
        const int maxRetries = 3;
        const int retryDelayMs = 1000;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                var locator = _page.Locator($"xpath={xpath}");
                locator.WaitForAsync(new LocatorWaitForOptions 
                { 
                    State = WaitForSelectorState.Visible, 
                    Timeout = timeoutInSeconds * 1000 
                }).GetAwaiter().GetResult();
                return new PlaywrightWebElementAdapter(locator);
            }
            catch (Exception ex) when (ex is Microsoft.Playwright.PlaywrightException)
            {
                if (attempt == maxRetries - 1)
                    throw;

                System.Threading.Thread.Sleep(retryDelayMs);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to find element by XPath: {xpath}", ex);
            }
        }
        throw new Exception($"Failed to find element by XPath after {maxRetries} attempts: {xpath}");
    }

    public void SwitchToIframe(string selector)
    {
        var frame = _page.FrameLocator(selector);
        _currentFrame = frame;
    }

    public void SwitchToMainFrame()
    {
        _currentFrame = null!;
    }

    public void Dispose() => _page.CloseAsync().GetAwaiter().GetResult();

    private ILocator GetLocator(string selector) => 
        _currentFrame != null ? _currentFrame.Locator(selector) : _page.Locator(selector);
}

