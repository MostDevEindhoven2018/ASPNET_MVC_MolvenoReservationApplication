using System.ComponentModel.DataAnnotations;

namespace ASPNET_MVC_MolvenoReservationApplication.Models
{
    public class ReservationTableCoupling
    {
        public ReservationTableCoupling()
        {
        }

        public ReservationTableCoupling(Reservation res, Table table)
        {
            Reservation = res;
            Table = table;
        }

        [Key]
        public int ReservationTableCouplingID { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual Table Table { get; set; }
    }
}