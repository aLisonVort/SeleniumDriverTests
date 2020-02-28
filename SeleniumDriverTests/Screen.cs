using OpenQA.Selenium;

namespace SeleniumDriverTests
{

    class Screen 
    {
        public void CapturingFailedTest(bool statusOfTest, string testName,IWebDriver driver)
        {
            if (statusOfTest == false)
            {
                ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile($"C://screenFails/{testName}.png", ScreenshotImageFormat.Png);
            }
        }
    }
}
