using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication 
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public virtual Table _resTable { get; set; }
        public Guest _resGuest {  get; set; }
        public int _resPartySize {  get; set; }
        public DateTime _resArrivingTime {  get; set; }
        public DateTime _resLeavingTime {  get; set; }
        public bool _resHidePrices { get; set; }
        public string _resComments { get; set; }
        
    }
}
