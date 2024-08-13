using OnlineRestaurantReservation.Models;

namespace OnlineRestaurantReservation;

// Class is going to be the instance of our reservation system 
public class ReservationsHandler
{
    // We know that reservations are going to be in a first come first served bases so we have to use a queue to pop and add 
    public int TableSize { get; }
    private int SeatsOccupied { get; set; }
    private Queue<Reservation> _reservationsQueue;
    //Later add a private date field for what day the reservation handler is for... 

    public ReservationsHandler(int tableSize)
    {
        // Load our restaurants Queue
        TableSize = tableSize;
        SeatsOccupied = 0;
        _reservationsQueue = new Queue<Reservation>();
    }

    // Bool return type to verify successfull insertion
    // This will represent our accepted or rejected
    public string AddReservation(Reservation reservation)
    {
        int expectedSeatsOccupied = reservation.Quantatity + SeatsOccupied;

        if (TableSize < reservation.Quantatity || expectedSeatsOccupied > TableSize)
        {
            return ReservationStatus(false);
        }

        _reservationsQueue.Enqueue(reservation);
        SeatsOccupied += reservation.Quantatity;

        return ReservationStatus(true);
    }

    public string UpdateReservation(Reservation reservation)
    {
        // If no existing table size 
        return ReservationStatus(true);
    }

    public int CurrentReservationCount()
    {
        return _reservationsQueue.Count;
    }

    public string ReservationStatus(bool reservationOutput)
    {
        if (reservationOutput == false)
        {
            return "Rejected";
        }

        return "Accepted";
    }
}