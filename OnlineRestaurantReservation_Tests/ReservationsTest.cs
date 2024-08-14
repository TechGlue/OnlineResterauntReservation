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
        List<Table> tables = new List<Table> { new(tableSize) };
        ReservationsHandler myRestaurant = new ReservationsHandler(tables);
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
        ReservationsHandler myRestaurant = new ReservationsHandler(new List<Table>(){new Table(12)});
        ReservationsHandler myRestaurant13 = new ReservationsHandler(new List<Table>(){new Table(12)});
        ReservationsHandler myRestaurant12 = new ReservationsHandler(new List<Table>(){new Table(12)});
        
        Reservation res1 = new Reservation(DateTime.Now, 1);
        Reservation res13 = new Reservation(DateTime.Now, 13);
        Reservation res12 = new Reservation(DateTime.Now, 12);

        //Act 
        string acceptedReservation1 = myRestaurant.AddReservation(res1);
        string acceptedReservation12 = myRestaurant12.AddReservation(res12);
        string acceptedReservation13 = myRestaurant13.AddReservation(res13);

        //Assert
        Assert.AreEqual("Accepted", acceptedReservation1);
        Assert.AreEqual("Accepted", acceptedReservation12);
        Assert.AreEqual("Rejected", acceptedReservation13);
    }

    [TestMethod]
    public void MakeReservation_ExisitingReservation_CheckTablesCanAcceptCandidateRejected()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(4) };
        ReservationsHandler myRestaurant = new ReservationsHandler(tables);

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

        List<Table> tables = new List<Table>() { new Table(10) };
        ReservationsHandler myRestaurant = new ReservationsHandler(tables);

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
        List<Table> tables = new List<Table>() { new Table(10) };

        ReservationsHandler myRestaurant = new ReservationsHandler(tables);

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

    [TestMethod]
    public void MakeReservation_MultipleTables_CheckTablesRejected()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(2), new Table(2), new Table(4), new Table(4) };
        ReservationsHandler reservationsHandler = new ReservationsHandler(tables);
        Reservation candidateRes = new Reservation(DateTime.Parse("2023-09-14"), 5);

        //Act
        string candidateResStatus = reservationsHandler.AddReservation(candidateRes);

        //Assert
        Assert.AreEqual("Rejected", candidateResStatus);
    }

    [TestMethod]
    public void MakeReservation_MultipleTables_CheckTablesAccepted()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(2), new Table(2), new Table(4), new Table(4) };
        ReservationsHandler reservationsHandler = new ReservationsHandler(tables);
        Reservation candidateRes = new Reservation(DateTime.Parse("2023-09-14"), 4);

        //Act
        string candidateResStatus = reservationsHandler.AddReservation(candidateRes);

        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);
    }

    [TestMethod]
    public void MakeReservation_MultipleTablesAndReservations_CheckTablesAccepted()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(2), new Table(2), new Table(4), new Table(4) };
        ReservationsHandler reservationsHandler = new ReservationsHandler(tables);
        Reservation existingRes = new Reservation(DateTime.Parse("2024-06-07"), 2);
        Reservation newRes = new Reservation(DateTime.Parse("2024-06-07"), 4);

        //Act
        _ = reservationsHandler.AddReservation(existingRes);
        string candidateResStatus = reservationsHandler.AddReservation(newRes);

        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);
    }


    [TestMethod]
    public void MakeReservation_MultipleTablesAndReservations_CheckTablesRejected()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(2), new Table(2), new Table(4) };
        ReservationsHandler reservationsHandler = new ReservationsHandler(tables);
        Reservation existingRes = new Reservation(DateTime.Parse("2024-06-07"), 3);
        Reservation newRes = new Reservation(DateTime.Parse("2024-06-07"), 4);

        //Act
        string existingResStatus = reservationsHandler.AddReservation(existingRes);
        string candidateResStatus = reservationsHandler.AddReservation(newRes);

        //Assert
        Assert.AreEqual("Accepted", existingResStatus);
        Assert.AreEqual("Rejected", candidateResStatus);
    }

    [TestMethod]
    public void MakeReservation_MultipleTablesAndReservationsFullHouse_CheckTablesRejected()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(2), new Table(2), new Table(4) };
        ReservationsHandler reservationsHandler = new ReservationsHandler(tables);
        Reservation existingRes1 = new Reservation(DateTime.Parse("2024-06-07"), 3);
        Reservation existingRes2 = new Reservation(DateTime.Parse("2024-06-07"), 2);
        Reservation existingRes3 = new Reservation(DateTime.Parse("2024-06-07"), 2);

        Reservation candidateRes = new Reservation(DateTime.Parse("2024-06-07"), 1);

        //Act
        _ = reservationsHandler.AddReservation(existingRes1);
        _ = reservationsHandler.AddReservation(existingRes2);
        _ = reservationsHandler.AddReservation(existingRes3);
        
        string candidateResStatus = reservationsHandler.AddReservation(candidateRes);

        //Assert
        Assert.AreEqual("Rejected", candidateResStatus);
    }
    
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void MakeReservation_ExisitingReservations_TwoTableExceptionForInvalidMemebers()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(10), new Table(2)};

        ReservationsHandler myRestaurant = new ReservationsHandler(tables);

        Reservation res = new Reservation(DateTime.Parse("2023-09-14"), 3);
        Reservation candidateRes = new Reservation(DateTime.Parse("2023-09-14"), -1);

        //Act
        _ = myRestaurant.AddReservation(res);

        string candidateResStatus = myRestaurant.AddReservation(candidateRes);

        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);
    }
}