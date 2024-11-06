# CleanTest.Framework

## Overview

The CleanTest Framework provides a set of utilities for creating and managing web and API drivers in automated testing scenarios. This framework simplifies the process of interacting with different web browsers and API clients, allowing for a more streamlined testing experience. You can easily switch between various tools such as Playwright, Selenium, and others.

## Features

- **WebDriverFactory**: A factory class for creating instances of `IWebDriverAdapter` based on the specified WebDriverType and BrowserType. Supports multiple tools like Playwright and Selenium.
- **ApiDriverFactory**: A factory class for creating instances of `IApiDriverAdapter` based on the specified base URL and optional authenticator.
- **IWebDriverAdapter**: An interface that defines methods for navigating and interacting with web elements.

## Installation

You can install the CleanTest Framework via NuGet Package Manager:
```
Install-Package CleanTest.Framework
```

## Usage

### WebDriverFactory

To create a web driver adapter, use the `WebDriverFactory.Create` method. You can easily switch between different tools:

```csharp
using CleanTest.Framework.Factories;
using CleanTest.Framework.Drivers.WebDriver.Enums;

// Create a Selenium WebDriver Adapter for Chrome
var webDriverAdapter = WebDriverFactory.Create(WebDriverType.Selenium, BrowserType.Chrome);

// Create a Playwright WebDriver Adapter
var playwrightAdapter = WebDriverFactory.Create(WebDriverType.Playwright, BrowserType.Chromium);
```

### ApiDriverFactory

To create an API driver adapter, use the `ApiDriverFactory.Create` method:

```csharp
using CleanTest.Framework.Factories;

// Create a RestSharp API Driver Adapter
var apiDriverAdapter = ApiDriverFactory.Create("https://api.example.com");
```

### IWebDriverAdapter Interface

The `IWebDriverAdapter` interface provides methods for interacting with web elements:

```csharp
public interface IWebDriverAdapter
{
    void NavigateToUrl(string url);
    string GetCurrentUrl();
    IWebElementAdapter FindElementById(string id);
    // Other methods...
}
```

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any enhancements or bug fixes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

> Notes:
> - The README now highlights the ability to switch between different tools like Playwright and Selenium.
> - Ensure that the `WebDriverType` enum and any related classes are properly defined in your codebase to support this functionality.
