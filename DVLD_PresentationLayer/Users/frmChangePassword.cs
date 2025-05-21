using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.People.Controls;
using DVLD_Project.Users.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Users
{
    public partial class frmChangePassword: Form
    {
        private int _UserID = -1;
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;

            //Even it's non static we can access to it just by the name of control
            //It's should be only public so we can access to it
            //ctrlUserCard1.LoadUserInfo(_UserID);
            //_User = clsUser.FindByUserID(_UserID);
        }

        private void _ResetDefaultValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            _User = clsUser.FindByUserID(_UserID);

            if(_User == null)
            {
                MessageBox.Show("User with ID = " + _UserID + " is NOT Found!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {         
            if(string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "This Field is required");
                return;
            }
            
            if(_User.Password != txtCurrentPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password is wrong!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, null);
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "This Field is Required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!clsValidation.IsMatchedPassword(txtNewPassword.Text, txtConfirmPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Unmatched Password!");
                //btnSave.Enabled = false; bcs btnSave will not be executed bcs of ErrorProvider
            }
            else
            { 
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //we don't continue because the form is NOT Valid!
            if (!this.ValidateChildren())
            { 
                MessageBox.Show("Some fields are Not Valid!, Put the mouse over the red icon(s) to see the error",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _User.Password = txtConfirmPassword.Text;

            if(_User.Save())
            {
                MessageBox.Show("Password changed successfully",
                                "Saved",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                _ResetDefaultValues();
                this.Close();
            }
            else
            {
                MessageBox.Show("An Error Occured while saving the New Password!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }         
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }     
    }
}
