using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsApplication
    {
        public enum enMode { AddNew = 1, Update = 2 };
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };
        public enum enApplicationType { NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
                                        ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicense = 5,
                                        NewInternationalLicense = 6, RetakeTest = 7 };

        public enMode Mode = enMode.AddNew;
        
        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }
        public DateTime ApplicationDate { set; get; }
        public int ApplicationTypeID { set; get; }
        public enApplicationStatus ApplicationStatus { set; get; } //byte
        public DateTime LastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }

        public string ApplicantFullName //ApplicantFullName here is not User but Driver
        { 
            get { return clsPerson.FindPerson(ApplicantPersonID).FullName; }
        }

        //COMPOSITION
        public clsPerson PersonInfo { set; get; } 

        //COMPOSITION
        public clsApplicationType ApplicationTypeInfo;

        //COMPOSITION
        public clsUser CreatedByUserInfo; //we use it while loading data in form

        public string StatusText
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }

        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0.00f;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        public clsApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate,
                              int ApplicationTypeID, enApplicationStatus ApplicationStatus,
                              DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.PersonInfo = clsPerson.FindPerson(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID); 
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID); 

            Mode = enMode.Update;
        }

        public static clsApplication GetApplicationInfoByID(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = System.DateTime.Now, LastStatusDate = System.DateTime.Now;
            byte ApplicationStatus = 1; //No enum here bcs there is no enum in database later we convert
            float PaidFees = 0.00f;

            if(clsApplicationData.GetApplicationInfoByID(ApplicationID, ref ApplicantPersonID, 
                                                          ref ApplicationDate, ref ApplicationTypeID,
                                                          ref ApplicationStatus, ref LastStatusDate,
                                                          ref PaidFees, ref CreatedByUserID))
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate,
                                          ApplicationTypeID, (enApplicationStatus)ApplicationStatus,
                                          LastStatusDate, PaidFees, CreatedByUserID);
            else
                return null;
        }

        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, 
                                 this.ApplicationDate, this.ApplicationTypeID, 
                                 (byte)this.ApplicationStatus, this.LastStatusDate,
                                 this.PaidFees, this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID,
                                                        this.ApplicationDate, this.ApplicationTypeID,
                                                        (byte)this.ApplicationStatus, this.LastStatusDate,
                                                        this.PaidFees, this.CreatedByUserID);
        }

        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID = -1;
            DateTime ApplicationDate = DateTime.Now; int ApplicationTypeID = -1;
            byte ApplicationStatus = 1; DateTime LastStatusDate = DateTime.Now;
            float PaidFees = 0; int CreatedByUserID = -1;

            bool IsFound = clsApplicationData.GetApplicationInfoByID
                                (
                                    ApplicationID, ref ApplicantPersonID,
                                    ref ApplicationDate, ref ApplicationTypeID,
                                    ref ApplicationStatus, ref LastStatusDate,
                                    ref PaidFees, ref CreatedByUserID
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsApplication(ApplicationID, ApplicantPersonID,
                                     ApplicationDate, ApplicationTypeID,
                                    (enApplicationStatus)ApplicationStatus, LastStatusDate,
                                     PaidFees, CreatedByUserID);
            else
                return null;
        }

        public bool CancelApplication()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, 2);
        }

        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, 3);
        }

        public bool DeleteApplication()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID);
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }

        public static bool IsThisApplicationActive(int ApplicantPersonID, int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(ApplicantPersonID, ApplicationTypeID);
        }

        public bool IsThisApplicationActive(int ApplicationTypeID)
        {
            return IsThisApplicationActive(this.ApplicantPersonID, ApplicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, int ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(ApplicantPersonID, ApplicationTypeID, LicenseClassID);
        }

        public static int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true; 
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateApplication();

                default:
                    return false;
            }
        }
    }
}
