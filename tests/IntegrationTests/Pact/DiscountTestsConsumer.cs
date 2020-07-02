namespace Pact
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using PactNet;
    using PactNet.Mocks.MockHttpService;
    using PactNet.Mocks.MockHttpService.Models;
    using Xunit;

    public class DiscountTestsConsumer : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly IPactBuilder _pactBuilder;
        private readonly IMockProviderService _mockProviderService;
        private const int DiscountServicePort = 4235;

        public DiscountTestsConsumer()
        {
            _driver = new ChromeDriver();

            _pactBuilder = new PactBuilder(new PactConfig
            {
                SpecificationVersion = "2.0.0",
                EnableCors = true // need to explicity enable CORS
            });

            //each test should have a separate pact builder because they overwrite the same pact file
            _pactBuilder.ServiceConsumer("UserInterface").HasPactWith("DiscountService");

            //if test is interrupted after initiating MockService, then need to kill ruby runtime manually
            //also creating MockServices automatically starts it
            _mockProviderService = _pactBuilder.MockService(DiscountServicePort);
            _mockProviderService.ClearInteractions();

            _mockProviderService
                .Given("The server contains a calculation")
                .UponReceiving("A GET request to fetch all calculations")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    //Path name needs to match exactly "/discount" != "/discount/" != "/Discount/"
                    Path = "/discount/"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    }
                });
        }

        [Fact]
        public async void SubmitForm_GetCorrectResults_Maxima()
        {
            _mockProviderService
                .Given("The service is able to calculate disocunts")
                .UponReceiving("A POST request to calculate discount")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/discount/",
                    Body = new
                    {
                        customerType = 2,
                        discountOnProduct = 0,
                        loyaltyType = 1,
                        originalPrice = 20
                    },
                    //If there is a body content-type must be specified
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json;charset=UTF-8" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        discountedPrice = 18,
                        moneySavedInCard = (decimal?)null,
                        customerType = 2,
                        discountOnProduct = 0,
                        loyaltyType = 1,
                        originalPrice = 20
                    }
                });

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
                    resultsAssertsExecuted = true;
                }
            }

            Assert.True(resultsAssertsExecuted);

            //if a GET request is made, but is not described in _mockServiceProvider error is thrown here
            _mockProviderService.VerifyInteractions();
            //should not stop the service because _pactBuilder.Build() contacts it
            //_mockProviderService.Stop();
        }

        public void Dispose()
        {
            _driver.Close();
            //this method creates the pact file (and overwrites it). Also it contacts _mockServiceProvider
            _pactBuilder.Build();
        }
    }
}