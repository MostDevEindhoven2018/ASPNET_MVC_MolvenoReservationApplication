using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.ViewModels
{
    public class IndexViewModel
    {

        //Each property from each object should be included in the ViewModel
        //No need for data annotations

        public int ReservationID { get; set; }

        //public int TableID { get; set; }

        //public int GuestID { get; set; }

        public DateTime Arrivingdate { get; set; }

        public string dtpart { get; set; }
            

        public string tpart { get; set; }

        public int ArrivingHour { get; set; }

        public int ArrivingMinute { get; set; }

        //public DateTime leavingtime { get; set; }

        public int Partysize { get; set; }

        public string GuestName { get; set; }

        public string GuestPhone { get; set; }

        public string GuestEmail { get; set; }

        public bool Hideprices { get; set; }

        public string ResComments { get; set; }

        public int tableID { get; set; }

    }
}
