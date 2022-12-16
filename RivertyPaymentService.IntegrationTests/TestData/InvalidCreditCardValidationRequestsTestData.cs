using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RivertyPaymentService.IntegrationTests.TestData
{
    public class InvalidCreditCardValidationRequestsTestData : IEnumerable<object[]>
    {         
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new CreditCardValidationRequest
                {
                    Number = "123",
                    OwnerName = "Cool Nerd",
                    Cvc="123",
                    ExpirationDate=$"01/{ DateTime.Now.Year + 1 }",
                },
                new List<string> { "Number" },
            };
            yield return new object[]
            {
                new CreditCardValidationRequest
                {
                    Number = "4012888888881881",
                    OwnerName = "123",
                    Cvc="123",
                    ExpirationDate=$"01/{ DateTime.Now.Year + 1 }"
                },
                new List<string> { "OwnerName" },
            };
            yield return new object[]
            {
                new CreditCardValidationRequest
                {
                    Number = "378282246310005",
                    OwnerName = "Cool Nerd",
                    Cvc="12",
                    ExpirationDate=$"01/{ DateTime.Now.Year + 1 }"
                },
                new List<string> { "Cvc" },
            };
            yield return new object[]
            {
                new CreditCardValidationRequest
                {
                    Number = "378282246310005",
                    OwnerName = "Cool Nerd",
                    Cvc="1234",
                    ExpirationDate=$"01/{ DateTime.Now.Year - 1 }"
                },
                new List<string> { "ExpirationDate" },
            };
            // Multiple errors
            yield return new object[]
            {
                new CreditCardValidationRequest
                {
                    Number = "30569309025904",//diners club number
                    OwnerName = "Coolest Nerd On The Earth ever",
                    Cvc="12",
                    ExpirationDate=$"01/{ DateTime.Now.Year - 1 }"
                },
                new List<string> { "ExpirationDate",  "Cvc", "OwnerName", "Number"},
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
