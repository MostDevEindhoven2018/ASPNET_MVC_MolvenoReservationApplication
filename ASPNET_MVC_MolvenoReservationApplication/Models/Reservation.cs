using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.Models
{
    public class Reservation
    {

        public int ReservationID { get; set; }

        [Display(Name = "ReservationTableCouplings", AutoGenerateField = false)]
        public virtual ICollection<ReservationTableCoupling> _resReservationTableCouplings { get; set; }

        [Display(Name = "Guest", AutoGenerateField = false)]
        public Guest _resGuest { get; set; }

        [Display(Name = "Party size", AutoGenerateField = true)]
        [Required(ErrorMessage = "Please enter the amount of people attending.")]
        public int _resPartySize { get; set; }


        [Display(Name = "DateOfReservation", AutoGenerateField = true)]
        [Required(ErrorMessage = "Please pick a date.")]
        public DateTime _resArrivingTime { get; set; }


        // This now gets hardcoded. This will need to be changed so a config file sets this default.
        [Display(Name = "DurationOfReservation", AutoGenerateField = false)]
        public int _resDurationOfReservation { get; set; } = 3;


        public DateTime _resLeavingTime
        {
            get
            {
                return _resArrivingTime.AddHours(_resDurationOfReservation);
            }
        }

        [Display(Name = "Hide prices", AutoGenerateField = true)]
        public bool _resHidePrices { get; set; }

        [Display(Name = "Comments", AutoGenerateField = true)]
        [DataType(DataType.MultilineText)]
        public string _resComments { get; set; }

        public Reservation()
        {

        }



        /// <summary>
        /// Reservation constructor. The default leaving time is (arriving time) + 3 hours.
        /// </summary>
        /// <param name="partySize"></param>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        public Reservation(int partySize, DateTime arrivingTime)
        {
            _resPartySize = partySize;
            _resArrivingTime = arrivingTime;
        }

        /// <summary>
        /// Reservation constructor with custom duration as input.
        /// </summary>
        /// <param name="partySize">int. The number of people attending the event.</param>
        /// <param name="arrivingDateTime">datetime. The arriving date and time.</param>
        /// <param name="leavingDateTime">datetime. The leaving date and time.</param>
        public Reservation(int partySize, DateTime arrivingTime, int duration)
        {
            _resPartySize = partySize;
            _resArrivingTime = arrivingTime;
            _resDurationOfReservation = duration;
        }


        /// <summary>
        /// Reservation constructor with an added table. Please note that this will be converted to a ReservationTableCoupling and only then added.
        /// </summary>
        /// <param name="partySize"></param>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="duration"></param>
        /// <param name="table">will be converted to a ReservationTableCoupling</param>
        /// <param name="guest"></param>
        public Reservation(int partySize, DateTime arrivingTime, int duration, Guest guest)
        {
            _resPartySize = partySize;
            _resArrivingTime = arrivingTime;
            _resDurationOfReservation = duration;
            _resGuest = guest;
        }
    }
}
