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

namespace DVLD_Project.Licenses.Local_Licenses
{
    public partial class frmIssueDriverLicenseForFirstTime : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmIssueDriverLicenseForFirstTime(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }
      
        private void frmIssueDriverLicenseForFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application With ID = " + _LocalDrivingLicenseApplicationID,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (!_LocalDrivingLicenseApplication.IsPassedAllTests())
            {
                MessageBox.Show("This Person Should Pass All Tests First",
                                "Not Allowed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();

            if(LicenseID != -1)
            {
                MessageBox.Show("This Person Have Already a License With LicenseID = " + LicenseID,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingApplicationID(_LocalDrivingLicenseApplicationID);
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            int LicenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirtTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if(LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully With LicenseID = " + LicenseID,
                                "Successed!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("An Error Occured While Creating This License!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlLocalDrivingLicenseApplicationInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
