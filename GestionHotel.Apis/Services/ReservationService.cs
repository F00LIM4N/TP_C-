public class ReservationService
{
    private readonly ApplicationDbContext _context;

    public ReservationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation> GetReservationById(int id)
    {
        return await _context.Reservations.FindAsync(id);
    }

    public async Task<Reservation> AddReservation(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task<Reservation> UpdateReservation(int id, Reservation reservation)
    {
        if (id != reservation.Id)
        {
            throw new ArgumentException("L'ID de la réservation ne correspond pas à l'ID spécifié");
        }

        _context.Entry(reservation).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReservationExists(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return reservation;
    }

    public async Task<bool> DeleteReservation(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return false;
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }

    private bool ReservationExists(int id)
    {
        return _context.Reservations.Any(e => e.Id == id);
    }
}