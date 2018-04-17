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
        public int _tableId { get; set; }
        public int _tableCapacity { get; set; }
        public TableAreas MyProperty { get; set; }
    }
}
