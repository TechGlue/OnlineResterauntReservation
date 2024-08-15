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
            return AddReservationBoutiqueTable(reservation);
        }

        // Regular Restaurant Case Logic
        Table? bestPossibleTable = null;
        double bestPossibleDifference = double.MaxValue;

        // Search through tables that are currently empty
        bestPossibleTable = _tables.Where(x => x.FetchCurrentlyOccupied() <= 0)
            .Select(x => new { table = x, diff = x.TableSize / (double)reservation.Quantity })
            .Where(x => x.diff >= 1).MinBy(x => x.diff)
            ?.table;

        // Search and see if we can squeeze them at a later date 
        if (bestPossibleTable == null)
        {
            foreach (Reservation res in _reservationsQueue)
            {
                if (res.ReservedTable != null)
                {
                    double currentDifference = res.ReservedTable.TableSize / (double)reservation.Quantity;

                    if (currentDifference >= 1 && currentDifference < bestPossibleDifference &&
                        res.ReservationDate.AddHours(_seatingDuration) <= reservation.ReservationDate)
                    {
                        bestPossibleTable = res.ReservedTable;
                        bestPossibleDifference = currentDifference;
                    }
                }
            }
        }

        // Add the reservations 
        if (bestPossibleTable != null)
        {
            bestPossibleTable.UpdateCurrentlyOccupied(reservation.Quantity);
            reservation.ReservedTable = bestPossibleTable;
            _reservationsQueue.Enqueue(reservation);
            return ReservationStatus(true);
        }

        return ReservationStatus(false);
    }

    public string AddReservationBoutiqueTable(Reservation reservation)
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

    public int GetReservationSize()
    {
        return _reservationsQueue.Count;
    }

    private string ReservationStatus(bool reservationOutput)
        => reservationOutput switch
        {
            false => "Rejected",
            true => "Accepted"
        };
}