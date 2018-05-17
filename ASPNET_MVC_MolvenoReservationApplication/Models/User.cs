using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ASPNET_MVC_MolvenoReservationApplication.Models;
using ASPNET_MVC_MolvenoReservationApplication.Controllers;

namespace ASPNET_MVC_MolvenoReservationApplication.Models
{
    public class User
    {
        public int UserID { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "This field is required!")]
        public string UserName { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "This field is required!")]
        public string Password { get; set; }


        public User()
        {

        }
    }
}
