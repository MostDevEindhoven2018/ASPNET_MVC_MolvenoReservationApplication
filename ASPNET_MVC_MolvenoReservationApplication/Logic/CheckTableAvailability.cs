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
        
    public class CheckTableAvailability : DbContext
    {
        
       

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
            // https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sqlite#writing-tests
            // Connect to database
            //var connection = new SqliteConnection("DataSource=:memory:");
            //var connection = new SqliteConnection("DataSource=C:/Program Files/Microsoft SQL Server/MSSQL14.MSSQLSERVER/MSSQL/DATA/ReservationDB.db");
            //var sqlCon = new SqliteConnection("Data Source=.\\SQLEXPRESS;AttachDBFileName=|DataDirectory|\\ReservationDB.mdf; integrated security=true;user instance=true;");


            ////@"Data Source=(MachineName;IntstanceName=Sql server name); Intitial Catalog (DBName); Integrated Security=True"
            SqlConnection sqlCon = new SqlConnection(@"Data Source=.\\MSSQL14.MSSQLSERVER; Initial Catalog = ReservationDB; Integrated Security=True");


            var options = new DbContextOptionsBuilder<MyDBContext>().UseSqlite(sqlCon).Options;

            using (var context = new MyDBContext(options))
            {
                // https://stackoverflow.com/questions/19238413/how-to-display-foreign-key-values-in-mvc-view
                // Select records of the existing reservations that have the same reservation date as the current reservation
                var query = from res in context.Reservations.Include(p => p._resTable)
                            where res._resArrivingTime.Date == date.Date
                            select res;
                // Saves the seleted records from the database to the list "listReservationDate"
                listReservationDate = query.ToList();

            }          

            return listReservationDate;


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

        

        /// <summary>
        /// From CheckDateAvailability method's list of overlapping reservations dates from existing reservations and current reservation, checks if there are overlapping reservation times
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool CheckTime(DateTime time)
        {
            foreach (Reservation res in listReservationDate)
            {
                DateTime prevResLeavingTime = res._resArrivingTime.AddHours(3);
                DateTime currentResLeavingTime = time.AddHours(3);

                //if this is true do nothing: the current reservation is before the existing reservation OR the current reservation is after the existing reservation
                // the current reservation is before the existing reservation == time < res._resArrivingTime && currentResLeavingTime <= res._resArrivingTime
                // the current reservation is after the existing reservation == time >= prevResLeavingTime && currentResLeavingTime > prevResLeavingTime
                // if this is false, then add record to the listReservationDateTime
                if ((time < res._resArrivingTime && currentResLeavingTime <= res._resArrivingTime) || (time >= prevResLeavingTime && currentResLeavingTime > prevResLeavingTime))
                { }
                else
                {
                    listReservationDateTime.Add(res);
                }
            }

            // If listReservationDateTime has no records, then overlapping date and time of current reservations with existing ones
            if (listReservationDateTime.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// From CheckDateAvailability method's list of overlapping reservations dates from existing reservations and current reservation, checks if there are overlapping reservation times
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool CheckTimePerTable()
        {


            //Select records of the existing reservations that have the same reservation date as the current reservation
            var optionsBuilder = new DbContextOptionsBuilder<MyDBContext>();
            optionsBuilder.UseSqlite("Data Source=ReservationDB.db");

            using (var context = new MyDBContext(optionsBuilder.Options))
            {
                var query = from res in context.Tables
                            select new Table()
                            {
                                TableID = res.TableID,
                                _tableCapacity = res._tableCapacity,
                                _tableArea = res._tableArea
                            };
                listTables = query.ToList();
            }





            var test2NotInTest1 = listTables.Where(t2 => !listReservationDateTime.Any(t1 => t2.Equals(t1)));
            foreach (Table i in test2NotInTest1)
            {
                listFreeTables.Add(i);
            }

            Console.ReadKey();

            // If listReservationDateTime has no records, then overlapping date and time of current reservations with existing ones
            if (listFreeTables.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckPartySize(int partySize)
        {
            throw new NotImplementedException();
        }
    }
}
