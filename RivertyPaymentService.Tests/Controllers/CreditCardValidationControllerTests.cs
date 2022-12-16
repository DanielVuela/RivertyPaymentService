using AutoFixture;
using Moq;
using RivertyPaymentService.Controllers;
using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Core.Handlers;
using RivertyPaymentService.Models;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace RivertyPaymentService.Tests.Controllers
{
    public class CreditCardValidationControllerTests : TestBase
    {
        private CreditCardValidationController controller;
        private readonly Mock<ICreditCardHandler> creditCardHandlerMock;
        public CreditCardValidationControllerTests()
        {
            creditCardHandlerMock = Mock<ICreditCardHandler>();
            controller = new CreditCardValidationController(creditCardHandlerMock.Object);
        }
        [Fact]
        public void ValidateCreditCard_ShouldReturn200Status() 
        {
            // Arrange
            var creditCardValidationRequest = fixture.Create<CreditCardValidationRequest>();
            var expectedCardType = CardType.MasterCard;
            creditCardHandlerMock.Setup(service => service.GetCardType(It.IsAny<string>()))
                .Returns(expectedCardType);

            // Act
            var response = controller.ValidateCreditCard(creditCardValidationRequest);

            // Assert
            var okResult = response as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedCardType.ToString());   
        }
    }
}
