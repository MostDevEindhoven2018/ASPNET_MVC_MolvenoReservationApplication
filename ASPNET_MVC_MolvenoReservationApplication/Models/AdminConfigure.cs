﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ASPNET_MVC_MolvenoReservationApplication.Models
{
    public class AdminConfigure
    {
        [Key]
        public int AdminID { get; set; }

        // Need this for form: when to start the reservation time
        // Need to store this in database
        [Display(Name = "Opening Hours", AutoGenerateField = true)]
        [Range(0, 23)]
        public int OpeningHour { get; set; } = 12;

        // Need this for form: when is the last possible reservation on a day
        // Need this for _resDurationOfReservation (Reservation class), this will be used for CheckTableAvailability
        // Need to store this in database
        [Display(Name = "Closing Hours", AutoGenerateField = true)]
        [Range(0, 23)]
        public int ClosingHours { get; set; } = 3;

        //// Need to store this in database
        public int LastPossibleReservationHour
        {
            get
            {
                if (ClosingHours - _resDurationHour < 0)
                {
                    return ClosingHours - _resDurationHour + 24;
                }
                else
                {
                    return ClosingHours - _resDurationHour;
                }
            }
        }

        [Display(Name = "Duration reservation in Hours", AutoGenerateField = true)]
        [Range(0, 23)]
        public int _resDurationHour { get; set;}

        // Percentage that of max capacity of restaurant that can be reserved
        // Need to store this in database
        [Display(Name = "Percentage of maximum capacity that can be reserved", AutoGenerateField = true)]
        [Range(0, 100)]
        public int PercentageMaxCapacity { get; set; }

        // Get the maximum number of tables that can be reserved for the day
        // Not stored in database, can calculate when needed. Stored number of tables and maxpercentage in database
        public int MaxNumberOfTablesAllowedRes (MyDBContext _context)
        {
            var AllTables = _context.Tables.Select(t => t).ToList();
            decimal number2 = AllTables.Count() * (PercentageMaxCapacity / 100);
            int number3 = Convert.ToInt32(Math.Floor(number2));
            return number3;
        }

        public AdminConfigure() { }
    }
}
