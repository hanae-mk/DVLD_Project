using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsInternationalLicense : clsApplication
    {
        public enum enMode {AddNew = 1, Update = 2};
        public enMode Mode = enMode.AddNew;
     
        public int DriverID { set; get; }

        //COMPOSITION : Is A Relationship Between Classes : An Object of a Class Inside Another Class
        public clsDriver DriverInfo;

        public int InternationalLicenseID = -1;
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }

        public clsInternationalLicense()
        {
            this.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = false;
 
            Mode = enMode.AddNew;
        }

        public clsInternationalLicense(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate,
                                       int ApplicationTypeID, enApplicationStatus ApplicationStatus, 
                                       DateTime LastStatusDate, float PaidFees, int CreatedByUserID,
                                       int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
                                       DateTime IssueDate, DateTime ExpirationDate, bool IsActive)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            //why here we set ApplicationStatus as enApplicationStatus and not byte
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;

            this.InternationalLicenseID = InternationalLicenseID;
            this.DriverID = DriverID;
            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;

            Mode = enMode.Update;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense
                (this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, 
                 this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);

            return (this.InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense
                (this.InternationalLicenseID, this.ApplicationID, this.DriverID, 
                 this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, 
                 this.IsActive, this.CreatedByUserID);
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = true;

            

            if (clsInternationalLicenseData.GetInternationalLicenseInfoByID(IssuedUsingLocalLicenseID,
                ref ApplicationID, ref DriverID, ref IssuedUsingLocalLicenseID, ref IssueDate,
                ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                clsApplication Application = clsApplication.GetApplicationInfoByID(ApplicationID);

                return new clsInternationalLicense(Application.ApplicationID, Application.ApplicantPersonID,
                                                   Application.ApplicationDate, Application.ApplicationTypeID,
                                                   (enApplicationStatus)Application.ApplicationStatus,
                                                   Application.LastStatusDate, Application.PaidFees,
                                                   Application.CreatedByUserID, InternationalLicenseID,
                                                   DriverID, IssuedUsingLocalLicenseID,
                                                   IssueDate, ExpirationDate, IsActive);                                           
            }
            else
                return null;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }

        public bool Save()
        {
            //Because of used inheritance principle we call first  the save method
            //in the base class, it will take care of adding all information to
            //the application table
            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                if (_AddNewInternationalLicense())
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                    return false;
                
                case enMode.Update:
                    return _UpdateInternationalLicense();

                default:
                    return false;
            }
        }
    }
}
