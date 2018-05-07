using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Logic;
using GetFreeTablesTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckAvailabilityTest
{

    [TestClass]
    public class CheckAvailabilityTests
    {

        private GetFreeTablesMock VersionOne;

        [TestInitialize]
        public void Initialize()
        {
            Guest MockGuest = new Guest("Harry", "0612345678");
            List<Reservation> TestReservations = new List<Reservation>
            {
                new Reservation(1, new System.DateTime(2018,2,1,12,00,00), 2, MockGuest){ReservationID = 1},
                new Reservation(2, new System.DateTime(2018,2,1,16,00,00), 3, MockGuest){ReservationID = 2},
                new Reservation(3, new System.DateTime(2018,2,1,20,00,00), 3, MockGuest){ReservationID = 3},
                new Reservation(4, new System.DateTime(2018,2,1,10,00,00), 1, MockGuest){ReservationID = 4},
                new Reservation(5, new System.DateTime(2018,2,1,21,00,00), 2, MockGuest){ReservationID = 5}

            };

            List<Table> TestTables = new List<Table>
            {
                new Table(4, TableAreas.Fireplace){TableID = 1 },
                new Table(6, TableAreas.Fireplace){TableID = 2 },
                new Table(4, TableAreas.Fireplace){TableID = 3 },
                new Table(8, TableAreas.Fireplace){TableID = 4 },
                new Table(4, TableAreas.Fireplace){TableID = 5 }

            };

            List<ReservationTableCoupling> TestRTCs = new List<ReservationTableCoupling>
            {
                new ReservationTableCoupling(TestReservations[0],TestTables[0]),    // ResID 1 has tables 1 and
                new ReservationTableCoupling(TestReservations[0],TestTables[1]),    // 2  

                new ReservationTableCoupling(TestReservations[1],TestTables[1]),    // ResID 2 has tables 2 and
                new ReservationTableCoupling(TestReservations[1],TestTables[2]),    // 3

                new ReservationTableCoupling(TestReservations[2],TestTables[2]),    // ResID 3 has tables 3 and
                new ReservationTableCoupling(TestReservations[2],TestTables[3]),    // 4

                new ReservationTableCoupling(TestReservations[3],TestTables[0]),    // ResID 4 has tables 1 and
                new ReservationTableCoupling(TestReservations[3],TestTables[1]),    // 2 and
                new ReservationTableCoupling(TestReservations[3],TestTables[2]),    // 3 and
                new ReservationTableCoupling(TestReservations[3],TestTables[3]),    // 4 and
                new ReservationTableCoupling(TestReservations[3],TestTables[4]),    // 5 

                

                new ReservationTableCoupling(TestReservations[4],TestTables[4])     // ResID 5 has table  5
            };


            VersionOne = new GetFreeTablesMock(TestRTCs,TestReservations, TestTables);
        }



        [TestMethod]
        public void DoesTheMethodReturnTheCorrectTables1()
        {
            // Scenario 
            // Arrange


            DateTime CheckStart = new System.DateTime(2018, 2, 1, 21, 00, 00);
            DateTime CheckEnd = new System.DateTime(2018, 2, 1, 23, 00, 00);
           

            // Act

            List<Table> result = VersionOne.GetFreeTables(CheckStart, CheckEnd);


            // Assert
            // Between 21 and 23 tables 3, 4, and 5 are occupied. 
            // The returned list should be of length 2 with [0] = ID 1 and [1] is ID 2.

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].TableID);
            Assert.AreEqual(2, result[1].TableID);

            
        }

        [TestMethod]
        public void DoesTheMethodReturnTheCorrectTables2()
        {
            // Scenario 
            // Arrange


            DateTime CheckStart = new System.DateTime(2018, 2, 1, 13, 00, 00);
            DateTime CheckEnd = new System.DateTime(2018, 2, 1, 17, 00, 00);


            // Act

            List<Table> result = VersionOne.GetFreeTables(CheckStart, CheckEnd);


            // Assert
            // Between 13 and 17 tables 1, 2, and 3 are occupied. 
            // The returned list should be of length 2 with [0] = ID 4 and [1] is ID 5.

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(4, result[0].TableID);
            Assert.AreEqual(5, result[1].TableID);


        }
        [TestMethod]
        public void DoesTheMethodReturnTheCorrectTables3()
        {
            // Scenario 
            // Arrange


            DateTime CheckStart = new System.DateTime(2018, 2, 1, 19, 00, 00);
            DateTime CheckEnd = new System.DateTime(2018, 2, 1, 20, 00, 00);


            // Act

            List<Table> result = VersionOne.GetFreeTables(CheckStart, CheckEnd);


            // Assert
            // Between 19 and 20 no tables are occupied
            // The returned list should be of length 5 with [0] = ID 1 and [1] is ID 2.

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, result[0].TableID);
            Assert.AreEqual(2, result[1].TableID);
        }

        [TestMethod]
        public void DoesTheMethodReturnTheCorrectTables4()
        {
            // Scenario 
            // Arrange


            DateTime CheckStart = new System.DateTime(2018, 2, 1, 10, 00, 00);
            DateTime CheckEnd = new System.DateTime(2018, 2, 1, 13, 00, 00);


            // Act

            List<Table> result = VersionOne.GetFreeTables(CheckStart, CheckEnd);


            // Assert
            // Between 18 and 19 all tables are occupied
            // The returned list should be of length 0 

            Assert.AreEqual(0, result.Count);
            
        }






    }
}


