using CleanTest.Framework.Drivers.WebDriver.Interfaces;
using Microsoft.Playwright;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters.Elements;

public class PlaywrightWebElementAdapter(ILocator element) : IWebElementAdapter
{
    public void SendKeys(string text) => element.FillAsync(text).GetAwaiter().GetResult();

    public void Click() => element.ClickAsync().GetAwaiter().GetResult();
    
    public string GetText() => element.TextContentAsync().GetAwaiter().GetResult()!;
    
    public void SelectOptionByText(string optionText)
    {
        element.SelectOptionAsync(new[] { new SelectOptionValue { Label = optionText } }).GetAwaiter().GetResult();
    }
    
    public IWebElementAdapter Nth(int index) => new PlaywrightWebElementAdapter(element.Nth(index));
    public bool IsDisplayed() => element.IsVisibleAsync().GetAwaiter().GetResult();

    public IWebElementAdapter FindElementByCssSelector(string cssSelector) =>
        new PlaywrightWebElementAdapter(element.Locator(cssSelector));
}


