using CleanTest.Framework.WebDriver.Interfaces;
using Microsoft.Playwright;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters;

public class PlaywrightWebElementAdapter : IWebElementAdapter
{
    private readonly ILocator _element;

    public PlaywrightWebElementAdapter(ILocator element)
    {
        _element = element;
    }

    public void SendKeys(string text) => _element.FillAsync(text).GetAwaiter().GetResult();

    public void Click() => _element.ClickAsync().GetAwaiter().GetResult();

    public string GetText()
    {
        throw new NotImplementedException();
    }

    public string Text => _element.TextContentAsync().GetAwaiter().GetResult() ?? string.Empty;

    public void SelectOptionByText(string optionText)
    {
        _element.SelectOptionAsync(new[] { new SelectOptionValue { Label = optionText } }).GetAwaiter().GetResult();
    }
}


