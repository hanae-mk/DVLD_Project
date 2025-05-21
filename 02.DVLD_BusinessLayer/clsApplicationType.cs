using DVLD_DataAccessLayer;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 1, Update = 2};
        public enMode Mode;

        //3lash ma3mlnahaxi hnaya
        //public enum enApplicationType { NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense=3,
        //ReplaceDamagedDrivingLicense=4, ReleaseDetainedDrivingLicsense=5, NewInternationalLicense=6,RetakeTest=7
        //3lash 3melnaha f clsApplication

        public int ApplicationTypeID { set; get; }
        public string ApplicationTypeTitle { set; get; }
        public float ApplicationTypeFees { set; get; }

        //Parameter Less Constructor used in AddNew Mode
        public clsApplicationType()
        {
            ApplicationTypeID = -1;
            ApplicationTypeTitle = null;
            ApplicationTypeFees = 0.00f;

            Mode = enMode.AddNew;
        }

        //Parameterized Constructor FULL with MEAGNIFUL data
        public clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationTypeFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationTypeFees = ApplicationTypeFees;

            Mode = enMode.Update;
        }

        private bool _AddNewApplicationType()
        {
            this.ApplicationTypeID = clsApplicationTypeData.AddNewApplicationType(this.ApplicationTypeTitle, this.ApplicationTypeFees);
            
            return (this.ApplicationTypeID != -1);
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationTypeFees);
        }

        public static clsApplicationType Find(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";
            float ApplicationTypeFees = 0.00f;

            if (clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationTypeFees))
                return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationTypeFees);
            else
                return null;
        }

        public static DataTable GetApplicationsTypesList()
        {
            return clsApplicationTypeData.GetApplicationTypesList();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                {
                    if (_AddNewApplicationType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                }
                case enMode.Update:
                    return _UpdateApplicationType();

                default:
                    return false;
            }
        }     
    }
}
