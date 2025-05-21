using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Project.Global_Classes
{
    public class clsFormat
    {
        public static string ShortDateTime(DateTime Date)
        {
            return Date.ToString("dd/MM/yyyy");
        }
    }
}
