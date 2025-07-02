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

namespace DVLD_Project.Applications.Application_Types
{
    public partial class frmEditApplicationType : Form
    {
        
        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;

        public frmEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            lblApplicationTypeID.Text = _ApplicationTypeID.ToString();

            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            if(_ApplicationType != null)
            {
                txtApplicationTypeTitle.Text = _ApplicationType.ApplicationTypeTitle;
                txtApplicationTypeFees.Text = _ApplicationType.ApplicationTypeFees.ToString();
            }
            else
            {
                MessageBox.Show("Application Type with ID = " + _ApplicationTypeID + " is NOT found!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void txtApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtApplicationTypeTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeTitle, "This Field is Required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtApplicationTypeTitle, null);
            }
        }

        private void txtApplicationTypeTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar) /*&& char.IsControl(e.KeyChar)*/;    
        }

        private void txtApplicationTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtApplicationTypeFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeFees, "This Field is Required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtApplicationTypeFees, null);
            }

            //e.Handled = char.IsDigit(e.KeyChar) && char.IsControl(e.KeyChar);    
            if (!clsValidation.IsNumber(txtApplicationTypeFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeFees, "Invalid Number!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtApplicationTypeFees, null);
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

            _ApplicationType.ApplicationTypeTitle = txtApplicationTypeTitle.Text.Trim();
            _ApplicationType.ApplicationTypeFees = Convert.ToSingle(txtApplicationTypeFees.Text.Trim());

            if(_ApplicationType.Save())
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
