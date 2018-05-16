using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ASPNET_MVC_MolvenoReservationApplication.Logic;

namespace TableManagerTests
{
    [TestClass]
    public class OptimalConfigFinderTests
    {
        ISolutionScorer _scorer = new SolutionScorerVersion1();
        ISolutionFinder _finder = new SolutionFinderVersion3();

        List<List<int>> ViableSolutions;
        int PartySize;
        [TestInitialize]
        public void Initialize()
        {
            PartySize = 12;
            // get some solutions for N = 12 with 8 ,6 4 and 2 tables
            // which are {8,4} {8,2,2} {6,6} {6,4,2} { 6,2,2,2} {4,4,4} {4,4,2,2} {4,2,2,2,2} and {2,2,2,2,2,2}
            ViableSolutions = _finder.GetSolutions(new List<int> { 8, 6, 4, 2 }, PartySize);
        }

        [TestMethod]
        public void GetBestResult1()
        {
            List<int> Result = _scorer.GetBestTableConfiguration(ViableSolutions, PartySize);
            List<int> Expected = new List<int>
            {
                6,6
            };

            CollectionAssert.AreEqual(Expected, Result);
        }

        [TestMethod]
        public void GetBestResult2()
        {

            List<int> i1 = new List<int> { 8, 4, 2 };
            List<int> i2 = new List<int> { 8, 6, 6, 6, 6, 4, 2 };
            List<int> i3 = new List<int> { 8, 4, 2, 2, 2, 2 };
            List<int> i4 = new List<int> { 8 };
            List<int> i5 = new List<int> { 8, 8, 8, 8, 6, 6, 6, 4, 4, 2 };
            ViableSolutions = new List<List<int>>
            {
                i1,
                i2,
                i3,
                i4,
                i5

            };
            List<int> Result = _scorer.GetBestTableConfiguration(ViableSolutions, PartySize);
            List<int> Expected = i4;

            CollectionAssert.AreEqual(Expected, Result);
        }
    }
}
