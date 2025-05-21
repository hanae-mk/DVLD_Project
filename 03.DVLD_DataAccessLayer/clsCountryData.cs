using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsCountryData
    {

        public static DataTable GetListCountries()
        {
            DataTable Table = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Countries";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    Table.Load(Reader);
                }

                Reader.Close();

            }
            catch (Exception Ex)
            {
                //Console.WriteLine("Error : " + Ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return Table;
        }

        public static bool GetCountryInfoByID(int CountryID, ref string CountryName)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;
                    CountryName = (string)Reader["CountryName"];
                }

                Reader.Close();
            }
            catch(Exception Ex)
            {
                //Console.WriteLine("Error : " + Ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool GetCountryInfoByName(string CountryName, ref int CountryID)
        {

            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    CountryID = (int)Reader["CountryID"];

                    Reader.Close();

                }

            }
            catch (Exception Ex)
            {
                //Console.WriteLine("Error : " + Ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

  
    }
}
