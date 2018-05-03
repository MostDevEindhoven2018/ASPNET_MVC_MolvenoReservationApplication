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

        public int ReservationID { get; set; }

        public int table { get; set; }
        
        public string name { get; set; }

        public string phone { get; set; }

        public string email { get; set; }
        
        public int partysize { get; set; }
        
        public DateTime arrivingdate { get; set; }
        
        public DateTime leavingtime { get; set; }
        
        public bool hideprices { get; set; }
        
        public string comments { get; set; }

    }
}
