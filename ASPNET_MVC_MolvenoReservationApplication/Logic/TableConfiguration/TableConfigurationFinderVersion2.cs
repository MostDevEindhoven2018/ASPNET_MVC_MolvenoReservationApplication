using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public class TableConfigurationFinderVersion2 : ITableConfigurationFinder
    {
        //public List<Table> GetBestTableConfiguration(List<Table> freeTables, int partySize)
        //{
        //    List<int> TableCaps = GetDescendingTableCapacities(freeTables);
        //    List<List<int>> AllSolutions = GetSolutions(TableCaps, partySize);


        //    return 
        //}


        private List<int> GetDescendingTableCapacities(List<Table> freeTables)
        {
            return freeTables.Select(table => table._tableCapacity).Distinct().OrderByDescending(x => x).ToList();
        }




        public List<List<int>> GetViableSolutions(List<List<int>> AllSolutions, List<Table> freeTables)
        {
            List<int> TableCaps = GetDescendingTableCapacities(freeTables);
            Dictionary<int, int> TableCapAmounts = new Dictionary<int, int>();

            List<List<int>> SolutionsUsingCapacities = new List<List<int>>();

            // Fill the dictionary with Keys TableCapacities and Values How many free tables of that capacity
            // are present.

            foreach (int cap in TableCaps)
            {
                TableCapAmounts.Add(cap, freeTables.Where(table => table._tableCapacity == cap).Count());
            }


            // get a list of all solutions with instead of the indexes in the tablecaps list, the actual table 
            // themselves.
            foreach (List<int> Solution in AllSolutions)
            {
                List<int> SolutionUsingCapacities = new List<int>();

                for (int i = 0; i < Solution.Count; i++)
                {
                    SolutionUsingCapacities.Add(TableCaps[Solution[i]]);
                }
            }

            // now check every solution for the amount of tables they use and whether or not that is more 
            // than we have. Create a dict for each solution and 
            foreach (List<int> SolutionUsingCapacities in SolutionsUsingCapacities)
            {

                List<int> UsedCapacities = SolutionUsingCapacities.Select(x => x).Distinct().OrderByDescending(x => x).ToList();

                foreach (int key in UsedCapacities)
                {
                    if (TableCapAmounts[key] < SolutionUsingCapacities.Where(x => x == key).Count())
                    {
                        SolutionsUsingCapacities.Remove(SolutionUsingCapacities);
                    }
                }
            }
            return SolutionsUsingCapacities;
        }

        public List<Table> GetBestTableConfiguration(List<List<Table>> ViableSolutions)
        {
            return new List<Table>();
        }
    }
}
