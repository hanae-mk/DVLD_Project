using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationData
    {
        public static DataTable GetLocalDrivingLicenseApplicationsList()
        {

            DataTable Table = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT * FROM LocalDrivingLicenseApplications_View 
                             ORDER BY ApplicationDate DESC";

            SqlCommand Command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                    Table.Load(Reader);

                Reader.Close();
            }

            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return Table;
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID, 
                                                                     ref int ApplicationID,
                                                                     ref int LicenseClassID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT * FROM LocalDrivingLicenseApplications 
                             WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    ApplicationID = (int)Reader["ApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                }
  
                Reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(int ApplicationID,
                                                       ref int LocalDrivingLicenseApplicationID,
                                                                         ref int LicenseClassID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT * FROM LocalDrivingLicenseApplications 
                             WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                }

                Reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO LocalDrivingLicenseApplications(ApplicationID, 
                             LicenseClassID)
                             VALUES (@ApplicationID, @LicenseClassID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    LocalDrivingLicenseApplicationID = InsertedID;
                }
            }

            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return LocalDrivingLicenseApplicationID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, 
                                                                int ApplicationID, int LicenseClassID)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update LocalDrivingLicenseApplications  
                             SET ApplicationID = @ApplicationID,
                             LicenseClassID = @LicenseClassID
                             WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"DELETE LocalDrivingLicenseApplications 
                             WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();

                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static bool IsPassTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {

            bool IsPass = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TOP 1 TestResult
                             FROM LocalDrivingLicenseApplications 
                             INNER JOIN TestAppointments
                             ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                             INNER JOIN Tests
                             ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE
                             (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                             AND (TestAppointments.TestTypeID = @TestTypeID)
                             ORDER BY TestAppointments.TestAppointmentID DESC";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && bool.TryParse(Result.ToString(), out bool ReturnedResult))
                {
                    IsPass = ReturnedResult;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsPass;
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TOP 1 Found = 1
                             FROM LocalDrivingLicenseApplications 
                             INNER JOIN TestAppointments
                             ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                             INNER JOIN Tests
                             ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)                           
                             AND (TestAppointments.TestTypeID = @TestTypeID)
                             ORDER BY TestAppointments.TestAppointmentID DESC";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    IsFound = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            byte TotalTrialsPerTest = 0; //because there are 3 tests

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TotalTrialsPerTest = COUNT(TestID)
                             FROM LocalDrivingLicenseApplications 
                             INNER JOIN TestAppointments
                             ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                             INNER JOIN Tests
                             ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                             AND (TestAppointments.TestTypeID = @TestTypeID)";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && byte.TryParse(Result.ToString(), out byte Trials))
                {
                    TotalTrialsPerTest = Trials;
                }
            }

            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return TotalTrialsPerTest;
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool IsActive = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TOP 1 Found = 1
                             FROM LocalDrivingLicenseApplications 
                             INNER JOIN TestAppointments
                             ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                             WHERE (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)  
                             AND (TestAppointments.TestTypeID = @TestTypeID) AND IsLocked = 0
                             ORDER BY TestAppointments.TestAppointmentID DESC";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                   IsActive = true;               
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsActive;
        }
    }
}
