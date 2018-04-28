using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNET_MVC_MolvenoReservationApplication.Models;

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

        public IActionResult Index()
        {
            var return_AllReservations = from reservations in _dbContextobj.Reservations
                                         select reservations;

            var rAllReservations = return_AllReservations.ToList();

            return View(rAllReservations);
        }

        [HttpPost]
        public IActionResult CreateReservation(Reservation newReservation)
        {

            _dbContextobj.Reservations.Add(newReservation);
            _dbContextobj.SaveChanges();
            return View();
        }

        public IActionResult CreateReservation()
        {

            return View();
        }


    }
}
