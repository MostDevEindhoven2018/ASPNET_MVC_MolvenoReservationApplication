using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.ViewModels
{
    public class GuestViewModel
    {
        public DateTime arrival { get; set; }

        public int size { get; set; }

        public string GuestName { get; set; }

        public string GuestPhone { get; set; }

        public string GuestEmail { get; set; }

        public bool Hideprices { get; set; }

        public string ResComments { get; set; }
    }
}
