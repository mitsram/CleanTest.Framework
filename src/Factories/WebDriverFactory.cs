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
    public static IWebDriverAdapter Create(WebDriverType webDriverType, BrowserType browserType, bool headless = true)
    {
        return webDriverType switch
        {
            WebDriverType.Selenium => new SeleniumWebDriverAdapter(CreateSeleniumDriver(browserType, headless)),
            WebDriverType.Playwright => new PlaywrightWebDriverAdapter(CreatePlaywrightDriver(browserType, headless)),
            _ => throw new ArgumentException("Invalid WebDriverType"),
        };
    }

    /// <summary>
    /// Creates a Selenium WebDriver based on the specified BrowserType and headless configuration.
    /// </summary>
    /// <param name="browserType">The type of browser to create a driver for.</param>
    /// <param name="headless">A boolean indicating whether to run the browser in headless mode.</param>
    /// <returns>An instance of IWebDriver.</returns>
    private static IWebDriver CreateSeleniumDriver(BrowserType browserType, bool headless)
    {
        switch (browserType)
        {
            case BrowserType.Chrome:
                var chromeOptions = new ChromeOptions();
                if (headless)
                {
                    chromeOptions.AddArguments("--headless"); // Enable headless mode if configured
                }
                chromeOptions.AddArguments(
                    "--start-maximized",
                    "--incognito",
                    "--disable-extensions",
                    "--disable-notifications",
                    "--disable-infobars"
                );
                return new ChromeDriver(chromeOptions);
                
            case BrowserType.Firefox:
                var firefoxOptions = new FirefoxOptions();
                if (headless)
                {
                    firefoxOptions.AddArguments("--headless"); // Enable headless mode if configured
                }
                firefoxOptions.AddArguments(
                    "-private",
                    "--start-maximized"
                );
                return new FirefoxDriver(firefoxOptions);
                
            default:
                throw new ArgumentException("Invalid BrowserType for Selenium");
        }
    }

    /// <summary>
    /// Creates a Playwright driver for the specified BrowserType and headless configuration.
    /// </summary>
    /// <param name="browserType">The type of browser to create a Playwright driver for.</param>
    /// <param name="headless">A boolean indicating whether to run the browser in headless mode.</param>
    /// <returns>An instance of IPage.</returns>
    private static IPage CreatePlaywrightDriver(BrowserType browserType, bool headless)
    {
        var browser = CreatePlaywrightBrowser(browserType, headless);
        var context = CreateBrowserContext(browser).GetAwaiter().GetResult();
        return context.NewPageAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Creates a Playwright browser instance based on the specified BrowserType and headless configuration.
    /// </summary>
    /// <param name="browserType">The type of browser to launch.</param>
    /// <param name="headless">A boolean indicating whether to run the browser in headless mode.</param>
    /// <returns>An instance of IBrowser.</returns>
    private static IBrowser CreatePlaywrightBrowser(BrowserType browserType, bool headless)
    {
        var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        var launchOptions = new BrowserTypeLaunchOptions 
        { 
            Headless = headless // Set headless mode based on configuration
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



