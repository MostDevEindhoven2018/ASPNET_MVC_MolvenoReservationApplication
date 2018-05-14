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

            Recurse2(partySize, tableCaps, 0, currentSolution, Solutions);

            return Solutions;
        }
        
        private void Recurse2(int N, List<int> tableCaps, int index, List<int> currentSolution, List<List<int>> solutions)
        {
            // Only read the ...comments after going through the entire method at least once.
            // ... So now we are back here again in the for loop. The first complete recursion is done. 
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
                    if (result == 0)
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


        // Earlier Attempt. This sort of works, but instead of actually recursing, it goes deeper with every step
        // as we are never returning to the top. Therefore the stack is never cleared and overflows start to arise
        // at about N = 75 and tablecaps.count = 3. Look at the logic though and try to get why the algorithm above 
        // works better.

        //private void Recurse(int N, List<int> tableCaps, int index, List<int> currentSolution, List<List<int>> solutions)
        //{
            
        //    int result = N;

        //    if (index < tableCaps.Count)
        //    {
        //        // subtract the current table cap from the total and see what happens
        //        result = N - tableCaps[index];
        //        currentSolution.Add(index);

        //        // do the recursion like norma
        //        // If the result is more than zero, we might be able to subtract the same number again. Lets try!
        //        if (result > 0)
        //        {
        //            Recurse(result, tableCaps, index, currentSolution, solutions);
        //        }

        //        // If the result is exactly zero, we found a solution! Add it to the list of solutions. Then, look for
        //        // more solutions by undoing the last step and continuing in a new branch.
        //        else
        //        {
        //            if (result == 0)
        //            {
        //                List<int> ValidSolution = new List<int>(currentSolution);
        //                solutions.Add(ValidSolution);
        //            }

        //            // undo last step by  adding the last subtraction back to the total
        //            result += tableCaps[currentSolution[currentSolution.Count - 1]];
        //            // and removing the last item in the current solution.
        //            currentSolution.RemoveAt(currentSolution.Count - 1);

        //            // Now it is like we are one step up the recursion tree again. lets take the next branch 
        //            // by increasing the index by one and calling the method again.
                 
                    
        //            Recurse(result, tableCaps, index+1, currentSolution, solutions);
        //        }
        //    }
        //    else
        //    {
        //        // we went over the index cap, meaning we 'completed' this branch.
        //        // lets navigate not one up and go to the next, but two up and and go to the next. 
        //        // Because we saved all steps we did in the currentsolution, we can use it to retrace our path
        //        // if we take the last entry in our solution, let the index be (that +1) and then remove the last entry
        //        // and its effect on N, we continue to the next branch.

        //        if (currentSolution.Count > 0)
        //        {
        //            index = currentSolution[currentSolution.Count - 1] + 1;
        //            result += tableCaps[currentSolution[currentSolution.Count - 1]];
        //            currentSolution.RemoveAt(currentSolution.Count - 1);

        //            Recurse(result, tableCaps, index, currentSolution, solutions);
        //        }
               
        //    }
            
        //}
    }
}



