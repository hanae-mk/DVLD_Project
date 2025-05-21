using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Global_Classes
{
    class clsLogin
    {
        public static clsUser CurrentUser;

        public static bool IsUserExist(string UserName, string Password)
        {
            CurrentUser = clsUser.FindByUserNameAndPassword(UserName, Password);

            if (CurrentUser != null)              
                return true;
            else
                return false;
        }


    }
}
