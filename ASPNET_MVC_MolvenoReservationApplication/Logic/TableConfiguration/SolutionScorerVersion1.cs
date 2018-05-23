using ASPNET_MVC_MolvenoReservationApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public class SolutionScorerVersion1 : ISolutionScorer
    {
        public List<int> GetBestTableConfiguration(List<List<int>> ViableSolutions, int PartySize)
        {
            // Overkill is how much room is unoccupied using this solution. It is defined as Overall Capacity (of the solution
            // as a whole) - Initial Party Size
            int Overkill;


            Dictionary<List<int>, double> Scores = new Dictionary<List<int>, double>();
            // Here we need to implement the scoring system with the rules we came up with
            // We want the solution with:

            // The least amount of tables
            // the least variability between those tables
            // the least 'overkill' on summed capacities and partysize

            // the scoring weights used here are completely arbitrary.
            double CountWeight = 10;
            double OKWeight = 0.2;
            double SDWeight = 0.05;
            foreach (List<int> config in ViableSolutions)
            {
                // calculate its Overall Capacity
                int OC = config.Sum();
                Overkill = OC - PartySize;

                double score = CountWeight * (1/(double)config.Count) - SDWeight * StandardDeviation(config) - OKWeight * Overkill;


                Scores.Add(config, score);
            }
            
            // Order the dictionary, select only the keys (the configs) and convert to list.
            List<List<int>> OrderedConfigs = Scores.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();

            return OrderedConfigs.First();
        }

        private double StandardDeviation(List<int> valueList)
        {
            if (valueList.Count > 1)
            {
                double M = 0.0;
                double S = 0.0;
                int k = 0;
                foreach (double value in valueList)
                {
                    k++;
                    double tmpM = M;
                    M += (value - tmpM) / k;
                    S += (value - tmpM) * (value - M);
                }
                return Math.Sqrt(S / (k - 1));
            }
            // The standard deviation of a single number is not possible and is set to NaN. 
            // For our purposes though we want it to be 0 as a single number should always be the best solution.
            else
            {
                return 0;
            }

        }
    }
}
