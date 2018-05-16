using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Controllers;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.Sqlite;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    /// <summary>
    /// This is main hub where everything for getting the best table configuration is done. The larger methods
    /// are split up within components. A few small helper functions reside in the manager itself 
    /// (stuff like get all table capacities of the free tables and creating the dictionary with the actual number 
    /// of these tables)
    /// </summary>
    public class TableManager
    {
        MyDBContext _context { get; set; }
        private IFreeTableFinder _freeTableFinder { get; set; }
        private ISolutionFinder _solutionFinder { get; set; }
        private ISolutionChecker _solutionChecker { get; set; }
        private ISolutionScorer _solutionScorer { get; set; }

        /// <summary>
        /// This one is basically for testing purposes only as 
        /// it is not connected to a database. Do not use it outside of testing.
        /// </summary>
        public TableManager()
        {
            _solutionFinder = new SolutionFinderVersion3();
            _solutionChecker = new SolutionCheckerVersion1();
            _solutionScorer = new SolutionScorerVersion1();
        }
        /// <summary>
        /// This one is basically for testing purposes only as 
        /// it is not connected to a database. Do not use it outside of testing.
        /// </summary>
        public TableManager(IFreeTableFinder ftf) {
            _freeTableFinder = ftf;
            _solutionFinder = new SolutionFinderVersion3();
            _solutionChecker = new SolutionCheckerVersion1();
            _solutionScorer = new SolutionScorerVersion1();
        }

        /// <summary>
        /// Use this one.
        /// </summary>
        /// <param name="context"></param>
        public TableManager(MyDBContext context)
        {
            _context = context;
            _freeTableFinder = new FreeTableFinder(_context);
            _solutionFinder = new SolutionFinderVersion3();
            _solutionChecker = new SolutionCheckerVersion1();
            _solutionScorer = new SolutionScorerVersion1();
        }

        public TableManager(MyDBContext context, IFreeTableFinder ftf, ISolutionFinder sf, ISolutionChecker sc, ISolutionScorer ss)
        {
            _context = context;
            _freeTableFinder = ftf;
            _solutionFinder = sf;
            _solutionChecker = sc;
            _solutionScorer = ss;
        }

        public List<Table> GetOptimalTableConfig(DateTime start, DateTime end, int partySize)
        {
            // First get all free tables for the time we want to make a reservation
            List<Table> freeTables = _freeTableFinder.GetFreeTables(start, end);
            // Now rewrite those into a dictionary by table capacities and get a list of just the Keys.
            Dictionary<int, int> TableCapAmounts = GetAvailabilityDictionary(freeTables);
            List<int> TableCaps = TableCapAmounts.Keys.ToList();

            // Using these table capacities, see which options we have to seat these people if we would have 
            // unlimited tables.
            List<List<int>> PossibleSolutions = _solutionFinder.GetSolutions(TableCaps, partySize);

            // Now using the dictionary, delete all solutions that are using more tables of a particular kind 
            // than we actually have.
            List<List<int>> ViableSolutions = _solutionChecker.GetViableSolutions(PossibleSolutions, TableCapAmounts);

            // And get the scores in so we can pick the configuration with the highest score.
            List<int> BestSolution = _solutionScorer.GetBestTableConfiguration(ViableSolutions, partySize);


            // Now look for the tables with these capacities and return them
            List<Table> Result = GetActualTablesWithCapacities(freeTables, BestSolution);

            return Result;
        }
        

        public Dictionary<int,int> GetAvailabilityDictionary(List<Table> tables)
        {
            List<int> TableCaps = tables.Select(table => table._tableCapacity).Distinct().OrderByDescending(x => x).ToList();
            Dictionary<int, int> TableCapAmounts = new Dictionary<int, int>();

            // Fill the dictionary with Keys TableCapacities and Values How many free tables of that capacity
            // are present.

            foreach (int cap in TableCaps)
            {
                TableCapAmounts.Add(cap, tables.Where(table => table._tableCapacity == cap).Count());
            }

            return TableCapAmounts;
        }

        private List<Table> GetActualTablesWithCapacities(List<Table> freeTables, List<int> solution)
        {
            List<Table> Result = new List<Table>();
            foreach (int cap in solution)
            {
                Table currentTable = freeTables.First(t1 => t1._tableCapacity == cap);
                Result.Add(currentTable);
                freeTables.Remove(currentTable);
            }

            return Result;
        }
        
        public double GetFreeTablePercentage(DateTime start, DateTime end)
        {
            List<Table> FreeTables = _freeTableFinder.GetFreeTables(start, end);
            List<Table> AllTables = _context.Tables.Select(table => table).ToList();
            int FreeTableCap = GetTotalCapacity(FreeTables);
            int AllTableCap = GetTotalCapacity(AllTables);

            return FreeTableCap / AllTableCap;

        }

        private int GetTotalCapacity(List<Table> tables)
        {
            int result = 0;
            foreach (Table table in tables)
            {
                result += table._tableCapacity;
            }
            return result;
        }
    }
}