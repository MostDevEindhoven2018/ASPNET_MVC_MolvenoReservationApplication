using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ASPNET_MVC_MolvenoReservationApplication.Models
{
    public class AdminConfigure
    {
        // Need this for form: when to start the reservation time
        // Need to store this in database
        [Range(0, 23)]
        public int OpeningHour { get; set; }
        [Range(0, 59)]
        public int OpeningMinutes { get; set; }

        // Need this for form: when is the last possible reservation on a day
        // Need this for _resDurationOfReservation (Reservation class), this will be used for CheckTableAvailability
        // Need to store this in database
        [Range(0, 23)]
        public int ClosingHours { get; set; }
        [Range(0, 59)]
        public int ClosingMinutes { get; set; }

        // Need to store this in database?
        public int LastPossibleReservationHour
        {
            get
            {
                if (ClosingHours - _resDurationHour - 1 < 0)
                {
                    if (ClosingMinutes == 0)
                    {
                        return ClosingHours - _resDurationHour + 24;
                    }
                    else
                    {
                        return ClosingHours - _resDurationHour - 1 + 24;
                    }
                }
                else
                {
                    if (ClosingMinutes == 0)
                    {
                        return ClosingHours - _resDurationHour;
                    }
                    else
                    {
                        return ClosingHours - _resDurationHour - 1;
                    }

                }
            }
        }

        // Need to store this in database?
        public int LastPossibleReservationMinutes
        {
            get
            {
                if (ClosingMinutes - _resDurationMinutes < 0)
                {
                    return ClosingHours - _resDurationHour + 60;
                }
                else
                {
                    return ClosingHours - _resDurationHour;
                }
            }
        }

        // Need to set the Duration of Reservation by Admin
        // Need to store this in database
        [Range(0, 23)]
        public int _resDurationHour { get; set; }
        [Range(0, 59)]
        public int _resDurationMinutes { get; set; }
        // This will be used for CheckTableAvailability
        // This weil set the _resDurationReservation

        // Need to store this in database?
        public TimeSpan _resDurationOfReservation
        {
            get
            {
                return new TimeSpan(_resDurationHour, _resDurationMinutes, 0);
            }
        }

        // Percentage that of max capacity of restaurant that can be reserved
        // Need to store this in database
        [Range(0, 100)]
        public int PercentageMaxCapacity { get; set; }


        MyDBContext _context;

        //public AdminConfigure(MyDBContext context)
        //{
        //    _context = context;
        //}

        // Get the maximum number of tables that can be reserved for the day
        // Need to store this in database?
        public int MaxNumberOfTablesAllowedRes
        {
            get
            {
                //var AllTables = _context.Tables.Select(t=>t._tableCapacity).ToList();
                //int number = AllTables.Sum();
                //return number;

                var AllTables = _context.Tables.Select(t => t).ToList();
                int number = AllTables.Count();
                decimal number2 = number * (PercentageMaxCapacity / 100);
                int  number3= Convert.ToInt32(Math.Floor(number2));
                return number3;
            }
        }




    }
}
