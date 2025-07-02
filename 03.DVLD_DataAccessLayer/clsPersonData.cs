
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer 
{
    public class clsPersonData  //All methods in data access should be static 
    {

        public static DataTable GetAllPeople()
        {

            DataTable Table = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, 
                             LastName, 
                             CASE 
                                 WHEN Gender = 0 THEN 'Male'
                                 ELSE 'Female'
                             END as Gender,
                             DateOfBirth, Address, Phone, Email, CountryName as Nationality, 
                             ImagePath
                             FROM People INNER JOIN Countries
                             ON CountryID = NationalityCountryID
                             Order By PersonID";

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
            catch(Exception Ex)
            {
                // Console.WriteLine("Error: " + Ex.Message);
            }
            finally
            {             
                Connection.Close();            
            }

            return Table;

        }

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName,
                                       string ThirdName, string LastName, DateTime DateOfBirth,
                                       byte Gender, string Address, string Phone, string Email,
                                       int NationalityCountryID, string ImagePath)                                     
        {
            int PersonID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName,
                                                 LastName, DateOfBirth, Gender, Address, 
                                                 Phone, Email, NationalityCountryID, ImagePath)                                           
                                                 VALUES (@NationalNo, @FirstName, @SecondName, 
                                                         @ThirdName, @LastName, @DateOfBirth, 
                                                         @Gender, @Address, @Phone, @Email,
                                                         @NationalityCountryID, @ImagePath);   
                             SELECT SCOPE_IDENTITY();"; //will return auto number
                                                        
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gender", Gender);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ThirdName != "" && ThirdName != null)
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);

            if (Email != "" && Email != null)
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", DBNull.Value);

            if (ImagePath != "" && ImagePath != null) 
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            try
            {
                
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    PersonID = InsertedID;
                }

            }
            catch(Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName,
                                        string SecondName, string ThirdName, string LastName,
                                        DateTime DateOfBirth, byte Gender, string Address,
                                        string Phone, string Email, int NationalityCountryID,
                                        string ImagePath)
        {
            
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE People 
                             set NationalNo = @NationalNo, 
                                 FirstName = @FirstName,
                                 SecondName = @SecondName, 
                                 ThirdName = @ThirdName, 
                                 LastName = @LastName,
                                 DateOfBirth = @DateOfBirth, 
                                 Gender = @Gender, 
                                 Address = @Address,
                                 Phone = @Phone, 
                                 Email = @Email, 
                                 NationalityCountryID = @NationalityCountryID,
                                 ImagePath = @ImagePath 
                             WHERE PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);       
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gender", Gender);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);           
            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if(ThirdName != "" && ThirdName != null)
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);

            if(Email != "" && Email != null)
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", DBNull.Value);

            if (ImagePath != "" && ImagePath != null)
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            int RowsAffected = 0;

            try
            {
                 Connection.Open();    
                
                 RowsAffected = Command.ExecuteNonQuery();  
                
            }
            catch(Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);

        }
                        
        public static bool GetPersonInfoByID(int PersonID, ref string NationalNo, ref string FirstName,
                                             ref string SecondName, ref string ThirdName, ref string LastName,
                                             ref DateTime DateOfBirth, ref byte Gender, ref string Address,
                                             ref string Phone, ref string Email, ref int NationalityCountryID,
                                             ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read()) //NOT the same as .HasRows
                {
                    //Record was found!
                    IsFound = true; 

                    NationalNo = (string)Reader["NationalNo"]; 
                    FirstName = (string)Reader["FirstName"];
                    SecondName = (string)Reader["SecondName"];           
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)(Reader["DateOfBirth"]); 
                    Gender = (byte)Reader["Gender"];
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];                    
                    NationalityCountryID = (int)Reader["NationalityCountryID"];

                    //we should handle those fields because they allows NULL in database

                    if (Reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)Reader["ThirdName"];
                    else
                        ThirdName = "";

                    if (Reader["Email"] != DBNull.Value)
                        Email = (string)Reader["Email"];
                    else
                        Email = "";

                    if (Reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)Reader["ImagePath"];
                    else
                        ImagePath = "";

                   Reader.Close();
                }
            }
            catch(Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName,
                                                     ref string SecondName, ref string ThirdName, ref string LastName,
                                                     ref DateTime DateOfBirth, ref byte Gender, ref string Address,
                                                     ref string Phone, ref string Email, ref int NationalityCountryID,
                                                     ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read()) 
                {
                    IsFound = true;

                    PersonID = (int)Reader["PersonID"];
                    FirstName = (string)Reader["FirstName"];
                    SecondName = (string)Reader["SecondName"];                    
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)(Reader["DateOfBirth"]);
                    Gender = (byte)Reader["Gender"];
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];             
                    NationalityCountryID = (int)Reader["NationalityCountryID"];

                    if (Reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)Reader["ThirdName"];
                    else
                        ThirdName = "";

                    if (Reader["Email"] != DBNull.Value)
                        Email = (string)Reader["Email"];
                    else
                        Email = "";

                    if (Reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)Reader["ImagePath"];
                    else
                        ImagePath = "";

                   Reader.Close();
                }
            }
            catch(Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool DeletePerson(int PersonID)
        {
            
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            
            string Query = "DELETE People WHERE PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            int RowsAffected = 0;

            try
            {
                Connection.Open();

                RowsAffected = Command.ExecuteNonQuery();
      
            }
            catch(Exception Ex)
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

        public static bool IsPersonExist(int PersonID)
        {
            bool IsFound = false;
            
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT Found = 1 FROM People WHERE PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                IsFound = Reader.HasRows;

                Reader.Close();

            }
            catch(Exception Ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;

        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool IsFound = false;
            
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT Found = 1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

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
    }
}
