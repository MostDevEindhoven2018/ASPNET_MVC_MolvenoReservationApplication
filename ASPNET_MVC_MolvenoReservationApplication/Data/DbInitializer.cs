using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;

namespace ASPNET_MVC_MolvenoReservationApplication.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MyDBContext context)
        {
            context.Database.EnsureCreated();

            var Guests = new Guest[]
            {
                new Guest("George Vasileiadis", "06942020937"),//0
                new Guest("Mathijs van ser Ven", "djfas@emial.com"),//1
                new Guest("Erica Panayidou", "daifudsf@fgdf.ds", "058951213"),//2
                new Guest("Xiao Jong", "437879334")//3
            };

            var Tables = new Table[]
            {
                new Table(4, TableAreas.Main),//0
                new Table(8, TableAreas.Fireplace),//1
                new Table(4, TableAreas.Fireplace),//2
                new Table(4, TableAreas.Lake),//3
                new Table(8, TableAreas.Lake)//4
            };

            var Reservations = new Reservation[]
            {
                new Reservation(4, new DateTime(2018,5,4,18,45,00), 3, Guests[0]),//0
                new Reservation(8, new DateTime(2018,5,4,14,30,00), 3, Guests[1] ),//1
                new Reservation(6, new DateTime(2018,5,4,15,15,00), 3, Guests[2] ),//2
                new Reservation(12, new DateTime(2018,5,4,20,30,00), 3, Guests[3]),//3
                new Reservation(20, new DateTime(2018,5,4,12,15,00), 3, Guests[1]),//4
                new Reservation(2, new DateTime(2018,5,4,13,45,00), 3, Guests[2]),//5
                new Reservation(2, new DateTime(2018,5,4,17,30,00), 3, Guests[1]) //6
                
            };

            var ReservationTableCouplings = new ReservationTableCoupling[]
            {
                new ReservationTableCoupling(Reservations[0],Tables[0]),
                new ReservationTableCoupling(Reservations[1],Tables[1]),
                new ReservationTableCoupling(Reservations[2],Tables[2]),
                new ReservationTableCoupling(Reservations[3],Tables[3]),
                new ReservationTableCoupling(Reservations[4],Tables[4]),
                new ReservationTableCoupling(Reservations[5],Tables[0]),
                new ReservationTableCoupling(Reservations[6],Tables[1]),

            };

            if (!context.Tables.Any())
            {
                foreach (Table t in Tables)
                {
                    context.Tables.Add(t);
                }

                context.SaveChanges();
            }

            if (!context.Guests.Any())
            {
                foreach (Guest g in Guests)
                {
                    context.Guests.Add(g);
                }

                context.SaveChanges();
            }

            if (!context.Reservations.Any())
            {
                foreach (Reservation r in Reservations)
                {
                    context.Reservations.Add(r);
                }

                context.SaveChanges();
            }

            if (!context.ReservationTableCouplings.Any())
            {
                foreach (ReservationTableCoupling rtc in ReservationTableCouplings)
                {
                    context.ReservationTableCouplings.Add(rtc);
                }

                context.SaveChanges();
            }
        }
    }
}
