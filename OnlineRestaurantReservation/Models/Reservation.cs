namespace OnlineRestaurantReservation.Models;

public class Reservation
{
    public Guid id;
    public DateTime ReservationDate { get; }
    //Quantatiy cannot be negative
    public int Quantatity { get; }

    public Reservation(DateTime reservationDate, int quantatity)
    {
        if (quantatity <= 0 )
        {
            throw new Exception("Can't have less than 1 member in a a reservation");
        }

        id = new Guid();
        ReservationDate = reservationDate;
        Quantatity = quantatity;
    }
}