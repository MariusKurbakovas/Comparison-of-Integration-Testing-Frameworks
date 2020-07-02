namespace Pact
{
    using System.Collections.Generic;
    using PactNet;
    using PactNet.Infrastructure.Outputters;
    using Xunit;
    using Xunit.Abstractions;

    public class DiscountTestsProvider
    {
        private const string ServiceUri = "http://localhost:4235";
        private readonly IPactVerifier _pactVerifier;

        public DiscountTestsProvider(ITestOutputHelper output)
        {
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(output)
                },
                Verbose = true
            };
            _pactVerifier = new PactVerifier(config);
        }

        [Fact]
        public void VerifyPact()
        {
            _pactVerifier
                .ServiceProvider("DiscountService", ServiceUri)
                .HonoursPactWith("UserInterface")
                .PactUri("pacts/userinterface-discountservice.json")
                .Verify();
        }
    }
}