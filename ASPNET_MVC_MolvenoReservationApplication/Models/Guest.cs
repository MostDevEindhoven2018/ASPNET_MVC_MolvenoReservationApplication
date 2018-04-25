using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_MVC_MolvenoReservationApplication
{
    public class Guest
    {
        public int GuestID { get; set; }

        [StringLength(30, MinimumLength = 4)]
        [Required(ErrorMessage = "Please enter your name.")]
        [DataType(DataType.Text)]
        public string _guestName { get; set; }

        [StringLength(13, MinimumLength = 13, ErrorMessage = "Your phone should be 13 numbers long.")]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string _guestPhone { get; set; }

        [StringLength(40, MinimumLength = 5)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string _guestEmail { get; set; }


        public Guest() { }

        public Guest(string name, string phoneOrEmail)
        {
            this._guestName = name;
            if (long.TryParse(phoneOrEmail, out long n))
            {
                this._guestPhone = phoneOrEmail;
            }
            else
            {
                this._guestEmail = phoneOrEmail;
            }
        }

        public Guest(string name, string email, string phone)
        {
            this._guestName = name;
            this._guestEmail = email;
            this._guestPhone = phone;
        }


    }
}
