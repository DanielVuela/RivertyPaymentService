using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Core.DomainModels;
using RivertyPaymentService.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RivertyPaymentService.ActionFilters
{
    public class CreditCardValidatorFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var cardValidationRequest = context.ActionArguments.SingleOrDefault(p => p.Value is CreditCardValidationRequest).Value as CreditCardValidationRequest;
            var creditCard = new CreditCard(cardValidationRequest.OwnerName, cardValidationRequest.Number,
                                            cardValidationRequest.ExpirationDate, cardValidationRequest.Cvc);
            Validate(context, creditCard, isCardNumberValid, nameof(creditCard.Number), creditCard.Number);
            Validate(context, creditCard, isOwnerNameValid, nameof(creditCard.OwnerName), creditCard.OwnerName);
            Validate(context, creditCard, isCvcValid, nameof(creditCard.Cvc), creditCard.Cvc);
            Validate(context, creditCard, isExpirationDateValid, nameof(creditCard.ExpirationDate), creditCard.ExpirationDate);
            if (!context.ModelState.IsValid) 
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void Validate(ActionExecutingContext context, CreditCard creditCard, Func<CreditCard, bool> isValid,
                             string fieldName, string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                context.ModelState.AddModelError(fieldName, $"{fieldName} is required.");
            }
            else if (!isValid(creditCard))
            {
                context.ModelState.AddModelError(fieldName, $"{fieldName} is not valid.");
            }
        }
        public bool isOwnerNameValid(CreditCard creditCard) => Regex.Match(creditCard.OwnerName, @"^((?:[A-Za-z]+ ?){1,3})$").Success;
        public bool isCvcValid(CreditCard creditCard)
        {
            var cvcStatategy = new Dictionary<CardType, Func<string, bool>>
            {
                { CardType.Visa, (string cvc) => Regex.Match(creditCard.Cvc, @"^[0-9]{3}$").Success  },
                { CardType.MasterCard, (string cvc) => Regex.Match(creditCard.Cvc, @"^[0-9]{3}$").Success },
                { CardType.AmericanExpress, (string cvc) => Regex.Match(creditCard.Cvc, @"^[0-9]{4}$").Success },
                { CardType.Unknown, (string cvc) => Regex.Match(creditCard.Cvc, @"^[0-9]{3,4}$").Success }
            };
            return cvcStatategy[creditCard.getCreditCardType()](creditCard.Cvc);
        }
        public bool isExpirationDateValid(CreditCard creditCard) => 
            isExpirationDateFormatValid(creditCard.ExpirationDate) && !IsCardExpired(creditCard.ExpirationDate);
        public bool isCardNumberValid(CreditCard creditCard) => creditCard.getCreditCardType() != CardType.Unknown;
        private bool IsCardExpired(string expirationDate)
        {
            var dateValues = expirationDate.Split("/");
            var dateFormat = dateValues[1].Length == 2 ? "MM/yy" : "MM/yyyy";
            int firstDayOfTheMonth = 1;
            var expirationDateTime = DateTime.ParseExact(expirationDate, dateFormat, CultureInfo.InvariantCulture);
            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, firstDayOfTheMonth);

            return currentMonth > expirationDateTime;
        }
        private bool isExpirationDateFormatValid(string expirationDate) 
            => Regex.Match(expirationDate, @"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$").Success;
    }
}
