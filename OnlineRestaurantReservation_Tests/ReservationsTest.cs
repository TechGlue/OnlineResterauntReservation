using OnlineRestaurantReservation;
using OnlineRestaurantReservation.Models;

namespace OnlineRestaurantReservation_Tests;

[TestClass]
public class ReservationsTest
{
    [TestMethod]
    [DataRow("2050-08-08 19:30:00", 3, 4, "Accepted")] // Accepted res
    [DataRow("2022-11-27 18:45:00", 4, 2, "Rejected")] // Rejected res
    [DataRow("2014-02-27 13:22:00", 12, 10, "Rejected")] // Rejected res
    public void MakeReservation_EmptyQueue_CheckTableSize(string reservationTime, int quantity, int tableSize,
        string expectedStatus)
    {
        //Arrange
        ReservationsHandler myRestaurant = new ReservationsHandler(tableSize);
        Reservation newReservation = new Reservation(DateTime.Parse(reservationTime), quantity);

        //Act 
        string acceptedReservation = myRestaurant.AddReservation(newReservation);

        //Assert
        Assert.AreEqual(expectedStatus, acceptedReservation);
    }

    [TestMethod]
    public void MakeReservation_NoReservations_CheckTables()
    {
        //Arrange
        ReservationsHandler myRestaurant = new ReservationsHandler(12);
        ReservationsHandler myRestaurant13 = new ReservationsHandler(12);
        ReservationsHandler myRestaurant12 = new ReservationsHandler(12);
        Reservation res1 = new Reservation(DateTime.Now, 1);
        Reservation res13 = new Reservation(DateTime.Now, 13);
        Reservation res12 = new Reservation(DateTime.Now, 12);

        //Act 
        string acceptedReservation1 = myRestaurant.AddReservation(res1);
        string acceptedReservation13 = myRestaurant13.AddReservation(res13);
        string acceptedReservation12 = myRestaurant12.AddReservation(res12);

        //Assert
        Assert.AreEqual("Accepted", acceptedReservation1);
        Assert.AreEqual("Rejected", acceptedReservation13);
        Assert.AreEqual("Accepted", acceptedReservation12);
    }

    [TestMethod]
    public void MakeReservation_ExisitingReservation_CheckTablesCanAcceptCandidateRejected()
    {
        //Arrange
        ReservationsHandler myRestaurant = new ReservationsHandler(4);
        
        Reservation res2 = new Reservation(DateTime.Parse("2023-09-14"), 2);
        Reservation res3 = new Reservation(DateTime.Parse("2023-09-14"), 3);
        
        //Act
        _ = myRestaurant.AddReservation(res2);
        string res3Reservation = myRestaurant.AddReservation(res3);

        //Assert
        Assert.AreEqual("Rejected", res3Reservation);
    }
    
    [TestMethod]
    public void MakeReservation_ExisitingReservation_CheckTablesCanAcceptCandidateAccepted()
    {
        //Arrange
        ReservationsHandler myRestaurant = new ReservationsHandler(10);
        
        Reservation res2 = new Reservation(DateTime.Parse("2023-09-14"), 2);
        Reservation res3 = new Reservation(DateTime.Parse("2023-09-14"), 3);
        
        //Act
        _ = myRestaurant.AddReservation(res2);
        string res3Reservation = myRestaurant.AddReservation(res3);

        //Assert
        Assert.AreEqual("Accepted", res3Reservation);
    }
    
    [TestMethod]
    public void MakeReservation_ThreeExisitingReservations_CheckTablesCanAcceptCandidate()
    {
        //Arrange
        ReservationsHandler myRestaurant = new ReservationsHandler(10);
        
        
        Reservation res = new Reservation(DateTime.Parse("2023-09-14"), 3);
        Reservation res2 = new Reservation(DateTime.Parse("2023-09-14"), 2);
        Reservation res3 = new Reservation(DateTime.Parse("2023-09-14"), 3);
        
        Reservation candidateRes = new Reservation(DateTime.Parse("2023-09-14"), 3);
        
        //Act
        _ = myRestaurant.AddReservation(res);
        _ = myRestaurant.AddReservation(res2);
        _ = myRestaurant.AddReservation(res3);
        
        string candidateResStatus = myRestaurant.AddReservation(candidateRes);

        //Assert
        Assert.AreEqual("Rejected", candidateResStatus);
    }
}