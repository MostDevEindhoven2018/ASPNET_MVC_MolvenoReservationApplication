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

    public class TableManager
    {
        MyDBContext _context { get; set; }
        IFreeTableFinder _freeTableFinder { get; set; }
        ISolutionFinder _solutionFinder { get; set; }
        ITableConfigurationFinder _tableConfigurationFinder { get; set; }

        public TableManager(MyDBContext context)
        {
            _context = context;
            _freeTableFinder = new FreeTableFinder(_context);
            _solutionFinder = new SolutionFinderVersion1();
            _tableConfigurationFinder = new TableConfigurationFinderVersion1();
        }

        public TableManager(MyDBContext context, IFreeTableFinder ftf, ISolutionFinder sf, ITableConfigurationFinder tcf)
        {
            _context = context;
            _freeTableFinder = ftf;
            _solutionFinder = sf;
            _tableConfigurationFinder = tcf;
        }
    }
}