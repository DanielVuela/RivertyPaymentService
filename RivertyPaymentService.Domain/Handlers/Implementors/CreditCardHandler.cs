using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Core.Utils;

namespace RivertyPaymentService.Core.Handlers.Implementors
{
    public class CreditCardHandler : ICreditCardHandler
    {
        public CardType GetCardType(string creditCardNumber) => CardTypeUtils.GetCardType(creditCardNumber);
    }
}
