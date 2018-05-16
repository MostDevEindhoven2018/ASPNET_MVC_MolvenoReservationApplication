using ASPNET_MVC_MolvenoReservationApplication.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TableManagerTests
{
   /* The first few tests appear to be not working. There are too many answers. That is because we now also add solutions that 
    * go 'one over'. if you want to check for these tests to go green, change the code of the solutionFinderVersion3 at 
    * line 52 to == instead of <=
    */
    [TestClass]
    public class SolutionFinderTests
    {

        private SolutionFinderVersion3 VersionOne = new SolutionFinderVersion3();
        private SolutionCountFinder CountFinder = new SolutionCountFinder();

        [TestInitialize]
        public void Initialize()
        {
        }


        [TestMethod]
        public void GetSolutions1()
        {

            List<int> availableCapsList = new List<int> { 10, 5, 2, 1 };
            int[] availableCapsArray= new int[]{ 10, 5, 2, 1 };
            int N = 200;
            List<List<int>> result = new List<List<int>>();
            
                result = VersionOne.GetSolutions(availableCapsList, N);

            
           
            
            Assert.AreEqual(CountFinder.CountWays(availableCapsArray, availableCapsArray.Length, N), result.Count);

        }

        [TestMethod]
        public void GetSolutions2()
        {

            List<int> availableCaps = new List<int> { 6, 4, 2 };
            int N = 12;
            List<List<int>> result = new List<List<int>>();

            result = VersionOne.GetSolutions(availableCaps, N);

            // All solutions are (6,6) (6,4,2) (6,2,2,2) (4,4,4) (4,4,2,2) (4,2,2,2,2), (2,2,2,2,2,2)

            Assert.AreEqual(7, result.Count);

        }

        [TestMethod]
        public void GetSolutionsCountUsingCountFinder1()
        {

            List<int> availableCapsList = new List<int> { 6, 4, 2 };
            int[] availableCapsArray = new int[] { 6, 4, 2 };
            int N = 20;
            List<List<int>> result = new List<List<int>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start(); 
            result = VersionOne.GetSolutions(availableCapsList, N);
            stopwatch.Stop();

            Assert.AreEqual(CountFinder.CountWays(availableCapsArray, availableCapsArray.Length, N), result.Count);
        }

        [TestMethod]
        public void GetSolutionsCountUsingCountFinder2()
        {
            List<int> availableCapsList = new List<int> { 6, 4, 2 };
            int[] availableCapsArray = new int[] { 6, 4, 2 };
            int N = 100;
            List<List<int>> result = new List<List<int>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            result = VersionOne.GetSolutions(availableCapsList, N);
            stopwatch.Stop();

            //Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetSolutionsForOne()
        {
            List<int> availableCapsList = new List<int> { 6, 4, 2 };
            int[] availableCapsArray = new int[] { 6, 4, 2 };
            int N = 1;
            List<List<int>> result = new List<List<int>>();

            var stopwatch = new Stopwatch();
           
            result = VersionOne.GetSolutions(availableCapsList, N);
            // A
            Assert.AreEqual(3, result.Count);
        }
    }
}

