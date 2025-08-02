using DVLD_BusinessLayer;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.People;
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
    public partial class frmInternationalLicenseApplicationList : Form
    {
        public DataTable _dtAllInternationalLicenseApplications;         
        
        public frmInternationalLicenseApplicationList()
        {
            InitializeComponent();
        }

        private void frmInternationalLicenseApplicationList_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            
            _dtAllInternationalLicenseApplications = clsInternationalLicense.GetAllInternationalLicenses();
            dgvInternationalDrivingLicenseApplications.DataSource = _dtAllInternationalLicenseApplications;

            lblRowsCount.Text = dgvInternationalDrivingLicenseApplications.Rows.Count.ToString();

            if(dgvInternationalDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvInternationalDrivingLicenseApplications.Columns[0].HeaderText = "License ID";
                dgvInternationalDrivingLicenseApplications.Columns[0].Width = 100;

                dgvInternationalDrivingLicenseApplications.Columns[1].HeaderText = "Application ID";
                dgvInternationalDrivingLicenseApplications.Columns[1].Width = 100;

                dgvInternationalDrivingLicenseApplications.Columns[2].HeaderText = "Driver ID";
                dgvInternationalDrivingLicenseApplications.Columns[2].Width = 100;

                dgvInternationalDrivingLicenseApplications.Columns[3].HeaderText = "Local License ID";
                dgvInternationalDrivingLicenseApplications.Columns[3].Width = 120;

                dgvInternationalDrivingLicenseApplications.Columns[4].HeaderText = "Issue Date";
                dgvInternationalDrivingLicenseApplications.Columns[4].Width = 120;

                dgvInternationalDrivingLicenseApplications.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalDrivingLicenseApplications.Columns[5].Width = 120;

                dgvInternationalDrivingLicenseApplications.Columns[6].HeaderText = "Is Active";
                dgvInternationalDrivingLicenseApplications.Columns[6].Width = 100;
            }
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();

            //Refresh
            frmInternationalLicenseApplicationList_Load(null, null);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalDrivingLicenseApplications.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindLicenseByLicenseIDByDriverID(DriverID).PersonID;
            
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            frmShowLicenseInfo frm = new frmShowLicenseInfo((int)dgvInternationalDrivingLicenseApplications.CurrentRow.Cells[3].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalDrivingLicenseApplications.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindLicenseByLicenseIDByDriverID(DriverID).PersonID;

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Enabled = false;
                cbIsActive.Enabled = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
                return;
            }

            txtFilterValue.Visible = (cbFilterBy.Text != "None");
            cbIsActive.Visible = false;

            if(cbFilterBy.Text == "None")
                txtFilterValue.Enabled = false;
            else
                txtFilterValue.Enabled = true;

            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;

            switch(FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }

            if (FilterValue == "All")
                _dtAllInternationalLicenseApplications.DefaultView.RowFilter = "";
            else
                _dtAllInternationalLicenseApplications.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, FilterValue);

            lblRowsCount.Text = dgvInternationalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            //Map Selected Filter To Real Column Name
            switch(cbFilterBy.Text)
            {
                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;

                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;

                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Local License ID":
                    FilterColumn = "LocalLicenseID";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if(FilterColumn == "None" || txtFilterValue.Text.Trim() == "")
                _dtAllInternationalLicenseApplications.DefaultView.RowFilter = "";
            else
                _dtAllInternationalLicenseApplications.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());

            dgvInternationalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //We Allow ONLY Numbers Becasue ALL Fiters Aare Numbers.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }     
    }
}
