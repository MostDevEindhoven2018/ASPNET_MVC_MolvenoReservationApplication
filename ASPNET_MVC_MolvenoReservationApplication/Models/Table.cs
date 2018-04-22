using ASPNET_MVC_MolvenoReservationApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication
{
    public class Table
    {
        public int TableID { get; set; }
        public int _tableCapacity { get; set; }
        public TableAreas _tableArea { get; set; }

        public Table() { }

        public Table(int capacity)
        {
            this._tableCapacity = capacity;
        }

        public Table(int capacity, TableAreas area)
        {
            this._tableCapacity = capacity;
            this._tableArea = area;
        }
    }
}
