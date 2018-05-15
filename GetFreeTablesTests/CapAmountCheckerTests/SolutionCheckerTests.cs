using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ASPNET_MVC_MolvenoReservationApplication.Logic;
using ASPNET_MVC_MolvenoReservationApplication.Models;

namespace TableManagerTests
{
    [TestClass]
    public class SolutionCheckerTests
    {
        // This class will check a given solution against a list of free tables and see whether or not the solution
        // is viable given the actual amounts of free tables. No direct connections to databases are required in the method
        // so no mocks are made.

        List<Table> freetables = new List<Table>();
        TableManager _tableManager = new TableManager();
        ISolutionChecker _solutionChecker = new SolutionCheckerVersion1();


        [TestInitialize]
        public void Initialize()
        {

            // Create a set of tables by entering the caps and amounts, then adding them to a list.
            int[] CapsArray = new int[]
            {
                8,6,4,2
            };

            int[] AmountsArray = new int[]
            {
                4,3,2,1
            };

            for (int i = 0; i < CapsArray.Length; i++)
                for (int j = 0; j < AmountsArray[i]; j++)
                {
                    freetables.Add(new Table(CapsArray[i]));
                }
        }

        [TestMethod]
        public void DictionaryCheck()
        {
            Dictionary<int, int> CapsAmountsDict = _tableManager.GetAvailabilityDictionary(freetables);

            Dictionary<int, int> TestReference = new Dictionary<int, int>
            {
                {8,4 },
                {6,3 },
                {4,2 },
                {2,1 }

            };


            CollectionAssert.AreEqual(TestReference, CapsAmountsDict);

        }

        [TestMethod]
        public void ViableSolutionsCheckTest()
        {
            Dictionary<int, int> CapsAmountsDict = _tableManager.GetAvailabilityDictionary(freetables);

            /* For Reference
                {8,4 },
                {6,3 },
                {4,2 },
                {2,1 }
                */
            List<int> i1 = new List<int> { 8, 4, 2 };
            List<int> i2 = new List<int> { 8, 6, 6, 6, 6, 4, 2 };
            List<int> i3 = new List<int> { 8, 4, 2, 2, 2, 2 };
            List<int> i4 = new List<int> { 8 };
            List<int> i5 = new List<int> { 8, 8, 8, 8, 6, 6, 6, 4, 4, 2 };
            List<List<int>> AllSolutions = new List<List<int>>()
            {
                i1,     // correct
                i2,     // incorrect (too many 6's)
                i3,     // incorrect (too many 2's)
                i4,     // correct
                i5      // correct

            };

            List<List<int>> Result = _solutionChecker.GetViableSolutions(AllSolutions, CapsAmountsDict);

            List<List<int>> Expected = new List<List<int>>
            {
                i1,
                i4,
                i5
            };

            CollectionAssert.AreEqual(Expected, Result);

        }
    }
}

