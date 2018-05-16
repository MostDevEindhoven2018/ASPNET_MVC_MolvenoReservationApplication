using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TableManagerTests
{

    [TestClass]
    public class CompleteProcessTests
    {
        private IFreeTableFinder _freeTableFinder;
        private TableManager _tableManager;
        Guest MockGuest;
        List<Reservation> TestReservations;
        List<Table> TestTables;
        List<ReservationTableCoupling> TestRTCs;

        [TestInitialize]
        public void Initialize()
        {
            MockGuest = new Guest("Harry", "0612345678");
            TestReservations = new List<Reservation>
            {
                new Reservation(6, new System.DateTime(2018,2,1,12,00,00), 2, MockGuest){ReservationID = 1},
                new Reservation(8, new System.DateTime(2018,2,1,16,00,00), 3, MockGuest){ReservationID = 2},
                new Reservation(8, new System.DateTime(2018,2,1,20,00,00), 3, MockGuest){ReservationID = 3},
                new Reservation(22, new System.DateTime(2018,2,1,11,00,00), 3, MockGuest){ReservationID = 4},
                new Reservation(2, new System.DateTime(2018,2,1,21,00,00), 2, MockGuest){ReservationID = 5},
                new Reservation(16, new System.DateTime(2018,2,1,14,00,00), 7, MockGuest){ReservationID = 6}
            };

            TestTables = new List<Table>
            {
                new Table(2, TableAreas.Fireplace){TableID = 1 },
                new Table(2, TableAreas.Fireplace){TableID = 2 },
                new Table(2, TableAreas.Fireplace){TableID = 3 },
                new Table(2, TableAreas.Fireplace){TableID = 4 },
                new Table(2, TableAreas.Fireplace){TableID = 5 },
                new Table(2, TableAreas.Fireplace){TableID = 6 },
                new Table(4, TableAreas.Fireplace){TableID = 7 },
                new Table(4, TableAreas.Fireplace){TableID = 8 },
                new Table(4, TableAreas.Fireplace){TableID = 9 },
                new Table(4, TableAreas.Fireplace){TableID = 10 },
                new Table(4, TableAreas.Fireplace){TableID = 11 },
                new Table(4, TableAreas.Fireplace){TableID = 12 },
                new Table(6, TableAreas.Fireplace){TableID = 13 },
                new Table(6, TableAreas.Fireplace){TableID = 14 },
                new Table(6, TableAreas.Fireplace){TableID = 15 },
                new Table(6, TableAreas.Fireplace){TableID = 16 },
                new Table(8, TableAreas.Fireplace){TableID = 17 },
                new Table(8, TableAreas.Fireplace){TableID = 18 },
                new Table(8, TableAreas.Fireplace){TableID = 19 }
            };

            TestRTCs = new List<ReservationTableCoupling>
            {
                new ReservationTableCoupling(TestReservations[0],TestTables[0]),    // ResID 1 has tables 1 and
                new ReservationTableCoupling(TestReservations[0],TestTables[10]),    // 11  

                new ReservationTableCoupling(TestReservations[1],TestTables[1]),    // ResID 2 has tables 2 and
                new ReservationTableCoupling(TestReservations[1],TestTables[12]),    // 13

                new ReservationTableCoupling(TestReservations[2],TestTables[2]),    // ResID 3 has tables 3 and
                new ReservationTableCoupling(TestReservations[2],TestTables[13]),    // 14

                new ReservationTableCoupling(TestReservations[3],TestTables[3]),    // ResID 4 has tables 4 and
                new ReservationTableCoupling(TestReservations[3],TestTables[1]),    // 2 and
                new ReservationTableCoupling(TestReservations[3],TestTables[12]),    // 13 and
                new ReservationTableCoupling(TestReservations[3],TestTables[13]),    // 14 and
                new ReservationTableCoupling(TestReservations[3],TestTables[14]),    // 15 

                new ReservationTableCoupling(TestReservations[4],TestTables[4]),    // ResID 5 has table  5

                new ReservationTableCoupling(TestReservations[5],TestTables[6]),    // ResID 5 has table  5
                new ReservationTableCoupling(TestReservations[5],TestTables[7]),     // ResID 5 has table  5
                new ReservationTableCoupling(TestReservations[5],TestTables[18])     // ResID 5 has table  5


            };

            // Now create a table manager which is almost like normal, but instead of a database uses this environment

            _freeTableFinder = new GetFreeTablesMock(TestRTCs, TestReservations, TestTables);
            _tableManager = new TableManager(_freeTableFinder);
        }
        
        
        [TestMethod]
        public void CompleteProcess1()
        {
            DateTime CheckStart = new System.DateTime(2018, 2, 1, 21, 00, 00);
            DateTime CheckEnd = new System.DateTime(2018, 2, 1, 23, 00, 00);

            List<Table> Result = _tableManager.GetOptimalTableConfig(CheckStart, CheckEnd, 12);
            List<Table> Expected = new List<Table> { TestTables[12], TestTables[14] };

            CollectionAssert.AreEqual(Expected, Result);
        }

        [TestMethod]
        public void CompleteProcess2()
        {
            DateTime CheckStart = new System.DateTime(2018, 2, 1, 21, 00, 00);
            DateTime CheckEnd = new System.DateTime(2018, 2, 1, 23, 00, 00);

            List<Table> Result = _tableManager.GetOptimalTableConfig(CheckStart, CheckEnd, 1);
            List<Table> Expected = new List<Table> { TestTables[0]};

            CollectionAssert.AreEqual(Expected, Result);
        }
    }
}