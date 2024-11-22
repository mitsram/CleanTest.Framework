# CleanTest.Framework
CleanTest.Framework is a powerful solution designed to streamline your test automation process. With CleanTest, you can easily switch between different automation tools without the need to rewrite your existing tests. This flexibility allows you to adapt to changing project requirements and leverage the best tools available in the market.

Whether you're using Selenium for its extensive browser support or Playwright for its modern features, CleanTest.Framework ensures that your test suite remains robust and maintainable. Focus on delivering quality software while we handle the complexities of test automation.

## How to use

### Where to install
In the project(s) where your tests and page objects resides, search and install this package from NuGet Package Library. Or alternatively, from the command line:

```
dotnet add package CleanTest.Framework --version 1.x
```

### Test Project configuration (Recommended)
In your test project, create TestConfiguration class, to read your configuration file:

```csharp
using CleanTest.Framework.Drivers.WebDriver.Enums;
using Microsoft.Extensions.Configuration;

public class TestConfiguration(IConfiguration configuration)
{
    public WebDriverType WebDriverType { get; } = configuration.GetValue<WebDriverType>("WebDriverType", WebDriverType.Selenium);
    public BrowserType BrowserType { get; } = configuration.GetValue<BrowserType>("BrowserType", BrowserType.Chrome);
    public string BaseUrl { get; } = configuration.GetValue<string>("TestSettings:BaseUrl", "https://www.example.com/");
    public int Timeout { get; } = configuration.GetValue<int>("TestSettings:Timeout", 30);
}
```

Create a testconfig.json file to hold your configuration:

```json
{
    "WebDriverType": "Selenium",
    "BrowserType": "Chrome",
    "TestSettings": {
      "BaseUrl": "https://www.example.com",
      "Timeout": 30
    }
}
```

In your BaseTest class

```csharp
using CleanTest.Framework.Factories;
using CleanTest.Framework.WebDriver.Interfaces;

public class BaseTest 
{
    // ... other code here

    [SetUp]
    public void SetUp()
    {
        driver = WebDriverFactory.Create(TestConfig.WebDriverType, TestConfig.BrowserType);
    }

    // ... other code here
}
```

### How to use in page objects

```csharp
using CleanTest.Framework.WebDriver.Interfaces;

public class ShopPage
{
    private readonly IWebDriverAdapter driver;
    private bool _disposed;

    public ShopPage(IWebDriverAdapter driverAdapter)
    {
        driver = driverAdapter;
    }

    public void AddProductToCart(string productName)
    {
        var addButton = driver.FindElementByXPath($"//div[text()='{productName}']/ancestor::div[@class='inventory_item']//button[contains(@id, 'add-to-cart')]");
        addButton.Click();
    }

    // ... rest of the code here
}
```
The `driver` is of type IWebDriverAdapter, that will be implemented by specific adapter like Playwright or Selenium depending on the configuration.

## Example Test Project

For a complete example test project using CleanTest.Framework, please visit the following GitHub repository:

* [SauceDemo.Tests](https://github.com/mitsram/SauceDemo.Tests)

## Contribution
We welcome contributions to CleanTest.Framework! If you would like to contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Make your changes and commit them with clear messages.
4. Push your changes to your forked repository.
5. Submit a pull request detailing your changes.

Please ensure that your code adheres to the existing style and includes appropriate tests.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.