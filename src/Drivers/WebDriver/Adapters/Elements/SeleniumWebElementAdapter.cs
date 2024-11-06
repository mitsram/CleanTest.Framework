using SeleniumElement = OpenQA.Selenium.IWebElement;
using CleanTest.Framework.WebDriver.Interfaces;
using OpenQA.Selenium.Support.UI;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters;

public class SeleniumWebElementAdapter : IWebElementAdapter
{
    private readonly SeleniumElement _element;

    public SeleniumWebElementAdapter(SeleniumElement element)
    {
        _element = element;
    }

    public void SendKeys(string text) => _element.SendKeys(text);
    public void Click() => _element.Click();

    public string GetText()
    {
        throw new NotImplementedException();
    }

    public string Text => _element.Text;

    public void SelectOptionByText(string optionText)
    {
        var selectElement = new SelectElement(_element);
        selectElement.SelectByText(optionText);
    }
}