using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_MVC_MolvenoReservationApplication
{
    public class Guest
    {
        public int _guestId { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage ="Please enter your name since it is required in order to make a reservation")]
        public string _guestName { get; set; }

        [StringLength(13)]
        public string _guestPhone { get; set; }

        [StringLength(40)]
        public string _guestEmail { get; set; }
        
    }
}
