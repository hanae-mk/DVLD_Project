using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsTestData
    {
        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, 
                                           ref bool TestResult, ref string Notes,
                                           ref int CreatedByUserID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Tests WHERE TestID = @TestID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    TestResult = (bool)Reader["TestResult"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];

                    if (Reader["Notes"] == DBNull.Value)
                        Notes = "";
                    else
                        Notes = (string)Reader["Notes"];
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

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, 
                                   int LicenseClassID, int TestTypeID, ref int TestID,
                                   ref int TestAppointmentID, ref bool TestResult,
                                   ref string Notes, ref int CreatedByUserID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TOP 1 Tests.TestID, 
                             Tests.TestAppointmentID, Tests.TestResult, 
			                 Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                             FROM LocalDrivingLicenseApplications 
                             INNER JOIN Tests
                             INNER JOIN TestAppointments
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID 
                             ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                             INNER JOIN Applications
                             ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             WHERE (Applications.ApplicantPersonID = @PersonID) 
                             AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                             AND ( TestAppointments.TestTypeID=@TestTypeID)
                             ORDER BY Tests.TestAppointmentID DESC";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    TestID = (int)Reader["TestID"];
                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    TestResult = (bool)Reader["TestResult"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];

                    if (Reader["Notes"] == DBNull.Value)
                        Notes = "";
                    else
                        Notes = (string)Reader["Notes"];
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

        public static DataTable GetAllTests()
        {

            DataTable Table = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Tests ORDER BY TestID";

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

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes,
                                     int CreatedByUserID)
        {
            int TestID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO Tests (TestAppointmentID,TestResult,
                                                Notes, CreatedByUserID)
                            Values (@TestAppointmentID, @TestResult,
                                    @Notes, @CreatedByUserID);                   
                            UPDATE TestAppointments 
                            SET IsLocked = 1 WHERE TestAppointmentID = @TestAppointmentID;
                            SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            if (Notes != "" && Notes != null)
                Command.Parameters.AddWithValue("@Notes", Notes);
            else
                Command.Parameters.AddWithValue("@Notes", System.DBNull.Value);  

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    TestID = InsertedID;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,
                                      string Notes, int CreatedByUserID)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Tests  
                             SET TestAppointmentID = @TestAppointmentID,
                             TestResult = @TestResult,
                             Notes = @Notes,
                             CreatedByUserID = @CreatedByUserID
                             WHERE TestID = @TestID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestID", TestID);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            Command.Parameters.AddWithValue("@Notes", Notes);
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

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT PassedTestCount = COUNT(TestTypeID)
                             FROM Tests 
                             INNER JOIN TestAppointments
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						     WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                             AND TestResult = 1";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && byte.TryParse(Result.ToString(), out byte Counter))
                {
                    PassedTestCount = Counter;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }

            return PassedTestCount;
        }
    }
}
