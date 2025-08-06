using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;
using DVLD_Project.Applications.Local_Driving_License;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.People;

namespace DVLD_Project.Applications.Controls
{
    public partial class ctrlLocalDrivingLicenseApplicationInfo : UserControl
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private int _LocalDrivingLicenseApplicationID = -1;

        private int _LicenseID= -1;

        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }

        public ctrlLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplicationID = -1;
            ctrlApplicationBasicInfo1.ResetApplicationInfo();
            lblLocalDrivingLicenseApplicationID.Text = "[???]";
            lblAppliedForLicense.Text = "[???]";
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();

            //Incase there is a license enabled in the show link.
            linklblShowLicenseInfo.Enabled = (_LicenseID != -1);

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblAppliedForLicense.Text = clsLicenseClass.FindLicenseByLicenseIDLicenseByLicenseClassID(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblPassedTests.Text = _LocalDrivingLicenseApplication.GetPassedTestCount().ToString() + " /3";
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);
        }

        public void LoadApplicationInfoByLocalDrivingApplicationID(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();

                MessageBox.Show("Application with ID = " + LocalDrivingLicenseApplicationID + " Is NOT Found!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByApplicationID(ApplicationID);

            if(_LocalDrivingLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();

                MessageBox.Show("Application with ID = " + ApplicationID + " Is NOT Found!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        private void linklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_LocalDrivingLicenseApplication.GetActiveLicenseID());
            frm.ShowDialog();
        }
    }
}
