using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.Models
{
    public class MyDBContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// Creating the constructor for the class MyDBContext
        /// </summary>
        public MyDBContext(DbContextOptions <MyDBContext> options)
            :base (options)
        {

        }
    }
}
