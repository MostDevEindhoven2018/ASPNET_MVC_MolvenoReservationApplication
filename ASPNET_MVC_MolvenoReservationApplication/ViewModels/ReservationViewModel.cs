using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPNET_MVC_MolvenoReservationApplication.ViewModels
{
    public class ReservationViewModel
    {

        //Each property from each object should be included in the ViewModel
        //No need for data annotations

        public int ReservationID { get; set; }

        //public int TableID { get; set; }

        //public int GuestID { get; set; }

        public DateTime Arrivingdate { get; set; }

        public int ArrivingHour { get; set; }

        public int ArrivingMinute { get; set; }

        //public DateTime leavingtime { get; set; }

        public int Partysize { get; set; }

        public string GuestName { get; set; }

        public string GuestPhone { get; set; }

        public string GuestEmail { get; set; }
        
        public bool Hideprices { get; set; }
        
        public string ResComments { get; set; }

        //public int PossibleReservationHoursID { get; set; }
        //public IEnumerable<SelectListItem> PossibleReservationHours { get; set; }

        public List<int> PossibleReservationHours { get; set; }
    }
}
