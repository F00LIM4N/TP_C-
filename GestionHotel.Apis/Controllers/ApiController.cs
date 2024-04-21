using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GestionHotel.Externals.PaiementGateways;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;
    private readonly PaymentService _paymentService;

    public ReservationsController(ReservationService reservationService, PaymentService paymentService)
    {
        _reservationService = reservationService;
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> PostReservationWithPayment(Reservation reservation, string paymentGateway, string cardNumber, string expiryDate)
    {
        bool paymentSuccess = false;
        if (paymentGateway == "Paypal")
        {
            paymentSuccess = await _paymentService.ProcessPaypalPayment(cardNumber, expiryDate, reservation.TotalAmount.ToString());
        }
        else if (paymentGateway == "Stripe")
        {
            paymentSuccess = await _paymentService.ProcessStripePayment(cardNumber, expiryDate, reservation.TotalAmount.ToString());
        }
        else
        {
            return BadRequest("Mode de paiement non valide.");
        }

        if (!paymentSuccess)
        {
            return BadRequest("Le paiement a échoué. Impossible de créer la réservation.");
        }

        await _reservationService.AddReservation(reservation);
        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutReservation(int id, Reservation reservation)
    {
        var updatedReservation = await _reservationService.UpdateReservation(id, reservation);
        if (updatedReservation == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var deleted = await _reservationService.DeleteReservation(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
