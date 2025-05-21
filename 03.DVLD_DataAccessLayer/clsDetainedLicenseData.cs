using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsDetainedLicenseData
    {
         public static bool GetDetainedLicenseInfoByDetainID(int DetainID, ref int LicenseID,
                                                       ref DateTime DetainDate, ref float FineFees,
                                                       ref int CreatedByUserID, ref bool IsReleased,
                                                       ref DateTime ReleaseDate, ref int ReleasedByUserID,
                                                       ref int ReleaseApplicationID)
         {
             bool IsFound = false;

             SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

             string Query = @"SELECT * FROM DetainedLicenses 
                              WHERE DetainID = @DetainID";

             SqlCommand Command = new SqlCommand(Query, Connection);

             Command.Parameters.AddWithValue("@DetainID", DetainID);

             try
             {
                 Connection.Open();
                 SqlDataReader Reader = Command.ExecuteReader();

                 if (Reader.Read())
                 {
                     IsFound = true;

                     LicenseID = (int)Reader["LicenseID"];
                     DetainDate = (DateTime)Reader["DetainDate"];
                     FineFees = Convert.ToSingle(Reader["FineFees"]);
                     CreatedByUserID = (int)Reader["CreatedByUserID"];
                     IsReleased = (bool)Reader["IsReleased"];

                     if (Reader["ReleaseDate"] == DBNull.Value)
                         ReleaseDate = DateTime.MaxValue;
                     else
                         ReleaseDate = (DateTime)Reader["ReleaseDate"];

                     if (Reader["ReleasedByUserID"] == DBNull.Value)
                         ReleasedByUserID = -1;
                     else
                         ReleasedByUserID = (int)Reader["ReleasedByUserID"];

                     if (Reader["ReleaseApplicationID"] == DBNull.Value)
                         ReleaseApplicationID = -1;
                     else
                         ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];
                 }

                 Reader.Close();
             }
             catch 
             {

             }
             finally
             {
                 Connection.Close();
             }

             return IsFound;
         }

          public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref int DetainID,
                                                               ref DateTime DetainDate, ref float FineFees,
                                                               ref int CreatedByUserID, ref bool IsReleased,
                                                               ref DateTime ReleaseDate, ref int ReleasedByUserID,
                                                               ref int ReleaseApplicationID)
          {
              bool IsFound = false;

              SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

              string Query = @"SELECT TOP 1 * 
                               FROM DetainedLicenses 
                               WHERE LicenseID = @LicenseID 
                               ORDER BY DetainID DESC";

              SqlCommand Command = new SqlCommand(Query, Connection);

              Command.Parameters.AddWithValue("@LicenseID", LicenseID);

              try
              {
                  Connection.Open();
                  SqlDataReader Reader = Command.ExecuteReader();

                  if (Reader.Read())
                  {
                      IsFound = true;

                      DetainID = (int)Reader["DetainID"];
                      DetainDate = (DateTime)Reader["DetainDate"];
                      FineFees = Convert.ToSingle(Reader["FineFees"]);
                      CreatedByUserID = (int)Reader["CreatedByUserID"];
                      IsReleased = (bool)Reader["IsReleased"];

                      if (Reader["ReleaseDate"] == DBNull.Value)
                          ReleaseDate = DateTime.MaxValue;
                      else
                          ReleaseDate = (DateTime)Reader["ReleaseDate"];

                      if (Reader["ReleasedByUserID"] == DBNull.Value)
                          ReleasedByUserID = -1;
                      else
                          ReleasedByUserID = (int)Reader["ReleasedByUserID"];

                      if (Reader["ReleaseApplicationID"] == DBNull.Value)
                          ReleaseApplicationID = -1;
                      else
                          ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];
                  }
    
                  Reader.Close();
              }
              catch 
              {

              }
              finally
              {
                  Connection.Close();
              }

              return IsFound;
          }

          public static DataTable GetAllDetainedLicenses()
          {

              DataTable Table = new DataTable();

              SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

              string Query = @"SELECT * FROM detainedLicenses_View 
                               ORDER BY IsReleased, DetainID";

              SqlCommand Command = new SqlCommand(Query, Connection);

              try
              {
                  Connection.Open();

                  SqlDataReader Reader = Command.ExecuteReader();

                  if (Reader.HasRows)
                      Table.Load(Reader);
                  
                  Reader.Close();
              }
              catch 
              {
                 
              }
              finally
              {
                  Connection.Close();
              }

              return Table;
          }

        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate,
                                                float FineFees, int CreatedByUserID)
        {
            int DetainID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO dbo.DetainedLicenses (LicenseID, DetainDate,         
                                 FineFees, CreatedByUserID, IsReleased)                                                                                          
                                 VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0);                                                
                                 SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            //IsReleased = 0 it's by default FALSE

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    DetainID = InsertedID;                
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
                                    
            return DetainID;               
        }

        public static bool UpdateDetainedLicense(int DetainID, int LicenseID,
                                                 DateTime DetainDate, float FineFees, 
                                                 int CreatedByUserID)
        {
        
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        
            string Query = @"UPDATE DetainedLicenses
                             SET LicenseID = @LicenseID, 
                             DetainDate = @DetainDate, 
                             FineFees = @FineFees,
                             CreatedByUserID = @CreatedByUserID,   
                             WHERE DetainID = @DetainID;";
        
            SqlCommand Command = new SqlCommand(Query, Connection);
        
            Command.Parameters.AddWithValue("@DetainedLicenseID", DetainID);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
             
            try
            {
                Connection.Open();

                RowsAffected = Command.ExecuteNonQuery();    
            }
            catch
            {
                
            }     
            finally
            {
                Connection.Close();
            }
        
            return (RowsAffected > 0);
        }

        public static bool IsDetainedLicenseReleased(int DetainID, int ReleasedByUserID,
                                                     int ReleaseApplicationID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE DetainedLicenses
                             SET IsReleased = 1, 
                             ReleaseDate = @ReleaseDate, 
                             ReleaseApplicationID = @ReleaseApplicationID   
                             WHERE DetainID = @DetainID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);
            Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

            try
            {
                Connection.Open();

                RowsAffected = Command.ExecuteNonQuery();
            }
            catch 
            {

            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT IsDetained = 1 
                             FROM DetainedLicenses 
                             WHERE LicenseID = @LicenseID                         
                             AND IsReleased = 0";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                    IsDetained = Convert.ToBoolean(Result);               
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsDetained;           
        }      
    }
}



