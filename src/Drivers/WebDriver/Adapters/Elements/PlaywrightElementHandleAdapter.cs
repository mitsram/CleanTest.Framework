using CleanTest.Framework.WebDriver.Interfaces;
using Microsoft.Playwright;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters;

/// <summary>
/// Represents an adapter for Playwright's IElementHandle, implementing the IWebElementAdapter interface.
/// </summary>
public class PlaywrightElementHandleAdapter : IWebElementAdapter
{
    private readonly IElementHandle _element;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaywrightElementHandleAdapter"/> class.
    /// </summary>
    /// <param name="element">The IElementHandle instance to be used by the adapter.</param>
    public PlaywrightElementHandleAdapter(IElementHandle element)
    {
        _element = element;
    }

    /// <summary>
    /// Sends keystrokes to the element.
    /// </summary>
    /// <param name="text">The text to send to the element.</param>
    public void SendKeys(string text) => _element.FillAsync(text).GetAwaiter().GetResult();

    /// <summary>
    /// Clicks on the element.
    /// </summary>
    public void Click() => _element.ClickAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Retrieves the text content of the element.
    /// </summary>
    /// <returns>The text content of the element.</returns>
    /// <exception cref="NotImplementedException">Thrown when the method is called, as it is not implemented.</exception>
    public string GetText()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the text content of the element as a string.
    /// </summary>
    public string Text => _element.TextContentAsync().GetAwaiter().GetResult() ?? string.Empty;

    /// <summary>
    /// Selects an option in a dropdown by its visible text.
    /// </summary>
    /// <param name="optionText">The visible text of the option to select.</param>
    public void SelectOptionByText(string optionText)
    {
        _element?.AsElement()?.SelectOptionAsync(new[] { new SelectOptionValue { Label = optionText } }).GetAwaiter().GetResult();
    }
}