namespace CleanTest.Framework.WebDriver.Interfaces;

public interface IWebDriverAdapter
{
    string GetCurrentUrl();
    void NavigateToUrl(string url);    
    IWebElementAdapter FindElementById(string id);
    IWebElementAdapter FindElementByClassName(string className);   
    IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector);
    IWebElementAdapter FindElementByXPath(string xpath);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath);
    IWebElementAdapter FindElementByPlaceholder(string placeholder);
    IWebElementAdapter FindElementByRole(string role);
    IWebElementAdapter FindElementByLabel(string label);
    IWebElementAdapter WaitAndFindElementByXPath(string xpath, int timeoutInSeconds = 15);
    void Dispose();
}

