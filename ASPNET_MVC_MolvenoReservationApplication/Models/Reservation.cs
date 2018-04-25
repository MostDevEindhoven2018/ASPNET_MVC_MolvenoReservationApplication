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

        /// <summary>
        /// Reservation constructor. The default leaving time is (arriving time) + 3 hours.
        /// </summary>
        /// <param name="partySize">int. The number of people attending the event.</param>
        /// <param name="arrivingDateTime">datetime. The arriving date and time.</param>
        public Reservation(int partySize, DateTime arrivingDateTime)
        {
            this._resPartySize = partySize;
            this._resArrivingTime = arrivingDateTime;
            this._resLeavingTime = arrivingDateTime.AddHours(3d);
        }

        /// <summary>
        /// Reservation constructor with custom leaving time as input.
        /// </summary>
        /// <param name="partySize">int. The number of people attending the event.</param>
        /// <param name="arrivingDateTime">datetime. The arriving date and time.</param>
        /// <param name="leavingDateTime">datetime. The leaving date and time.</param>
        public Reservation(int partySize, DateTime arrivingDateTime, DateTime leavingDateTime)
        {
            this._resPartySize = partySize;
            this._resArrivingTime = arrivingDateTime;
            this._resLeavingTime = leavingDateTime;
        }
    }
}
