using Microsoft.Playwright;

namespace CleanTest.Framework.Drivers.WebDriver.Interfaces;

public interface IWebDriverAdapter : IElementFinder
{
    string GetCurrentUrl();
    void NavigateToUrl(string url);    
    IWebElementAdapter FindElementById(string id);
    IWebElementAdapter FindElementByClassName(string className);   
    IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector);
    IWebElementAdapter FindElementByXPath(string xpath);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath);
    IWebElementAdapter FindElementByTagName(string tagName);
    IWebElementAdapter FindElementByPlaceholder(string placeholder);
    IWebElementAdapter FindElementByRole(AriaRole role, string? name = null, PageGetByRoleOptions? pageOptions = null);
    IWebElementAdapter FindElementByRole(string role, string? name = null);
    IWebElementAdapter FindElementByLabel(string label);
    IWebElementAdapter FindElementByTitle(string title);
    IWebElementAdapter WaitAndFindElementByXPath(string xpath, int timeoutInSeconds = 15);
    void SwitchToIframe(string selector);
    void SwitchToMainFrame();
    void Dispose();
    IWebElementAdapter FindElementByText(string text);
    
    // IWebElementAdapter FindElementByCssSelector(string cssSelector);
}
