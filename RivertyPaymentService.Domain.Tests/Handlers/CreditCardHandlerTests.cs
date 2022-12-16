using FluentAssertions;
using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Core.Handlers;
using RivertyPaymentService.Core.Handlers.Implementors;
using Xunit;

namespace RivertyPaymentService.Core.Tests.Handlers
{
    public class CreditCardHandlerTests : TestBase
    {
        private readonly ICreditCardHandler handler;
        public CreditCardHandlerTests() 
        {
            handler = new CreditCardHandler();
        }
        [Theory]
        [InlineData("5555555555554444", CardType.MasterCard)]
        [InlineData("4111111111111111", CardType.Visa)]
        [InlineData("371449635398431", CardType.AmericanExpress)]
        [InlineData("6011000990139424", CardType.Unknown)]
        public void GetCardType(string cardNumber, CardType expectedType)
        {
            // arrange & act
            var result = handler.GetCardType(cardNumber);
            // assert
            result.Should().Be(expectedType);
        }
    }
}
 