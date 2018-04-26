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
        
    public class CheckTableAvailability
    {        
        // Connect to database that was opened by the ReservationController
        // Make a new object of type DBContext to use for this class
        public MyDBContext _DbContext;

        // Make a constructor of the CheckTableAvailability with as parameter the DBContext(here can you input the context that was opnened in the ReservationController)
        public CheckTableAvailability(MyDBContext dbContext)
        {
            _DbContext = dbContext;
        }
        
        public List<Table> GetAvailableTables(DateTime start, DateTime end, int partySize)
        {
            List<Reservation> ReservationsInOurTimeSlot = new List<Reservation>();
            List<Table> ReservedTables = new List<Table>();             //  Step 1

            List<Table> AllTables = new List<Table>();                  //  Step 2
            List<Table> FreeTables = new List<Table>();                 //  Step 3
            List<Table> FreeTablesWithCorrectSize = new List<Table>();  //  Step 4

            // Step 1
            // First get all reservation in our time slot by using these four rules.

            ReservationsInOurTimeSlot = _DbContext.Reservations.Where(reservation =>

            //Rule 1:
            // If the start time is between the two times... WRONG!
            (reservation._resArrivingTime > start && reservation._resArrivingTime < end)    ||
            // If the end time is between the two times... WRONG!
            (reservation._resLeavingTime > start && reservation._resLeavingTime < end)      ||
            // If the start time is before the first time and the end time is after the second time... WRONG!
            (reservation._resArrivingTime < start && reservation._resLeavingTime > end)     ||
            // If both the start and end times are exactly the same... WRONG!
            (reservation._resArrivingTime == start && reservation._resLeavingTime == end)

            ).ToList();

            ReservedTables = ReservationsInOurTimeSlot.Select(reservation => reservation._resTable).ToList();


            // Step 2

            AllTables = _DbContext.Tables.Select(table => table).ToList();


            // Step 3
            FreeTables = AllTables.Where(TableFromAllTables => 

            !ReservedTables.Any(TableFromReservedTables => 
            TableFromReservedTables.TableID == TableFromAllTables.TableID)
            
            ).ToList();

            // The Linq Above is basically the same as this:

            //foreach(Table TableFromAllTables in AllTables)
            //{
            //    foreach(Table TableFromReservedTables in ReservedTables)
            //    {
            //        if(!(TableFromReservedTables.TableID == TableFromAllTables.TableID))
            //        {
            //            FreeTables.Add(TableFromAllTables);
            //        }
            //    }
            //}

            // Step 4

            FreeTablesWithCorrectSize = FreeTables.Where(table => table._tableCapacity >= partySize).ToList();

            // Step 5 

            FreeTablesWithCorrectSize.OrderBy(table => table._tableCapacity).ToList();



            // Happy days
            return FreeTablesWithCorrectSize;
           
        }
    }
}
