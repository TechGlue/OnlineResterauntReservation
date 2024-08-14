namespace OnlineRestaurantReservation.Models;

public class Table
{
    public int TableSize { get; }
    private int _currentlyOccupied; 

    public Table(int tableSize)
    {
        TableSize = tableSize;
        _currentlyOccupied = 0;
    }

    public void UpdateCurrentlyOccupied(int updatedCurrentlyOccupied)
    {
        _currentlyOccupied = updatedCurrentlyOccupied;
    }

    public int FetchCurrentlyOccupied()
    {
        return _currentlyOccupied;
    }
}