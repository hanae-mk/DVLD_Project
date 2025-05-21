using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsInternationalLicenseData
    {
        public static DataTable GetAllInternationalLicenses()
        {

            DataTable Table = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT InternationalLicenseID, ApplicationID, DriverID,
		                            IssuedUsingLocalLicenseID, IssueDate, ExpirationDate,
                                    IsActive
		                     FROM InternationalLicenses 
                             ORDER BY IsActive, ExpirationDate DESC";

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

        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID,
                                                           ref int ApplicationID, ref int DriverID,
                                                           ref int IssuedUsingLocalLicenseID,
                                                           ref DateTime IssueDate, 
                                                           ref DateTime ExpirationDate, 
                                                           ref bool IsActive, ref int CreatedByUserID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT * FROM InternationalLicenses 
                             WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    IsFound = true;

                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    IssuedUsingLocalLicenseID = (int)Reader["IssuedUsingLocalLicenseID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    IsActive = (bool)Reader["IsActive"];
                    CreatedByUserID = (int)Reader["DriverID"];
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

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable Table = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT InternationalLicenseID, ApplicationID,
		                     IssuedUsingLocalLicenseID, IssueDate, 
                             ExpirationDate, IsActive
		                     FROM InternationalLicenses 
                             WHERE DriverID = @DriverID
                             ORDER BY ExpirationDate DESC";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static int AddNewInternationalLicense(int ApplicationID, int DriverID,
                                                     int IssuedUsingLocalLicenseID,
                                                     DateTime IssueDate, DateTime ExpirationDate, 
                                                     bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update InternationalLicenses 
                             SET IsActive = 0
                             WHERE DriverID = @DriverID;
                             INSERT INTO InternationalLicenses
                             (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate,
                             ExpirationDate, IsActive, CreatedByUserID)    
                             VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID,    
                             @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);     
                             SELECT SCOPE_IDENTITY();";   
                                
     
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    InternationalLicenseID = InsertedID;
                
            }
            catch 
            {

            }
            finally
            {
                Connection.Close();
            }

            return InternationalLicenseID;
        }

        public static bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID,
                                                      int DriverID, int IssuedUsingLocalLicenseID,
                                                      DateTime IssueDate, DateTime ExpirationDate, 
                                                      bool IsActive, int CreatedByUserID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE InternationalLicenses
                             SET ApplicationID = @ApplicationID,                       
                                 DriverID = @DriverID,
                                 IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                                 IssueDate = @IssueDate,
                                 ExpirationDate = @ExpirationDate,
                                 IsActive = @IsActive,
                                 CreatedByUserID = @CreatedByUserID
                             WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
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

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TOP 1 InternationalLicenseID                    
                             FROM InternationalLicenses 
                             WHERE DriverID = @DriverID AND GetDate() 
                             BETWEEN IssueDate AND ExpirationDate 
                             ORDER BY ExpirationDate DESC;";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    InternationalLicenseID = InsertedID;
                }
            }
            catch 
            {

            }
            finally
            {
                Connection.Close();
            }

            return InternationalLicenseID;
        }
    }
}
