﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationData
    {
        public static DataTable GetApplicationsList()
        {
            DataTable Table = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * from Applications ORDER BY ApplicationDate DESC";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

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
                Connection.Close();
            }

            return Table;
        }

        public static bool GetApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID,
                                                  ref DateTime ApplicationDate, ref int ApplicationTypeID,
                                                  ref byte ApplicationStatus, ref DateTime LastStatusDate,
                                                  ref float PaidFees, ref int CreatedByUserID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    ApplicantPersonID = (int)Reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"];
                    ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
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

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, 
                                            int ApplicationTypeID, byte ApplicationStatus, 
                                            DateTime LastStatusDate, float PaidFees, 
                                            int CreatedByUserID)
        {

            int ApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Applications(ApplicantPersonID,ApplicationDate, 
                             ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees,
                             CreatedByUserID)
                             VALUES (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,
                                     @ApplicationStatus,@LastStatusDate,
                                     @PaidFees, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
            Command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
            Command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
            Command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
            Command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
            Command.Parameters.AddWithValue("PaidFees", @PaidFees);
            Command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    ApplicationID = InsertedID;
                }
            }

            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return ApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, 
                                             DateTime ApplicationDate, int ApplicationTypeID,
                                             byte ApplicationStatus, DateTime LastStatusDate,
                                             float PaidFees, int CreatedByUserID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Applications  
                             SET ApplicantPersonID = @ApplicantPersonID,
                                 ApplicationDate = @ApplicationDate,
                                 ApplicationTypeID = @ApplicationTypeID,
                                 ApplicationStatus = @ApplicationStatus, 
                                 LastStatusDate = @LastStatusDate,
                                 PaidFees = @PaidFees,
                                 CreatedByUserID = @CreatedByUserID
                             WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
            Command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
            Command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
            Command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
            Command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
            Command.Parameters.AddWithValue("PaidFees", @PaidFees);
            Command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);

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

        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Applications  
                             SET ApplicationStatus = @NewStatus, 
                                 LastStatusDate = @LastStatusDate
                             WHERE ApplicationID = @ApplicationID;";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@NewStatus", NewStatus);
            Command.Parameters.AddWithValue("LastStatusDate", DateTime.Now);

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

        public static bool DeleteApplication(int ApplicationID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "DELETE Applications WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT Found = 1 FROM Applications 
                             WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();
                IsFound = Reader.HasRows;

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

        public static int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT ActiveApplicationID = Applications.ApplicationID  
                             FROM Applications
                             INNER JOIN LocalDrivingLicenseApplications
                             ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                             WHERE ApplicantPersonID = @ApplicantPersonID 
                             AND ApplicationTypeID = @ApplicationTypeID 
							 AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                             AND ApplicationStatus = 1";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int ApplicationID))
                {
                    ActiveApplicationID = ApplicationID;
                }
            }
            catch (Exception ex)
            {
 
            }
            finally
            {
                Connection.Close();
            }

            return ActiveApplicationID;
        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT ActiveApplicationID = ApplicationID 
                             FROM Applications 
                             WHERE ApplicantPersonID = @ApplicantPersonID 
                             AND ApplicationTypeID = @ApplicationTypeID 
                             AND ApplicationStatus = 1";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int ApplicationID))
                {
                    ActiveApplicationID = ApplicationID;
                }
            }
            catch 
            {

            }
            finally
            {
                Connection.Close();
            }

            return ActiveApplicationID;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            //Incase the ActiveApplication ID != -1 it's returns TRUE.
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
        }

        //public static bool IsThisApplicationActive(int ApplicantPersonID, int ApplicationTypeID)
        //{
        //   
        //    bool IsActive = false;

        //    SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        //    string Query = @"SELECT IsActive = 1 
        //                     FROM Applications 
        //                     WHERE ApplicantPersonID = @ApplicantPersonID 
        //                     AND ApplicationTypeID = @ApplicationTypeID 
        //                     AND ApplicationStatus = 1";

        //    SqlCommand Command = new SqlCommand(Query, Connection);

        //    Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
        //    Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

        //    try
        //    {
        //        Connection.Open();
        //        SqlDataReader Reader = Command.ExecuteReader();

        //        if (Reader.HasRows)
        //            IsActive = true;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        Connection.Close();
        //    }

        //    return IsActive;
        //}
    }
}
