using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using CleanTest.Framework.WebDriver.Interfaces;

namespace CleanTest.Framework.Drivers.WebDriver.Adapters;

/// <summary>
/// Represents an adapter for Selenium WebDriver, implementing the IWebDriverAdapter interface.
/// </summary>
public class SeleniumWebDriverAdapter : IWebDriverAdapter
{
    private readonly IWebDriver _driver;
    
    public SeleniumWebDriverAdapter(IWebDriver driver)
    {
        _driver = driver;
    }

    /// <summary>
    /// Gets the current URL of the browser.
    /// </summary>
    /// <returns>The current URL as a string.</returns>
    public string GetCurrentUrl() => _driver.Url;

    /// <summary>
    /// Navigates to the specified URL.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    public void NavigateToUrl(string url) => _driver.Navigate().GoToUrl(url);

    /// <summary>
    /// Finds an element by its ID and returns an IWebElementAdapter.
    /// </summary>
    /// <param name="id">The ID of the element to find.</param>
    /// <returns>An IWebElementAdapter representing the found element.</returns>
    public IWebElementAdapter FindElementById(string id) => 
        new SeleniumWebElementAdapter(_driver.FindElement(By.Id(id)));

    /// <summary>
    /// Finds an element by its class name and returns an IWebElementAdapter.
    /// </summary>
    /// <param name="className">The class name of the element to find.</param>
    /// <returns>An IWebElementAdapter representing the found element.</returns>
    public IWebElementAdapter FindElementByClassName(string className) =>
        new SeleniumWebElementAdapter(_driver.FindElement(By.ClassName(className)));

    /// <summary>
    /// Finds multiple elements by their class name and returns a collection of IWebElementAdapter.
    /// </summary>
    /// <param name="className">The class name to find elements.</param>
    /// <returns>A collection of IWebElementAdapter representing the found elements.</returns>
    public IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className) =>
        _driver.FindElements(By.ClassName(className))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();

    /// <summary>
    /// Finds multiple elements by their CSS selector and returns a collection of IWebElementAdapter.
    /// </summary>
    /// <param name="cssSelector">The CSS selector to find elements.</param>
    /// <returns>A collection of IWebElementAdapter representing the found elements.</returns>
    public IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector) =>
        _driver.FindElements(By.CssSelector(cssSelector))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();

    /// <summary>
    /// Finds an element by its XPath and returns an IWebElementAdapter.
    /// </summary>
    /// <param name="xpath">The XPath of the element to find.</param>
    /// <returns>An IWebElementAdapter representing the found element.</returns>
    public IWebElementAdapter FindElementByXPath(string xpath) =>
        new SeleniumWebElementAdapter(_driver.FindElement(By.XPath(xpath)));

    /// <summary>
    /// Finds multiple elements by their XPath and returns a collection of IWebElementAdapter.
    /// </summary>
    /// <param name="xpath">The XPath to find elements.</param>
    /// <returns>A collection of IWebElementAdapter representing the found elements.</returns>
    public IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath) =>
        _driver.FindElements(By.XPath(xpath))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();

    /// <summary>
    /// Finds an element by its placeholder attribute and returns an IWebElementAdapter.
    /// </summary>
    /// <param name="placeholder">The placeholder text of the element to find.</param>
    /// <returns>
    /// An <see cref="IWebElementAdapter"/> representing the found element.
    /// </returns>
    /// <exception cref="NoSuchElementException">Thrown if no element with the specified placeholder is found.</exception>
    public IWebElementAdapter FindElementByPlaceholder(string placeholder) =>
        new SeleniumWebElementAdapter(_driver.FindElement(By.CssSelector($"[placeholder='{placeholder}']")));

    /// <summary>
    /// Finds an element by its role attribute and returns an IWebElementAdapter.
    /// </summary>
    /// <param name="role">The role of the element to find.</param>
    /// <returns>
    /// An <see cref="IWebElementAdapter"/> representing the found element.
    /// </returns>
    /// <exception cref="NoSuchElementException">Thrown if no element with the specified role is found.</exception>
    public IWebElementAdapter FindElementByRole(string role) =>
        new SeleniumWebElementAdapter(_driver.FindElement(By.CssSelector($"[role='{role}']")));

    /// <summary>
    /// Finds an element by its label text and returns an IWebElementAdapter.
    /// This method locates the label element and retrieves the associated input element.
    /// </summary>
    /// <param name="label">The text of the label associated with the element to find.</param>
    /// <returns>
    /// An <see cref="IWebElementAdapter"/> representing the found element.
    /// </returns>
    /// <exception cref="NoSuchElementException">Thrown if no element with the specified label is found.</exception>
    public IWebElementAdapter FindElementByLabel(string label) =>
        new SeleniumWebElementAdapter(_driver.FindElement(By.XPath($"//label[text()='{label}']/following-sibling::*")));

    /// <summary>
    /// Waits for an element to be found by its XPath, retrying a specified number of times if necessary.
    /// </summary>
    /// <param name="xpath">The XPath of the element to find.</param>
    /// <param name="timeoutInSeconds">The maximum time to wait for the element to be found.</param>
    /// <returns>An IWebElementAdapter representing the found element.</returns>
    /// <exception cref="WebDriverException">Thrown if the element cannot be found after the maximum retries.</exception>
    public IWebElementAdapter WaitAndFindElementByXPath(string xpath, int timeoutInSeconds = 15)
    {
        const int maxRetries = 3;
        const int retryDelayMs = 1000;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
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

    /// <summary>
    /// Disposes of the WebDriver instance, quitting the browser.
    /// </summary>
    public void Dispose() => _driver.Quit();
}

