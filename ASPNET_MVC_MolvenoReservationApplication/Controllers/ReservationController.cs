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
        CheckTableAvailability _AvailabilityCheck;

        public ReservationController(MyDBContext _context)
        {
            _AvailabilityCheck = new CheckTableAvailability(_context);

            _dbContextobj = _context;
            _dbContextobj.Database.EnsureCreated(); //Checks if a database is already created, if not it creates it
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dbContextobj.Reservations.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
            //return View("CreateReservationView");
        }



        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            reservation._resLeavingTime = reservation._resArrivingTime.AddHours(3);

            List<Table> AvailableTables = _AvailabilityCheck.GetAvailableTables(reservation._resArrivingTime, reservation._resLeavingTime, reservation._resPartySize);

            if (AvailableTables.Count > 0)
            {
                reservation._resTable = AvailableTables.First();

                if (ModelState.IsValid)
                {
                    _dbContextobj.Reservations.Add(reservation);
                    _dbContextobj.SaveChanges();

                    return RedirectToAction("Index");
                }
                // Add a message about the Inputs not being valid. ModelState is false.
                return View(reservation);

                

            }

            // Add a message about this time and date being fully booked. 

            ////From ValidationServerSide branch
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                //Logic.CheckTableAvailability ca = new CheckTableAvailability();
                _dbContextobj.Add(reservation);
                await _dbContextobj.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);

        }
        ////From the ValidationClientSide branch
        public IActionResult Index()
        {
            var return_AllReservations = from reservations in _dbContextobj.Reservations
                                         select reservations;

            var rAllReservations = return_AllReservations.ToList();

            return View(rAllReservations);
        }

        ////From the ValidationClientSide branch
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
