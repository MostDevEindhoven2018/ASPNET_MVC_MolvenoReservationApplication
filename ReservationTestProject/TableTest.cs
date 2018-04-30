using System;
using Xunit;
using ASPNET_MVC_MolvenoReservationApplication.Logic;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication;

namespace ReservationTestProject
{
    public class TableTest
    {
        private CheckTableAvailability _check;
        private MyDBContext _context;

        public TableTest(MyDBContext context)
        {
            this._context = context;
            this._check = new CheckTableAvailability(context);
        }

        [Fact]
        public void TestTableSelection()
        {
            DateTime arrival = new DateTime(2019, 01, 01, 18, 0, 0);
            DateTime leave = new DateTime(2019, 01, 01, 21, 0, 0);
            
        }
    }
}
