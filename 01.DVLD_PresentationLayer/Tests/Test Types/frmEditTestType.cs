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

namespace DVLD_Project.Tests.Test_Types
{
    public partial class frmEditTestType : Form
    {
        private clsTestType.enTestType _TestTypeID;
        private clsTestType _TestType;
        
        public frmEditTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;        
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(_TestTypeID);

            if (_TestType != null)
            {
                //Display Data
                lblTestTypeID.Text = ((int)_TestTypeID).ToString();
                txtTestTypeTitle.Text = _TestType.TestTypeTitle;
                txtTestTypeDescription.Text = _TestType.TestTypeDescription;
                txtTestTypeFees.Text = _TestType.TestTypeFees.ToString();
            }
            else
            {
                MessageBox.Show("Test Type with ID = " + _TestTypeID + " is NOT found!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
            //In this case The sender can be Title Or Description 
            TextBox Temp = (TextBox)sender;

            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This Field is Required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Checks Both Title and Description
            e.Handled = char.IsDigit(e.KeyChar);
        }

        private void txtTestTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTypeFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeFees, "This Field is Required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTestTypeFees, null);
            }

            if (!clsValidation.IsNumber(txtTestTypeFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeFees, "Invalid Number!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTestTypeFees, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show(@"Some Fields Are Not Valid, Please put the mouse 
                                  over the red icon(s) to see the error!",
                                  "Validation Error(s)",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                return; //So here we don't continue because the form is NOT valid!
            }

            //Fill Object with modified Data
            _TestType.TestTypeTitle = txtTestTypeTitle.Text;
            _TestType.TestTypeDescription = txtTestTypeDescription.Text;
            _TestType.TestTypeFees = Convert.ToSingle(txtTestTypeFees.Text);

            if(_TestType.Save())
                MessageBox.Show("Data Saved Successfully!",
                                "Save",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            else
                MessageBox.Show("An Error Occured while saving Data, Please Try Again!",
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
