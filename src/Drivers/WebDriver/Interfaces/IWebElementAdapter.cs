namespace CleanTest.Framework.WebDriver.Interfaces;

public interface IWebElementAdapter
{
    void Click();
    void SendKeys(string text);
    string GetText();
    string Text { get; }
    void SelectOptionByText(string optionText);
}
