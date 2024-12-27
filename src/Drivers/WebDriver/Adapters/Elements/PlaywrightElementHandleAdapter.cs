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
}