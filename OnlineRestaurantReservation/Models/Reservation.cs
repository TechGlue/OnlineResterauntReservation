namespace OnlineRestaurantReservation.Models;

public class Reservation
{
    public DateTime ReservationDate { get; }
    public int Quantity { get; }
    
    public Table? ReservedTable { get; set; }

    public Reservation(DateTime reservationDate, int quantity)
    {
        if (quantity <= 0 )
        {
            throw new Exception("Can't have less than 1 member in a a reservation");
        }

        ReservationDate = reservationDate;
        Quantity = quantity;
        ReservedTable = null;
    }
}