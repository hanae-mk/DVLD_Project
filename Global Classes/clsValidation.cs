using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Global_Classes
{
    class clsValidation
    {

        public static bool IsValidEmail(string Email)
        {          
            var Pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            //Regex = Regular Expression
            //var it's an Anonymous DataType.
            var Regex = new Regex(Pattern);

            //Here Regex compare if the email is matching the pattern or not
            return Regex.IsMatch(Email);

        }

        public static bool IsMatchedPassword(string Password, string ConfirmPassword)
        {
            return (Password == ConfirmPassword);
        }

        public static bool IsValidInteger(string sNumber)
        {
            var Pattern = @"^[0-9]*$";

            var Regex = new Regex(Pattern);

            return Regex.IsMatch(sNumber);
        }

        public static bool IsValidFloat(string sNumber)
        {
            var Pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var Regex = new Regex(Pattern);

            return Regex.IsMatch(sNumber);
        }

        public static bool IsNumber(string sNumber)
        {
            return IsValidInteger(sNumber) || IsValidFloat(sNumber);
        }

    }
}
