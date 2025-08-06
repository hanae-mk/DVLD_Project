using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew = 1, Update = 2};
        private enMode _Mode;

        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1};
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;

        public clsTestType.enTestType TestTypeID
        {
            get {  return  _TestTypeID;  }

            set
            {
                _TestTypeID = value;

                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                    {
                         gbTestType.Text = "Vision Test";
                         pbTestTypeImage.Image = Resources.Vision_512;
                         break;
                    }

                    case clsTestType.enTestType.WrittenTest:
                    {
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }

                    case clsTestType.enTestType.StreetTest:
                    {
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
                }
            }
        }

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        public void LoadInfo(int LocalDrivingLicenseApplicationID, int AppointmentID = -1)
        {
            //If No AppointmentID this means AddNew mode otherwise it's update mode
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppointmentID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID,
                                "Error",
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            //Decide if the creation mode is retake test or not based if the person attended this test before

            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

            if(_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblRetakeApplicationFees.Text = clsApplicationType.FindApplication((int)clsApplication.enApplicationType.RetakeTest).ApplicationTypeFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestApplicationID.Text = "0";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeApplicationFees.Text = "0";
                lblRetakeTestApplicationID.Text = "N/A";
            }

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.PersonFullName;

            //This will show the trials for this test before

            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(TestTypeID).ToString();

            if(_Mode == enMode.AddNew)
            {
                lblFees.Text = clsTestType.FindLicenseByLicenseID(_TestTypeID).TestTypeFees.ToString();
                dtpTestDate.MinDate = DateTime.Now;
                lblRetakeTestApplicationID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if(!_LoadTestAppointmentData())
                    return;          
            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeApplicationFees.Text)).ToString();

            if(!_HandleActiveTestAppointmentContraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePreviousTestConstraint())
                return;
        }

        private bool _HandleActiveTestAppointmentContraint()
        {
            if(_Mode == enMode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, _TestTypeID))
            {
                lblUserMessage.Text = "This Person Already have an active appointment for this test!";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }

            return true;
        }

        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.FindLicenseByLicenseIDTestAppointmentByID(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID,
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment.PaidFees.ToString();

            //we compare the current date with the appointment date to set the min date.
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate;

            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetakeApplicationFees.Text = "0";
                lblRetakeTestApplicationID.Text = "N/A";
            }
            else
            {
                lblRetakeApplicationFees.Text = _TestAppointment.RetakeTestApplicationInfo.PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestApplicationID.Text = _TestAppointment.RetakeTestApplicationID.ToString();

            }
            return true;
        }

        private bool _HandleAppointmentLockedConstraint()
        {
            if(_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person Already Sat For The Test, Appointment Locked!";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
            {
                lblUserMessage.Visible = false;
                return true;
            }
        }

        private bool _HandlePreviousTestConstraint()
        {
            //We need to make sure that this person passed the prvious required test
            //Before apply to the new test.
            //person cannot apply for written test unless s/he passes the vision test.
            //And cannot apply for street test unless s/he passes the written test.

            switch(TestTypeID)
            {
                //In this case no required prvious test to pass
                case clsTestType.enTestType.VisionTest: 
                    lblUserMessage.Visible = false;
                    return true;

                //We cannot sechdule it before person passes the vision test
                //We Check If Pass Vision Test 
                case clsTestType.enTestType.WrittenTest:
                    if(!_LocalDrivingLicenseApplication.IsPassTest(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Schedule, Vision Test should be passed first!";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {  
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Visible = true;
                        return true;
                    }

                //Street Test, you cannot sechdule it before person passes the written test
                //We check if pass Written 
                case clsTestType.enTestType.StreetTest:
                    if(!_LocalDrivingLicenseApplication.IsPassTest(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                        return true;
                    }
                default:
                    return true;             
            }
        }

        private bool _HandleRetakeApplication()
        {
            //This will decide to create a seperate application for retake test or not.
            //And will create it if needed, then it will link it to the appoinment.
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                //Incase the mode is AddNew and creation mode is retake test
                //we should create a seperate application for it.
                //Then we link it with the appointment.

                //We First Create Application 
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.FindApplication((int)clsApplication.enApplicationType.RetakeTest).ApplicationTypeFees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if(!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("An Error Occured While Creating This Application", 
                                    "Error", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully", 
                                "Saved", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
            }
            else        
                MessageBox.Show("Error: Data Is Not Saved Successfully", 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);         
        }

        

        private void ctrlScheduleTest_Load(object sender, EventArgs e)
        {

        }

        
    }
}
