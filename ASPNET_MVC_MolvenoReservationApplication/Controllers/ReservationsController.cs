﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNET_MVC_MolvenoReservationApplication;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Logic;
using ASPNET_MVC_MolvenoReservationApplication.ViewModels;


namespace ASPNET_MVC_MolvenoReservationApplication.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly MyDBContext _context;
        private CheckTableAvailability _AvailabilityCheck;

        public ReservationsController(MyDBContext context)
        {
            //_AvailabilityCheck = new CheckTableAvailability(context);
            _context = context;

        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .SingleOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        //public IActionResult Create(ReservationViewModel viewModel)
        //{

        //    var guest = new Guest()

        //    {
        //        _guestName = viewModel.name,
        //       _guestEmail = viewModel.email,
        //       _guestPhone = viewModel.phone,
        //     };

        //    var table = new Table();
        //    var reservation = new Reservation();
            

        //    return View("Create");

        //}

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public async Task<IActionResult> Create([Bind("ReservationID,_resPartySize,_resArrivingTime,_resLeavingTime,_resHidePrices,_resComments")] Reservation reservation)
        [HttpPost]
        //[ValidateAntiForgeryToken]        
        //public IActionResult Create([Bind("ReservationID,_resPartySize,_resArrivingTime.Date,_resArrivingTime.Hour,_resArrivingTime.Minute,_resLeavingTime,_resHidePrices,_resComments,_resGuest._guestName,_resGuest._guestPhone,_resGuest._guestEmail")] Reservation reservation)
        public IActionResult Create(ReservationViewModel reservationInput)
        {
            DateTime resArrivingDate = new DateTime(reservationInput.Arrivingdate.Year, reservationInput.Arrivingdate.Month, reservationInput.Arrivingdate.Day, reservationInput.ArrivingHour, reservationInput.ArrivingMinute, 0);

            Guest resGuest = new Guest()
            {
                _guestName = reservationInput.GuestName,
                _guestPhone = reservationInput.GuestPhone,
                _guestEmail= reservationInput.GuestEmail
            };
            Table resTable = new Table();           

            Reservation reservation = new Reservation()
            {
                _resArrivingTime = resArrivingDate,                
                _resPartySize = reservationInput.Partysize,
                _resHidePrices = reservationInput.Hideprices,
                _resComments = reservationInput.ResComments
            };

            //// Error: System.NullReferenceException: 'Object reference not set to an instance of an object.'
            //reservation._resGuest._guestName = reservationInput.GuestName;
            //reservation._resGuest._guestPhone = reservationInput.GuestPhone;
            //reservation._resGuest._guestEmail = reservationInput.GuestEmail;

            




            reservation._resHidePrices = reservationInput.Hideprices;
            reservation._resComments = reservationInput.ResComments;

            reservation._resLeavingTime = reservation._resArrivingTime.AddHours(3);

            // Commeted out in MASTER
            //List<Table> AvailableTables = _AvailabilityCheck.GetAvailableTables(reservation._resArrivingTime,
            //    reservation._resLeavingTime, reservation._resPartySize);
            

            //PLACEHOLDER for the get available tables feature.
            List<Table> AvailableTables = new List<Table>()
            {
                new Table(4, TableAreas.Lake)
            };
            try
            {
                if (AvailableTables.Any()) //if (AvailableTables.Any())
                {
                    // Create a new ReservationTableCoupling with the current reservation and add the tables found by the availabilitychecker.
                    ReservationTableCoupling RTC = new ReservationTableCoupling()
                    {
                        Reservation = reservation,
                        // Here the tables will be added. For now it is just the first free table found by the availabilitycheck.
                        Table = AvailableTables.First()
                    };

                    reservation._resReservationTableCouplings.Add(RTC);

                    if (ModelState.IsValid)
                    {
                        _context.Reservations.Add(reservation);
                        _context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    return View(reservation);
                }
                else
                {
                    ModelState.AddModelError("No availability",
                        "We are sorry, but there are no tables available for your search. Try reserving a different date.");
                    return View(reservation);
                    //return Json(new { status = "failed", message = "No free tables." });
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(reservationInput);
            }
        }
        
        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.SingleOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,_resPartySize,_resArrivingTime,_resLeavingTime,_resHidePrices,_resComments")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .SingleOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.SingleOrDefaultAsync(m => m.ReservationID == id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationID == id);
        }
    }
}
