using ASPNET_MVC_MolvenoReservationApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_MVC_MolvenoReservationApplication.Models
{
    public class Table
    {
        public int TableID { get; set; }


        
        [Required]
        [Display(Name = "Capacity", AutoGenerateField = true)]
        [Range(1, 50, ErrorMessage = "Between 1 and 50 pleople can fit to a table.")]
        public int _tableCapacity { get; set; }

        /// <summary>
        /// enum. The area in which the table will be placed. Defaults to: Main area.
        /// </summary>
        /// 
        // What is the default?
        [Display(Name = "Area", AutoGenerateField = true)]
        public TableAreas _tableArea { get; set; }


        [Display(Name = "Reservations", AutoGenerateField = false)]
        public virtual ICollection<ReservationTableCoupling> _tableReservationTableCouplings { get; set; }

        public Table() { }

        /// <summary>
        /// Constructor. Defaults the table area to 'Main'.
        /// </summary>
        /// <param name="capacity">The max capacity of the table.</param>
        public Table(int capacity)
        {
            this._tableCapacity = capacity;
            this._tableArea = TableAreas.Main;
        }

        public Table(int capacity, TableAreas area)
        {
            this._tableCapacity = capacity;
            this._tableArea = area;
        }
    }
}
