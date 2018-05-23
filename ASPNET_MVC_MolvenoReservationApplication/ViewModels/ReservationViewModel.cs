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
        public DateTime arrival { get; set; }

        public string ArrivingDate { get; set; }
        
        public int ArrivingHour { get; set; }

        public int ArrivingMinute { get; set; }

        public int Partysize { get; set; }

        public string GuestName { get; set; }

        public string GuestPhone { get; set; }

        public string GuestEmail { get; set; }

        public bool Hideprices { get; set; }

        public string ResComments { get; set; }

        public List<int> PossibleReservationHours { get; set; }
    }
}
