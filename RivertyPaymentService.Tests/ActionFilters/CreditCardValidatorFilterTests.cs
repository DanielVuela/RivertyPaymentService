using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RivertyPaymentService.ActionFilters;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using RivertyPaymentService.Models;
using Xunit;
using FluentAssertions;
using Moq;
using RivertyPaymentService.Core.Handlers;
using RivertyPaymentService.Core.DomainModels;
using Microsoft.Extensions.Primitives;

namespace RivertyPaymentService.Tests.ActionFilters
{
    public class CreditCardValidatorFilterTests : TestBase
    {
        private readonly CreditCardValidatorFilter filter;

        public CreditCardValidatorFilterTests()
        {
            filter = new CreditCardValidatorFilter();
        }

        [Theory]
        [MemberData(nameof(ValidationRequestsData))]
        public void OnActionExecutingValidatesTheFields_ShouldUpdateModelState(CreditCardValidationRequest creditCardValidationRequest, bool isValid)
        {
            // arrange
            var context = CreateExecutingContext(creditCardValidationRequest);

            // act
            filter.OnActionExecuting(context);

            // assert
            context.ModelState.IsValid.Should().Be(isValid);

        }

        [Theory]
        [MemberData(nameof(CardOwnerData))]
        public void isOwnerNameValid_ShouldValidate(string cardowner, bool isExpectedToBeValid)
        {
            // arrange
            var creditCard = new CreditCard(cardowner, string.Empty, string.Empty, string.Empty);
            // act
            var result = filter.isOwnerNameValid(creditCard);
            // assert
            result.Should().Be(isExpectedToBeValid);

        }

        [Theory]
        [MemberData(nameof(CardNumberData))]
        public void isCardNumberValid_ShouldValidate(string cardNumber, bool isExpectedToBeValid)
        {
            // arrange
            var creditCard = new CreditCard(string.Empty, cardNumber, string.Empty, string.Empty);
            // act
            var result = filter.isCardNumberValid(creditCard);
            // assert
            result.Should().Be(isExpectedToBeValid);

        }

        [Theory]
        [MemberData(nameof(CvcData))]
        public void isCvcValid_ShouldValidate(string cardNumber, string cvc, bool isExpectedToBeValid)
        {
            // arrange
            var creditCard = new CreditCard(string.Empty, cardNumber, string.Empty, cvc);
            // act
            var result = filter.isCvcValid(creditCard);
            // assert
            result.Should().Be(isExpectedToBeValid);
        }

        [Theory]
        [MemberData(nameof(ExpirationDateData))]
        public void isExpirationDateValid_ShouldValidate(string month, string year, bool isExpectedToBeValid)
        {
            // arrange
            var creditCard = new CreditCard(string.Empty, string.Empty, $"{month}/{year}", string.Empty);
            // act
            var result = filter.isExpirationDateValid(creditCard);
            // assert
            result.Should().Be(isExpectedToBeValid);
        }

        private ActionExecutingContext CreateExecutingContext(CreditCardValidationRequest cardValidationRequest)
        {
            var actionArgs = new Dictionary<string, object>()
            {
                {"creditCard",  cardValidationRequest}
            };
            var actionContext = new ActionContext(
                Mock<HttpContext>().Object,
                Mock<RouteData>().Object,
                Mock<ActionDescriptor>().Object,
                new ModelStateDictionary()//Mock<ModelStateDictionary>().Object
            );
            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                actionArgs,
                Mock<Controller>().Object
            );
        }

        public static IEnumerable<object[]> ValidationRequestsData =>
        new List<object[]>
        {
            // Visa
            new object[]
            {
                new CreditCardValidationRequest()
                {
                    OwnerName= "Cool Nerd",
                    Number= "4111111111111111",
                    Cvc="123",
                    ExpirationDate=$"01/{DateTime.Now.Year + 2}"
                },
                true
            },
            // Expired Card
            new object[]
            {
                new CreditCardValidationRequest()
                {
                    OwnerName= "Cool Nerd",
                    Number= "4111111111111111",
                    Cvc="123",
                    ExpirationDate=$"01/{DateTime.Now.Year - 3}"
                },
                false,
            },
        };
        public static IEnumerable<object[]> CardOwnerData =>
            new List<object[]>
            {
                new object[] {"Cool Nerd", true},
                new object[] {"Three worded name", true},
                new object[] {"oneName", true},
                new object[] {"one two three four", false},
                new object[] {"123", false},
                new object[] {"$pecial Character$", false}
            };
        public static IEnumerable<object[]> CardNumberData =>
            new List<object[]>
            {
                //visa
                new object[] { "4111111111111111", true},
                //amex
                new object[] { "377400111111115", true},
                // mastercard
                new object[] { "5431111111111111", true},
                // discover, which is not supported
                new object[] { "6011309900001248", false},
                // non digits
                new object[] { "nonsense", false},
            };
        public static IEnumerable<object[]> CvcData =>
            new List<object[]>
            {
                //visa
                new object[] { "4111111111111111", "123", true},
                //amex
                new object[] { "377400111111115", "1234", true },
                // mastercard
                new object[] { "5431111111111111", "123", true },
                // wrong cvc visa
                new object[] { "4111111111111111", "123", true},
                // wrong cvc amex
                new object[] { "377400111111115", "1234", true },
                // wrong cvc mastercard
                new object[] { "5431111111111111", "123", true },
                // unknwon, with an expected cvc
                new object[] { "6011309900001248", "123", true},
                // unknwon, with an expected cvc
                new object[] { "6011309900001248", "1234", true},
                // unknwon, with an unexpected cvc
                new object[] { "6011309900001248", "12345", false},
                // non digits
                new object[] { "5431111111111111", "asd", false},
            };
        public static IEnumerable<object[]> ExpirationDateData =>
            new List<object[]>
            {
                // not expired yet, 4 digits
                new object[] { "01", (DateTime.Now.Year + 1).ToString(), true},
                // not expired yet, 2 digits
                new object[] { "01", ((DateTime.Now.Year + 1) % 100).ToString(), true},
                // current month 4 digits
                new object[] { 
                    DateTime.Now.Month <= 9 ? $"0{DateTime.Now.Month}" : DateTime.Now.Month.ToString(),
                    (DateTime.Now.Year).ToString(), true},
                // current month 2 digits
                new object[] { DateTime.Now.Month <= 9 ? $"0{DateTime.Now.Month}" : DateTime.Now.Month.ToString(),
                    ((DateTime.Now.Year) % 100).ToString(), true},
                // expired, 4 digits
                new object[] { "01", (DateTime.Now.Year - 1).ToString(), false},
                // expired yet, 2 digits
                new object[] { "01", ((DateTime.Now.Year - 1) % 100).ToString(), false},
                //wrong formated month
                 new object[] { "1", (DateTime.Now.Year + 1).ToString(), false},
                 //wrong formated year, 3 digits
                 new object[] { "01", ((DateTime.Now.Year - 1) % 1000).ToString(), false},
            };
    }
}
