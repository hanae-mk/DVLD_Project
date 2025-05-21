using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsCountry
    {
      
        public int CountryID { set; get; }
        public string CountryName { set; get; }

        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = string.Empty;
        }

        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }

        public static DataTable GetListCountries()
        {
            return clsCountryData.GetListCountries();
        }

        public static clsCountry FindCountry(string CountryName) 
        {
            int CountryID = -1;

            if (clsCountryData.GetCountryInfoByName(CountryName, ref CountryID))
                return new clsCountry(CountryID, CountryName);
            else
                return null;
        }

        public static clsCountry FindCountry(int CountryID)
        {
            string CountryName = string.Empty;

            if (clsCountryData.GetCountryInfoByID(CountryID, ref CountryName))
                return new clsCountry(CountryID, CountryName);
            else
                return null;

        }


    }
}
