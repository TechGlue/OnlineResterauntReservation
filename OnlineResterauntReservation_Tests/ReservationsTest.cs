using OnlineRestaurantReservation;
using OnlineRestaurantReservation.Models;

namespace OnlineResterauntReservation_Tests;

[TestClass]
public class ReservationsTest
{
    [TestMethod]
    [DataRow("2050-08-08 19:30:00", 3)]
    [DataRow("2022-11-27 18:45:00", 4)]
    [DataRow("2014-02-27 13:22:00", 12)]
    public void MakeReservation_EmptyHouse(string reservationTime, int quantatity)
    {
        //Arrange
        ReservationsHandler myRestaurant = new ReservationsHandler();
        Reservation newReservation = new Reservation(DateTime.Parse(reservationTime), quantatity);
        
        //Act 
        bool acceptedReservation = myRestaurant.AddReservation(newReservation);

        //Assert
        Assert.IsTrue(acceptedReservation);
        Assert.AreEqual(myRestaurant.CurrentReservationCount(), 1);
    }
}