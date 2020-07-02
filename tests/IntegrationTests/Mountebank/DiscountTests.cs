namespace Mountebank
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using MbDotNet;
    using MbDotNet.Enums;
    using MbDotNet.Models.Predicates;
    using MbDotNet.Models.Predicates.Fields;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using Xunit;

    public class DiscountTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly MountebankClient _client;
        private const int DiscountMetadataServicePort = 12862;

        public DiscountTests()
        {
            _driver = new ChromeDriver();
            _client = new MountebankClient();

            var imposter = _client.CreateHttpImposter(DiscountMetadataServicePort, "DiscountMetadataImposter");

            var maximaPredicateFields = new HttpPredicateFields
            {
                Method = Method.Get,
                Path = "/metadata",
                QueryParameters = new Dictionary<string, string> { { "loyaltyType", "Maxima" } }
            };

            var maximaPredicate = new MatchesPredicate<HttpPredicateFields>(maximaPredicateFields);

            imposter.AddStub()
                .On(maximaPredicate)
                .ReturnsJson(HttpStatusCode.OK,
                    new
                    {
                        customerDiscount = 0.03,
                        percentDepositedInCard = (decimal?)null,
                        discountFromLoyalty = 0.02,
                        areDiscountsSummed = true
                    });

            imposter.AddStub()
                .OnPathAndMethodEqual("/metadata", Method.Get)
                .ReturnsJson(HttpStatusCode.OK,
                    new
                    {
                        customerDiscount = 0.06,
                        percentDepositedInCard = 0.02,
                        discountFromLoyalty = 0.04,
                        areDiscountsSummed = false
                    });

            _client.Submit(imposter);
        }

        [Fact]
        public async void SubmitForm_GetCorrectResults_Maxima()
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
                    Assert.Equal("Kaina su nuolaida: 19", price.Text);
                    var moneyInCard = discountResult.FindElements(By.Id("currentCalculationMoneyInCard"));
                    Assert.False(moneyInCard.Any());
                    resultsAssertsExecuted = true;
                }
            }

            Assert.True(resultsAssertsExecuted);
        }

        [Fact]
        public async void SubmitForm_GetCorrectResults_Other()
        {
            _driver.Url = "http://localhost:3000/";
            var originalPriceInput = _driver.FindElement(By.Id("originalPriceInput"));
            originalPriceInput.SendKeys("20");

            var loyaltyTypeSelect = new SelectElement(_driver.FindElement(By.Id("loyaltyTypeInput")));
            loyaltyTypeSelect.SelectByValue("2");

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
                    Assert.Equal("Kaina su nuolaida: 18.8", price.Text);
                    var moneyInCard = discountResult.FindElement(By.Id("currentCalculationMoneyInCard"));
                    Assert.Equal("Pinigai pridėti į kortelę: 0.376", moneyInCard.Text);
                    resultsAssertsExecuted = true;
                }
            }

            Assert.True(resultsAssertsExecuted);
        }

        public void Dispose()
        {
            _driver.Close();
            _client.DeleteAllImposters();
        }
    }
}