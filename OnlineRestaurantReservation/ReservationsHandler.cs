using OnlineRestaurantReservation.Models;
namespace OnlineRestaurantReservation;

public class ReservationsHandler
{
    private List<Table> _tables;
    private Queue<Reservation> _reservationsQueue;
    private Double _seatingDuration;

    public ReservationsHandler(List<Table> restaurantTables, Double seatingDuration = 0)
    {
        // Load our restaurants Queue
        _tables = restaurantTables;
        _reservationsQueue = new Queue<Reservation>();
        _seatingDuration = seatingDuration;
    }

    public string AddReservation(Reservation reservation)
    {
        // If we only have one table we are a boutique restaurant fit everyone in where there are seats we have a single start time so time does not play a part in this case
        if (_tables.Count == 1)
        {
            foreach (Table table in _tables)
            {
                int expectedSeatsOccupied = reservation.Quantity + table.FetchCurrentlyOccupied();

                if (table.TableSize < reservation.Quantity || expectedSeatsOccupied > table.TableSize)
                {
                    return ReservationStatus(false);
                }

                _reservationsQueue.Enqueue(reservation);
                table.UpdateCurrentlyOccupied(expectedSeatsOccupied);
            }

            return ReservationStatus(true);
        }
        
        // Regular Restaurant Case Logic
        Table bestPossibleTable = null;
        double bestPossibleDifference = double.MaxValue;

        // Search through tables that are currently empty
        foreach (Table table in _tables)
        {
            if (!(table.FetchCurrentlyOccupied() > 0))
            {
                // Best possible table logic
                double currentDifference = table.TableSize / (double) reservation.Quantity;
                if (currentDifference >= 1)
                {
                    if (currentDifference < bestPossibleDifference)
                    {
                        bestPossibleTable = table;
                        bestPossibleDifference = currentDifference;
                    }
                }
            }
        }

        // Search and see if we can squeeze them at a later date 
        if (bestPossibleTable == null)
        {
            foreach (Reservation res in _reservationsQueue)
            {
                if (res.ReservedTable != null)
                {
                    double currentDifference = res.ReservedTable.TableSize / (double) reservation.Quantity;

                    if (currentDifference >= 1 && currentDifference < bestPossibleDifference && res.ReservationDate.AddHours(_seatingDuration) <= reservation.ReservationDate)
                    {
                        bestPossibleTable = res.ReservedTable;
                        bestPossibleDifference = currentDifference;
                    }
                }
            }
        }

        // Add the reservation 
        if (bestPossibleTable != null)
        {
            bestPossibleTable.UpdateCurrentlyOccupied(reservation.Quantity);
            reservation.ReservedTable = bestPossibleTable;
            _reservationsQueue.Enqueue(reservation);
            return ReservationStatus(true);
        }

        return ReservationStatus(false);
    }

    public int GetReservationSize()
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