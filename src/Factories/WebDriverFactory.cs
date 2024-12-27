using OpenQA.Selenium;
using Microsoft.Playwright;
using BrowserType = CleanTest.Framework.Drivers.WebDriver.Enums.BrowserType;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using CleanTest.Framework.Drivers.WebDriver.Adapters;
using CleanTest.Framework.Drivers.WebDriver.Enums;
using CleanTest.Framework.Drivers.WebDriver.Interfaces;

namespace CleanTest.Framework.Factories;

public static class WebDriverFactory
{    
    public static IWebDriverAdapter Create(WebDriverType webDriverType, BrowserType browserType, bool headless = true)
    {
        return webDriverType switch
        {
            WebDriverType.Selenium => new SeleniumWebDriverAdapter(CreateSeleniumDriver(browserType, headless)),
            WebDriverType.Playwright => new PlaywrightWebDriverAdapter(CreatePlaywrightDriver(browserType, headless)),
            _ => throw new ArgumentException("Invalid WebDriverType"),
        };
    }
    
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
    
    private static IPage CreatePlaywrightDriver(BrowserType browserType, bool headless)
    {
        var browser = CreatePlaywrightBrowser(browserType, headless);
        var context = CreateBrowserContext(browser).GetAwaiter().GetResult();
        return context.NewPageAsync().GetAwaiter().GetResult();
    }
    
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



