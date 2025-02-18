using CleanTest.Framework.Drivers.WebDriver.Interfaces;
using Microsoft.Playwright;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters.Elements;

public class PlaywrightElementHandleAdapter(IElementHandle element) : IWebElementAdapter
{
    public void SendKeys(string text) => element.FillAsync(text).GetAwaiter().GetResult();
    
    public void Click() => element.ClickAsync().GetAwaiter().GetResult();

    public string GetText() => element.TextContentAsync().GetAwaiter().GetResult() ?? string.Empty;
    
    public void SelectOptionByText(string optionText)
    {
        element?.AsElement()?.SelectOptionAsync(new[] { new SelectOptionValue { Label = optionText } }).GetAwaiter().GetResult();
    }

    public IWebElementAdapter Nth(int index)
    {
        throw new NotImplementedException();
    }

    public bool IsDisplayed() => element.IsVisibleAsync().GetAwaiter().GetResult();

    public IWebElementAdapter FindElementByCssSelector(string cssSelector) =>
        new PlaywrightElementHandleAdapter(
            element.WaitForSelectorAsync(cssSelector).GetAwaiter().GetResult() 
            ?? throw new Exception($"Element not found with selector: {cssSelector}"));

    public IWebElementAdapter FindElementByClassName(string className) =>
        new PlaywrightElementHandleAdapter(
            element.WaitForSelectorAsync($".{className}").GetAwaiter().GetResult()
            ?? throw new Exception($"Element not found with class: {className}"));

    public IWebElementAdapter FindElementByXPath(string xpath) =>
        new PlaywrightElementHandleAdapter(
            element.WaitForSelectorAsync(xpath).GetAwaiter().GetResult()
            ?? throw new Exception($"Element not found with XPath: {xpath}"));
}