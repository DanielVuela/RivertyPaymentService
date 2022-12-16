using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RivertyPaymentService.IntegrationTests.TestData
{
    public class CreditCardValidationRequestsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {

            yield return new object[]
            {
                new CreditCardValidationRequest
                {
                    Number = "5555555555554444",
                    OwnerName = "Cool Nerd",
                    Cvc="123",
                    ExpirationDate=$"01/{ DateTime.Now.Year + 1 }",
                },
                CardType.MasterCard,
            };
            yield return new object[]
            {
                new CreditCardValidationRequest
                {
                    Number = "4012888888881881",
                    OwnerName = "Cool Nerd",
                    Cvc="123",
                    ExpirationDate=$"01/{ DateTime.Now.Year + 1 }"
                },
                CardType.Visa,
            };
            yield return new object[]
            {
                new CreditCardValidationRequest
                {
                    Number = "378282246310005",
                    OwnerName = "Cool Nerd",
                    Cvc="1234",
                    ExpirationDate=$"01/{ DateTime.Now.Year + 1 }"
                },
                CardType.AmericanExpress,
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
