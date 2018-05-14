using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GetFreeTablesTests
{

    [TestClass]
    public class GetTableConfigurationTests
    {

        private GetTableConfigurationMock VersionOne = new GetTableConfigurationMock();

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void GetSolutions1()
        {
           
            List<int> availableCaps = new List<int> { 5, 3, 2 };
            int N = 10;
            List<List<int>> result = new List<List<int>>();

            result = VersionOne.GetTableConfigurations(availableCaps, N);

            // Here I want the answers (5,5) (5,3,2) and (2,2,2,2,2), so count == 3

            Assert.AreEqual(4, result.Count);

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
        public void GetSolutions3()
        {

            List<int> availableCaps = new List<int> { 6, 4, 2 };
            int N = 20;
            List<List<int>> result = new List<List<int>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            result = VersionOne.GetTableConfigurations(availableCaps, N);
            stopwatch.Stop();


            Assert.AreEqual(20, result.Count);

            
            

        }

    }
}

