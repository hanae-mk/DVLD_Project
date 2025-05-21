using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Tests.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests
{
    public partial class frmTakeTest : Form
    {
        private int _AppointmentID = -1;
        private clsTestType.enTestType _TestType;

        private int _TestID = -1;
        private clsTest _Test;
        
        public frmTakeTest(int AppointmentID, clsTestType.enTestType TestType)
        {
            InitializeComponent();

            _AppointmentID = AppointmentID;
            _TestType = TestType;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestTypeID = _TestType;
            ctrlScheduledTest1.LoadInfo(_AppointmentID);

            if (ctrlScheduledTest1.TestAppointmentID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;


            int _TestID = ctrlScheduledTest1.TestID;

            if(_TestID != -1)
            {
                _Test = clsTest.Find(_TestID);

                if (_Test.TestResult)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;

                txtNotes.Text = _Test.Notes;

                lblUserMessage.Visible = true;
                rbPass.Enabled = false;
                rbFail.Enabled = false;
            }
            else
                _Test = new clsTest();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Save? After Saving Results You CANNOT Change Them",
                                "Confirm",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.No)
                return;

            _Test.TestAppointmentID = _AppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully", 
                                "Saved", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
                MessageBox.Show("An Error Occured While Saving Data",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
        }                      

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
