using DVLD_BusinessLayer;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests
{
    public partial class frmListTestAppointments : Form
    {
        private DataTable _dtLicenseTestsAppointments;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;
             
        public frmListTestAppointments(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestType = TestType;
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch(_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                {
                    lblTitle.Text = "Vision Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.Vision_512;
                    break;
                }

                case clsTestType.enTestType.WrittenTest:
                {
                    lblTitle.Text = "Written Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.Written_Test_512;
                    break;
                }

                case clsTestType.enTestType.StreetTest:
                {
                    lblTitle.Text = "Street Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.driving_test_512;
                    break;
                }
            }
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingApplicationID(_LocalDrivingLicenseApplicationID);
            _dtLicenseTestsAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestType);

            dgvAppointments.DataSource = _dtLicenseTestsAppointments;
            lblRecords.Text = dgvAppointments.Rows.Count.ToString();

            if(dgvAppointments.Rows.Count > 0)
            {
                dgvAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvAppointments.Columns[0].Width = 150;

                dgvAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvAppointments.Columns[1].Width = 200;

                dgvAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvAppointments.Columns[2].Width = 150;

                dgvAppointments.Columns[3].HeaderText = "Is Locked";
                dgvAppointments.Columns[3].Width = 100;
            }
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindApplicationByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if(LocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person Already have an Active Appointment for this test, You Cannot Add New Appointment", 
                                "Not allowed", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = LocalDrivingLicenseApplication.GetLastTestPerTestType(_TestType);

            if(LastTest == null)
            {
                  frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType);
                  frm.ShowDialog();
                  frmListTestAppointments_Load(null, null);
                  return;
            }

            //If The Person Already Passed The Test he cannot retake it!
            if(LastTest.TestResult)
            {
                MessageBox.Show(@"This Person Already Passed This Test Before, you can only retake faild test", 
                                 "Not Allowed", 
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest(LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestType);
            frm2.ShowDialog();

            //Refresh Form
            frmListTestAppointments_Load(null, null);
        }

        private void editAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType, (int)dgvAppointments.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListTestAppointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTakeTest frm = new frmTakeTest((int)dgvAppointments.CurrentRow.Cells[0].Value, _TestType);
            frm.ShowDialog();

            frmListTestAppointments_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }    
    }
}
