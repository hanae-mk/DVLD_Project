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
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        private int _NewLicenseID = -1;
        
        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            lblOldLicenseID.Text = obj.ToString();

            linklblShowLicenseHistory.Enabled = (obj != -1);

            if(obj == -1)
                return;

            int DefaultValidityLength = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength;
            lblExpirationDate.Text = clsFormat.ShortDateTime(DateTime.Now.AddYears(DefaultValidityLength));
            lblLicenseFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();
            txtNotes.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;

            //Check The License If it's expired Or Not!
            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is Not Expired, It Will Expire On: " + 
                                 clsFormat.ShortDateTime(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate), 
                                 "Not Allowed", 
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is Not Active, Choose An Active License",
                                 "Not Allowed",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            btnRenewLicense.Enabled = true;
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            btnRenewLicense.Enabled = false;
            linklblShowNewLicenseInfo.Enabled = false;
            linklblShowLicenseHistory.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();

            lblApplicationDate.Text = clsFormat.ShortDateTime(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationTypeFees.ToString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }
   
        private void linklblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void linklblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Renew This License?",
                                "Renew",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.No)
                return;           

            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if(NewLicense == null)
            {
                MessageBox.Show("An Error Occured While Renewing This License",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            lblRenewLicenseApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblRenewedLicenseID.Text = _NewLicenseID.ToString();

            MessageBox.Show("License Renewed Successfully With ID = " + _NewLicenseID.ToString(),
                            "Renew License",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            btnRenewLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.IsFilterEnabled = false;
            linklblShowNewLicenseInfo.Enabled = true;
        }

        private void frmRenewLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
