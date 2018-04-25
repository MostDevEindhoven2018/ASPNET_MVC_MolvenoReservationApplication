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
        private MyDBContext _dbContext;
        private Reservation _reservation;

        public CheckTableAvailability(MyDBContext context, Reservation reservation)
        {
            _dbContext = context;
            _reservation = reservation;
        }               

        public bool CheckPartySize()
        {
            return false;
        }

        // Make lists
        // listReservationDate: list with existing reservations that have the same reservation date as the current reservation
        // listReservationDateTime: list with existing reservations that have the same reservation date and time as the current reservation
        List<Reservation> listReservationDate = new List<Reservation>();
        List<Reservation> listReservationDateTime = new List<Reservation>();
        List<Table> listTables = new List<Table>();
        List<Table> listFreeTables = new List<Table>();
        List<Table> listFreeTablesNumberOfGuest = new List<Table>();

        /// https://www.youtube.com/watch?v=Wt9p1og8aSo
        /// <summary>
        /// From database Reservations gets the existing reservations that have the same date as the current reservation.
        /// </summary>
        /// <param name="date">The current reservation date</param>
        /// <returns>Returns a bool. True: No existing reservations on the current reservation date. False: Existing reservations on the current reservation date</returns>
        public /*bool*/ List<Reservation> CheckDateAvailability(DateTime date)
        {
            // Info tp get values of foreign keys in the table https://stackoverflow.com/questions/19238413/how-to-display-foreign-key-values-in-mvc-view
            // Select records of the existing reservations that have the same reservation date as the current reservation
            var query = from res in _DbContext.Reservations.Include(p => p._resTable)
                        where res._resArrivingTime.Date == date.Date
                        select res;

            return query.ToList();

            //// Checks if the list is empty, if it is empty, there a no reservations for that day and returns true (can make a reservation), or else returns false
            //if (listReservationDate.Count == 0)
            //{
            //    return true;
            //}
            //else
            //{                
            //    return false;
            //}
        }



        //        /// <summary>
        //        /// From CheckDateAvailability method's list of overlapping reservations dates from existing reservations and current reservation, checks if there are overlapping reservation times
        //        /// </summary>
        //        /// <param name="time"></param>
        //        /// <returns></returns>
        //        public bool CheckTime(DateTime time)
        //        {
        //            foreach (Reservation res in listReservationDate)
        //            {
        //                DateTime prevResLeavingTime = res._resArrivingTime.AddHours(3);
        //                DateTime currentResLeavingTime = time.AddHours(3);

        //                //if this is true do nothing: the current reservation is before the existing reservation OR the current reservation is after the existing reservation
        //                // the current reservation is before the existing reservation == time < res._resArrivingTime && currentResLeavingTime <= res._resArrivingTime
        //                // the current reservation is after the existing reservation == time >= prevResLeavingTime && currentResLeavingTime > prevResLeavingTime
        //                // if this is false, then add record to the listReservationDateTime
        //                if ((time < res._resArrivingTime && currentResLeavingTime <= res._resArrivingTime) || (time >= prevResLeavingTime && currentResLeavingTime > prevResLeavingTime))
        //                { }
        //                else
        //                {
        //                    listReservationDateTime.Add(res);
        //                }
        //            }

        //            // Checks if the list is empty, if it is empty, there a no reservations on time on that day and returns true (can make a reservation), or else returns false
        //            if (listReservationDateTime.Count() == 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //        /// <summary>
        //        /// From CheckDateAvailability method's list of overlapping reservations dates from existing reservations and current reservation, checks if there are overlapping reservation times
        //        /// </summary>
        //        /// <param name="time"></param>
        //        /// <returns></returns>
        //        public bool CheckTimePerTable()
        //        {
        //            // https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sqlite#writing-tests
        //            // Connect to database
        //            ////@"Data Source=(MachineName;IntstanceName=Sql server name); Intitial Catalog (DBName); Integrated Security=True"
        //            SqlConnection sqlCon = new SqlConnection(@"Data Source=.\\MSSQL14.MSSQLSERVER; Initial Catalog = ReservationDB; Integrated Security=True");

        //            var options = new DbContextOptionsBuilder<MyDBContext>().UseSqlite(sqlCon).Options;

        //            using (var context = new MyDBContext(options))
        //            {
        //                // https://stackoverflow.com/questions/19238413/how-to-display-foreign-key-values-in-mvc-view
        //                // Makes a list of all tables with their ID and capacity
        //                var query = from res in context.Tables
        //                     select new Table(0)
        //                     {
        //                         TableID = res.TableID,
        //                         _tableCapacity = res._tableCapacity,
        //                         _tableArea = res._tableArea
        //                     };
        //                listTables = query.ToList();
        //            }

        //            // Test if list with Table IDs (listTables) are present in the list with reservations that have overlapping reservation time and date (listReservationDateTime)
        //            // Add a table record (with tableID) to the list (listFreeTables) to get a list of tables that do not have overlapping reseration time and date
        //            var test2NotInTest1 = listTables.Where(t2 => !listReservationDateTime.Any(t1 => t2.Equals(t1)));
        //            foreach (Table i in test2NotInTest1)
        //            {
        //                listFreeTables.Add(i);
        //            }

        //            // If listFreeTables has no records, then there are not an tables free on the  current reservation date and time. Return false (can't make a reservation)
        //            if (listFreeTables.Count() == 0)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }

        //        public bool CheckPartySize(int partySize)
        //        {
        //            foreach (Table table in listFreeTables)
        //            {
        //                if (table._tableCapacity <= partySize)
        //                {
        //                    listFreeTablesNumberOfGuest.Add(table);
        //                }
        //                //??? Possible to link tables? Table right = TableID#; Table left TableID#. 
        //                //??? If a table with ID=A is free and Table left or right is also free, we can sum the number of _tableCapacity for that table to be able to place more people in a reservation
        //                //else if ()

        //            }

        //            // If the list with tables that have enough seats for the current party size is zero, then returns false (can't make reservation with the partysize on the current reservation time and date)
        //            if (listFreeTablesNumberOfGuest.Count() == 0)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
    }
}
