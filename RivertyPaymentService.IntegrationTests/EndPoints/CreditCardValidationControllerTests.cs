using FluentAssertions;
using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.IntegrationTests.TestData;
using RivertyPaymentService.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RivertyPaymentService.IntegrationTests.EndPoints
{
    public class CreditCardValidationControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient client;

        public CreditCardValidationControllerTests(TestFixture<Startup> fixture)
        {
            client = fixture.Client;
        }


        [Theory]
        [ClassData(typeof(CreditCardValidationRequestsTestData))]
        public async Task ValidatingCreditCatds_ShouldReturn200(CreditCardValidationRequest creditCardValidationRequest, 
                                                                CardType? expectedCardType)
        {
            // arrange
            var request = $"/api/CreditCardValidation?OwnerName={creditCardValidationRequest.OwnerName}&Number={creditCardValidationRequest.Number}&CVC={creditCardValidationRequest.Cvc}&ExpirationDate={creditCardValidationRequest.ExpirationDate}";

            // act
            var response = await client.GetAsync(request);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var cardType = JsonSerializer.Deserialize<string>(response.Content.ReadAsStringAsync().Result);
            cardType.Should().Be(expectedCardType.ToString());
        }

        [Theory]
        [ClassData(typeof(InvalidCreditCardValidationRequestsTestData))]
        public async Task ValidatingCreditCatds_ShouldReturn422(CreditCardValidationRequest creditCardValidationRequest,
                                                                List<string> invalidFieldName)
        {
            // arrange
            var request = $"/api/CreditCardValidation?OwnerName={creditCardValidationRequest.OwnerName}"
                    + "&Number={creditCardValidationRequest.Number}" 
                    + "&CVC={creditCardValidationRequest.Cvc}"
                    + "&ExpirationDate={creditCardValidationRequest.ExpirationDate}";

            // act
            var response = await client.GetAsync(request);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            var result = response.Content.ReadAsStringAsync().Result;
            var errorMessages = JsonSerializer.Deserialize<Dictionary<string, string[]>>(result);
            errorMessages.Should().ContainKeys(invalidFieldName);
        }
    }
}
