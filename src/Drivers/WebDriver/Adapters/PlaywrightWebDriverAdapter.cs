using CleanTest.Framework.WebDriver.Interfaces;
using Microsoft.Playwright;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters;

public class PlaywrightWebDriverAdapter : IWebDriverAdapter
{
    private readonly IPage _page;
    
    public PlaywrightWebDriverAdapter(IPage page)
    {
        _page = page;
    }

    /// <summary>
    /// Gets the current URL of the page.
    /// </summary>
    /// <returns>The current URL as a string.</returns>
    public string GetCurrentUrl() => _page.Url;

    /// <summary>
    /// Navigates to the specified URL.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    public void NavigateToUrl(string url) => _page.GotoAsync(url).GetAwaiter().GetResult();

    /// <summary>
    /// Finds an element by its ID.
    /// </summary>
    /// <param name="id">The ID of the element to find.</param>
    /// <returns>An instance of <see cref="IWebElementAdapter"/> representing the found element.</returns>
    public IWebElementAdapter FindElementById(string id) => 
        new PlaywrightWebElementAdapter(_page.Locator($"#{id}"));

    /// <summary>
    /// Finds an element by its class name.
    /// </summary>
    /// <param name="className">The class name of the element to find.</param>
    /// <returns>An instance of <see cref="IWebElementAdapter"/> representing the found element.</returns>
    public IWebElementAdapter FindElementByClassName(string className) =>
        new PlaywrightWebElementAdapter(_page.Locator($".{className}"));

    /// <summary>
    /// Finds multiple elements by their class name.
    /// </summary>
    /// <param name="className">The class name to use for finding elements.</param>
    /// <returns>A collection of <see cref="IWebElementAdapter"/> representing the found elements.</returns>
    public IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className) =>
        _page.QuerySelectorAllAsync($".{className}").GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    /// <summary>
    /// Finds multiple elements by their CSS selector.
    /// </summary>
    /// <param name="cssSelector">The CSS selector to use for finding elements.</param>
    /// <returns>A collection of <see cref="IWebElementAdapter"/> representing the found elements.</returns>
    public IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector) =>
        _page.QuerySelectorAllAsync(cssSelector).GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    /// <summary>
    /// Finds an element by its XPath.
    /// </summary>
    /// <param name="xpath">The XPath of the element to find.</param>
    /// <returns>An instance of <see cref="IWebElementAdapter"/> representing the found element.</returns>
    public IWebElementAdapter FindElementByXPath(string xpath) => 
        new PlaywrightWebElementAdapter(_page.Locator(xpath));

    /// <summary>
    /// Finds multiple elements by their XPath.
    /// </summary>
    /// <param name="xpath">The XPath to use for finding elements.</param>
    /// <returns>A collection of <see cref="IWebElementAdapter"/> representing the found elements.</returns>
    public IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath) =>
        _page.QuerySelectorAllAsync(xpath).GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    /// <summary>
    /// Finds an element by its placeholder attribute.
    /// </summary>
    /// <param name="placeholder">The placeholder text of the element to find.</param>
    /// <returns>
    /// An instance of <see cref="IWebElementAdapter"/> representing the found element.
    /// If no element is found, this method may return null or throw an exception based on implementation.
    /// </returns>
    public IWebElementAdapter FindElementByPlaceholder(string placeholder) =>
        new PlaywrightWebElementAdapter(_page.Locator($"[placeholder='{placeholder}']"));

    /// <summary>
    /// Finds an element by its role attribute.
    /// </summary>
    /// <param name="role">The role of the element to find.</param>
    /// <returns>
    /// An instance of <see cref="IWebElementAdapter"/> representing the found element.
    /// If no element is found, this method may return null or throw an exception based on implementation.
    /// </returns>
    public IWebElementAdapter FindElementByRole(string role) =>
        new PlaywrightWebElementAdapter(_page.Locator($"[role='{role}']"));

    /// <summary>
    /// Finds an element by its label text.
    /// </summary>
    /// <param name="label">The text of the label associated with the element to find.</param>
    /// <returns>
    /// An instance of <see cref="IWebElementAdapter"/> representing the found element.
    /// If no element is found, this method may return null or throw an exception based on implementation.
    /// </returns>
    public IWebElementAdapter FindElementByLabel(string label) =>
        new PlaywrightWebElementAdapter(_page.Locator($"label:has-text('{label}') + *"));

    /// <summary>
    /// Waits for an element to be visible and finds it by its XPath.
    /// </summary>
    /// <param name="xpath">The XPath of the element to find.</param>
    /// <param name="timeoutInSeconds">The timeout in seconds to wait for the element to be visible.</param>
    /// <returns>An instance of <see cref="IWebElementAdapter"/> representing the found element.</returns>
    /// <exception cref="Exception">Thrown if the element cannot be found within the specified attempts.</exception>
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

    /// <summary>
    /// Disposes of the resources used by the adapter.
    /// </summary>
    public void Dispose() => _page.CloseAsync().GetAwaiter().GetResult();
}

