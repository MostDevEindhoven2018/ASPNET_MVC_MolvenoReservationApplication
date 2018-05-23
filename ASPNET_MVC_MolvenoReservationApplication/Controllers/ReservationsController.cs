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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ASPNET_MVC_MolvenoReservationApplication.Services;


namespace ASPNET_MVC_MolvenoReservationApplication.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly MyDBContext _context;
        private TableManager _tableManager;

        public ReservationsController(MyDBContext context)
        {
            //_AvailabilityCheck = new CheckTableAvailability(context);
            _context = context;
            _tableManager = new TableManager(context);
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            List<Reservation> resList = new List<Reservation>();
            resList = await _context.Reservations.Include("_resGuest").ToListAsync();
            foreach (Reservation r in resList)
            {
                r.Date = r._resArrivingTime.ToShortDateString();
                r.Time = r._resArrivingTime.ToShortTimeString();
            }
            return View(resList);
        }


        [HttpPost]
        public IActionResult Index(Reservation res)
        {
            res.Date = res._resArrivingTime.ToShortDateString();
            res.Time = res._resArrivingTime.ToShortTimeString();

            return View(res);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.Include("_resGuest").SingleOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        public List<int> PossibleResHours()
        {
            // Gets admin with ID 1 from database
            var adminConfigure1 = _context.Admins.FirstOrDefault(y => y.AdminID == 1);

            // Set the LastPossibleReservationHour to variable
            int _lastPossibleReservationhour;
            // If Closinghour minus OpeningHour results in a negative number or zero, add 24 to the hour
            if (adminConfigure1.ClosingHours - adminConfigure1.OpeningHour <= 0)
            {
                _lastPossibleReservationhour = adminConfigure1.LastPossibleReservationHour + 24;

            }
            else
            {
                _lastPossibleReservationhour = adminConfigure1.LastPossibleReservationHour;
            }

            List<int> PossibleResHoursList = new List<int>();
            // Add all the hours from opening till closinghour in list PossibleReservationHours        
            for (var i = adminConfigure1.OpeningHour; i <= _lastPossibleReservationhour; i++)
            {                
                PossibleResHoursList.Add(i);
            }

            return PossibleResHoursList;
        }


        // GET: Reservations/Create 
        [AllowAnonymous]
        public IActionResult Create()
        {
            // Make an instance of the ReservationViewModel and later send it to the view
            ReservationViewModel resVM = new ReservationViewModel();
            resVM.PossibleReservationHours = PossibleResHours();

            // Send the instance of ReservationViewModel to the view
            return View(resVM);
        }

        private int ParseIntToString(string input)
        {
            int output;
            if (int.TryParse(input, out output))
                return output;
            else
                throw new ArgumentException();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public async Task<IActionResult> Create([Bind("ReservationID,_resPartySize,_resArrivingTime,_resLeavingTime,_resHidePrices,_resComments")] Reservation reservation)
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(ReservationViewModel reservationInput)
        {
            // Gets admin with ID 1 from database
            var adminConfigure1 = _context.Admins.FirstOrDefault(y => y.AdminID == 1);
            reservationInput.PossibleReservationHours = PossibleResHours();

            DateTime resArrivingDate = new DateTime();

            string[] _arrDate = reservationInput.ArrivingDate.Split("-");

            resArrivingDate = new DateTime(ParseIntToString(_arrDate[2]), ParseIntToString(_arrDate[1]),
                ParseIntToString(_arrDate[0]), reservationInput.ArrivingHour, reservationInput.ArrivingMinute, 0);            
           
            DateTime resLeavingDate = resArrivingDate.AddHours(adminConfigure1._resDurationHour);

            // Get a list of all free tables for this particular time and calculate whether or not we have sufficient 
            // room for the partysize.
            List<Table> freetables = _tableManager.GetFreeTables(resArrivingDate, resLeavingDate);
            bool sufficientTables = _tableManager.SufficientCapacity(freetables, reservationInput.Partysize);

            // random if statement to simulate the check table availability
            if (sufficientTables && ModelState.IsValid)
            {
                string[] arr = new string[] {reservationInput.ArrivingDate, reservationInput.ArrivingHour.ToString(),
                    reservationInput.ArrivingMinute.ToString(), reservationInput.Partysize.ToString()};
                //TODO: change this return statement to redirect to Guest Details Input

                return RedirectToAction("Create", "Guests", new { guestViewModel = arr });
            }
            else
            {
                ModelState.AddModelError("No availability",
                    "We are sorry, but there are no tables available for your search. Try reserving a different date.");
                return View(reservationInput);
            }

            //if (ModelState.IsValid)
            //{
            //    Guest guest;
            //    Reservation res;
            //    try
            //    {
            //        guest = new Guest(reservationInput.GuestName, reservationInput.GuestEmail, reservationInput.GuestPhone);
            //    }
            //    catch
            //    {
            //        if (String.IsNullOrEmpty(reservationInput.GuestPhone) || String.IsNullOrWhiteSpace(reservationInput.GuestPhone))
            //        {
            //            guest = new Guest(reservationInput.GuestName, reservationInput.GuestEmail);
            //            res = new Reservation(reservationInput.Partysize, resArrivingDate, 3, guest);


            //        }
            //    }
            //}

            // TODO: check for available tables and return them
            // if (CheckTableAvailability > 0)
            // then go on with guest input details
            // else return to the begining of the form

            //if (ModelState.IsValid)
            //{
            //    _context.Guests.Add(resGuest);
            //    _context.Reservations.Add(reservation);
            //    _context.SaveChanges();

            //    return RedirectToAction("Index");
            //}
        }

        //PLACEHOLDER for the get available tables feature.
        ////List<Table> AvailableTables = new List<Table>()
        ////{
        ////    new Table(4, TableAreas.Lake)
        ////};
        ////try
        ////{
        ////    if (AvailableTables.Any()) //if (AvailableTables.Any())
        ////    {
        ////        // Create a new ReservationTableCoupling with the current reservation and add the tables found by the availabilitychecker.
        ////        ReservationTableCoupling RTC = new ReservationTableCoupling()
        ////        {
        ////            Reservation = reservation,
        ////            // Here the tables will be added. For now it is just the first free table found by the availabilitycheck.
        ////            Table = AvailableTables.First()
        ////        };

        ////        reservation._resReservationTableCouplings.Add(RTC);

        ////        // Commented this from MASTER
        ////        //s    if (ModelState.IsValid)
        ////        if (ModelState.IsValid)
        ////        {
        ////            _context.Reservations.Add(reservation);
        ////            _context.SaveChanges();

        ////            return RedirectToAction("Index");
        ////        }
        ////        return View(reservation);
        ////    }
        ////    else
        ////    {
        ////        ModelState.AddModelError("No availability",
        ////            "We are sorry, but there are no tables available for your search. Try reserving a different date.");
        ////        return View(reservationInput);
        ////        //return Json(new { status = "failed", message = "No free tables." });
        ////    }
        ////}
        ////catch(Exception ex)
        ////{
        ////    ModelState.AddModelError("Error", ex.Message);
        ////    return View(reservationInput);
        ////}


        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.Include("_resGuest").SingleOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,_resPartySize,_resArrivingTime,_resLeavingTime,_resHidePrices,_resComments,_resGuest._guestName,_resGuest._guestEmail,_resGuest._guestPhone,_resGuest")] Reservation reservation)
        {


            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Guest guest = new Guest();
                    guest = reservation._resGuest;
                    _context.Update(guest);
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

            var reservation = await _context.Reservations.Include("_resGuest").SingleOrDefaultAsync(m => m.ReservationID == id);
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

            //Include("_resGuest")
            var reservation = await _context.Reservations.SingleOrDefaultAsync(m => m.ReservationID == id);

            // Also remove the reservation couplings.
            List<ReservationTableCoupling> AllIncludedRTCs = _context.ReservationTableCouplings.Where(rtc => rtc.Reservation.ReservationID == id).ToList();

            foreach (ReservationTableCoupling rtc in AllIncludedRTCs)
            {
                _context.ReservationTableCouplings.Remove(rtc);
            }

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
