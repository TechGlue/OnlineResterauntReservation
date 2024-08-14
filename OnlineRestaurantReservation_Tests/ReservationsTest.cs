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
        ReservationsHandler myRestaurant = new ReservationsHandler(new List<Table>() { new Table(12) });
        ReservationsHandler myRestaurant13 = new ReservationsHandler(new List<Table>() { new Table(12) });
        ReservationsHandler myRestaurant12 = new ReservationsHandler(new List<Table>() { new Table(12) });

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
    [ExpectedException(typeof(Exception))]
    public void MakeReservation_ExisitingReservations_TwoTableExceptionForInvalidMemebers()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(10), new Table(2) };

        ReservationsHandler myRestaurant = new ReservationsHandler(tables);

        Reservation res = new Reservation(DateTime.Parse("2023-09-14"), 3);
        Reservation candidateRes = new Reservation(DateTime.Parse("2023-09-14"), -1);

        //Act
        _ = myRestaurant.AddReservation(res);

        string candidateResStatus = myRestaurant.AddReservation(candidateRes);

        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);
    }


    [TestMethod]
    public void MakeReservation_NoReservations_PrioritizeSmallerMembersToSmallerTables()
    {
        //Arrange
        List<Table> tables = new List<Table>() { new Table(10), new Table(2) };
        ReservationsHandler myRestaurant = new ReservationsHandler(tables);
        Reservation res = new Reservation(DateTime.Parse("2023-09-14"), 1);

        //Act
        string candidateResStatus = myRestaurant.AddReservation(res);
        Table? smallestTableOccupied = tables.Find(x => x.TableSize == 2);

        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);

        if (smallestTableOccupied != null)
            Assert.AreEqual(1, smallestTableOccupied.FetchCurrentlyOccupied());
        else
            Assert.Fail();
    }
    
    [TestMethod]
    public void MakeReservation_NoReservations_PrioritizeLargeMemberSizesToLargeTablesAccepted()
    {
        //Arrange
        List<Table> tables = new List<Table> { new Table(2), new Table(2), new Table(4), new Table(1), new Table(10)};
        ReservationsHandler myRestaurant = new ReservationsHandler(tables);
        Reservation res1 = new Reservation(DateTime.Parse("2023-09-14"), 1);
        Reservation res2 = new Reservation(DateTime.Parse("2023-09-14"), 4);
        Reservation res3 = new Reservation(DateTime.Parse("2023-09-14"), 2);
        Reservation res4 = new Reservation(DateTime.Parse("2023-09-14"), 2);
        Reservation res5 = new Reservation(DateTime.Parse("2023-09-14"), 10);

        //Act
        _ = myRestaurant.AddReservation(res1);
        _ = myRestaurant.AddReservation(res2);
        _ = myRestaurant.AddReservation(res3);
        _ = myRestaurant.AddReservation(res4);
        
        string candidateResStatus = myRestaurant.AddReservation(res5);
        Table? largestTableOccupied = tables.Find(x => x.TableSize == 10);
        Table? smallestTableOccupied = tables.Find(x => x.TableSize == 1);

        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);

        // Assert largest table has the ten members
        if (largestTableOccupied != null)
            Assert.AreEqual(10, largestTableOccupied.FetchCurrentlyOccupied());
        else
            Assert.Fail();
        
        // Assert the smallest table has the party with 1 member
        if (smallestTableOccupied != null)
            Assert.AreEqual(1, smallestTableOccupied.FetchCurrentlyOccupied());
        else
            Assert.Fail();
    }
    
    [TestMethod]
    public void MakeReservation_TwoReservations_Accepted()
    {
        //Arrange
        List<Table> tables = new List<Table> {new Table(2), new Table(2), new Table(4)};
        ReservationsHandler myRestaurant = new ReservationsHandler(tables, 2);
        Reservation res1 = new Reservation(DateTime.Parse("2023-09-14 18:00:00"), 4);
        Reservation res2 = new Reservation(DateTime.Parse("2023-09-14 20:00:00"), 3);

        //Act
        _ = myRestaurant.AddReservation(res1);
        string candidateResStatus= myRestaurant.AddReservation(res2);
        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);
    }
    
    [TestMethod]
    public void MakeReservation_PrioritizeExisitingTablesAndTimes_Rejected()
    {
        //Arrange
        List<Table> tables = new List<Table> {new Table(2), new Table(4), new Table(4)};
        ReservationsHandler myRestaurant = new ReservationsHandler(tables, 2.5);
        Reservation res1 = new Reservation(DateTime.Parse("2023-10-22 18:00:00"), 2);
        Reservation res2 = new Reservation(DateTime.Parse("2023-10-22 18:15:00"), 1);
        Reservation res3 = new Reservation(DateTime.Parse("2023-10-22 17:45:00"), 2);
        Reservation candidateReservation = new Reservation(DateTime.Parse("2023-10-22 20:00:00"), 3);

        //Act
        _ = myRestaurant.AddReservation(res1);
        _ = myRestaurant.AddReservation(res2);
        _ = myRestaurant.AddReservation(res3);
        string candidateResStatus= myRestaurant.AddReservation(candidateReservation);
        
        //Assert
        Assert.AreEqual("Rejected", candidateResStatus);
        Assert.AreEqual(3, myRestaurant.GetReservationSize());
    }
    
    [TestMethod]
    public void MakeReservation_PrioritizeExisitingTablesAndTimes_Accepted()
    {
        //Arrange
        List<Table> tables = new List<Table> {new Table(2), new Table(4), new Table(4)};
        ReservationsHandler myRestaurant = new ReservationsHandler(tables, 2.5);
        Reservation res1 = new Reservation(DateTime.Parse("2023-10-22 18:00:00"), 2);
        Reservation res2 = new Reservation(DateTime.Parse("2023-10-22 17:45:00"), 2);
        Reservation candidateReservation  = new Reservation(DateTime.Parse("2023-10-22 20:45:00"), 3);

        //Act
        _ = myRestaurant.AddReservation(res1);
        _ = myRestaurant.AddReservation(res2);
        string candidateResStatus= myRestaurant.AddReservation(candidateReservation);
        
        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);
        Assert.AreEqual(3, myRestaurant.GetReservationSize());
    }
    
    [TestMethod]
    public void MakeReservation_PrioritizeExisitingTablesAndTimes_Rejected2()
    {
        //Arrange
        List<Table> tables = new List<Table> {new Table(2), new Table(4), new Table(4)};
        ReservationsHandler myRestaurant = new ReservationsHandler(tables, 2.5);
        Reservation res1 = new Reservation(DateTime.Parse("2023-10-22 18:00:00"), 2);
        Reservation res2 = new Reservation(DateTime.Parse("2023-10-22 18:15:00"), 1);
        Reservation res3 = new Reservation(DateTime.Parse("2023-10-22 17:45:00"), 2);
        Reservation candidateReservation  = new Reservation(DateTime.Parse("2023-10-22 20:45:00"), 3);

        //Act
        _ = myRestaurant.AddReservation(res1);
        _ = myRestaurant.AddReservation(res2);
        _ = myRestaurant.AddReservation(res3);
        string candidateResStatus = myRestaurant.AddReservation(candidateReservation);
        
        //Assert
        Assert.AreEqual("Accepted", candidateResStatus);
        Assert.AreEqual(4, myRestaurant.GetReservationSize());
    }
}