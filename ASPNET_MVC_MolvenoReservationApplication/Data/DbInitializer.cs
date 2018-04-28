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
                new Reservation(4,new DateTime(2019,1,1,20,0,0), new DateTime(2019,1,1,23,0,0), Tables[2], Guests[0]),//a
                new Reservation(4,new DateTime(2019,1,1,13,0,0), new DateTime(2019,1,1,16,0,0), Tables[3], Guests[1]),//b
                new Reservation(4,new DateTime(2019,1,1,15,0,0), new DateTime(2019,1,1,18,0,0), Tables[2], Guests[2]),//c
                new Reservation(4,new DateTime(2019,1,1,18,0,0), new DateTime(2019,1,1,21,0,0), Tables[0], Guests[3]),//d
                new Reservation(4,new DateTime(2019,1,1,14,0,0), new DateTime(2019,1,1,17,0,0), Tables[1], Guests[0]),//e
                new Reservation(4,new DateTime(2019,1,1,12,0,0), new DateTime(2019,1,1,15,0,0), Tables[0], Guests[1]),//f
                new Reservation(4,new DateTime(2019,1,1,18,0,0), new DateTime(2019,1,1,21,0,0), Tables[1], Guests[2])//g
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
        }
    }
}
