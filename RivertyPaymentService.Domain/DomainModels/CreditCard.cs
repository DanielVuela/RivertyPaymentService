using RivertyPaymentService.Core.Enumerables;
using RivertyPaymentService.Core.Utils;

namespace RivertyPaymentService.Core.DomainModels
{
    public class CreditCard
    {
        private CreditCard() { }
        public CreditCard(string ownerName, string number, string expirationDate, string cvc)
        {
            OwnerName = ownerName;
            Number = number;
            ExpirationDate = expirationDate;
            Cvc = cvc;
        }
        public string OwnerName { get; set; }
        public string Number { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvc { get; set; }
        public CardType getCreditCardType() => CardTypeUtils.GetCardType(Number);
    }
}
