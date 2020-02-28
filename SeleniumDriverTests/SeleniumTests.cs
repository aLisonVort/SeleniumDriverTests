using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using SeleniumDriverTests;
using System.Configuration;
using System.Threading;

namespace SeleniumTest
{
    public class SeleniumTests
    {
        protected IWebDriver driver;
        Actions actions;
        IWait<IWebDriver> wait;
        TestResultsLogging log;
        Screen screen;
        //DownloadedFiles file;
        IJavaScriptExecutor js;

        [SetUp]
        public void OnceSetUp()
        {
            log = new TestResultsLogging();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            Console.WriteLine($"Test name  : {log.GetTestName()}");
            driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));
        }

        [Test, Order(0)]
        [Ignore("Unpredictible results")]
        public void ABTesting()
        {
            IWebElement abTesting = driver.FindElement(By.LinkText("A/B Testing"));
            abTesting.Click();
            IWebElement headerAB = driver.FindElement(By.XPath("//div[@class='example']/h3"));

            string actualHeaderAB = headerAB.Text;
            string expectedHeaderAB = "A/B Test Variation 1";
            Assert.AreEqual(expectedHeaderAB, actualHeaderAB);
            //string expectedHeaderAB = "A/B Test Control";

            IWebElement pTagContent = driver.FindElement(By.XPath("//div[@class = 'example']/p"));
            string actualPTagContent = pTagContent.Text;
            string expectedPTagContent = "Also known as split testing. This is a way in which businesses are able to simultaneously test and learn different versions of a page to see which text and/or functionality works best towards a desired outcome (e.g. a user action such as a click-through).";
            Assert.AreEqual(expectedPTagContent, actualPTagContent);
        }

        [Test, Order(1)]
        [Ignore("Feature doesn't work")]
        public void AddRemoveTest()

        {
            IWebElement addRemoveelementsLink = driver.FindElement(By.LinkText("Add/Remove Elements"));
            addRemoveelementsLink.Click();

            IWebElement addElementButton = driver.FindElement(By.XPath("//div[@class='example']/button"));
            addElementButton.Click();

            IWebElement deleteButton = driver.FindElement(By.XPath("//div[@id='elements']/button"));
            //IWebElement deleteButton = driver.FindElement(By.Id("elements"));
            bool visible = deleteButton.Displayed;
            Assert.IsTrue(visible);
            deleteButton.Click();
            try
            {
                visible = deleteButton.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                visible = false;
            }
            Assert.IsFalse(visible);
        }

        [Test, Order(2)]
        public void AuthenticationTest()
        {
            IWebElement basicauthLink = driver.FindElement(By.LinkText("Basic Auth"));
            basicauthLink.Click();
            driver.Navigate().GoToUrl("http://admin:admin@the-internet.herokuapp.com/basic_auth");
            IWebElement basicAuthHeader = driver.FindElement(By.XPath("//div[@class= 'example']/h3"));
            string actualHeader = basicAuthHeader.Text;
            string expectedHeader = "Basic Auth";
            Assert.AreEqual(expectedHeader, actualHeader);
        }

        [Test, Order(3)]
        [Ignore("The first 2 images is broken")]
        public void BrokenImagesTest()
        {
            IWebElement brokenImagesLink = driver.FindElement(By.LinkText("Broken Images"));
            brokenImagesLink.Click();
            List<IWebElement> imagesList = driver.FindElements(By.XPath("//div[@class='example']/img")).ToList();
            List<bool> statuses = new List<bool>();
            foreach (var item in imagesList)
            {
                var stringWidth = item.GetAttribute("naturalWidth");
                int intWidth = int.Parse(stringWidth);
                if (intWidth == 0)
                {
                    statuses.Add(false);
                }
                else
                {
                    statuses.Add(true);
                }
            }
            throw new Exception($"First image is {statuses[0]} \n " +
                $"Second image is {statuses[1]}  \n" +
                $"Third image is {statuses[2]}");
        }

        [Test, Order(4)]
        public void DOMTests()
        {
            IWebElement challengeDOMLink = driver.FindElement(By.LinkText("Challenging DOM"));
            challengeDOMLink.Click();

            List<string> columnValues = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                columnValues.Add(driver.FindElement(By.XPath($"//div[@class='large-10 columns']/table/tbody/tr[{i + 1}]/td[3]")).Text);
            }

            List<string> columnValuesPair = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                IWebElement element = (driver.FindElement(By.XPath($"//div[@class='large-10 columns']/table/tbody/tr[{i + 1}]/td[3]")));
                string elementText = element.Text;
                char lastCharacter = elementText[elementText.Length - 1];
                int lastCharacterInt = Convert.ToInt32(lastCharacter);

                if (lastCharacterInt % 2 == 0)
                {
                    columnValuesPair.Add(driver.FindElement(By.XPath($"//div[@class='large-10 columns']/table/tbody/tr[{i + 1}]/td[3]")).Text);
                }

            }
            //for (int i = 0; i < 10; i=i+2)
            //{
            //    columnValuesPair.Add(driver.FindElement(By.XPath($"//div[@class='large-10 columns']/table/tbody/tr[{i + 1}]/td[3]")).Text);
            //}
        }

        [Test, Order(5)]
        public void CheckBoxestest()
        {
            IWebElement checkboxesLink = driver.FindElement(By.LinkText("Checkboxes"));
            checkboxesLink.Click();

            IWebElement checkbox1 = driver.FindElement(By.XPath("//form[@id='checkboxes']/input[1]"));
            IWebElement checkbox2 = driver.FindElement(By.XPath("//form[@id='checkboxes']/input[2]"));
            checkbox1.Click();
            checkbox2.Click();

            Assert.IsTrue(checkbox1.Enabled);
            Assert.IsTrue(checkbox2.Enabled);
        }

        [Test, Order(6)]
        [Ignore("TimeOut no such alert")]
        public void ContextMenutest()
        {
            IWebElement contextMenulink = driver.FindElement(By.LinkText("Context Menu"));
            contextMenulink.Click();

            IWebElement contextMenu = driver.FindElement(By.XPath("//div[@id='hot-spot']"));
            actions = new Actions(driver);
            actions.ContextClick(contextMenu).Perform();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert = driver.SwitchTo().Alert();

            try
            {
                Assert.IsTrue(alert != null);
            }
            catch (NoAlertPresentException)
            {

            }
            alert.Accept();
            try
            {
                driver.SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                Assert.Pass("Alert is not present on the page");
            }
        }

        [Test, Order(7)]
        public void DigestAuth()
        {
            IWebElement digestAuth = driver.FindElement(By.LinkText("Digest Authentication"));
            digestAuth.Click();

            driver.Navigate().GoToUrl("http://admin:admin@the-internet.herokuapp.com/digest_auth");

            IWebElement pTagOnDigestAuthPage = driver.FindElement(By.XPath("//div[@class= 'example']/p"));
            string actualContent = pTagOnDigestAuthPage.Text;
            string expectedContent = "Congratulations! You must have the proper credentials.";
            Assert.AreEqual(expectedContent, actualContent);
        }

        [Test, Order(8)]
        [Ignore("http error")]
        public void DissapearingElemenysTest()
        {
            IWebElement dissappearingElementsLink = driver.FindElement(By.LinkText("Disappearing Elements"));
            dissappearingElementsLink.Click();

            IWebElement aboutButton = driver.FindElement(By.LinkText("About"));
            aboutButton.Click();

            //IWebElement notFound = driver.FindElement(By.XPath("//h1"));
            //string actualElement = notFound.Text;

            string expectedUrl = "http://the-internet.herokuapp.com/about/";
            string actualUrl = driver.Url;
            Assert.AreEqual(expectedUrl, actualUrl);
        }

        [Test, Order(9)]
        [Ignore("http error")]
        public void DissapearingElemenysTestHome()
        {
            IWebElement dissappearingElementsLink = driver.FindElement(By.LinkText("Disappearing Elements"));
            dissappearingElementsLink.Click();

            IWebElement homeButton = driver.FindElement(By.LinkText("Home"));
            homeButton.Click();

            IWebElement headingHomePage = driver.FindElement(By.XPath("//div[@id = 'content']/h1[@class = 'heading']"));
            string actualHeadingText = headingHomePage.Text;
            string expectedHeadingText = "Welcome to the-internet";

            Assert.AreEqual(expectedHeadingText, actualHeadingText);
        }

        [Test, Order(10)]

        [Ignore("Elements is not replaced")]
        public void DragAndDropTests()
        {
            IWebElement dragDropLink = driver.FindElement(By.LinkText("Drag and Drop"));
            dragDropLink.Click();

            IWebElement elementColumn1 = driver.FindElement(By.XPath("//div[@id = 'column-a']"));
            IWebElement elementColumn2 = driver.FindElement(By.XPath("//div[@id = 'column-b']"));

            string actualHeaderColumn1 = elementColumn1.Text;

            string expectedCurrentHeader = "A";

            Assert.AreEqual(expectedCurrentHeader, actualHeaderColumn1);

            actions = new Actions(driver);
            actions.DragAndDrop(elementColumn2, elementColumn1).Build().Perform();

            string actualHeaderColumn2 = elementColumn2.Text;
            Assert.AreEqual(expectedCurrentHeader, actualHeaderColumn2);
        }

        [Test, Order(11)]
        public void DropdownTest()
        {
            driver.FindElement(By.LinkText("Dropdown")).Click();

            IWebElement dropdown = driver.FindElement(By.Id("dropdown"));
            SelectElement selectDD = new SelectElement(dropdown);
            Assert.AreEqual(selectDD.SelectedOption.Text, "Please select an option");

            selectDD.SelectByValue("1");
            Assert.AreEqual("Option 1", selectDD.SelectedOption.Text);

            selectDD.SelectByIndex(2);
            Assert.AreEqual("true", selectDD.SelectedOption.GetAttribute("selected"));
        }

        [Test, Order(12)]
        [Ignore("The page contain bug")]
        public void DynamicContentTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/dynamic_content");
            string srcImage1 = driver.FindElement(By.XPath("//div[@class='large-2 columns']/img")).GetAttribute("src");

            IWebElement clickHereButton = driver.FindElement(By.LinkText("click here"));
            clickHereButton.Click();

            string currentsrcImage1 = driver.FindElement(By.XPath("//div[@class='large-2 columns']/img")).GetAttribute("src");
            Assert.AreNotEqual(srcImage1, currentsrcImage1);

            string textField3 = driver.FindElement(By.XPath("//*[@id='content']/div[3]")).Text;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            clickHereButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("click here")));
            clickHereButton.Click();

            string afterClickingTextField3 = driver.FindElement(By.XPath("//*[@id='content']/div[3]")).Text;

            Assert.AreNotEqual(afterClickingTextField3, textField3);
        }
        [Test, Order(13)]
        public void DynamicControlTests()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/dynamic_controls");
            IWebElement removeButton = driver.FindElement(By.XPath("//div[@id='checkbox']/following::button[1]"));
            removeButton.Click();
            string actualPText = driver.FindElement(By.Id("message")).Text;
            string expectedPText = "It's gone!";
            Assert.AreEqual(expectedPText, actualPText);

            IWebElement addButton = driver.FindElement(By.XPath("//form[@id='checkbox-example']/child::button"));
            bool visible = addButton.Displayed;
            if (visible == true)
            {
                addButton.Click();
            }
            else
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(addButton));
            }

            string expectedPTextAfterChanging = "It's back!";
            string actualPTextAfterChanging = driver.FindElement(By.Id("message")).Text;
            Assert.AreEqual(expectedPTextAfterChanging, actualPTextAfterChanging);
        }
        [Test, Order(14)]
        public void EntryadTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/entry_ad");
            IWebElement modalWindow = driver.FindElement(By.XPath("//div[@id='modal']/child::div[@class='modal']/div[2]/p"));
            string actualTextP = modalWindow.GetAttribute("innerText");
            string expectedTextP = "It's commonly used to encourage a user to take an action (e.g., give their e-mail address to sign up for something or disable their ad blocker).";
            Assert.AreEqual(expectedTextP, actualTextP);

            IWebElement closeButton = driver.FindElement(By.XPath("//div[@id = 'modal']/descendant::div[@class= 'modal-footer']/p"));
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript("arguments[0].click();", closeButton);
            string actualContent = driver.FindElement(By.XPath("//div[@id='content']/descendant::p[2]")).GetAttribute("textContent");
            string expectedContent = "If closed, it will not appear on subsequent page loads.";
            Assert.AreEqual(expectedContent, actualContent);
        }

        [Test, Order(15)]
        public void ExitIntentTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/exit_intent");
            IWebElement e = driver.FindElement(By.CssSelector("h3"));

            actions = new Actions(driver);
            try
            {
                actions.MoveToElement(e).MoveByOffset(600, 0).Build().Perform();
            }
            catch (Exception)
            {
            }
            finally
            {
                Assert.Pass();
            }

            try
            {
                string actualModalWindowText = driver.FindElement(By.XPath("//div[@class='modal-title']/h3")).GetAttribute("innerText");
                string expectedModalWindowText = "This is a modal window";
                Assert.AreEqual(expectedModalWindowText, actualModalWindowText);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail();
                throw;
            }
        }


        //[Test, Order(16)]
        //public void DownloadFileTest()
        //{
        //    driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/download");
        //    IWebElement fileCar = driver.FindElement(By.XPath("//div[@id = 'content']/descendant::a[1]"));
        //    string fileCarName = "FirefoxGrid.yaml";
        //    fileCar.Click();
        //    file = new DownloadedFiles();
        //    Thread.Sleep(3000);
        //    bool fileExist = file.CheckFileDownloaded(fileCarName);
        //    Assert.IsTrue(fileExist);
        //}

        [Test, Order(17)]
        public void UploadingFileTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/upload");
            driver.FindElement(By.Id("file-upload")).SendKeys("C:/sample.jpg");
            IWebElement uploadButton = driver.FindElement(By.Id("file-submit"));
            uploadButton.Click();

            string actualText = driver.FindElement(By.XPath("//div[@id='content']/descendant::h3")).Text;
            string expectedText = "File Uploaded!";
            Assert.AreEqual(expectedText, actualText);

        }

        [Test, Order(18)]
        public void FloatingMenutests()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/floating_menu");
            driver.FindElement(By.LinkText("Home")).Click();
            string actualUrlHome = driver.Url;
            string expectedUrlHome = "http://the-internet.herokuapp.com/floating_menu#home";
            Assert.AreEqual(expectedUrlHome, actualUrlHome);

            driver.FindElement(By.LinkText("News")).Click();
            string actualUrlNews = driver.Url;
            string expectedUrlNews = "http://the-internet.herokuapp.com/floating_menu#news";
            Assert.AreEqual(expectedUrlNews, actualUrlNews);
        }

        [Test, Order(19)]
        public void ForgotPasswordTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/forgot_password");
            driver.FindElement(By.XPath("//label/following::input")).SendKeys("test@ua");
            driver.FindElement(By.Id("form_submit")).Click();
            IWebElement content = driver.FindElement(By.Id("content"));
            string actualText = content.Text;
            string expectedText = "Your e-mail's been sent!";
            Assert.AreEqual(expectedText, actualText);

        }

        [Test, Order(20)]
        public void LoginFormTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/login");
            string userName = "tomsmith";
            string password = "SuperSecretPassword!";

            driver.FindElement(By.Id("username")).SendKeys(userName);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.XPath("//input[@id = 'password']/following::button")).Click();

            string expectedFlashtext = "You logged into a secure area!\r\n×";
            string actualFlashText = driver.FindElement(By.Id("flash")).Text;
            Assert.AreEqual(expectedFlashtext, actualFlashText);
        }

        [Test, Order(21)]
        public void FramesTest()
        {
            string textAreaText = "Test sample";
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/frames");
            driver.FindElement(By.LinkText("iFrame")).Click();
            //determine weather driver on the iFrame Page
            string actualCurrentUrlIFrame = driver.Url;
            string expectedCurrentUrlIFrame = "http://the-internet.herokuapp.com/iframe";
            Assert.AreEqual(expectedCurrentUrlIFrame, actualCurrentUrlIFrame);

            var frame = driver.SwitchTo().Frame("mce_0_ifr");
            var textArea = frame.FindElement(By.XPath("//body[@id = 'tinymce' ]/descendant::p"));

            textArea.Clear();
            textArea.SendKeys(textAreaText);

            string strEditable = textArea.GetAttribute("isContentEditable");
            bool editable = bool.Parse(strEditable);
            Assert.IsTrue(editable);

            //var editButton = driver.FindElement(By.XPath("//div[@id = 'mceu_14']//div[@id = 'mceu_16']/button[@id = 'mceu_16-open']"));
            //editButton.Click();
            //IWebElement editButtonCopy = frame.FindElement(By.Id("mceu_39-text"));
            //editButtonCopy.Click();
        }

        [Test, Order(22)]
        public void HoversTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/hovers");
            actions = new Actions(driver);
            IWebElement thirdImage = driver.FindElement(By.XPath("//div[@id = 'content']/descendant::img[3]"));
            actions.MoveToElement(thirdImage).Build().Perform();
            IWebElement H5 = driver.FindElement(By.XPath("//div[@id = 'content']/descendant::h5[3]"));
            bool H5IsVisible = H5.Displayed;
            Assert.IsTrue(H5IsVisible);

            driver.FindElement(By.LinkText("View profile")).Click();
            string currentUrl = driver.Url;
            string expectedUrl = "http://the-internet.herokuapp.com/users/3";
            Assert.AreEqual(expectedUrl, currentUrl);
        }

        [Test, Order(23)]
        public void JQueryUITest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/jqueryui/menu#");
            actions = new Actions(driver);

            var enabledButtonElemnt = driver.FindElement(By.Id("ui-id-2"));
            actions.MoveToElement(enabledButtonElemnt).Build().Perform();
            var downloadButton = driver.FindElement(By.XPath("//a[text() = 'Downloads']"));
            var backToJQueryButton = driver.FindElement(By.XPath("//a[@id = 'ui-id-2']/following-sibling::ul/descendant::a[contains(text() , 'JQuery')]"));
            backToJQueryButton.Click();
            string currentURL = driver.Url;
            var expectedURL = ConfigurationManager.AppSettings["expectedURLJQuery"];
            Assert.AreEqual(expectedURL, currentURL);
        }

        [Test, Order(24)]
        public void InfiniteScrolltest()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/infinite_scroll");
            js = (IJavaScriptExecutor)driver;
            bool isPresent = false;

            while (isPresent == false)
            {
                try
                {
                    var element = driver.FindElement(By.XPath("//div[10]"));
                    isPresent = true;
                }
                catch (NoSuchElementException)
                {
                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                }
            }
            Assert.IsTrue(driver.FindElement(By.XPath("//div[10]")).Displayed);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        [Test, Order(25)]
        public void InputsTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/inputs");
            var input = driver.FindElement(By.XPath("//div[@id= 'content']/descendant::input"));
            string expectedInput = ConfigurationManager.AppSettings["expectedInputData"];
            input.SendKeys(expectedInput);
            string actualInput = input.GetAttribute("value");
            Assert.AreEqual(expectedInput, actualInput);
        }

        [Test, Order(26)]
        public void JSAlertsTests()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/javascript_alerts");
            var alertButton = driver.FindElement(By.XPath("//div[@id= 'content']/descendant::li[1]/button"));
            alertButton.Click();
            driver.SwitchTo().Alert().Accept();

            var confirmButton = driver.FindElement(By.XPath("//div[@id= 'content']/descendant::li[2]/button"));
            confirmButton.Click();
            driver.SwitchTo().Alert().Dismiss();

            var promptButton = driver.FindElement(By.XPath("//div[@id= 'content']/descendant::li[3]/button"));
            promptButton.Click();
            string text = "Hello Test!";
            driver.SwitchTo().Alert().SendKeys(text);
            driver.SwitchTo().Alert().Accept();

            string actualPText = driver.FindElement(By.Id("result")).Text;
            string expectedPText = $"You entered: {text}";
            Assert.AreEqual(expectedPText, actualPText);
        }

        [Test, Order(27)]

        public void KeyPresses()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/key_presses");
            var input = driver.FindElement(By.Id("target"));
            string keys = "A";
            input.SendKeys(keys);

            var p = driver.FindElement(By.Id("result"));
            string actualPText = p.Text;
            string expectedPText = $"You entered: {keys}";
            Assert.AreEqual(expectedPText, actualPText);
        }

        [Test, Order(28)]
        public void DeepDOMTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/large");
            var el50 = driver.FindElement(By.Id("sibling-50.2"));
            bool elVisible = el50.Displayed;
            Assert.IsTrue(elVisible);

            IWebElement td = driver.FindElement(By.XPath("//table[@id = 'large-table']/descendant::tr[@class = 'row-26']/td[13]"));
            string actualTdValue = td.GetAttribute("innerText");
            string expectedTdValue = "26.13";
            Assert.AreEqual(expectedTdValue, actualTdValue);
        }

        [Test, Order(29)]
        public void WindowsTest()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/windows");
            driver.FindElement(By.LinkText("Click Here")).Click();

            driver.SwitchTo().Window(driver.WindowHandles.Last());
            string actualtext = driver.FindElement(By.XPath("//div/h3")).Text;
            string expectedtext = "New Window";
            Assert.AreEqual(expectedtext, actualtext);
            driver.Close();
            int currentTabs = driver.WindowHandles.Count;
            if (currentTabs == 1)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test, Order(30)]
        public void NestedFrames()
        {
            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/nested_frames");
            Thread.Sleep(3000);
            driver.SwitchTo().Frame(driver.FindElement(By.XPath("//frame[@name='frame-top']")));
            driver.SwitchTo().Frame(driver.FindElement(By.XPath("//frame[@name='frame-left']")));
            string actualTextFrameLeft = driver.FindElement(By.XPath("//body")).Text;
            string expectedTextFrameLeft = "LEFT";
            Assert.AreEqual(expectedTextFrameLeft, actualTextFrameLeft);

            driver.SwitchTo().DefaultContent();
            var noFramesTag = driver.FindElement(By.XPath("//frameset[not(contains(@name,'frameset-middle'))]/noframes"));
            string actualNoFramesTagText = noFramesTag.GetAttribute("innerHTML");
            actualNoFramesTagText = actualNoFramesTagText.Replace("\r\n", string.Empty);
            string empty = "  ";
            actualNoFramesTagText = actualNoFramesTagText.Replace(empty, string.Empty);
            string expectedNoFramesTagText = "Frames are not rendering.";

            Assert.AreEqual(expectedNoFramesTagText, actualNoFramesTagText);
        }

        [TearDown]
        public void TearDown()
        {
            log = new TestResultsLogging();
            screen = new Screen();
            string testStatus = log.GetTestStatus();
            Console.WriteLine($"The test tatus : {testStatus}");
            bool testSatus = log.GetTestStatus($"{ testStatus }");
            string testName = log.GetTestName();
            screen.CapturingFailedTest(testSatus, testName, driver);
            Console.WriteLine("-------------");
            driver.Quit();
        }
    }
}
