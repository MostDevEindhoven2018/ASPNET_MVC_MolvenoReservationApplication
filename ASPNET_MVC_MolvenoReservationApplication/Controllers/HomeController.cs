﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Data;

namespace ASPNET_MVC_MolvenoReservationApplication.Controllers
{
    public class HomeController : Controller
    {
        MyDBContext _context;
        public HomeController(MyDBContext context)
        {
            //_AvailabilityCheck = new CheckTableAvailability(context);
            _context = context;

        }
        public IActionResult Index()
        {
            _context.Database.EnsureCreated();
            DbInitializer.Initialize(_context);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "About us:";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact info:";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ConfirmReservation()
        {
            return View();
        }
    }
}
