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
        MyDBContext _dbContextobj;
        
        Reservation[] listReservationDate = new Reservation[100];
        Reservation[] listReservationDateTime = new Reservation[100];

        public bool CheckDateAvailability(DateTime date)
        {            
            var query = from res in _dbContextobj.Reservations
                        where res._resArrivingTime == date
                        select res;

            listReservationDate = query.ToArray();

            if (listReservationDate.First() == null)
            {
                return true;
            }
            else
            {

                return false;
            }




        }

        /// <summary
        /// Checks if current reservation time is not already reserved by an excisting reservation
        /// </summary>
        /// <param name="time">Type=DateTime variable is the arrival time of the current reservation</param>
        /// <returns>Returns if there is a bool. True: no overlap of current reservation time with excisting reservations False: Overlap of current reservation time with excisting reservations </returns>
        public bool CheckTime(DateTime time)
        {


            for (int i = 0; i < 100; i++)
            {
                DateTime prevResLeavingTime = listReservationDate[i].
                    _resArrivingTime.AddHours(3);
                DateTime currentResLeavingTime = time.AddHours(3);

                //if the time our guest is arraving 
                //is earlier than the previous guest leaving time
                if (time < listReservationDate[i]._resArrivingTime)
                {
                    if (time < prevResLeavingTime)
                    {
                        if (currentResLeavingTime > listReservationDate[i + 1]._resArrivingTime)
                        {
                            listReservationDateTime[i]= listReservationDate[i];

                        }

                    }

                }

            }

            if (listReservationDateTime.First() == null)
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
