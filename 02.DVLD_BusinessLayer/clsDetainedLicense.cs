using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode;

        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }

        public float FineFees { set; get; }
        public int CreatedByUserID { set; get; }

        //COMPOSITION
        public clsUser CreatedByUserInfo { set; get; } 
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public clsUser ReleasedByUserInfo { set; get; }
        public int ReleaseApplicationID { set; get; }

        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = 0;
            this.ReleaseApplicationID = -1;

            Mode = enMode.AddNew;
        }

        public clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate,
                                  float FineFees, int CreatedByUserID, bool IsReleased,
                                  DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindLicenseByLicenseIDByUserID(this.CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleasedByUserInfo = clsUser.FindLicenseByLicenseIDByPersonID(this.ReleasedByUserID);
            this.ReleaseApplicationID = ReleaseApplicationID;
            
            Mode = enMode.Update;
        }
        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

        private bool _AddNewDetainedLicense()
        {
            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(this.LicenseID,
                                                                         this.DetainDate, 
                                                                         this.FineFees, 
                                                                         this.CreatedByUserID);
            return (this.DetainID != -1);
        }

        private bool _UpdateDetainedLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicense(this.DetainID, this.LicenseID,
                                                                this.DetainDate, this.FineFees, 
                                                                this.CreatedByUserID);
        }

        public static clsDetainedLicense FindLicenseByLicenseIDByDetainID(int DetainID)
        {
            int LicenseID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1, CreatedByUserID = -1; 
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0; 
            bool IsReleased = false; 

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByDetainID(DetainID, ref LicenseID,
                                                                        ref DetainDate, ref FineFees,
                                                                        ref CreatedByUserID, ref IsReleased,
                                                                        ref ReleaseDate, ref ReleasedByUserID,
                                                                         ref ReleaseApplicationID))
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees,
                                              CreatedByUserID, IsReleased, ReleaseDate,
                                              ReleasedByUserID, ReleaseApplicationID);
            else
                return null;
        }       

        public static clsDetainedLicense FindLicenseByLicenseIDByLicenseID(int LicenseID)
        {
            int DetainID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1, CreatedByUserID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool IsReleased = false;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseID, ref DetainID,
                                                                         ref DetainDate, ref FineFees,
                                                                         ref CreatedByUserID, ref IsReleased,
                                                                         ref ReleaseDate, ref ReleasedByUserID,
                                                                         ref ReleaseApplicationID))
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate,
                                              FineFees, CreatedByUserID, IsReleased,
                                              ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            else
                return null;
        }
        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        public bool IsDetainedLicenseReleased(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.IsDetainedLicenseReleased(this.DetainID,
                                                                    ReleasedByUserID, 
                                                                    ReleaseApplicationID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                    
                case enMode.Update:
                    return _UpdateDetainedLicense();

                default:
                    return false;
            }        
        }
    }
}
