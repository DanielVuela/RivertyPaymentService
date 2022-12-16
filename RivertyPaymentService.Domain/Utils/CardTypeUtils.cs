using RivertyPaymentService.Core.Enumerables;
using System.Text.RegularExpressions;

namespace RivertyPaymentService.Core.Utils
{
    public static class CardTypeUtils
    {
        public static CardType GetCardType(string cardNumber)
        {
            if (IsVisa(cardNumber)) return CardType.Visa;
            if (IsMasterCard(cardNumber)) return CardType.MasterCard;
            if (IsAmericanExpress(cardNumber)) return CardType.AmericanExpress;
            return CardType.Unknown;
        }

        private static bool IsVisa(string creditCardNumber) =>
            Regex.Match(creditCardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$").Success;
        private static bool IsMasterCard(string creditCardNumber) =>
            Regex.Match(creditCardNumber, @"^5[1-5][0-9]{14}|^(222[1-9]|22[3-9]\\d|2[3-6]\\d{2}|27[0-1]\\d|2720)[0-9]{12}$").Success;
        private static bool IsAmericanExpress(string creditCardNumber) =>
            Regex.Match(creditCardNumber, @"^3[47]\d{1,2}(| |-)\d{6}\1\d{6}$").Success;
    }
}
