using Microsoft.AspNetCore.Mvc;
using RivertyPaymentService.ActionFilters;
using RivertyPaymentService.Core.Handlers;
using RivertyPaymentService.Models;

namespace RivertyPaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardValidationController : ControllerBase
    {
        private readonly ICreditCardHandler creditCardService;
        public CreditCardValidationController(ICreditCardHandler creditCardService)
        {
            this.creditCardService = creditCardService;
        }   

        [HttpGet]
        [ServiceFilter(typeof(CreditCardValidatorFilter))]
        public IActionResult ValidateCreditCard([FromQuery] CreditCardValidationRequest creditCard)
        {
            var cardTypeResult = creditCardService.GetCardType(creditCard.Number);
            return Ok(cardTypeResult.ToString());
        }
    }
}
