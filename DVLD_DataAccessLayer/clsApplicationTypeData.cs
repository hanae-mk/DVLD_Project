using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationTypeData
    {
        public static DataTable GetApplicationTypesList()
        {
            DataTable Table = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM ApplicationTypes ORDER BY ApplicationTypeID";

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

            }
            finally
            {
                Connection.Close();
            }

            return Table;
        }

        //the parameters are by ref because we will display data in the form
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, ref string ApplicationTypeTitle,
                                                      ref float ApplicationFees)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    IsFound = true;
                    ApplicationTypeTitle = (string)Reader["ApplicationTypeTitle"];
                    ApplicationFees = Convert.ToSingle(Reader["ApplicationFees"]);//(float)

                    Reader.Close();
                }        
            }
            catch(Exception Ex)
            {
                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static int AddNewApplicationType(string ApplicationTypeTitle, float ApplicationFees)
        {
            int ApplicationTypeID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO ApplicationTypes
                             VALUES(@ApplicationTypeTitle, @ApplicationFees);
                             SELECT SCOPE_IDENTITY()";
           
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            Command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if(Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    ApplicationTypeID = InsertedID;
                }
            }
            catch(Exception Ex)
            {
                
            }
            finally
            {
                Connection.Close();
            }

            return ApplicationTypeID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle,
                                                 float ApplicationTypeFees)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE ApplicationTypes 
                             SET ApplicationTypeTitle = @ApplicationTypeTitle,
                             ApplicationFees = @ApplicationFees
                             WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            Command.Parameters.AddWithValue("@ApplicationFees", ApplicationTypeFees);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch(Exception Ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);
        }
    }
}
