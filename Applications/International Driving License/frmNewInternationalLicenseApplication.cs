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

namespace DVLD_Project.Applications.International_Driving_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int _InternationalLicenseID = -1;
        
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.ShortDateTime(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            //We Add One Year Because International License Availability In The System is 1 Year
            lblExpirationDate.Text = clsFormat.ShortDateTime(DateTime.Now.AddDays(1));
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationTypeFees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            lblLocalLicenseID.Text = obj.ToString();
            LinklblShowLicenseHistory.Enabled = (obj != -1);

            if (obj == -1)
                return;

            //We Check The License Class, Person Could NOT Issue International License
            //Without Having normal License Of Class 3.

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClass != 3)
            { MessageBox.Show("Selected License Should Be Class 3, Select Another One.",
                                "Not allowed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return; }

            //We Also Check If The Person Already Have An Active International License
            int ActiveInternationalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);

            if(ActiveInternationalLicenseID != -1)
            {
                MessageBox.Show("Person Already Have an Active International License With ID = " + ActiveInternationalLicenseID, 
                                "Not Allowed", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                LinklblShowLicenseInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternationalLicenseID;
                btnIssueLicense.Enabled = false;
                return;
            }

            btnIssueLicense.Enabled = true;
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Issue The License?", 
                                "Confirm", 
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            //Those are the informations from the base application, Because it inhirited
            //from clsInternationalLicense, they are part of the sub class.

            InternationalLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationTypeFees;
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            InternationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                //                                  ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.CreatedByUserID;
            if(!InternationalLicense.Save())
            {
                MessageBox.Show("An Error Occured While Issuing This International License", 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);

                return;
            }

            MessageBox.Show("International License Issued Successfully with ID = " + _InternationalLicenseID,
                            "License Issued",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            btnIssueLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.IsFilterEnabled = false;
            LinklblShowLicenseInfo.Enabled = true;
        }

        private void LinklblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void LinklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void frmNewInternationalLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
