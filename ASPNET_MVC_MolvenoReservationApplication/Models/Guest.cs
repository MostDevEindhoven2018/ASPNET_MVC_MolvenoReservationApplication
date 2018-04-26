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

        // Do we really want a minimum length of 4? What about a chinese dude called Hu?
        // Same with max length? I know a guy called Sebastiaan-Ferdinand van Bimsbergen tot Nijverdal, thats 49 characters
        // Does this check for weird characters like % or &?
        // Does it remove whitespace before and after the name?
        [StringLength(30, MinimumLength = 4)]
        [Required(ErrorMessage = "Please enter your name.")]
        [DataType(DataType.Text)]
        [Display(Name = "Name", AutoGenerateField = true)]
        public string _guestName { get; set; }

        // My phone number is 10 characters long. ( 0652680658 ;) xxx ). Now this does not get accepted.
        // Whitespace?
        // If I add the - between the 06 and the rest of my number, does it break?
        // other weird characters?
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Your phone should be 13 numbers long.")]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone", AutoGenerateField = true)]
        public string _guestPhone { get; set; }


        // Does it check for a @?
        // something before the @?
        // Something after the @?
        // A landcode like .nl or .com?
        [StringLength(40, MinimumLength = 5)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", AutoGenerateField = true)]
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
