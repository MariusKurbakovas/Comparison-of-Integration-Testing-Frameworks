namespace SeleniumEndToEnd
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;

    using Xunit;

    public class DiscountTests : IDisposable
    {
        private readonly IWebDriver _driver;

        public DiscountTests()
        {
            _driver = new ChromeDriver();
        }

        [Fact]
        public void PageRenders()
        {
            _driver.Url = "http://localhost:3000/";
            Assert.Equal("Nuolaidų skaičiuotuvas", _driver.Title);
            _driver.FindElement(By.ClassName("App"));
        }

        [Fact]
        public void TableDisplayedCorrectly()
        {
            _driver.Url = "http://localhost:3000/";

            //added after changes
            var showTableButton = _driver.FindElement(By.Id("showTableButton"));
            showTableButton.Click();


            var discountTable = _driver.FindElement(By.Id("discountTable"));
            Assert.True(discountTable.Displayed);
            var tableHeaders = discountTable.FindElements(By.CssSelector("th"));
            Assert.Equal(7, tableHeaders.Count);
            Assert.Equal("Skaičiavimo data", tableHeaders[0].Text);
            Assert.Equal("Originali kaina", tableHeaders[1].Text);
            Assert.Equal("Lojalumo tipas", tableHeaders[2].Text);
            Assert.Equal("Kliento tipas", tableHeaders[3].Text);
            Assert.Equal("Nuolaida produktui", tableHeaders[4].Text);
            Assert.Equal("Kaina su nuolaida", tableHeaders[5].Text);
            Assert.Equal("Pinigai pridėti į kortelę", tableHeaders[6].Text);
        }

        [Fact]
        public void FormDisplayedCorrectly()
        {
            _driver.Url = "http://localhost:3000/";
            var discountForm = _driver.FindElement(By.Id("discountForm"));
            Assert.True(discountForm.Displayed);

            var formLabels = discountForm.FindElements(By.CssSelector("label"));
            Assert.Equal(4, formLabels.Count);
            Assert.Equal("Originali kaina:", formLabels[0].Text);
            Assert.Equal("Lojalumo tipas:", formLabels[1].Text);
            Assert.Equal("Kliento tipas:", formLabels[2].Text);
            Assert.Equal("Nuolaida produktui:", formLabels[3].Text);

            var formInputs = discountForm.FindElements(By.CssSelector("input,select"));
            Assert.Equal(5, formInputs.Count);
            Assert.Equal(2, formInputs.Count(x => x.GetAttribute("type") == "number"));
            Assert.Equal(2, formInputs.Count(x => x.TagName == "select"));
            Assert.Equal(1, formInputs.Count(x => x.GetAttribute("type") == "submit"));
        }

        [Fact]
        public void ResultsNotDisplayedInitialy()
        {
            _driver.Url = "http://localhost:3000/";
            var discountResult = _driver.FindElements(By.Id("currentCalculationResult"));
            Assert.False(discountResult.Any());
        }

        [Fact]
        public async void SubmitForm_GetCorrectResults()
        {
            _driver.Url = "http://localhost:3000/";
            var originalPriceInput = _driver.FindElement(By.Id("originalPriceInput"));
            originalPriceInput.SendKeys("20");

            var loyaltyTypeSelect = new SelectElement(_driver.FindElement(By.Id("loyaltyTypeInput")));
            loyaltyTypeSelect.SelectByValue("1");

            var customerTypeSelect = new SelectElement(_driver.FindElement(By.Id("customerTypeInput")));
            customerTypeSelect.SelectByValue("2");

            var submitButton = _driver.FindElement(By.Id("submitDiscountCalculation"));
            submitButton.Click();

            var resultsAssertsExecuted = false;
            for (var i = 0; !resultsAssertsExecuted && i < 3; i++)
            {
                await Task.Delay(1000);
                var possibleDiscountResult = _driver.FindElements(By.Id("currentCalculationResult"));
                if (possibleDiscountResult.Any())
                {
                    var discountResult = possibleDiscountResult[0];
                    var price = discountResult.FindElement(By.Id("currentCalculationPrice"));
                    Assert.Equal("Kaina su nuolaida: 18", price.Text);
                    var moneyInCard = discountResult.FindElements(By.Id("currentCalculationMoneyInCard"));
                    Assert.False(moneyInCard.Any());
                    var createdOn = discountResult.FindElement(By.Id("currentCalculationCreatedOn"));
                    resultsAssertsExecuted = true;
                }
            }

            Assert.True(resultsAssertsExecuted);
        }

        [Fact]
        public async void SubmitForm_ResultsDisplayedInTable()
        {
            _driver.Url = "http://localhost:3000/";
            var originalPriceInput = _driver.FindElement(By.Id("originalPriceInput"));
            originalPriceInput.SendKeys("20");

            var loyaltyTypeSelect = new SelectElement(_driver.FindElement(By.Id("loyaltyTypeInput")));
            loyaltyTypeSelect.SelectByValue("1");

            var customerTypeSelect = new SelectElement(_driver.FindElement(By.Id("customerTypeInput")));
            customerTypeSelect.SelectByValue("2");

            var submitButton = _driver.FindElement(By.Id("submitDiscountCalculation"));
            submitButton.Click();

            var resultsFetched = false;
            string createdOn = null;
            for (var i = 0; !resultsFetched && i < 3; i++)
            {
                await Task.Delay(1000);
                var possibleDiscountResult = _driver.FindElements(By.Id("currentCalculationResult"));
                if (possibleDiscountResult.Any())
                {
                    var discountResult = possibleDiscountResult[0];
                    // remove the text "Skaičiavimo data: " at the beginning of the date
                    createdOn = discountResult.FindElement(By.Id("currentCalculationCreatedOn")).Text.Remove(0, 18);
                    resultsFetched = true;
                }
            }

            Assert.True(resultsFetched);

            //added after changes
            var showTableButton = _driver.FindElement(By.Id("showTableButton"));
            showTableButton.Click();

            var tableAssertsExecuted = false;
            for (var i = 0; !tableAssertsExecuted && i < 3; i++)
            {
                await Task.Delay(1000);
                var discountTable = _driver.FindElement(By.Id("discountTable"));
                var tableRows = discountTable.FindElements(By.CssSelector("tr"))
                    .ToDictionary(x => x, x => x.FindElements(By.CssSelector("td")));
                var resultRow = tableRows.FirstOrDefault(x =>
                    x.Value.Any() && x.Value[0].Text == createdOn).Value;
                if (resultRow != null)
                {
                    Assert.Equal("20", resultRow[1].Text);
                    Assert.Equal("Maxima", resultRow[2].Text);
                    Assert.Equal("Studentas", resultRow[3].Text);
                    Assert.Equal("0", resultRow[4].Text);
                    Assert.Equal("18", resultRow[5].Text);
                    Assert.Equal("0", resultRow[6].Text);

                    tableAssertsExecuted = true;
                }
            }

            Assert.True(tableAssertsExecuted);
        }

        public void Dispose()
        {
            _driver.Close();
        }
    }
}