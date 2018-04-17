using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNET_MVC_MolvenoReservationApplication.Models;

namespace ASPNET_MVC_MolvenoReservationApplication.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult CheckAvailability(DateTime _arrivingDateTime, int _partySize, TableAreas _tableArea)
        {
            /*
             * 1) Check for available table in that specific date. Return all tables that are OK.
             * 2) Take the array from (1) and check in that for tables that are free in that specific time.
             *    Return a (reduced) array with available tables.
             * 3) Take the array from (2) and check in that for tables that have the required capacity.
             *    Return a (reduced) array with available tables.
             * 4) Take the array from (3) and check in that for tables that are placed in the required area.
             *    Return the first table that you find.
             */

            throw new NotImplementedException();
        }
    }
}
