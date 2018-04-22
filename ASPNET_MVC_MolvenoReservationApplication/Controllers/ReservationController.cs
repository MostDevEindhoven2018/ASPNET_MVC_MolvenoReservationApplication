using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Logic;
using ASPNET_MVC_MolvenoReservationApplication.Controllers;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace ASPNET_MVC_MolvenoReservationApplication.Controllers
{
    public class ReservationController : Controller
    {
        //Creating an instance (object) of MyDBContext 
        MyDBContext _dbContextobj;

        public ReservationController(MyDBContext _context)
        {
            _dbContextobj = _context;
            _dbContextobj.Database.EnsureCreated(); //Checks if a database is already created, if not it creates it
        }

        public IActionResult Create()
        {
            return View();
            //return View("CreateReservationView");
        }

        public IActionResult CheckAvailability(DateTime _arrivingDateTime, int _partySize, TableAreas _tableArea)
        {
            /* tableId | date | time
             * 
             * Get an array with all tables booked for that date. 
             * If a table is not included in the array, goto (1).
             * If all tables are included, get all the tables of that date.
             * Foreach reservation check if it will be available that time.
             * If there is a possible reservation, get all the reservations of that table in that date.
             * Check if the table will be free for at least 3 hours before the next reservation.
             * If yes goto (1).
             * 
             * (1)Check if the specific table has the required capacity.
             */
             
            throw new NotImplementedException();
        }

            //var table1 = new Table
            //{
            //    // Can't set the ID, or else error:: SqlException: Cannot insert explicit value for identity column in table 'Tables' when IDENTITY_INSERT is set to OFF.
            //    //TableID = 1,
            //    _tableCapacity = 4,
            //    MyProperty = TableAreas.Window
            //};

            //var res1 = new Reservation
            //{
            //    //ReservationID = 1,
            //    _resTable = table1,
            //     _resPartySize=3,
            //    _resArrivingTime= new DateTime (2018,04,27,12,00,00),
            //    _resHidePrices = false,
            //    _resComments = ""
            //};

            //var table2 = new Table
            //{
            //    //TableID = 2,
            //    _tableCapacity = 6,
            //    MyProperty = TableAreas.Fireplace
            //};

            //var res2 = new Reservation
            //{
            //    //ReservationID = 2,
            //    _resTable = table2,
            //    _resPartySize = 5,
            //    _resArrivingTime = new DateTime(2018, 04, 27, 14, 00, 00),
            //    _resHidePrices = false,
            //    _resComments = ""
            //};

            //var res3 = new Reservation
            //{
            //    //ReservationID = 3,
            //    _resTable = table2,
            //    _resPartySize = 6,
            //    _resArrivingTime = new DateTime(2018, 04, 27, 17, 00, 00),
            //    _resHidePrices = false,
            //    _resComments = ""
            //};

            //var table3 = new Table
            //{
            //    //TableID = 3,
            //    _tableCapacity = 2,
            //    MyProperty = TableAreas.Lake
            //};

            //var res4 = new Reservation
            //{
            //    //ReservationID = 4,
            //    _resTable = table3,
            //    _resPartySize = 2,
            //    _resArrivingTime = new DateTime(2018, 04, 27, 15, 00, 00),
            //    _resHidePrices = false,
            //    _resComments = ""
            //};

            //_dbContextobj.Tables.Add(table1);
            //_dbContextobj.Reservations.Add(res1);
            //_dbContextobj.Tables.Add(table2);
            //_dbContextobj.Tables.Add(table3);
            //_dbContextobj.Reservations.Add(res2);
            //_dbContextobj.Reservations.Add(res3);
            //_dbContextobj.Reservations.Add(res4);
            //_dbContextobj.SaveChanges();

            /// FILL DATABASE END
            /// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            /// 

            DateTime Curr_res_arr = new DateTime(2018, 04, 27, 15, 0, 0);
            
            CheckTableAvailability Checkings = new CheckTableAvailability();
            
            List< Reservation> list1= Checkings.CheckDateAvailability(Curr_res_arr);

            //List<Reservation> list1 = new List<Reservation>();
            //list1 = Checkings.CheckDateAvailability(Curr_res_arr);

            //Checkings.CheckTime(Curr_res_date);
            ////Checkings.CheckTimePerTable();




            ////TEST: to see if database can be reached
            ////return View(list);
            //// https://stackoverflow.com/questions/19238413/how-to-display-foreign-key-values-in-mvc-view
            //var query = from res in _dbContextobj.Reservations.Include(p=>p._resTable)
            //            where res._resArrivingTime.Date == Curr_res_arr.Date
            //            select res;
            //// Saves the seleted records from the database to the list "listReservationDate"

            return View (list1);

        }

        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                Reservation reser = new Reservation();
                reser._resArrivingTime = reservation._resArrivingTime;
                reser._resHidePrices = reservation._resHidePrices;

                _dbContextobj.Add(reser);
                _dbContextobj.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(reservation);

        }
    }
}
