using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;

namespace ASPNET_MVC_MolvenoReservationApplication.Models
{
    public class MyDBContext : DbContext
    {
        //This defines the DB tables Reservations,Guests,Tables
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<ReservationTableCoupling> ReservationTableCouplings { get; set; }
        public DbSet<AdminConfigure> Admins { get; set; }

        /// <summary>
        /// Creating the constructor for the class MyDBContext
        /// </summary>
        public MyDBContext(DbContextOptions <MyDBContext> options)
            :base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>().ToTable("Reservations");
        }
    }
}
