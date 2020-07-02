namespace CucumberEndToEnd
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using Xunit;
    using Xunit.Gherkin.Quick;

    [FeatureFile("./DiscountTests.feature")]
    public sealed class DiscountTests : Feature, IDisposable
    {
        private readonly IWebDriver _driver;
        private IWebElement _discountResult;
        private ReadOnlyCollection<IWebElement> _resultsRow;

        public DiscountTests()
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I open (.+)")]
        public void I_open_page(string url)
        {
            _driver.Url = url;
        }

        [And(@"I input (.+) to '(.+)'")]
        public void I_input_value_to(string value, string labelText)
        {
            var input = _driver.FindElement(By.XPath($"//input[self::node()/preceding-sibling::label[text() = '{labelText}']][1]"));
            input.SendKeys(value);
        }

        [And(@"I select '(.+)' in '(.+)'")]
        public void I_select_value_in(string text, string labelText)
        {
            var selectElement = new SelectElement(_driver.FindElement(By.XPath($"//select[self::node()/preceding-sibling::label[text() = '{labelText}']][1]")));
            selectElement.SelectByText(text);
        }

        [When(@"I click submit")]
        public void I_click_submit()
        {
            var submitButton = _driver.FindElement(By.Id("submitDiscountCalculation"));
            submitButton.Click();
        }

        [Then(@"Results appear in (\d+) miliseconds")]
        public async Task Results_appear(int time)
        {
            await Task.Delay(time);
            var possibleDiscountResult = _driver.FindElements(By.Id("currentCalculationResult"));
            _discountResult = possibleDiscountResult[0];
        }

        [And(@"Price is ([\d\.?]+)")]
        public void Price_is(decimal value)
        {
            var price = _discountResult.FindElement(By.Id("currentCalculationPrice"));
            Assert.Equal($"Kaina su nuolaida: {value}", price.Text);
        }

        [Then(@"Results appear in table")]
        public void Results_appear_in_table()
        {
            //added after changes
            var possibleShowTableButton = _driver.FindElements(By.Id("showTableButton"));
            if (possibleShowTableButton.Any())
            {
                possibleShowTableButton[0].Click();
            }

            var createdOn = _discountResult.FindElement(By.Id("currentCalculationCreatedOn")).Text.Remove(0, 18);

            var discountTable = _driver.FindElement(By.Id("discountTable"));
            var tableRows = discountTable.FindElements(By.CssSelector("tr"))
                .ToDictionary(x => x, x => x.FindElements(By.CssSelector("td")));
            _resultsRow = tableRows.FirstOrDefault(x =>
                x.Value.Any() && x.Value[0].Text == createdOn).Value;
            Assert.True(_resultsRow != null && _resultsRow.Any());
        }

        [And(@"The (\d+) column is (.+)")]
        public void Column_is(int columnNumber, string value)
        {
            Assert.Equal(value, _resultsRow[columnNumber - 1].Text);
        }

    public void Dispose()
        {
            _driver.Close();
        }
    }
}