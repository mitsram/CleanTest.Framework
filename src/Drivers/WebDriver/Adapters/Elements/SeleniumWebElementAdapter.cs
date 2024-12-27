using CleanTest.Framework.Drivers.WebDriver.Interfaces;
using SeleniumElement = OpenQA.Selenium.IWebElement;
using OpenQA.Selenium.Support.UI;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters.Elements;

public class SeleniumWebElementAdapter(SeleniumElement element) : IWebElementAdapter
{
    public void SendKeys(string text) => element.SendKeys(text);
    
    public void Click() => element.Click();
    
    public string GetText() => element.Text;
    
    public void SelectOptionByText(string optionText)
    {
        var selectElement = new SelectElement(element);
        selectElement.SelectByText(optionText);
    }

    public IWebElementAdapter Nth(int index)
    {        
        throw new NotImplementedException();
    }
}