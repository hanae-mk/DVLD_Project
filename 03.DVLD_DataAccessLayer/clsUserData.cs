using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsUserData
    {
        //ALL METHODS SHOULD BE PUBLIC STATIC BECAUSE IT'S A DATA ACCESS LAYER

        public static DataTable GetAllUsers()
        {

            DataTable Table = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT Users.UserID, Users.PersonID, People.FirstName + ' ' + 
                             People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + 
                             People.LastName as FullName, Users.UserName, Users.IsActive	                         
                             FROM Users
                             INNER JOIN People
                             ON Users.PersonID = People.PersonID;";

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
                // Console.WriteLine("Error: " + Ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return Table;

        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)                                          
                             VALUES (@PersonID, @UserName, @Password, @IsActive);   
                             SELECT SCOPE_IDENTITY();"; //will return auto number

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {

                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    UserID = InsertedID;
                }

            }
            catch (Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return UserID;
        }

        //                            no need of PersonID 
        public static bool UpdateUser(int UserID, string UserName, string Password, bool IsActive)
        {
            int RowsAffected = 0;

            //PersonID CANNOT be modified because it's a P.K of another table
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE Users
                             set UserName = @UserName, 
                                 Password = @Password,
                                 IsActive = @IsActive 
                             WHERE UserID = @UserID";

            //In the query we will not write PersonID = @PersonID
            //bcs PersonID is a F.K and it's will be conflicted with the F.K constraint!

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);            

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName, 
                                               ref string Password, ref bool IsActive)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Users WHERE UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read()) //NOT the same as .HasRows
                {
                    //Record was found!
                    IsFound = true;

                    PersonID = (int)Reader["PersonID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];         
                   
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        //                              UserName            
        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string UserName, 
                                                 ref string Password, ref bool IsActive)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Users WHERE PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    UserID = (int)Reader["UserID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)(Reader["IsActive"]);

                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        //We use this method in LOGIN, if returns an object that means that
        //UserName and Password are valid, otherwise UserName OR Password is Invalid!
        public static bool GetUserInfoByUserNameAndPassword(string UserName, string Password, 
                                                            ref int UserID, ref int PersonID,
                                                            ref bool IsActive)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    UserID = (int)Reader["UserID"];
                    PersonID = (int)Reader["PersonID"];
                    IsActive = (bool)Reader["IsActive"];
                    //UserName = (string)Reader["UserName"];
                    //Password = (string)Reader["Password"];

                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool DeleteUser(int UserID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "DELETE Users WHERE UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);            

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                //rows cannot be deleted are rows that have referential integrity (F.K)
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);

        }

        public static bool IsUserExist(int UserID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT Found = 1 FROM Users WHERE UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                IsFound = Reader.HasRows;

                Reader.Close();

            }
            catch (Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;

        }

        public static bool IsUserExist(string UserName)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT Found = 1 FROM Users WHERE UserName = @UserName";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                IsFound = Reader.HasRows;

                Reader.Close();

            }
            catch (Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;

        }

        public static bool IsThisPersonUser(int PersonID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT Found = 1 FROM Users WHERE PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                IsFound = Reader.HasRows;

                Reader.Close();

            }
            catch (Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE Users
                             SET Password = @NewPassword
                             WHERE UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@NewPassword", NewPassword);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);

        }
    }
}
