using System.Net;
using CleanTest.Framework.Drivers.WebDriver.Adapters.Elements;
using CleanTest.Framework.Drivers.WebDriver.Interfaces;
using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters;

public class SeleniumWebDriverAdapter(IWebDriver driver) : IWebDriverAdapter
{
    public string GetCurrentUrl() => driver.Url;

    public void NavigateToUrl(string url) => driver.Navigate().GoToUrl(url);

    public IWebElementAdapter FindElementById(string id) => 
        new SeleniumWebElementAdapter(driver.FindElement(By.Id(id)));

    public IWebElementAdapter FindElementByClassName(string className) =>
        new SeleniumWebElementAdapter(driver.FindElement(By.ClassName(className)));

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className) =>
        driver.FindElements(By.ClassName(className))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();
    
    public IWebElementAdapter FindElementByCssSelector(string cssSelector) =>
        new SeleniumWebElementAdapter(driver.FindElement(By.CssSelector(cssSelector)));
    
    public IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector) =>
        driver.FindElements(By.CssSelector(cssSelector))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();

    public IWebElementAdapter FindElementByXPath(string xpath) =>
        new SeleniumWebElementAdapter(driver.FindElement(By.XPath(xpath)));

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath) =>
        driver.FindElements(By.XPath(xpath))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();

    public IWebElementAdapter FindElementByTagName(string tagName) =>
        new SeleniumWebElementAdapter(driver.FindElement(By.TagName(tagName)));
    
    public IWebElementAdapter FindElementByText(string text)
    {
        var escapedText = text
            .Replace("'", "&apos;")
            .Replace("\"", "&quot;");
        
        var xpath = $"""
                     //*[translate(normalize-space(), 
                                 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 
                                 'abcdefghijklmnopqrstuvwxyz') 
                             = 
                             translate('{escapedText}', 
                                     'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 
                                     'abcdefghijklmnopqrstuvwxyz')]
                     """;
            
        return new SeleniumWebElementAdapter(
            driver.FindElement(By.XPath(xpath))
        );
    }
    
    public IWebElementAdapter FindElementByPlaceholder(string placeholder) =>
        new SeleniumWebElementAdapter(driver.FindElement(By.CssSelector($"[placeholder='{placeholder}']")));

    public IWebElementAdapter FindElementByRole(string role, string? name = null)
    {
        var selector = $"[role='{role}']";
        if (!string.IsNullOrEmpty(name))
        {
            selector += $"[name='{name}']";
        }
        return new SeleniumWebElementAdapter(driver.FindElement(By.CssSelector(selector)));
    }

    public IWebElementAdapter FindElementByRole(AriaRole role, string? name = null, PageGetByRoleOptions? pageOptions = null)
    {
        throw new NotImplementedException();
    }

    public IWebElementAdapter FindElementByTitle(string title) =>
        new SeleniumWebElementAdapter(driver.FindElement(By.CssSelector($"[title='{title}']")));

    public IWebElementAdapter WaitAndFindElementByXPath(string xpath, int timeoutInSeconds = 15)
    {
        const int maxRetries = 3;
        const int retryDelayMs = 1000;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                var element = wait.Until(driver => driver.FindElement(By.XPath(xpath)));
                return new SeleniumWebElementAdapter(element);
            }
            catch (WebDriverException)
            {
                if (attempt == maxRetries - 1)
                    throw;

                System.Threading.Thread.Sleep(retryDelayMs);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to find element by XPath: {xpath}", ex);
            }
        }

        throw new Exception($"Failed to find element by XPath after {maxRetries} attempts: {xpath}");
    }

    public void SwitchToIframe(string selector)
    {
        var iframe = driver.FindElement(By.CssSelector(selector));
        driver.SwitchTo().Frame(iframe);
    }

    public void SwitchToMainFrame()
    {
        driver.SwitchTo().DefaultContent();
    }

    public void Dispose() => driver.Quit();

    public IWebElementAdapter FindElementByLabel(string label)
    {
        // First try label with matching text and 'for' attribute
        var labelElement = driver.FindElements(By.XPath($"//label[. = '{label}']"))
            .FirstOrDefault(e => !string.IsNullOrEmpty(e.GetAttribute("for")));

        if (labelElement != null)
        {
            var targetId = labelElement.GetAttribute("for");
            return new SeleniumWebElementAdapter(driver.FindElement(By.Id(targetId)));
        }

        // Then try label containing nested form elements
        var nestedElement = driver.FindElement(By.XPath($"//label[contains(., '{label}')]//input"));
        return new SeleniumWebElementAdapter(nestedElement);
    }
}

