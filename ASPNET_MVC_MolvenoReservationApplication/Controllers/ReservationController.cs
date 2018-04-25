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

        public IActionResult CheckAvailability(DateTime _arrivingDateTime, int _partySize/*, TableAreas _tableArea*/)
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

            DateTime Curr_res_arr = new DateTime(2018, 04, 27, 15, 0, 0);

            //CheckTableAvailability Checkings = new CheckTableAvailability(_dbContextobj);

            //List< Reservation> list1= Checkings.CheckDateAvailability(Curr_res_arr);

            ////TEST: to see if database can be reached
            ////return View(list);
            //// https://stackoverflow.com/questions/19238413/how-to-display-foreign-key-values-in-mvc-view

            //// Saves the seleted records from the database to the list "listReservationDate"
            var query = from res in _dbContextobj.Reservations.Include(p => p._resTable)
                        where res._resArrivingTime.Date == Curr_res_arr.Date
                        select res;
            return View(query.ToList());

            //return View (list1);

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
