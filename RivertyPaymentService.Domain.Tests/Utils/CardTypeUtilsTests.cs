using FluentAssertions;
using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Core.Utils;
using Xunit;

namespace RivertyPaymentService.Core.Tests.Utils
{
    public class CardTypeUtilsTests
    {
        [Theory]
        [InlineData("5555555555554444", CardType.MasterCard)]
        [InlineData("4111111111111111", CardType.Visa)]
        [InlineData("371449635398431", CardType.AmericanExpress)]
        [InlineData("6011000990139424", CardType.Unknown)]
        public void GetCardType(string cardNumber, CardType expectedType ) 
        {
            // arrange & act
            var result = CardTypeUtils.GetCardType(cardNumber);
            // assert
            result.Should().Be(expectedType);
        }
    }
}
