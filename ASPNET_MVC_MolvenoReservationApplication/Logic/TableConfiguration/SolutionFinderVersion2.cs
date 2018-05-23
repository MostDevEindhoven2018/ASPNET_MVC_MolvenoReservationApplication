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
    /// This is unfinished work. Look at it to laugh but do not think anything actually works here.
    /// </summary>
    public class SolutionFinderVersion2 //: ITableConfigurationFinder
    {
        public List<Table> GetTableConfiguration(List<Table> FreeTables, int partySize)
        {
            List<Table> TableConfiguration = new List<Table>();
            int TotalCapacity = 0;
            int Divider = 2;
            int Debt = 0;       // How far off we are when we are selecting tables. For instance, if we need a table
                                // with a capacity of 8 and only have one with 6, we are 2 'in debt'. When we find 
                                // a table with a higher capactity, lets say 10, we are -2 'in debt'. This is used
                                // in further searches for tables. If we are 2 in debt, we need to search for tables
                                // with a capacity of 2 more than we initially thought. 



            // Calculate Total Capacity (TC) of all free tables
            foreach (Table table in FreeTables)
            {
                TotalCapacity += table._tableCapacity;
            }

            // First check whether or not we have sufficient room to begin with by comparing the party size (PS) 
            // with the TC.
            if (partySize <= TotalCapacity)
            {
                // Check whether or not we can seat everyone at one table, by comparing the PS with the largest capacity
                // Order the Free tables and take the last item (the largest table) and check 
                FreeTables.OrderBy(table => table._tableCapacity).ToList();

                if (partySize <= FreeTables.Last()._tableCapacity)
                {
                    // apparently one table will do. Lets get the first table that has a sufficient capacity
                    // and be done with it.
                    TableConfiguration.Add(FreeTables.First(table => table._tableCapacity >= partySize));
                    return TableConfiguration;
                }
                // If that was not enough, we need to continue. And this is where the fun starts.

                // Divide the partysize and check again. Maybe we have space for two times half?
                int DividedPartySize = partySize / Divider;

                if (DividedPartySize <= FreeTables.Last()._tableCapacity)
                {
                    // The DividedPartySize is indeed lower than our biggest table. Now check whether or not we can 
                    // get a first table as close to the DividedPartySize as possible. If the exact number is not
                    // available, get the first bigger table.
                    Table FirstTable = FreeTables.First(table => table._tableCapacity >= DividedPartySize);

                    // Calculate the Debt
                    Debt += DividedPartySize - FirstTable._tableCapacity;
                    // and look for the rest of the tables

                    Table SecondTable = FreeTables.First(table => table._tableCapacity >= (DividedPartySize + Debt));
                    // If we found a table with this 

                }
            }
            // If the total capacity is not sufficient to host the party size, we need not to search anymore
            // as it will be impossible anyway. Return a empty list indicating we failed to find sufficient room.

            return TableConfiguration;
        }



        public List<Table> GetTableConfiguration2(List<Table> FreeTables, int partySize)
        {
            // TODO 

            //  -   FIX:    Now, whenever we do not find a table that fits our divided party size, we immediately break and
            //      start looking for a configuration with more tables. I m not sure whether or not this is right.
            //
            //  -   OPTIMISE: At the moment, when we are looking for a TC for 12 people, and the first table found  
            //      had a capacity of 6, but we do not have more tables of 6, we might add a table of size 8, resulting in 
            //      14 spaces being occupied to seat only 12. This happens without checking for a config of 8 and 4, which 
            //      might have been possible. Figure this out.
            //
            //  -   What should be returned when no configuration could be found?





            List<Table> TableConfiguration = new List<Table>();     // This will be filled and returned.

            List<Table> CurrentlyUsedFreeTables;                    // When a table is selected it will be removed from the list
                                                                    // Therefore we need to keep both a original and 'currently worked upon'list.



            int TotalCapacity = 0;
            int CurrentCapacity = 0;
            int DividedPartySize;           // Now an int. Maybe later allow for floats.
            int Divider = 1;
            int Debt = 0;       // How far off we are when we are selecting tables. For instance, if we need a table
                                // with a capacity of 8 and only have one with 6, we are 2 'in debt'. When we find 
                                // a table with a higher capactity, lets say 10, we are -2 'in debt'. This is used
                                // in further searches for tables. If we are 2 in debt, we need to search for tables
                                // with a capacity of 2 more than we initially thought. 

            Table CurrentTable;

            bool SolutionFound = false;

            foreach (Table table in FreeTables)
            {
                TotalCapacity += table._tableCapacity;
            }

            // First check whether or not we have sufficient room to begin with by comparing the party size (PS) 
            // with the TC.
            if (partySize <= TotalCapacity)
            {

                while (!SolutionFound)
                {
                    // refresh the list of free tables. When in earlier attempts to get a nice configuration that 
                    // didnt work out some tables were used, we should put those tables back and try again.
                    CurrentlyUsedFreeTables = new List<Table>(FreeTables);
                    // reset currentCapacity
                    CurrentCapacity = 0;


                    // lets find some tables!
                    DividedPartySize = partySize / Divider;

                    for (int i = 0; i < Divider; i++)
                    {
                        CurrentTable = FreeTables.FirstOrDefault(table => table._tableCapacity >= (DividedPartySize + Debt));

                        if (CurrentTable == null)
                        {
                            // We did not find a table with the required capacity. Apparently we do not have enough 
                            // free tables to make this configuration a thing. Lets for now try to find a configuration
                            // with an extra table. (see FIX and OPTIMISE above)
                            Divider++;
                            break;
                        }
                        else
                        {
                            Debt += (DividedPartySize - CurrentTable._tableCapacity);

                            TableConfiguration.Add(CurrentTable);
                        }
                    }

                    // Check whether or not that worked (This should be redundant, but fuck it)
                    // Calculate the capacity of the tableConfiguration that we have right now.
                    foreach (Table table in TableConfiguration)
                    {
                        CurrentCapacity += table._tableCapacity;
                    }
                    if (CurrentCapacity >= partySize)
                    {
                        // Hurray! We found a table configuration that can handle our partySize.
                        // It might not be the best, but it works.
                        SolutionFound = true;
                        break;
                    }

                    // If it for some reason didnt work out, try again with a updated divider
                    Divider++;
                }
                return TableConfiguration;
            }
            else
            {
                // The partySize was bigger than we can handle, return a empty list to show we couldnt find anything.
                // (Maybe later update this to a bool or something. (see FIX above)
                return new List<Table>();
            }
        }
    }
}