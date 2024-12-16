namespace CleanTest.Framework.WebDriver.Interfaces
{
    public interface IFrameHandler
    {
        IWebElementAdapter FindElementById(string id);
        IWebElementAdapter FindElementByClassName(string className);
        // Add other methods as needed
    }
}
