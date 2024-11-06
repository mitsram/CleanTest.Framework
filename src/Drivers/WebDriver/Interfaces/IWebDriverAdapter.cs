namespace CleanTest.Framework.WebDriver.Interfaces;

public interface IWebDriverAdapter
{
    void NavigateToUrl(string url);
    string GetCurrentUrl();
    IWebElementAdapter FindElementById(string id);
    IWebElementAdapter FindElementByXPath(string xpath);
    IWebElementAdapter FindElementByClassName(string className);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className);
    void Dispose();

    IWebElementAdapter WaitAndFindElementByXPath(string xpath, int timeoutInSeconds = 15);
}

