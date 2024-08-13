using OnlineRestaurantReservation.Models;
namespace OnlineRestaurantReservation;

// Class is going to be the instance of our reservation system 
public class ReservationsHandler
{
        // We know that reservations are going to be in a first come first served bases so we have to use a queue to pop and add 
        private Queue<Reservation> reservationsQueue;
        
        public ReservationsHandler()
        {
            // Load our restaurants Queue
            reservationsQueue = new Queue<Reservation>();
        }

        // Bool return type to verify successfull insertion
        public bool AddReservation(Reservation reservation)
        {
            reservationsQueue.Enqueue(reservation);
            return true;
        }

        public int CurrentReservationCount()
        {
            return reservationsQueue.Count;
        }
}