using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_BusinessLayer.clsLicense;

namespace DVLD_Project.Applications.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private int _NewLicenseID = -1;

        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private int _GetApplicationTypeID()
        {
            //This Will Decide Which Application Type To Use According 
            //To User Selection.
            if (rbDamagedLicense.Checked)
                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
        }

        private enIssueReason _GetIssueReason()
        {
            //This Will Decide Which Reason To Issue a Replacement For
            if (rbDamagedLicense.Checked)
                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement;
        }
        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.ShortDateTime(DateTime.Now);
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

            rbDamagedLicense.Checked = true;
            LinklblShowLicenseHistory.Enabled = false;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement For Damaged License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.FindApplication(_GetApplicationTypeID()).ApplicationTypeFees.ToString();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement For Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.FindApplication(_GetApplicationTypeID()).ApplicationTypeFees.ToString();
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            lblOldLicenseID.Text = obj.ToString();
            LinklblShowLicenseHistory.Enabled = (obj != -1);

            if(obj == -1)
                return;

            //Don't Allow a Replacement if is Active
            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is Not Active, Choose an Active License.",
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }

            btnIssueReplacement.Enabled = true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Issue a Replacement For This License?", 
                                "Confirm", 
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(_GetIssueReason(), clsGlobal.CurrentUser.UserID);

            if(NewLicense == null)
            {
                MessageBox.Show("Failed To Issue a Replacemnet For This  License", 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;

            lblReplacedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("License Replaced Successfully With ID = " + _NewLicenseID.ToString(), 
                            "License Issued", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);

            btnIssueReplacement.Enabled = false;
            gbReplacementFor.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.IsFilterEnabled = false;
            LinklblShowNewLicenseInfo.Enabled = true;
        }

        private void LinklblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void LinklblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
    }
}
