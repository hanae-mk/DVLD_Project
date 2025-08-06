using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Project.Tests.Test_Types;
using DVLD_Project.Tests;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.Licenses;

namespace DVLD_Project.Applications.Local_Driving_License
{
    public partial class frmLocalDrivingLicenseApplicationsList : Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplications;

        public frmLocalDrivingLicenseApplicationsList()
        {
            InitializeComponent();
        }

        public void FillComboBoxWithFilters()
        {
            cbFilterBy.Items.Add("None");
            cbFilterBy.Items.Add("L.D.L. AppID");
            cbFilterBy.Items.Add("National No.");
            cbFilterBy.Items.Add("Full Name");
            cbFilterBy.Items.Add("Status");
        }

        private void frmLocalDrivingLicenseApplicationsList_Load(object sender, EventArgs e)
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrivingLicenseApplications;

            FillComboBoxWithFilters();
            cbFilterBy.SelectedIndex = 0;

            lblRowsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();

            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 100;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 250;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns[2].Width = 110;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 250;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 100;
            }           
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            //Refresh
            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if(txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRowsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch(cbFilterBy.Text)
            {
                case "L.D.L. AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            //Reset Filters incase nothing is selected
            if(txtFilterValue.Text == "" || cbFilterBy.Text == "None")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRowsCount.Text = _dtAllLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }

            if(FilterColumn == "LocalDrivingLicenseApplicationID")
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            
            lblRowsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow just numbers incase L.D.L.AppID id is selected

            if(cbFilterBy.Text == "L.D.L.AppID")
            {
                //here we check character by character that why we cannot have txtfiltervalue.text
                //inside IsDigit()
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsTestAppointment Appointment = clsTestAppointment.GetLastTestAppointment((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.VisionTest);

            if (Appointment == null)
            {
                MessageBox.Show("No Vision Test Appointment Found!",
                                "Set Appointment",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            frmTakeTest frm = new frmTakeTest(Appointment.TestAppointmentID, clsTestType.enTestType.VisionTest);
            frm.ShowDialog();
        }

        private void WrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            if (!clsLocalDrivingLicenseApplication.IsPassTest(LocalDrivingLicenseApplicationID, clsTestType.enTestType.VisionTest))
            {
                MessageBox.Show("Person Should Pass the Vision Test First!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTestAppointment Appointment = clsTestAppointment.GetLastTestAppointment(LocalDrivingLicenseApplicationID, clsTestType.enTestType.WrittenTest);

            if (Appointment == null)
            {
                MessageBox.Show("No Written Test Appointment Found!",
                                "Set Appointment",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            frmTakeTest frm = new frmTakeTest(Appointment.TestAppointmentID, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
        }

        private void StreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsLocalDrivingLicenseApplication.IsPassTest((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.WrittenTest))
            {
                MessageBox.Show("Person Should Pass the Written Test First!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTestAppointment Appointment = clsTestAppointment.GetLastTestAppointment((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.StreetTest);

            if (Appointment == null)
            {
                MessageBox.Show("No Street Test Appointment Found!", 
                                "Set Appointment", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return;
            }

            frmTakeTest frm = new frmTakeTest(Appointment.TestAppointmentID, clsTestType.enTestType.StreetTest);
            frm.ShowDialog();
        }

        private void _ScheduleTest(clsTestType.enTestType TestType)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, TestType);
            frm.ShowDialog();

            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();

            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDriverLicenseForFirstTime frm = new frmIssueDriverLicenseForFirstTime((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            int TotalPassedTests = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;

            bool IsLicenseIssued = LocalDrivingLicenseApplication.IsLicenseIssued();

            //Enabled only if person passed all tests and Does not have a license yet
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && (!IsLicenseIssued);

            showLicenseToolStripMenuItem.Enabled = IsLicenseIssued;
            editApplicationToolStripMenuItem.Enabled = !IsLicenseIssued && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            scheduleTestsToolStripMenuItem.Enabled = !IsLicenseIssued;

            //Enable/Disable Cancel Menu Item
            //We only allow cancel incase the application status is new
            cancelApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            //Enable/Disable Delete Menu Item
            //We only allow delete incase the application status is new not complete or Cancelled
            deleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            //Enable Disable Schedule Menue and it's Sub Menue
            bool PassedVisionTest = LocalDrivingLicenseApplication.IsPassTest(clsTestType.enTestType.VisionTest); ;
            bool PassedWrittenTest = LocalDrivingLicenseApplication.IsPassTest(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.IsPassTest(clsTestType.enTestType.StreetTest);

            scheduleTestsToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            if (scheduleTestsToolStripMenuItem.Enabled)
            {
                //To Allow Schedule vision test, Person must not passed the same test before.
                scheduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                //To Allow Schedule written test, Person must pass the vision test and must not passed the same test before.
                scheduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                //To Allow Schedule street test, Person must pass the vision * written tests, and must not passed the same test before.
                scheduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure you want to cancel this Application?",
                                "Confirm",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            if(localDrivingLicenseApplication != null)
            {
                if(localDrivingLicenseApplication.CancelApplication()) //Cancel()
                {
                    MessageBox.Show("Application Cancelled Successfully",
                                    "Cancelled",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    //Refresh Form
                    frmLocalDrivingLicenseApplicationsList_Load(null, null);
                }
                else
                    MessageBox.Show("Could NOT Cancel Application",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You sure you want to delete this Application?",
                               "Confirm",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            if(LocalDrivingLicenseApplication != null)
            {
                if(LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully",
                                    "Deleted",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    //Refresh Form
                    frmLocalDrivingLicenseApplicationsList_Load(null, null);
                }
                else
                    MessageBox.Show("Could NOT Delete Application, Other Data Linked To This Application",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(LocalDrivingLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
