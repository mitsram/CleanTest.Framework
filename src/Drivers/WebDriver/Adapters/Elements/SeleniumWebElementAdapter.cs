using SeleniumElement = OpenQA.Selenium.IWebElement;
using CleanTest.Framework.WebDriver.Interfaces;
using OpenQA.Selenium.Support.UI;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters;

/// <summary>
/// Represents an adapter for Selenium's IWebElement, implementing the IWebElementAdapter interface.
/// </summary>
public class SeleniumWebElementAdapter : IWebElementAdapter
{
    private readonly SeleniumElement _element;

    /// <summary>
    /// Initializes a new instance of the <see cref="SeleniumWebElementAdapter"/> class.
    /// </summary>
    /// <param name="element">The IWebElement instance to be used by the adapter.</param>
    public SeleniumWebElementAdapter(SeleniumElement element)
    {
        _element = element;
    }

    /// <summary>
    /// Sends keystrokes to the element.
    /// </summary>
    /// <param name="text">The text to send to the element.</param>
    public void SendKeys(string text) => _element.SendKeys(text);

    /// <summary>
    /// Clicks on the element.
    /// </summary>
    public void Click() => _element.Click();

    /// <summary>
    /// Retrieves the text content of the element.
    /// </summary>
    /// <returns>The text content of the element.</returns>
    public string GetText()
    {
        return _element.Text;
    }

    /// <summary>
    /// Gets the text content of the element as a string.
    /// </summary>
    public string Text => _element.Text;

    /// <summary>
    /// Selects an option in a dropdown by its visible text.
    /// </summary>
    /// <param name="optionText">The visible text of the option to select.</param>
    public void SelectOptionByText(string optionText)
    {
        var selectElement = new SelectElement(_element);
        selectElement.SelectByText(optionText);
    }
}