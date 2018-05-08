using ASPNET_MVC_MolvenoReservationApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public class FreeTableFinder : IFreeTableFinder
    {
        MyDBContext _context;

        public FreeTableFinder(MyDBContext context)
        {
            _context = context;
        }


        public List<Table> GetFreeTables(DateTime start, DateTime end)
        {
            List<Table> FreeTables;
            List<Table> OccupiedTables;
            List<ReservationTableCoupling> ReservationTableCouplingsInTimeslot;

            // Get all ReservationTableCouplings with Reservations in our timeslot
            ReservationTableCouplingsInTimeslot = _context.ReservationTableCouplings.Where(RTC => !(RTC.Reservation._resLeavingTime <= start || RTC.Reservation._resArrivingTime >= end)).ToList();
            // Then get all occupied Tables.
            OccupiedTables = ReservationTableCouplingsInTimeslot.Select(RTC => RTC.Table).ToList();
            // Get all tables and subtract all tables from the Couplings in the list above. 

            FreeTables = _context.Tables.Where(TableFromAllTables => !OccupiedTables.Any(TableFromOccupiedTables => TableFromOccupiedTables.TableID == TableFromAllTables.TableID)).ToList();

            return FreeTables;
        }
    }
}
