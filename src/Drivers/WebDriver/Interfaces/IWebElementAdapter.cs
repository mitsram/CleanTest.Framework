namespace CleanTest.Framework.Drivers.WebDriver.Interfaces;

public interface IWebElementAdapter
{
    void Click();
    void SendKeys(string text);
    string GetText();
    void SelectOptionByText(string optionText);
    IWebElementAdapter Nth(int index);
}
