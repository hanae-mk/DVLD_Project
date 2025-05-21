using DVLD_DataAccessLayer;
using System;
using System.Data;

namespace DVLD_BusinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 1, Update = 2 };
        enMode Mode = enMode.AddNew;

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }

        //COMPOSITION : Create an object of another class inside your class.
        //This object is composed in clsUser
        //To facilate access to UserInfo For Example : _User.PersonInfo.Nationality
        public clsPerson PersonInfo;

        //Initialize members with default values
        public clsUser()
        {
            this.UserID = -1;
            //this.PersonID = -1; we already have PersonID
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;
            Mode = enMode.AddNew;
        }

        private clsUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;        
            this.PersonInfo = clsPerson.FindPerson(PersonID); //we can access to all User Infos

            Mode = enMode.Update;
            //return full object with meagniful data
        }

        public static DataTable GetUsersList()
        {
            return clsUserData.GetAllUsers();
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.Password, 
                                                 this.IsActive);

            return (this.UserID != -1);
        }

        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            if (clsUserData.GetUserInfoByUserID(UserID, ref PersonID, ref UserName,
                                             ref Password, ref IsActive))

                //we return object of that User FULL with Meagniful data
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }

        public static clsUser FindByPersonID(int PersonID) //FindByUserName
        {
            int UserID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            if (clsUserData.GetUserInfoByPersonID(PersonID, ref UserID, ref UserName, 
                                                  ref Password, ref IsActive))
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
               //we return object of that user FULL with Meagniful data
            else
                return null;
        }

        //we use it in LOGIN
        public static clsUser FindByUserNameAndPassword(string UserName, string Password)
        {
            int UserID = -1, PersonID = -1;
            bool IsActive = false;

            if (clsUserData.GetUserInfoByUserNameAndPassword(UserName, Password, ref UserID,
                                                            ref PersonID, ref IsActive))
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }

        private bool _UpdateUser() //we don't update PersonID why teacher put it in parameters?
        {
            return clsUserData.UpdateUser(this.UserID, this.UserName, this.Password, this.IsActive);
        }

        //we set this method as static because we will delete User
        //from class NOT from object because it's FASTER and if you delete
        //User from object it's will be deleted just from database
        //BUT it's will still in memory
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }

        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool IsThisPersonUser(int PersonID)
        {
            return clsUserData.IsThisPersonUser(PersonID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                        //teacher said if we did not set it update
                        //the record will be added many times
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateUser();

                default:
                    return false;
            }
        }

    }
}
