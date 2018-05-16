using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public class SolutionCheckerVersion1 : ISolutionChecker
    {
        
        public List<List<int>> GetViableSolutions(List<List<int>> AllSolutions, Dictionary<int,int> tableCapAmounts)
        {
            // now check every solution for the amount of tables they use and whether or not that is more 
            // than we have.
            List<List<int>> ViableSolutions = new List<List<int>>(AllSolutions);

            foreach (List<int> solution in AllSolutions)
            {
                
                List<int> UsedCapacities = solution.Select(x => x).Distinct().OrderByDescending(x => x).ToList();

                foreach (int key in UsedCapacities)
                {
                    // if the amount of free tables in the actual restaurant is lower than are used in the
                    // solution, remove it from the list.
                    if (tableCapAmounts[key] < solution.Where(x => x == key).Count())
                    {
                        ViableSolutions.Remove(solution);
                    }
                }
            }
            return ViableSolutions;
        }
    }
}
