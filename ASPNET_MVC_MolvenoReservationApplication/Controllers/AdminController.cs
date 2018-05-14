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
    //[authorize]
    public class AdminController : Controller
    {
        private readonly MyDBContext _context;
        private CheckTableAvailability _AvailabilityCheck;
        private AdminConfigure adminConfigure;
        
        public AdminController(MyDBContext context)
        {
            //_AvailabilityCheck = new CheckTableAvailability(context);
            _context = context;

            adminConfigure = _context.Admins.FirstOrDefault(a => a.AdminID == 1);
            if(adminConfigure  == null)
            {
                adminConfigure = new AdminConfigure();
                adminConfigure.OpeningHour = 12;
                adminConfigure.ClosingHours = 00;
                adminConfigure._resDurationHour = 3;
                adminConfigure.PercentageMaxCapacity = 100;
                _context.Admins.Add(adminConfigure);
                _context.SaveChanges();
            }
        }         
             
        
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reservations
        public async Task<IActionResult> ReservationOverview()
        {
            return View(await _context.Reservations.Include("_resGuest").ToListAsync());
            //return View(await _context.Reservations.Include("_resGuest").Include("_resReservationTableCouplings").ToListAsync());

        }

        [HttpPost]
        public IActionResult ReservationOverview(IndexViewModel IndexViewModelInput)
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
        public async Task<IActionResult> ReservationDetails(int? id)
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

        // GET: Reservations/Edit/5
        public async Task<IActionResult> ReservationEdit(int? id)
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
        public async Task<IActionResult> ReservationEdit(int id, [Bind("ReservationID,_resPartySize,_resArrivingTime,_resLeavingTime,_resHidePrices,_resComments")] Reservation reservation)
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
                return RedirectToAction(nameof(ReservationOverview));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> ReservationDelete(int? id)
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
        [HttpPost, ActionName("ReservationDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservationDeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.SingleOrDefaultAsync(m => m.ReservationID == id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReservationOverview));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationID == id);
        }

        // GET: Reservations/Create
        public IActionResult ReservationCreate()
        {
            // Make an instance of the ReservationViewModel and later send it to the view
            ReservationViewModel resVM = new ReservationViewModel();

            // Set the Openinghour to the variable "a"
            int a = adminConfigure.OpeningHour;

            // Set the LastPossibleReservationHour to variable "b"
            int b;
            // If Closinghour minus OpeningHour results in a negative number or zero, add 24 to the hour
            if (adminConfigure.ClosingHours - a <= 0)
            {
                b = adminConfigure.LastPossibleReservationHour + 24;

            }
            else
            {
                b = adminConfigure.LastPossibleReservationHour;
            }

            // Add all the hours from opening till closinghour in list
            List<int> c = new List<int>();

            for (var i = a; i <= b; i++)
            {
                c.Add(i);
            }

            // Save this list in the list PossibleReservationHours
            resVM.PossibleReservationHours = c;

            // Send the instance of ReservationViewModel to the view
            return View(resVM);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public async Task<IActionResult> Create([Bind("ReservationID,_resPartySize,_resArrivingTime,_resLeavingTime,_resHidePrices,_resComments")] Reservation reservation)
        [HttpPost]
        //[ValidateAntiForgeryToken]        
        //public IActionResult Create([Bind("ReservationID,_resPartySize,_resArrivingTime.Date,_resArrivingTime.Hour,_resArrivingTime.Minute,_resLeavingTime,_resHidePrices,_resComments,_resGuest._guestName,_resGuest._guestPhone,_resGuest._guestEmail")] Reservation reservation)
        public IActionResult ReservationCreate(ReservationViewModel reservationInput)
        {
            DateTime resArrivingDate = new DateTime(reservationInput.Arrivingdate.Year, reservationInput.Arrivingdate.Month, reservationInput.Arrivingdate.Day, reservationInput.ArrivingHour, reservationInput.ArrivingMinute, 0);

            string dtpart = resArrivingDate.ToShortDateString();
            string tpart = resArrivingDate.ToShortTimeString();

            Guest resGuest = new Guest()
            {
                _guestName = reservationInput.GuestName,
                _guestPhone = reservationInput.GuestPhone,
                _guestEmail = reservationInput.GuestEmail
            };
            Table resTable = new Table();

            Reservation reservation = new Reservation()
            {

                Date = dtpart,
                Time = tpart,
                _resArrivingTime = resArrivingDate,
                _resPartySize = reservationInput.Partysize,
                _resHidePrices = reservationInput.Hideprices,
                _resComments = reservationInput.ResComments,
                _resGuest = resGuest,
                _resDurationOfReservation = adminConfigure._resDurationHour
            };

            // Commeted out in MASTER
            //reservation._resLeavingTime = reservation._resArrivingTime.AddHours(3);
            //List<Table> AvailableTables = _AvailabilityCheck.GetAvailableTables(reservation._resArrivingTime,
            //    reservation._resLeavingTime, reservation._resPartySize);


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


            if (ModelState.IsValid)
            {
                _context.Guests.Add(resGuest);
                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                return RedirectToAction("ReservationOverview");
            }
            return View(reservation);
        }      
        
        
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        // GET: Admin stuff
        public async Task<IActionResult> ReservationSettings()
        {
            return View(await _context.Admins.ToListAsync());            
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> ReservationSettingsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.SingleOrDefaultAsync(m => m.AdminID == id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        
        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservationSettingsEdit(int id, [Bind("OpeningHour,ClosingHours,_resDurationHour,PercentageMaxCapacity")] AdminConfigure admin)
        {
            if (id != admin.AdminID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminConfigureExists(admin.AdminID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ReservationSettings));
            }
            return View(admin);
        }

        private bool AdminConfigureExists(int id)
        {
            return _context.Admins.Any(e => e.AdminID == id);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // GET: Tables
        public async Task<IActionResult> TableOverview()
        {
            return View(await _context.Tables.ToListAsync());
        }

        // GET: Tables/Details/5
        public async Task<IActionResult> TableDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables
                .SingleOrDefaultAsync(m => m.TableID == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // GET: Tables/Create
        public IActionResult TableCreate()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TableCreate([Bind("TableID,_tableCapacity,_tableArea")] Table table)
        {
            if (ModelState.IsValid)
            {
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // GET: Tables/Edit/5
        public async Task<IActionResult> TableEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables.SingleOrDefaultAsync(m => m.TableID == id);
            if (table == null)
            {
                return NotFound();
            }
            return View(table);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TableEdit(int id, [Bind("TableID,_tableCapacity,_tableArea")] Table table)
        {
            if (id != table.TableID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.TableID))
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
            return View(table);
        }

        // GET: Tables/Delete/5
        public async Task<IActionResult> TableDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables
                .SingleOrDefaultAsync(m => m.TableID == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("TableDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TableDeleteConfirmed(int id)
        {
            var table = await _context.Tables.SingleOrDefaultAsync(m => m.TableID == id);
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TableOverview));
        }

        private bool TableExists(int id)
        {
            return _context.Tables.Any(e => e.TableID == id);
        }


    }
}