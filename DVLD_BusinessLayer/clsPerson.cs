using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsPerson 
    {

        public enum enMode { AddNew = 1, Update = 2};
        enMode Mode = enMode.AddNew;

        public int PersonID { set; get; }
        public string NationalNo { set; get; }
        public string FirstName { set; get;}
        public string SecondName { set; get;}
        public string ThirdName { set; get;}
        public string LastName { set; get; }
        public string FullName //Read Only Property
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }          
        }
        public DateTime DateOfBirth { set; get; }
        public byte Gender { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }
        public string ImagePath { set; get; }

        //COMPOSITION : Create an object of another class inside your class.
        //This object is composed in clsPerson
        public clsCountry CountryInfo;
        

        //Initialize members with default values
        public clsPerson()
        { 
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gender = 0;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }

        private clsPerson(int PersonID, string NationalNo, string FirstName, string SecondName, 
                          string ThirdName, string LastName, DateTime DateOfBirth, byte Gender,
                          string Address, string Phone, string Email, int NationalityCountryID, 
                          string ImagePath) 
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;

            //return full object with meagniful data
            this.CountryInfo = clsCountry.FindCountry(NationalityCountryID);

            Mode = enMode.Update;
        }

        public static DataTable GetPeopleList()
        {
            return clsPersonData.GetAllPeople();
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName,
                                                       this.SecondName, this.ThirdName, this.LastName,
                                                       this.DateOfBirth, this.Gender, this.Address, 
                                                       this.Phone, this.Email, this.NationalityCountryID,
                                                       this.ImagePath);

            return (this.PersonID != -1);

        }

        public static clsPerson FindPerson(int PersonID)
        {

            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "",
            LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            byte Gender = 0;

            if (clsPersonData.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName,
                                             ref SecondName, ref ThirdName, ref LastName,
                                             ref DateOfBirth, ref Gender, ref Address,
                                             ref Phone, ref Email, ref NationalityCountryID,
                                             ref ImagePath))

                //we return object of that person FULL with Meagniful data
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, 
                                     LastName, DateOfBirth, Gender, Address, Phone, Email,
                                     NationalityCountryID, ImagePath);
            else
                return null;
        }

        public static clsPerson FindPerson(string NationalNo)
        {
                        
            string FirstName = "", SecondName = "", ThirdName = "",
            LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int PersonID = -1, NationalityCountryID = -1;
            byte Gender = 0;

            if (clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName,
                                                        ref SecondName, ref ThirdName, ref LastName,
                                                        ref DateOfBirth, ref Gender, ref Address,
                                                        ref Phone, ref Email, ref NationalityCountryID,
                                                        ref ImagePath))

                //we return object of that person FULL with Meagniful data
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName,
                                     LastName, DateOfBirth, Gender, Address, Phone, Email,
                                     NationalityCountryID, ImagePath);
            else
                return null;
        }

        private bool _UpdatePerson() 
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName,
                                              this.SecondName, this.ThirdName, this.LastName,
                                              this.DateOfBirth, this.Gender, this.Address,
                                              this.Phone, this.Email, this.NationalityCountryID,
                                              this.ImagePath);
        }

        //we set this method as static because we will delete Person
        //from class NOT from object because it's FASTER and if you delete
        //contact from object it's will be deleted just from database
        //BUT it's will still in memory
        public static bool DeletePerson(int PersonID) 
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }

        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:

                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                        //teacher said if we did not set it update
                        //the record will be added many times
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdatePerson();

                default:
                    return false;         
            }
        }





    }

}



    


