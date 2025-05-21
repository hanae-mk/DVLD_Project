using DVLD_Project.Global_Classes;
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

namespace DVLD_Project.Licenses
{
    public partial class frmDetainLicense : Form
    {
        private int _DetainID = -1;
        private int _SelectedLicenseID = -1;
    
        public frmDetainLicense()
        {
            InitializeComponent();
        }
        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Detain This License?",
                                "Confirm",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.No)
                return;

            _DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainLicense(Convert.ToSingle(txtFineFees.Text), clsGlobal.CurrentUser.UserID);

            if(_DetainID == -1)
            {
                MessageBox.Show("An Error Occured While Detaining This License", 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("License Detained Successfully with ID = " + _DetainID,
                            "License Issued", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
            
            lblDetainID.Text = _DetainID.ToString();
            btnDetainLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.IsFilterEnabled = false;
            txtFineFees.Enabled = false;
            LinklblShowLicenseInfo.Enabled = true;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;

            if (_SelectedLicenseID == -1)
                return;
            
            lblLicenseID.Text = _SelectedLicenseID.ToString();
            LinklblShowLicenseHistory.Enabled = (_SelectedLicenseID != -1);

            //Make Sure The License is NOT Already Detained
            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License is Already Detained, Choose Another One", 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return;
            }

            txtFineFees.Focus();
            btnDetainLicense.Enabled = true;
        }

        private void frmDetainedLicenses_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "This Field Is Required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, null);
            }

            if (!clsValidation.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Only Numbers Are Valid!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, null);
            }
        }

        private void LinklblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void LinklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }      
    }
}
