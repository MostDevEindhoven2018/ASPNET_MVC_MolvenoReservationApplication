using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public class SolutionFinderVersion3 : ISolutionFinder
    {
        public List<List<int>> GetSolutions(List<int> tableCaps, int partySize)
        {
            List<List<int>> Solutions = new List<List<int>>();
            List<int> currentSolution = new List<int>();

            Recurse2(partySize, tableCaps, 0, currentSolution, Solutions);

            return Solutions;
        }

        private void Recurse2(int N, List<int> tableCaps, int index, List<int> currentSolution, List<List<int>> solutions)
        {
            // Only read the ...comments after going through the entire method at least once.
            // ... So now we are back here again in the for loop. The first complete branch is done. 
            // ... We now do everything a couple of times more for the rest of the loop and when that is all
            // ... done we are done completely and are left with a fully filled list of solutions.

            // go into all the branches in this particular depth.
            for (int i = index; i < tableCaps.Count; i++)
            {
                // ... It is important to note here that after going through the entire thing once, here the result
                // ... gets reset to the original one we initially asked for. So we start over from the top.
                int result = N;

                // subtract the current table cap from the total and see what happens
                result = N - tableCaps[i];

                // Add the table cap we just subtracted to the current solution so we end up with a list
                // of all used table capacities.
                currentSolution.Add(tableCaps[i]);

                if (result > 0)
                {
                    // if the result is more than 0, we can try to subtract again. We call the function
                    // again with the new result (which will be the N in subsequent recursions).
                    // This will happen and we will go deeper and deeper untill the result will fall under 
                    // (or equal) 0.
                    Recurse2(result, tableCaps, i, currentSolution, solutions);
                }
                else
                {
                    // if not, we reached the end of a branch. So we need to get back to the top.
                    if (result <= 0)
                    {
                        // If the result was exactly 0, we found a solution to our problem. So lets
                        // save it.
                        List<int> ValidSolution = new List<int>(currentSolution);
                        solutions.Add(ValidSolution);
                    }
                }
                // We are almost done here. Lets remove the lastly added tablecap to undo everything we did in 
                // this particular branch (except saving any found solutions of course). Now read the comments in
                // the start of the method.
                currentSolution.Remove(tableCaps[i]);
            }
        }
    }
}

