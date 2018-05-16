using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.ViewModels
{
    public class ReservationViewModel
    {
        //Each property from each object should be included in the ViewModel
        //No need for data annotations
        public string ArrivingDate { get; set; }
        
        public int ArrivingHour { get; set; }

        public int ArrivingMinute { get; set; }

        public int Partysize { get; set; }
    }
}
