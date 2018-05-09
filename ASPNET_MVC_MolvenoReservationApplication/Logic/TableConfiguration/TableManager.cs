using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Controllers;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.Sqlite;




namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{

    public class TableManager
    {
        DbContext _context { get; set; }
        IFreeTableFinder _freeTableFinder { get; set; }
        ITableConfigurationFinder _tableConfigurationFinder { get; set; }

        public TableManager(DbContext context, IFreeTableFinder ftf, ITableConfigurationFinder tcf)
        {
            _context = context;
            _freeTableFinder = ftf;
            _tableConfigurationFinder = tcf;
        }
       
        
    }
}


// Earlier attempt. Just.. lol

//// Connect to database that was opened by the ReservationController
//// Make a new object of type DBContext to use for this class
//private MyDBContext _DbContext;

//// Make a constructor of the CheckTableAvailability with as parameter the DBContext(here can you input the context that was opnened in the ReservationController)
//public CheckTableAvailability(MyDBContext dbContext)
//{
//    this._DbContext = dbContext;
//}

//public List<Table> GetAvailableTables(DateTime start, DateTime end, int partySize)
//{
//    List<Reservation> ReservationsInOurTimeSlot = new List<Reservation>();
//    List<Table> ReservedTables = new List<Table>();             //  Step 1

//    List<Table> AllTables = new List<Table>();                  //  Step 2
//    List<Table> FreeTables = new List<Table>();                 //  Step 3
//    List<Table> FreeTablesWithCorrectSize = new List<Table>();  //  Step 4

//    // Step 1
//    // First get all reservation in our time slot by using these four rules.

//    ReservationsInOurTimeSlot = _DbContext.Reservations.Where(reservation =>

//    //Rule 1:
//    // If the start time is between the two times... WRONG!
//    (reservation._resArrivingTime > start && reservation._resArrivingTime < end)    ||
//    // If the end time is between the two times... WRONG!
//    (reservation._resLeavingTime > start && reservation._resLeavingTime < end)      ||
//    // If the start time is before the first time and the end time is after the second time... WRONG!
//    (reservation._resArrivingTime < start && reservation._resLeavingTime > end)     ||
//    // If both the start and end times are exactly the same... WRONG!
//    (reservation._resArrivingTime == start && reservation._resLeavingTime == end)

//    ).Include("_resTable").ToList();

//    ReservedTables = ReservationsInOurTimeSlot.Select(reservation => reservation._resTable).ToList();

//    ////////// Is it also possible to exclude these two conditions instead of including the four rules above?
//    ////////// Step 1
//    ////////// First get database/excisting reservations that overlap with the current/new reservation by excluding the database reservations that do not overlap with the current reservation
//    ////////ReservationsInOurTimeSlot = _DbContext.Reservations.Where(reservation =>
//    ////////// Exclude if one of the two conditions is true
//    ////////!(
//    //////////Condition 1:
//    ////////// No overlap of database/excisting reservation on right side of current/new reservation
//    ////////(reservation._resArrivingTime < start && reservation._resLeavingTime <= start) ||   // SAME (start > reservation._resArrivingTime && start >= reservation._resLeavingTime) ||
//    ////////// Condition 2:
//    ////////// No overlap of database/excisting reservation on left side of current/new reservation
//    ////////(reservation._resArrivingTime >= end && reservation._resLeavingTime > end)          // SAME (end <= reservation._resArrivingTime && end < reservation._resLeavingTime)

//    ////////)).ToList();

//    ////////ReservedTables = ReservationsInOurTimeSlot.Select(reservation => reservation._resTable).ToList();


//    // Step 2

//    AllTables = _DbContext.Tables.Select(table => table).ToList();


//    // Step 3
//    FreeTables = AllTables.Where(TableFromAllTables => 

//    !ReservedTables.Any(TableFromReservedTables => 
//    TableFromReservedTables.TableID == TableFromAllTables.TableID)

//    ).ToList();

//    // The Linq Above is basically the same as this:

//    //foreach(Table TableFromAllTables in AllTables)
//    //{
//    //    foreach(Table TableFromReservedTables in ReservedTables)
//    //    {
//    //        if(!(TableFromReservedTables.TableID == TableFromAllTables.TableID))
//    //        {
//    //            FreeTables.Add(TableFromAllTables);
//    //        }
//    //    }
//    //}

//    // Step 4

//    FreeTablesWithCorrectSize = FreeTables.Where(table => table._tableCapacity >= partySize).ToList();

//    // Step 5 

//    FreeTablesWithCorrectSize.OrderBy(table => table._tableCapacity).ToList();



//    // Happy days
//    return FreeTablesWithCorrectSize;
//     }