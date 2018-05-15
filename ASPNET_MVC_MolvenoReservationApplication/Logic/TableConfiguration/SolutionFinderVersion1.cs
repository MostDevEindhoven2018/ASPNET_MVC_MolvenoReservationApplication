using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public class SolutionFinderVersion1 : ISolutionFinder
    {
        public List<List<int>> GetSolutions(List<int> tableCaps, int partySize)
        {
            List<List<int>> Solutions = new List<List<int>>();
            List<int> currentSolution = new List<int>();

            Recurse(partySize, tableCaps, 0, currentSolution, Solutions);

            return Solutions;
        }
        // Earlier Attempt. This sort of works, but instead of actually recursing, it goes deeper with every step
        // as we are never returning to the top. Therefore the stack is never cleared and overflows start to arise
        // at about N = 75 and tablecaps.count = 3. Look at the logic though and try to get why the algorithm  
        // of version 3 works better.
        
        private void Recurse(int N, List<int> tableCaps, int index, List<int> currentSolution, List<List<int>> solutions)
        {
            //// Disclaimer: In this function I often get the last item of the list by using list[list.count -1].
            //// I know this is suboptimal and list.LastOrDefault() is less error prone. I used this syntax
            //// to keep consistency with the list.removeAt(list.count - 1) method that is called multiple times.
            //// I found it easier to read the code whilst having the suboptimal (but consistent) way. 
            int result = N;

            if (index < tableCaps.Count)
            {
                // subtract the current table cap from the total and see what happens
                result = N - tableCaps[index];
                currentSolution.Add(index);

                // do the recursion like normal
                // If the result is more than zero, we might be able to subtract the same number again. Lets try!
                if (result > 0)
                {
                    Recurse(result, tableCaps, index, currentSolution, solutions);
                }

                // If the result is exactly zero, we found a solution! Add it to the list of solutions. Then, look for
                // more solutions by undoing the last step and continuing in a new branch.
                else
                {
                    if (result <= 0)
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
                    // We are at the root and went over the index maximum. This means we are done.
                    return;
                }
            }
        }
    }
}
