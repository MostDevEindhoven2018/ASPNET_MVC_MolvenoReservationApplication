using ASPNET_MVC_MolvenoReservationApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public interface IFreeTableFinder
    {
        List<Table> GetFreeTables(DateTime start, DateTime end);
    }
}
