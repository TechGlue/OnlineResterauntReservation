namespace OnlineRestaurantReservation.Models;

public class Table
{
    public int TableSize { get; }
    private int CurrentlyOccupied; 

    public Table(int tableSize)
    {
        TableSize = tableSize;
        CurrentlyOccupied = 0;
    }

    public void UpdateCurrentlyOccupied(int updatedCurrentlyOccupied)
    {
        CurrentlyOccupied = updatedCurrentlyOccupied;
    }

    public int FetchCurrentlyOccupied()
    {
        return CurrentlyOccupied;
    }
}