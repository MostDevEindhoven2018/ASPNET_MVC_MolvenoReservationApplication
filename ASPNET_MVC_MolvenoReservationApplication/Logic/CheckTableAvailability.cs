using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Controllers;
using System.Data;


namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public class CheckTableAvailability
    {
        // The database
        MyDBContext _dbContextobj;

        // Make lists
        // listReservationDate: list with existing reservations that have the same reservation date as the current reservation
        // listReservationDateTime: list with existing reservations that have the same reservation date and time as the current reservation
        List<Reservation> listReservationDate = new List<Reservation>();
        List<Reservation> listReservationDateTime = new List<Reservation>();

        /// <summary>
        /// From database Reservations gets the existing reservations that have the same date as the current reservation.
        /// </summary>
        /// <param name="date">The current reservation date</param>
        /// <returns>Returns a bool. True: No existing reservations on the current reservation date. False: Existing reservations on the current reservation date</returns>
        public bool CheckDateAvailability(DateTime date)
        {
            // Select records of the existing reservations that have the same reservation date as the current reservation
            var query = from res in _dbContextobj.Reservations
                        where res._resArrivingTime == date
                        select res;

            // Saves the seleted records from the database to the list "listReservationDate"
            listReservationDate = query.ToList();

            // Checks if the list is empty, if it is empty, there a no reservations for that day and returns true (can make a reservation), or else returns false
            if (listReservationDate.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// From CheckDateAvailability method's list of overlapping reservations dates from existing reservations and current reservation, checks if there are overlapping reservation times
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool CheckTime(DateTime time)
        {

            foreach (Reservation res in listReservationDate)
            {
                DateTime prevResLeavingTime = res._resArrivingTime.AddHours(3);
                DateTime currentResLeavingTime = time.AddHours(3);

                //if this is true do nothing: the current reservation is before the existing reservation OR the current reservation is after the existing reservation
                // the current reservation is before the existing reservation == time < res._resArrivingTime && currentResLeavingTime <= res._resArrivingTime
                // the current reservation is after the existing reservation == time >= prevResLeavingTime && currentResLeavingTime > prevResLeavingTime
                // if this is false, then add record to the listReservationDateTime
                if ((time < res._resArrivingTime && currentResLeavingTime <= res._resArrivingTime) || (time >= prevResLeavingTime && currentResLeavingTime > prevResLeavingTime))
                { }
                else
                {
                    listReservationDateTime.Add(res);
                }

            }

            // If listReservationDateTime has no records, then overlapping date and time of current reservations with existing ones
            if (listReservationDateTime.Count() == 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        public bool CheckPartySize(int partySize)
        {
            throw new NotImplementedException();
        }
    }
}
