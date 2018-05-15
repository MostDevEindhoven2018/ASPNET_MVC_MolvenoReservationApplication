using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TableManagerTests
{

    [TestClass]
    public class SolutionFinderTests
    {

        private SolutionFinderMock VersionOne = new SolutionFinderMock();
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
            
                result = VersionOne.GetTableConfigurations(availableCapsList, N);

            
           
            
            Assert.AreEqual(CountFinder.CountWays(availableCapsArray, availableCapsArray.Length, N), result.Count);

        }

        [TestMethod]
        public void GetSolutions2()
        {

            List<int> availableCaps = new List<int> { 6, 4, 2 };
            int N = 12;
            List<List<int>> result = new List<List<int>>();

            result = VersionOne.GetTableConfigurations(availableCaps, N);

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
            result = VersionOne.GetTableConfigurations(availableCapsList, N);
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
            result = VersionOne.GetTableConfigurations(availableCapsList, N);
            stopwatch.Stop();

            //Assert.AreEqual(1, result.Count);
        }
    }
}

