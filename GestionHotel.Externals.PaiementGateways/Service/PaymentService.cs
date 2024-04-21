using GestionHotel.Externals.PaiementGateways.Paypal;
using GestionHotel.Externals.PaiementGateways.Stripe;

public class PaymentService
{
    public async Task<bool> ProcessPaypalPayment(string cardNumber, string expiryDate, string amount)
    {
        var paypalGateway = new PaypalGateway();
        var result = await paypalGateway.ProcessPaymentAsync(cardNumber, expiryDate, amount);
        return result.Status == PaypalResultStatus.Success;
    }

    public async Task<bool> ProcessStripePayment(string cardNumber, string expiryDate, string amount)
    {
        var stripeGateway = new StripeGateway();
        var paymentInfo = new StripePayementInfo
        {
            CardNumber = cardNumber,
            ExpiryDate = expiryDate,
            Amount = amount
        };
        return await stripeGateway.ProcessPaymentAsync(paymentInfo);
    }
}