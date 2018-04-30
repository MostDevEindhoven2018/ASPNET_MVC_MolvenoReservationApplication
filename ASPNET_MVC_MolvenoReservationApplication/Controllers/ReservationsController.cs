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


namespace ASPNET_MVC_MolvenoReservationApplication.Controllers
{
    public class ReservationsController : Controller
    {
        //private readonly MyDBContext _context;
        //private CheckTableAvailability _AvailabilityCheck;
        MyDBContext _context;
        CheckTableAvailability _AvailabilityCheck;

        public ReservationsController(MyDBContext context)
        {
            _AvailabilityCheck = new CheckTableAvailability(context);
            _context = context;
            
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservations.ToListAsync());
        }

        //// GET: Reservations/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var reservation = await _context.Reservations
        //        .SingleOrDefaultAsync(m => m.ReservationID == id);
        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(reservation);
        //}

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public async Task<IActionResult> Create([Bind("ReservationID,_resPartySize,_resArrivingTime,_resLeavingTime,_resHidePrices,_resComments")] Reservation reservation)
        [HttpPost]
        //[ValidateAntiForgeryToken]        
        //public IActionResult Create([Bind("ReservationID,_resPartySize,_resArrivingTime.Date,_resArrivingTime.Hour,_resArrivingTime.Minute,_resLeavingTime,_resHidePrices,_resComments,_resGuest._guestName,_resGuest._guestPhone,_resGuest._guestEmail")] Reservation reservation)
        public IActionResult Create(Reservation reservation)
        {
            reservation._resLeavingTime = reservation._resArrivingTime.AddHours(3);

            List<Table> AvailableTables = _AvailabilityCheck.GetAvailableTables(reservation._resArrivingTime,
                reservation._resLeavingTime, reservation._resPartySize);

            //if (AvailableTables.Any())
            if (AvailableTables.Count>0)
            {
                reservation._resTable = AvailableTables.First();


                if (ModelState.IsValid)
                {
                    //_context.Add(reservation);
                    //_context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    _context.Reservations.Add(reservation);
                    _context.SaveChanges();

                    return RedirectToAction("Index");

                }
                return View(reservation);
            }
            return View(reservation);
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
