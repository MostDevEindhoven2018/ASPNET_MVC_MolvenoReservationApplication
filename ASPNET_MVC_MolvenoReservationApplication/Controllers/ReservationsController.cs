using System;
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
            return View(await _context.Reservations.Include("_resGuest").ToListAsync());
            //return View(await _context.Reservations.Include("_resGuest").Include("_resReservationTableCouplings").ToListAsync());

        }


        [HttpPost]
        public IActionResult Index(IndexViewModel IndexViewModelInput)
        {
            DateTime resArrivingDate = new DateTime(IndexViewModelInput.Arrivingdate.Year, IndexViewModelInput.Arrivingdate.Month, IndexViewModelInput.Arrivingdate.Day, IndexViewModelInput.ArrivingHour, IndexViewModelInput.ArrivingMinute, 0);

            string dtpart = resArrivingDate.ToShortDateString();
            string tpart = resArrivingDate.ToShortTimeString();

            Guest resGuest = new Guest()
            {
                _guestName = IndexViewModelInput.GuestName,
                _guestPhone = IndexViewModelInput.GuestPhone,
                _guestEmail = IndexViewModelInput.GuestEmail
            };
            Table resTable = new Table();

            Reservation reservation = new Reservation()
            {

                Date = IndexViewModelInput.dtpart,
                Time = IndexViewModelInput.tpart,
                _resPartySize = IndexViewModelInput.Partysize,
                _resHidePrices = IndexViewModelInput.Hideprices,
                _resComments = IndexViewModelInput.ResComments,
                _resGuest = resGuest
            };



            return View();
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
            // TODO: get date, time and party size for all our iterations
            DateTime resArrivingDate = new DateTime();

            // if statement to check if this form is posted before or after checking for tables
            if (String.IsNullOrEmpty(reservationInput.GuestName))
            {
                string[] _arrDate = reservationInput.ArrivingDate.Split("-");

                resArrivingDate = new DateTime(ParseIntToString(_arrDate[2]), ParseIntToString(_arrDate[1]),
                    ParseIntToString(_arrDate[0]), reservationInput.ArrivingHour, reservationInput.ArrivingMinute, 0);

                // random if statement to simulate the check table availability
                if (Equals(typeof(CheckTableAvailability)))
                {
                    ViewData["TableFound"] = true;
                    return View(reservationInput);
                }
                else
                {
                    ModelState.AddModelError("No availability",
                        "We are sorry, but there are no tables available for your search. Try reserving a different date.");
                    return View(reservationInput);
                }
            }
            // if a guest name is provided it means that we are expecting his details
            else
            {
                if (ModelState.IsValid)
                {
                    Guest guest;
                    Reservation res;
                    try
                    {
                        guest = new Guest(reservationInput.GuestName, reservationInput.GuestEmail, reservationInput.GuestPhone);
                    }
                    catch
                    {
                        if (String.IsNullOrEmpty(reservationInput.GuestPhone) || String.IsNullOrWhiteSpace(reservationInput.GuestPhone))
                        {
                            guest = new Guest(reservationInput.GuestName, reservationInput.GuestEmail);
                            res = new Reservation(reservationInput.Partysize, resArrivingDate, 3, guest);

                
                        }
                    }
                }
            }
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

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,_resPartySize,_resArrivingTime,_resLeavingTime,_resHidePrices,_resComments,_resGuest._guestName")] Reservation reservation)
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
