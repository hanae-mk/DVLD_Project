using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
   //              SUB-CLASS                         BASE CLASS
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { AddNew = 1, Update = 2 };
        public enMode Mode = enMode.AddNew;

        public int LocalDrivingLicenseApplicationID { set; get; }
        public int LicenseClassID { set; get; }

        //COMPOSITION
        public clsLicenseClass LicenseClassInfo;

        public string PersonFullName
        { 
            get 
            {
                //we can access to the base class info via sub class
                return clsPerson.FindPerson(ApplicantPersonID).FullName; //base.PersonInfo.FullName;              
            }
        }

        public clsUser UserInfo;

        //We did not add ApplicationID and ApplicationInfo because
        //we already inherit clsApplication

        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            //this.ApplicationID = -1;
            this.LicenseClassID = -1;

            Mode = enMode.AddNew;
        }

        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, 
                                                 int ApplicationID, int ApplicantPersonID,
                                                 DateTime ApplicationDate, int ApplicationTypeID,
                                                 enApplicationStatus ApplicationStatus, 
                                                 DateTime LastStatusDate, float PaidFees,
                                                 int CreatedByUserID, int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;         //Properties
            this.ApplicantPersonID = ApplicantPersonID; // 
            this.ApplicationDate = ApplicationDate;     //Inherited
            this.ApplicationTypeID = ApplicationTypeID; //
            this.ApplicationStatus = ApplicationStatus; //From
            this.LastStatusDate = LastStatusDate;       //
            this.PaidFees = PaidFees;                   //
            this.CreatedByUserID = CreatedByUserID;     //Base Class
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.FindLicenseByClassID(LicenseClassID); //COMPOSITION

            Mode = enMode.Update;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationsList();
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(this.ApplicationID, this.LicenseClassID);
            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication
                (this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }

        public static clsLocalDrivingLicenseApplication FindLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID
                     (LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID))
            {
                //now we will find the base application because we have now the ApplicationID
                //from the sub class
                clsApplication Application = clsApplication.GetApplicationInfoByID(ApplicationID);
               
                //we return new object of L.D.License with the right data
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID,
                                                             Application.ApplicationID,
                                                             Application.ApplicantPersonID,
                                                             Application.ApplicationDate, 
                                                             Application.ApplicationTypeID,
                                                             (enApplicationStatus)Application.ApplicationStatus, 
                                                             Application.LastStatusDate,
                                                             Application.PaidFees, 
                                                             Application.CreatedByUserID, 
                                                             LicenseClassID);
            }
            else
                return null;
        }

        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {

            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID
               (ApplicationID, ref LocalDrivingLicenseApplicationID, ref LicenseClassID))
            {
                //now we will find the base application with the ApplicationID
                clsApplication Application = clsApplication.GetApplicationInfoByID(ApplicationID);

                //we return new object of L.D.License with the right data
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID,
                                                             Application.ApplicationID,
                                                             Application.ApplicantPersonID,
                                                             Application.ApplicationDate, 
                                                             Application.ApplicationTypeID,
                                                             (enApplicationStatus)Application.ApplicationStatus, 
                                                             Application.LastStatusDate,
                                                             Application.PaidFees, 
                                                             Application.CreatedByUserID, 
                                                             LicenseClassID);
            }
            else
                return null;
        }

        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all informations to the application table.
            base.Mode  = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;

            //After we save the main application now we save the sub/child application.
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
            }

            return false;
        }

        public bool Delete()
        {
            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;

            //First we delete the Local Driving License Application
            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);

            if (!IsLocalDrivingApplicationDeleted)
                return false;

            //Then we delete the base Application
            IsBaseApplicationDeleted = base.DeleteApplication();

            return IsBaseApplicationDeleted;
        }

        public bool IsPassTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsPassTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool IsPassTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsPassTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesPassPreviousTest(clsTestType.enTestType CurrentTestType)
        {
            switch (CurrentTestType)
            {
                case clsTestType.enTestType.VisionTest:
                    //in this case no required previous test to pass.
                    return true;

                case clsTestType.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.
                    return this.IsPassTest(clsTestType.enTestType.VisionTest);

                case clsTestType.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    return this.IsPassTest(clsTestType.enTestType.WrittenTest);

                default:
                    return false;
            }
        }

        public bool DoesAttendTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public byte TotalTrialsPerTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool AttendedTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }

        public bool AttendedTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }

        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public bool IsPassedAllTests()
        {
            return clsTest.PassedAllTests(this.LocalDrivingLicenseApplicationID);
        }

        public static bool IsPassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return clsTest.PassedAllTests(LocalDrivingLicenseApplicationID);
        }

        public int IssueLicenseForTheFirtTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;

            clsDriver Driver = clsDriver.FindByPersonID(this.ApplicantPersonID);

            if (Driver == null)
            {
                //we check if the driver already there for this person.
                Driver = new clsDriver();

                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;

                if (Driver.Save())
                    DriverID = Driver.DriverID;
                else
                    return -1;              
            }
            else
                DriverID = Driver.DriverID;

            //now we diver is there, so we add new licesnse
            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                //now we should set the application status to complete. how we pass to clsApplication
                //bcs clsApplication inherits this class
                this.CompleteApplication();

                return License.LicenseID;
            }

            else
                return -1;
        }

        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }

        public int GetActiveLicenseID()
        {
            //this will get the license id that belongs to this application
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }
    }
}
