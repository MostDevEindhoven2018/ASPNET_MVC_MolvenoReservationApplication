using System;
using System.Collections.Generic;
using System.Text;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using System.Linq;
using ASPNET_MVC_MolvenoReservationApplication.Logic;

namespace TableManagerTests
{
    public class GetFreeTablesMock : IFreeTableFinder
    {
        List<ReservationTableCoupling> ReservationTableCouplingsList;
        List<Reservation> ReservationList;
        List<Table> TableList;

        public GetFreeTablesMock(List<ReservationTableCoupling> rTCList, List<Reservation> resList, List<Table> tableList)
        {
            ReservationTableCouplingsList = rTCList;
            ReservationList = resList;
            TableList = tableList;
        }
        
        public List<Table> GetFreeTables(DateTime start, DateTime end)
        {
            List<Table> FreeTables;
            List<Table> OccupiedTables;
            List<ReservationTableCoupling> ReservationTableCouplingsInTimeslot;

            // Get all ReservationTableCouplings with Reservations in our timeslot
            ReservationTableCouplingsInTimeslot = ReservationTableCouplingsList.Where(RTC => !(RTC.Reservation._resLeavingTime <= start || RTC.Reservation._resArrivingTime >= end)).ToList();
            // Then get all occupied Tables.
            OccupiedTables = ReservationTableCouplingsInTimeslot.Select(RTC => RTC.Table).ToList();
            // Get all tables and subtract all tables from the Couplings in the list above. 

            FreeTables = TableList.Where(TableFromAllTables => !OccupiedTables.Any(TableFromOccupiedTables => TableFromOccupiedTables.TableID == TableFromAllTables.TableID)).ToList();

            return FreeTables;
        }
    }
}
