using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew = 1, Update = 2};
        private enMode _Mode = enMode.AddNew;
       
        private int _SelectedPersonID = -1; //We use this variable in DataBack Event

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;

        //we have 2 constructors : OverLoading Constructor
        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        //Parameterized Constructor
        public frmAddUpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _Mode = enMode.Update;
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable Table = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow Row in Table.Rows)
            {
                cbLicenseClass.Items.Add(Row["ClassName"]);
            }
        }

        private void _ResetDefaultValues()
        {
            _FillLicenseClassesInComboBox();

            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application"; //this.Text means the title of the form
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;

                lblLocalDrivingLicenseApplicationID.Text = "[???]";
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                cbLicenseClass.SelectedIndex = 2; //Ordinary Driving License
                //Here we chose enApplicationType.NewDrivingLicense because we are in AddNew Mode
                lblFees.Text = clsApplicationType.FindApplication((int)clsApplication.enApplicationType.NewDrivingLicense).ApplicationTypeFees.ToString();   
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";           
                tpApplicationInfo.Enabled = true;             
            }
        }

        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            //we have app ID bcs we are in update mode
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindLicenseByLicenseIDLocalDrivingLicenseApplicationInfoByID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Application with ID = " + _LocalDrivingLicenseApplicationID + " is NOT Found!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
                return;
            }
           
            //We transfer Data from Object to Controls
            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.ApplicationID.ToString(); 

            //We can also use .ToShortDateString();
            lblApplicationDate.Text = clsFormat.ShortDateTime(_LocalDrivingLicenseApplication.ApplicationDate);

            //First we FindLicenseByLicenseID LicenseClassID we get it's Name then we search the LicenseClassName
            //In the ComboBox using FindLicenseByLicenseIDString() Method
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.FindLicenseByLicenseIDLicenseByLicenseClassID(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);

            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.FindLicenseByLicenseIDByUserID(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //Incase of Add New Mode
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please Select A Person",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus(); 
                return;
            }

            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
                return;
            }

            //Incase of AddNew Mode we don't check if this person have an application
            //or not because every person can have more than one application
            btnSave.Enabled = true;
            tpApplicationInfo.Enabled = true;
            tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsLicenseClass.FindLicenseByClassName(cbLicenseClass.Text).LicenseClassID;

            //we check if the applicant have already an active application type NewDrivingLicense for a specific license class
            //bool IsActiveApplication
            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, (int)clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);
          
            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose Another License Class, The Selected Person Already have an active application for the selected class with ID = " + ActiveApplicationID, 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }

            //then we check if the applicant already have an active license
            //of the same driving license class.
            if (clsLicense.IsLicenseExist(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {
                MessageBox.Show("This Person already have an active license with the same applied driving class, Choose a different driving class",
                                "Not allowed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            //Save Controls Info in The Object
            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;

            if (_LocalDrivingLicenseApplication.Save())
            {
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();                         
                MessageBox.Show("Data Saved Successfully", 
                                "Saved", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("An Error Occured While Saving Data", 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
        }

        //we didn't use it here we use it in ctrlPersonCardWithFilter to return data
        private void _DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received why we didn't use this method in 
            //frmAddUpdateUser because when we click on search we receive data
            _SelectedPersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_SelectedPersonID);
        }

        //here we receive PersonID from ctrlPersonCardWithFilter
        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        //Load() Event is fired before Activated() Event
        //private void frmAddUpdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        //{
        //    ctrlPersonCardWithFilter1.FilterFocus();
        //}

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
