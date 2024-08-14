using OnlineRestaurantReservation.Models;
namespace OnlineRestaurantReservation;

// Class is going to be the instance of our reservation system 
public class ReservationsHandler
{
    private List<Table> _tables;
    private Queue<Reservation> _reservationsQueue;

    public ReservationsHandler(List<Table> restaurantTables)
    {
        // Load our restaurants Queue
        _tables = restaurantTables;
        _reservationsQueue = new Queue<Reservation>();
    }

    // Bool return type to verify successfull insertion
    // This will represent our accepted or rejected
    public string AddReservation(Reservation reservation)
    {
        // This is a boutique restaurant if we only have one table add
        if (_tables.Count == 1)
        {
            foreach (Table table in _tables)
            {
                int expectedSeatsOccupied = reservation.Quantatity + table.FetchCurrentlyOccupied();
                
                if (table.TableSize < reservation.Quantatity || expectedSeatsOccupied > table.TableSize)
                {
                    return ReservationStatus(false);
                }
                
                _reservationsQueue.Enqueue(reservation);
                table.UpdateCurrentlyOccupied(expectedSeatsOccupied);
            }
            
            return ReservationStatus(true);
        }
        
        // iterate through the tables given a reservation 
        // Todo: scan through all the tables first to find an index size that has the smallest amount of seats for the person 
        foreach (Table table in _tables)
        {
            //first check if the table has already been reserved, if not continue 
            // Todo: ensure we aren't over booking
            if (!(table.FetchCurrentlyOccupied() > 0))
            { 
                // Check if we can fit the party into this table
                if (table.TableSize / reservation.Quantatity >= 1)
                {
                    _reservationsQueue.Enqueue(reservation);
                    table.UpdateCurrentlyOccupied(reservation.Quantatity);
                    return ReservationStatus(true);
                }
                // If not table fits party size loop out and return rejected status
            }
        }
        
        return ReservationStatus(false);
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