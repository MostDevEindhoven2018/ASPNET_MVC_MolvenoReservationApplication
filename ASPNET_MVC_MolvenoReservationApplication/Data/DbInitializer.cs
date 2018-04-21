using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;

namespace ASPNET_MVC_MolvenoReservationApplication.Data
{
    public static class DbInitializer
    {
        public static void Itianilize(MyDBContext context)
        {
            context.Database.EnsureCreated();

            if (context.Reservations.Any())
            {
                return;
            }

            var Guests = new Guest[]
            {
                new Guest("George Vasileiadis", "06942020937"),
                new Guest("Mathijs van ser Ven", "djfas@emial.com"),
                new Guest("Erica Panayidou", "daifudsf@fgdf.ds", "058951213"),
                new Guest("Xiao Jong", "437879334"),
                new Guest("Li Wei", "hgddsfs@grgfd.rg", "75435741")
            };

            foreach (Guest g in Guests)
            {
                context.Guests.Add(g);
            }

            var Tables = new Table[]
            {
                new Table(4, TableAreas.Main),
                new Table(4, TableAreas.Main),
                new Table(4, TableAreas.Main),
                new Table(4, TableAreas.Main),
                new Table(8, TableAreas.Fireplace),
                new Table(4, TableAreas.Fireplace),
                new Table(4, TableAreas.Lake),
                new Table(4, TableAreas.Lake),
                new Table(8, TableAreas.Lake),
            };

            foreach(Table t in Tables)
            {
                context.Tables.Add(t);
            }


        }
    }
}
