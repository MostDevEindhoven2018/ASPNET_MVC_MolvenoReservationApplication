using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;


namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public interface ITableConfigurationFinder
    {
        List<Table> GetTableConfiguration(List<Table> freeTables, int partySize);
    }
}
