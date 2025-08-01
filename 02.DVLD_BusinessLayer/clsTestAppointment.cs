﻿using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { set; get; }
        public clsTestType.enTestType TestTypeID { set; get; }
        public int LocalDrivingLicenseApplicationID { set; get; }
        public DateTime AppointmentDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsLocked { set; get; }
        public int RetakeTestApplicationID { set; get; }
        public clsApplication RetakeTestApplicationInfo { set; get; }

        public int TestID
        {
            get { return _GetTestID(); }
        }

        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = clsTestType.enTestType.VisionTest;
            AppointmentDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
            RetakeTestApplicationID = -1;

            Mode = enMode.AddNew;
        }

        public clsTestAppointment(int TestAppointmentID, clsTestType.enTestType TestTypeID,
                                  int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, 
                                  float PaidFees, int CreatedByUserID, bool IsLocked,
                                  int RetakeTestApplicationID)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestApplicationInfo = clsApplication.GetApplicationInfoByID(RetakeTestApplicationID);
            
            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointment()
        { 
            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment((int)this.TestTypeID,
                                     this.LocalDrivingLicenseApplicationID, this.AppointmentDate,
                                     this.PaidFees, this.CreatedByUserID, this.RetakeTestApplicationID);
            
            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, 
                   (int)this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                   this.AppointmentDate, this.PaidFees, this.CreatedByUserID, 
                   this.IsLocked, this.RetakeTestApplicationID);
        }

        public static clsTestAppointment FindLicenseByLicenseIDTestAppointmentByID(int TestAppointmentID)
        {
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1, 
                CreatedByUserID = -1, RetakeTestApplicationID = -1;

            DateTime AppointmentDate = DateTime.Now; 
            float PaidFees = 0;
            bool IsLocked = false;

            if (clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, 
                               ref TestTypeID, ref LocalDrivingLicenseApplicationID,
                               ref AppointmentDate, ref PaidFees, ref CreatedByUserID, 
                               ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)TestTypeID, 
                                              LocalDrivingLicenseApplicationID, AppointmentDate,
                                              PaidFees, CreatedByUserID, IsLocked, 
                                              RetakeTestApplicationID);
            else
                return null;
        }

        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            int CreatedByUserID = -1, RetakeTestApplicationID = -1, TestAppointmentID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            bool IsLocked = false;

            if (clsTestAppointmentData.GetLastTestAppointmentInfo(LocalDrivingLicenseApplicationID, 
                      (int)TestTypeID, ref TestAppointmentID, ref AppointmentDate, ref PaidFees,
                      ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, 
                                              LocalDrivingLicenseApplicationID,
                                              AppointmentDate, PaidFees, CreatedByUserID, 
                                              IsLocked, RetakeTestApplicationID);
            else
                return null;
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }

        public DataTable GetApplicationTestAppointmentsPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                    
                case enMode.Update:
                    return _UpdateTestAppointment();

                default:
                    return false;
            }
        }

        private int _GetTestID()
        {
            return clsTestAppointmentData.GetTestID(TestAppointmentID);
        }
    }
}
