using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public class TableConfigurationFinderVersion2 : ITableConfigurationFinder
    {
        

        public List<Table> GetTableConfiguration(List<Table> freeTables, int partySize)
        {
            List<Table> TableConfiguration = new List<Table>();


            // In this version we at first do not care about the actual tables we have. Only take the capacities
            // and act as if we have unlimited tables of that size. So a solution for a partysize of 8 might be 2,2,2,2
            // even though we only have 3 tables of cap 2.

            

            

            return TableConfiguration;
        }
    }
}
