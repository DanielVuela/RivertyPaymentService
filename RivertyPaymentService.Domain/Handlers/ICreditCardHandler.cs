using RivertyPaymentService.Core.Enumerables;

namespace RivertyPaymentService.Core.Handlers
{
    public interface ICreditCardHandler
    {
        CardType GetCardType(string creditCardNumber);
    }
}
