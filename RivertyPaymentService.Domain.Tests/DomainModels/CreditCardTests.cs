using System;
using FluentAssertions;
using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Core.DomainModels;
using Xunit;

namespace RivertyPaymentService.Core.Tests.Models
{
    public class CreditCardTests
    {
        [Theory]
        [InlineData("4111111111111111", CardType.Visa)]
        [InlineData("377400111111115", CardType.AmericanExpress)]
        [InlineData("5431111111111111", CardType.MasterCard)]
        [InlineData("6011309900001248", CardType.Unknown)]
        public void CalculateCreditCardType(string cardNumber, CardType expectedCardType)
        {
            // arrange
            CreditCard card = new CreditCard("Cool Nerd", cardNumber, string.Empty, string.Empty);

            // act
            var result = card.getCreditCardType();

            // assert
            result.Should().Be(expectedCardType);
        }
    }
}
