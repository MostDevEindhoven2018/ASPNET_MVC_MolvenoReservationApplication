using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ASPNET_MVC_MolvenoReservationApplication.Services;

namespace ASPNET_MVC_MolvenoReservationApplication.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly MyDBContext _context;
                
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        private ApplicationUser _Users;
        private ApplicationUser Users
        {
            get
            {
                if (_Users == null)
                {
                    _Users = _context.Users.FirstOrDefault(a => a.UserId == 1);
                    if (_Users == null)
                    {
                        string email = "test@testersexample.nl";
                        _Users = new ApplicationUser();
                        _Users.UserId = 1;
                        _Users.UserName = email;
                        _Users.Email = email;
                        _Users.EmailConfirmed = true;
                        string Password = "P@ssw0rd";

                        var result = _userManager.CreateAsync(_Users, Password);
                        _signInManager.SignInAsync(_Users, isPersistent: false);
                        //_logger.LogInformation("User created a new account with password.");



                        //var res = _userManager.AddPasswordAsync(_Users, Password);
                        //string code = "1";
                        //var result2 = _userManager.ConfirmEmailAsync(_Users, code);


                        //_context.Users.Add(_Users);
                        //_context.SaveChanges();
                    }
                }
                return _Users;
            }
            set { throw new InvalidOperationException(); }
        }

        private AdminConfigure _adminConfigure;

        public AdminConfigure AdminConfigure
        {
            get
            {
                if (_adminConfigure == null)
                {
                    _adminConfigure = _context.Admins.FirstOrDefault(a => a.AdminID == 1);
                    if (_adminConfigure == null)
                    {
                        _adminConfigure = new AdminConfigure();
                        _adminConfigure.OpeningHour = 12;
                        _adminConfigure.ClosingHours = 00;
                        _adminConfigure._resDurationHour = 3;
                        _adminConfigure.PercentageMaxCapacity = 100;
                        _context.Admins.Add(_adminConfigure);
                        _context.SaveChanges();
                    }
                }

                return _adminConfigure;
            }
            set { throw new InvalidOperationException(); }
        }
               
        public HomeController(MyDBContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
        {            
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

        }
        public IActionResult Index()
        {
            _context.Database.EnsureCreated();
            var x = AdminConfigure;
            var y = Users;
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
    }
}
