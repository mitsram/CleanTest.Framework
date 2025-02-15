namespace CleanTest.Framework.Drivers.WebDriver.Interfaces;

public interface IElementFinder
{
    IWebElementAdapter FindElementByCssSelector(string cssSelector);
}
