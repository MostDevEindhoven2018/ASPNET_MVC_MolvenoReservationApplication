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
        private AdminConfigure _adminConfigure;

        private AdminConfigure AdminConfigure
        {
            get {
                if(_adminConfigure == null)
                {
                    _adminConfigure = _context.Admins.FirstOrDefault(a => a.AdminID == 1);
                    if (_adminConfigure == null)
                    {
                        _adminConfigure = new AdminConfigure();
                        _adminConfigure.OpeningHour = 12;
                        _adminConfigure.ClosingHours = 00;
                        _adminConfigure._resDurationHour = 3;
                        _adminConfigure.PercentageMaxCapacity = 100;
                        _context.Admins.Add(_adminConfigure);
                        _context.SaveChanges();
                    }
                }

                return _adminConfigure;
            }
            set { throw new InvalidOperationException(); }
        }
        
        public AdminController(MyDBContext context)
        {
            //_AvailabilityCheck = new CheckTableAvailability(context);
            _context = context;
        }         
             
        
        public IActionResult Index()
        {
            return View();
        }

        
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        // GET: Admin stuff
        public async Task<IActionResult> ReservationSettings()
        {
            var x = AdminConfigure;
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
        public async Task<IActionResult> ReservationSettingsEdit(int id, [Bind("AdminID,OpeningHour,ClosingHours,_resDurationHour,PercentageMaxCapacity")] AdminConfigure admin)
        {
            if (id != admin.AdminID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    AdminConfigure.OpeningHour = admin.OpeningHour;
                    AdminConfigure.ClosingHours = admin.ClosingHours;                    
                    AdminConfigure._resDurationHour = admin._resDurationHour;
                    AdminConfigure.PercentageMaxCapacity = admin.PercentageMaxCapacity;                    

                    _context.Update(AdminConfigure);
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


    }
}