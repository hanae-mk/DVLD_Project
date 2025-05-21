using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Users
{
    public partial class frmAddUpdateUser: Form
    {
        public enum enMode { AddNew = 1, Update = 2};
        public enMode Mode;
      
        int _UserID = -1; //Global Member  (private by default)
        clsUser _User;    //Global Object  (private by default)

        public frmAddUpdateUser()
        {
            InitializeComponent();          
            Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            Mode = enMode.Update;
         
        }

        private void _ResetDefaultValues()
        {
            if(Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User"; // means this form but will know lbltitle.text
                _User = new clsUser(); //Empty Object 
                tpLoginInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
        }

        private void _LoadData()
        {

            ctrlPersonCardWithFilter1.FilterEnabled = false;

            _User = clsUser.FindByUserID(_UserID);

            if (_User == null)
            {
                MessageBox.Show("User with ID = " + _UserID + " is Not Found!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
           
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (Mode == enMode.Update)
                _LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please Select a Person", 
                                "Error", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            if (Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"]; //Login Info
                return;
            }

            if(clsUser.IsThisPersonUser(ctrlPersonCardWithFilter1.PersonID))
            {
                //we can write this code in OnPersonSelected Event
                MessageBox.Show("This Person is already a user, Please choose another one!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //nzid neamel debug bash nxuf dik not .
            //we don't continue because the form is NOT Valid!
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are Not Valid!, Put the mouse over the red icon(s) to see the error",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            //Whether in AddNew Mode Or Update Mode we must transfer data
            //from Controls to Object!
            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim(); //I forget Trim()
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chkIsActive.Checked;

            //We save data from the object because object have meagniful Data 
            if (_User.Save())
            {
                Mode = enMode.Update;
                lblUserID.Text = _User.UserID.ToString();
                lblTitle.Text = "Update User";           //elash zad hadu hnaya??????
                this.Text = "Update User";
                MessageBox.Show("Data Saved Successfully",
                                 "Saved",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("An Error Occured while saving Data!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
        }
      
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                //txtUserName.Focus();
                errorProvider1.SetError(txtUserName, "This Field is required!");
                return;
            }

            if (clsUser.IsUserExist(txtUserName.Text.Trim()) && 
                                                      txtUserName.Text.Trim() != _User.UserName)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "This UserName is already used, Please try another one!");              
            }
            else
            { 
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "This Field is Required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (clsValidation.IsMatchedPassword(txtPassword.Text.Trim(), txtConfirmPassword.Text.Trim()))
                errorProvider1.SetError(txtConfirmPassword, null);
            else
            {
                btnSave.Enabled = false; 
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Unmatched Password!");                   
            }
        }

        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
