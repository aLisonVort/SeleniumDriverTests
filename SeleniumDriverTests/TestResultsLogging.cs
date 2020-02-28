using NUnit.Framework;
namespace SeleniumDriverTests
{
    class TestResultsLogging
    {
        public string GetTestName()
        {
            return TestContext.CurrentContext.Test.Name;
        }

        public string GetTestStatus()
        {
            string testStatus = TestContext.CurrentContext.Result.Outcome.Status.ToString();
            return testStatus;
        }

        public bool GetTestStatus(string testStatus)
        {
            bool statusOfTest;
            if (testStatus == "Failed")
            {
                statusOfTest = false;
            }
            else
            {
                statusOfTest = true;
            }
            
            return statusOfTest;
        }

    }
}
