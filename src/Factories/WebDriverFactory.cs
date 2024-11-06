using OpenQA.Selenium;
using Microsoft.Playwright;
using BrowserType = CleanTest.Framework.Drivers.WebDriver.Enums.BrowserType;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using CleanTest.Framework.Drivers.WebDriver.Adapters;
using CleanTest.Framework.Drivers.WebDriver.Enums;
using CleanTest.Framework.WebDriver.Interfaces;

namespace CleanTest.Framework.Factories;

public static class WebDriverFactory
{
    /// <summary>
    /// Creates an instance of IWebDriverAdapter based on the specified WebDriverType and BrowserType.
    /// </summary>
    /// <param name="webDriverType">The type of WebDriver to create.</param>
    /// <param name="browserType">The type of browser to use.</param>
    /// <returns>An instance of IWebDriverAdapter.</returns>
    public static IWebDriverAdapter Create(WebDriverType webDriverType, BrowserType browserType)
    {
        return webDriverType switch
        {
            WebDriverType.Selenium => new SeleniumWebDriverAdapter(CreateSeleniumDriver(browserType)),
            WebDriverType.Playwright => new PlaywrightWebDriverAdapter(CreatePlaywrightDriver(browserType)),
            _ => throw new ArgumentException("Invalid WebDriverType"),
        };
    }

    /// <summary>
    /// Creates a Selenium WebDriver based on the specified BrowserType.
    /// </summary>
    /// <param name="browserType">The type of browser to create a driver for.</param>
    /// <returns>An instance of IWebDriver.</returns>
    private static IWebDriver CreateSeleniumDriver(BrowserType browserType)
    {
        switch (browserType)
        {
            case BrowserType.Chrome:
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments(
                    // "--headless",
                    "--start-maximized",
                    "--incognito",                    // Enable incognito mode
                    "--disable-extensions",           // Disable extensions
                    "--disable-notifications",        // Disable notifications
                    "--disable-infobars"             // Disable infobars
                );
                return new ChromeDriver(chromeOptions);
                
            case BrowserType.Firefox:
                var firefoxOptions = new FirefoxOptions();
                firefoxOptions.AddArguments(
                    "--headless",
                    "-private",                      // Enable private browsing
                    "--start-maximized"
                );
                return new FirefoxDriver(firefoxOptions);
                
            default:
                throw new ArgumentException("Invalid BrowserType for Selenium");
        }
    }

    /// <summary>
    /// Creates a Playwright driver for the specified BrowserType.
    /// </summary>
    /// <param name="browserType">The type of browser to create a Playwright driver for.</param>
    /// <returns>An instance of IPage.</returns>
    private static IPage CreatePlaywrightDriver(BrowserType browserType)
    {
        var browser = CreatePlaywrightBrowser(browserType);
        var context = CreateBrowserContext(browser).GetAwaiter().GetResult();
        return context.NewPageAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Creates a Playwright browser instance based on the specified BrowserType.
    /// </summary>
    /// <param name="browserType">The type of browser to launch.</param>
    /// <returns>An instance of IBrowser.</returns>
    private static IBrowser CreatePlaywrightBrowser(BrowserType browserType)
    {
        var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        var launchOptions = new BrowserTypeLaunchOptions 
        { 
            Headless = false
        };

        return browserType switch
        {
            BrowserType.Chrome => playwright.Chromium.LaunchAsync(launchOptions).GetAwaiter().GetResult(),
            BrowserType.Firefox => playwright.Firefox.LaunchAsync(launchOptions).GetAwaiter().GetResult(),
            _ => throw new ArgumentException("Invalid BrowserType for Playwright")
        };
    }

    /// <summary>
    /// Creates a new browser context for the specified browser.
    /// </summary>
    /// <param name="browser">The browser to create a context for.</param>
    /// <returns>A Task representing the asynchronous operation, with an IBrowserContext as the result.</returns>
    private static async Task<IBrowserContext> CreateBrowserContext(IBrowser browser)
    {
        // Create a new incognito context
        return await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            JavaScriptEnabled = true,
            ViewportSize = new ViewportSize
            {
                Width = 1920,
                Height = 1080
            }
        });
    }
}



