namespace RivertyPaymentService.Models
{
    public class CreditCardValidationRequest
    {
        public string OwnerName { get; set; }
        public string Number { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvc { get; set; }
    }
}
