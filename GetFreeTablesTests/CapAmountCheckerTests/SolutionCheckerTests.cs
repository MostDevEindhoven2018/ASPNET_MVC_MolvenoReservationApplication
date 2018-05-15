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
            List<int> freetables = new List<int>();

            for (int i = 0; i < CapsArray.Length; i++)
                for (int j = 0; j < AmountsArray[i]; i++)
                {
                    freetables.Add(i);
                }
        }



        [TestMethod]
        public void Test1()
        {
            int o = 0;
        }
    }
}

