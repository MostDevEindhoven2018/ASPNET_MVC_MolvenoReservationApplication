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
            //return View(await _context.Reservations.Include("_resGuest").Include("_resReservationTableCouplings").ToListAsync());


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

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
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
        //[ValidateAntiForgeryToken]        
        //public IActionResult Create([Bind("ReservationID,_resPartySize,_resArrivingTime.Date,_resArrivingTime.Hour,_resArrivingTime.Minute,_resLeavingTime,_resHidePrices,_resComments,_resGuest._guestName,_resGuest._guestPhone,_resGuest._guestEmail")] Reservation reservation)
        public IActionResult Create(ReservationViewModel reservationInput)
        {
            DateTime resArrivingDate = new DateTime();

            string[] _arrDate = reservationInput.ArrivingDate.Split("-");

            resArrivingDate = new DateTime(ParseIntToString(_arrDate[2]), ParseIntToString(_arrDate[1]),
                ParseIntToString(_arrDate[0]), reservationInput.ArrivingHour, reservationInput.ArrivingMinute, 0);


            // watch out! Hard coded stuff! 
            DateTime resLeavingDate = resArrivingDate.AddHours(3);

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

            return View();
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

            // See whether or not it might be a good idea to redo the configuration check

            List<ReservationTableCoupling> AllIncludedRTCs = await _context.ReservationTableCouplings.Where(rtc => rtc.Reservation.ReservationID == id).Include(rtc => rtc.Table).ToListAsync();
            List<Table> AllIncludedTables = AllIncludedRTCs.Select(rtc => rtc.Table).ToList();
            // if the partysize is not equal to the total capacity of the tables, maybe a better solution arose, or maybe it is 
            // not possible at all to up the partysize at this moment. Start a transaction to check it.
            if (_tableManager.GetTotalCapacity(AllIncludedTables) != reservation._resPartySize)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // Remove all tables currently used (to let them be seen as free tables instead of occupied) from the reservation and look for a new configuration.
                    foreach (ReservationTableCoupling rtc in AllIncludedRTCs)
                    {
                        _context.ReservationTableCouplings.Remove(rtc);
                    }
                    // See whether or not there is sufficient room for the new party size.
                    if (_tableManager.SufficientCapacity(_tableManager.GetFreeTables(reservation._resArrivingTime, reservation._resLeavingTime), reservation._resPartySize))
                    {
                        // If so, continue with looking for new tables.
                        List<Table> newTables = _tableManager.GetOptimalTableConfig(reservation._resArrivingTime, reservation._resLeavingTime, reservation._resPartySize);

                        foreach (Table t in newTables)
                        {
                            _context.ReservationTableCouplings.Add(new ReservationTableCoupling(reservation, t));
                        }
                        // Hurray! We found a new configuration. Lets keep this.
                        transaction.Commit();
                    }
                    else
                    {
                        // there was not enough room to change the party size. Send a message and rollback the transaction.
                        ModelState.AddModelError("No availability",
                        "There is not enough capacity for this partysize at this time. The changes made will not take effect.");
                        transaction.Rollback();
                    }
                }
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
