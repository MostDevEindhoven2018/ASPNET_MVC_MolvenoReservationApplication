using ASPNET_MVC_MolvenoReservationApplication.Models;
using System.Collections.Generic;
using System.Linq;

namespace GetFreeTablesTests
{
    internal class GetTableConfigurationMock
    {
        public List<List<int>> GetTableConfigurations(List<int> tableCaps, int partySize)
        {
            List<List<int>> Solutions = new List<List<int>>();
            List<int> currentSolution = new List<int>();

            Recurse(partySize, tableCaps, 0, currentSolution, Solutions);

            return Solutions;
        }

        private void Recurse(int N, List<int> tableCaps, int index, List<int> currentSolution, List<List<int>> solutions)
        {
            int result = N;

            if (index < tableCaps.Count)
            {
                // subtract the current table cap from the total and see what happens
                result = N - tableCaps[index];
                currentSolution.Add(index);

                // do the recursion like norma
                // If the result is more than zero, we might be able to subtract the same number again. Lets try!
                if (result > 0)
                {
                    Recurse(result, tableCaps, index, currentSolution, solutions);
                }

                // If the result is exactly zero, we found a solution! Add it to the list of solutions. Then, look for
                // more solutions by undoing the last step and continuing in a new branch.
                else
                {
                    if (result == 0)
                    {
                        List<int> ValidSolution = new List<int>(currentSolution);
                        solutions.Add(ValidSolution);
                    }

                    // undo last step by  adding the last subtraction back to the total
                    result += tableCaps[currentSolution[currentSolution.Count - 1]];
                    // and removing the last item in the current solution.
                    currentSolution.RemoveAt(currentSolution.Count - 1);

                    // Now it is like we are one step up the recursion tree again. lets take the next branch 
                    // by increasing the index by one and calling the method again.
                    index += 1;
                    Recurse(result, tableCaps, index, currentSolution, solutions);
                }
            }
            else
            {
                // we went over the index cap, meaning we 'completed' this branch.
                // lets navigate not one up and go to the next, but two up and and go to the next. 
                // Because we saved all steps we did in the currentsolution, we can use it to retrace our path
                // if we take the last entry in our solution, let the index be (that +1) and then remove the last entry
                // and its effect on N, we continue to the next branch.

                if (currentSolution.Count > 0)
                {
                    index = currentSolution[currentSolution.Count - 1] + 1;
                    result += tableCaps[currentSolution[currentSolution.Count - 1]];
                    currentSolution.RemoveAt(currentSolution.Count - 1);

                    Recurse(result, tableCaps, index, currentSolution, solutions);
                }
                else
                {
                    return;
                }
            }
        }
        
        public List<Table> GetTableConfiguration(List<Table> freeTables, int partySize)
        {
            List<Table> TableConfiguration = new List<Table>();
            // In this version we try to first get all possible solutions, then get all solutions that we actually have
            // the tables for, and then get the best solutions.

            // Get array of all distinct capacities in the free tables list and order ascendingly
            int[] DistinctTableCaps = freeTables.Select(table => table._tableCapacity).Distinct().OrderByDescending(c => c).ToArray();


            // get a dictionary with keys distinct caps and value actual amount of tables.
            // This will be used to determine whether or not we have sufficient tables for particular solutions.

            Dictionary<int, int> CapsAmounts = new Dictionary<int, int>();
            foreach (int cap in DistinctTableCaps)
            {
                int amountOfThisCapacity = freeTables.Where(table => table._tableCapacity == cap).Count();
                CapsAmounts.Add(cap, amountOfThisCapacity);
            }

            return TableConfiguration;
        }
    }
}



