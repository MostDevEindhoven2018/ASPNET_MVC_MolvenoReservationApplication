using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication 
{
    public class Reservation
    {

        public int ReservationID { get; set; }

        [DataType(DataType.Custom)]
        public virtual Table _resTable { get; set; }

        [DataType(DataType.Custom)]
        public Guest _resGuest {  get; set; }

        [Required(ErrorMessage = "Please enter the amount of people attending.")]
        public int _resPartySize {  get; set; }

        [Required(ErrorMessage ="Please enter your date and time of arrival.")]
        [DataType(DataType.DateTime)]
        public DateTime _resArrivingTime {  get; set; }

        [DataType(DataType.DateTime)]
        public DateTime _resLeavingTime {  get; set; }

        public bool _resHidePrices { get; set; }

        [DataType(DataType.MultilineText)]
        public string _resComments { get; set; }

    }
}
