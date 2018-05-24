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
    public class TablesController : Controller
    {

        private readonly MyDBContext _context;
        private TableManager _tableManager;

        public TablesController(MyDBContext context)
        {
            _context = context;
            _tableManager = new TableManager(context);
        }

        // GET: Tables
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tables.ToListAsync());
        }

        // GET: Tables/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TableID,_tableCapacity,_tableArea")] Table table)
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
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("TableID,_tableCapacity,_tableArea")] Table table)
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
        public async Task<IActionResult> Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.Tables.SingleOrDefaultAsync(m => m.TableID == id);

            // We want to remove a table but we are not yet sure whether or not we actually can without screwing up reservations using this table.
            // So lets start a transaction

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Tables.Remove(table);

                // Now we have deleted a table, lets again find the best table configuration for all reservations using this table.
                // Start by removing all included RTCs fromt the context (but still have them in a list) to open up space.

                List<ReservationTableCoupling> AllIncludedRTCs = _context.ReservationTableCouplings.Where(rtc => rtc.Table.TableID == id).Include(rtc => rtc.Reservation).Include(rtc => rtc.Table).ToList();
                List<Reservation> AffectedReservations = new List<Reservation>();

                foreach (ReservationTableCoupling rtc in AllIncludedRTCs)
                {
                    AffectedReservations.Add(rtc.Reservation);
                    _context.ReservationTableCouplings.Remove(rtc);
                }
                // remove multiple inputs
                AffectedReservations.Distinct();

                // Now lets see whether or not we have enough space to reorganise the other reservations.
                List<Reservation> ReservationsWithProblems = new List<Reservation>();
                foreach (Reservation res in AffectedReservations)
                {
                    if (!_tableManager.SufficientCapacity(_tableManager.GetFreeTables(res._resArrivingTime, res._resLeavingTime), res._resPartySize))
                    {
                        ReservationsWithProblems.Add(res);
                    }
                }
                if (ReservationsWithProblems.Count == 0)
                {
                    // Only now we know for sure that removing this table will not mess up other reservations, lets find new tables.
                    foreach (Reservation res in AffectedReservations)
                    {

                        List<Table> newTables = _tableManager.GetOptimalTableConfig(res._resArrivingTime, res._resLeavingTime, res._resPartySize);
                        foreach (Table t in newTables)
                        {
                            _context.ReservationTableCouplings.Add(new ReservationTableCoupling(res, t));
                        }
                    }

                    // Awesome, everything worked out. Lets Commit
                    transaction.Commit();

                }
                else
                {
                    // There were issues with removing this table. Show which reservations will have problems and stop any changes made
                    // (Being the removed tables and the removed RTCs)

                    string errorMessage = "There were reservations depending on this table at times:" + Environment.NewLine;
                    foreach (Reservation res in ReservationsWithProblems)
                    {
                        errorMessage += res._resArrivingTime.ToString() + Environment.NewLine;
                    }

                    errorMessage += "The changes were not implemented.";

                    // Send the errorMessage to the view
                    transaction.Rollback();
                }

            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableExists(int id)
        {
            return _context.Tables.Any(e => e.TableID == id);
        }
    }
}
