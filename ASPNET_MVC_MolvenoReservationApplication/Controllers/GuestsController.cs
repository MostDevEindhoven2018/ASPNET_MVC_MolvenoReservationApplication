using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNET_MVC_MolvenoReservationApplication;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.ViewModels;
using ASPNET_MVC_MolvenoReservationApplication.Logic;
using Microsoft.AspNetCore.Authorization;

namespace ASPNET_MVC_MolvenoReservationApplication.Controllers
{
    [Authorize]
    public class GuestsController : Controller
    {
        private readonly MyDBContext _context;
        private TableManager _tableManager; 

        public GuestsController(MyDBContext context)
        {
            _context = context;
            _tableManager = new TableManager(context);
        }

        // GET: Guests
        public async Task<IActionResult> Index()
        {
            return View(await _context.Guests.ToListAsync());
        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .SingleOrDefaultAsync(m => m.GuestID == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        private int ParseIntToString(string input)
        {
            int output;
            if (int.TryParse(input, out output))
                return output;
            else
                throw new ArgumentException();
        }

        // GET: Guests/Create
        public IActionResult Create(string[] guestViewModel)
        {
            DateTime resArrivingDate = new DateTime();

            string[] _arrDate = guestViewModel[0].Split("-");

            resArrivingDate = new DateTime(ParseIntToString(_arrDate[2]), ParseIntToString(_arrDate[1]),
                ParseIntToString(_arrDate[0]), ParseIntToString(guestViewModel[1]), ParseIntToString(guestViewModel[2]), 0);

            GuestViewModel gvm = new GuestViewModel
            {
                arrival = resArrivingDate,
                size = ParseIntToString(guestViewModel[3])
            };

            return View(gvm);
        }

        // POST: Guests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestViewModel guestViewModel)
        {
            if (ModelState.IsValid)
            {
                Guest guest = new Guest
                {
                    _guestName = guestViewModel.GuestName,
                    _guestEmail = guestViewModel.GuestEmail,
                    _guestPhone = guestViewModel.GuestPhone
                };
                _context.Add(guest);

                DateTime leaving = guestViewModel.arrival.AddHours(3);

                List<Table> TablesForThisReservation = _tableManager.GetOptimalTableConfig(guestViewModel.arrival, leaving, guestViewModel.size);

                Reservation currentRes = new Reservation(guestViewModel.size, guestViewModel.arrival, 3, guest);
                _context.Reservations.Add(currentRes);

                foreach (Table table in TablesForThisReservation)
                {
                    _context.ReservationTableCouplings.Add(new ReservationTableCoupling(currentRes, table));
                }



                await _context.SaveChangesAsync();
                return RedirectToAction("ConfirmReservation", "Home");
            }
            return View();
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.SingleOrDefaultAsync(m => m.GuestID == id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuestID,_guestName,_guestPhone,_guestEmail")] Guest guest)
        {
            if (id != guest.GuestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(guest.GuestID))
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
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .SingleOrDefaultAsync(m => m.GuestID == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guest = await _context.Guests.SingleOrDefaultAsync(m => m.GuestID == id);
            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuestExists(int id)
        {
            return _context.Guests.Any(e => e.GuestID == id);
        }
    }
}
