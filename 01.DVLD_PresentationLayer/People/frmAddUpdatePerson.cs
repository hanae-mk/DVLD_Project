using DVLD_BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project.People
{
    public partial class frmAddUpdatePerson : Form
    {
        //Declare a delegate
        public delegate void SendDataBack(object sender, int PersonID);
        public event SendDataBack DataBack;

        public enum enMode { AddNew = 1, Update = 2 };
        public enum enGender { Male = 0, Female = 1 };

        private enMode _Mode;
        private int _PersonID = -1;
        clsPerson _Person; //no need to write "private" access modifier because class is private by default

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
            _Mode = enMode.Update;
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void _ResetDefaultValues()
        {
            _FillComboBoxWithCountries();

            if (_Mode == enMode.AddNew)
            {
                _Person = new clsPerson();
                lblTitle.Text = "Add New Person";
            }
            else
                lblTitle.Text = "Update Person";

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-80);
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            txt1stName.Text = "";
            txt2ndName.Text = "";
            txt3rdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            RemoveLink.Visible = false;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            cbCountry.SelectedIndex = cbCountry.FindString("Morocco");
        }

        private void _LoadData()
        {
            //Global object type Person class
            _Person = clsPerson.FindPerson(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("This Person with ID : " + _PersonID + " is NOT found in the system!",
                                "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                this.Close();
                return;
            }

            //Filling Controls with data FROM object
            lblPersonID.Text = _PersonID.ToString();
            txt1stName.Text = _Person.FirstName;
            txt2ndName.Text = _Person.SecondName;
            txt3rdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);

            if (_Person.Gender == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            if (_Person.ImagePath != "")
            {
                if (File.Exists(_Person.ImagePath))
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                    RemoveLink.Visible = true;
                }
                else
                    pbPersonImage.Image = System.Drawing.SystemIcons.Error.ToBitmap();
            }
        }

        private void _FillComboBoxWithCountries()
        {
            DataTable CountriesTable = clsCountry.GetListCountries();

            foreach (DataRow Row in CountriesTable.Rows)
            {
                cbCountry.Items.Add(Row["CountryName"]);
            }
        }

        //General Method for text boxes
        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
            //sender may be txt1stName, txt2ndName, txtNationalNo, txtAddress...
            //It's can be any control
            //we cast sender to TextBox
            TextBox Temp = ((TextBox)sender);

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

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, null);
            }

            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.IsPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This NationalNo is already used by another user!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, null);
            }
        }
        
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            //Email is nullable in database so there is no need to validate it in case it's empty
            if (txtEmail.Text.Trim() == "")
                return;

            //validate email format
            if (!clsValidation.IsValidEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, null);
            }
        }

        private void ImageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Title = "Choose Image";
            openFileDialog1.InitialDirectory = @"C:\Users\pc\Downloads";
            openFileDialog1.DefaultExt = ".JPG";
            openFileDialog1.Filter = @"Image Files | *.JPG; *.JPE; *.GIF; *.PNG | All Files(*.*) | *.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string SourcePath = openFileDialog1.FileName;
                //pbPersonImage.ImageLocation = SourcePath;
                pbPersonImage.Load(SourcePath);
                RemoveLink.Visible = true;
            }
        }

        private bool _HandlePersonImage()
        {
            //we check if the image was changed or not
            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        //Delete the old image from the project folder!
                        File.Delete(_Person.ImagePath);
                        _Person.ImagePath = "";
                    }
                    catch (IOException Ex)
                    {
                        MessageBox.Show(Ex.Message, "Delete Image Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            if (pbPersonImage.ImageLocation != null && _Person.ImagePath == "")
            {
                string SourceImagePath = pbPersonImage.ImageLocation;

                if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImagePath))
                {
                    pbPersonImage.ImageLocation = SourceImagePath;
                    return true;
                }
                else
                {
                    MessageBox.Show("Copy Image Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //This event check all Validating events
            if (!this.ValidateChildren())
            {
                MessageBox.Show(@"Some Fields Are Not Valid, put the mouse on the red icon to 
                                  see the error",
                                  "Validation Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
                return;

            //Transffering data from controls to object
            _Person.FirstName = txt1stName.Text.Trim(); //Trim() Removes all white space
            _Person.SecondName = txt2ndName.Text.Trim();
            _Person.ThirdName = txt3rdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.NationalityCountryID = clsCountry.FindCountry(cbCountry.Text).CountryID;

            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else
                _Person.ImagePath = "";

            //Casting enum to byte
            if (rbMale.Checked)
                _Person.Gender = (byte)enGender.Male; //0
            else
                _Person.Gender = (byte)enGender.Female; //1

            if (_Person.Save())
            {
                _Mode = enMode.Update; //change the mode of this form
                lblPersonID.Text = _Person.PersonID.ToString();

                MessageBox.Show("Data Saved Successfully!",
                                "Saved",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                //Send data back to ANY form that calles this form
                //just by subscribing to the delegate
                //the reference of method is stored in DataBack
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("An Error Occured while saving Data!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);            
        }

        private void RemoveLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            RemoveLink.Visible = false;
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
            {
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        //private void rbFemale_CheckedChanged(object sender, EventArgs e)
        //{         
        //    pbPersonImage.Image = Resources.Female_512;       
        //}

        //private void rbMale_CheckedChanged(object sender, EventArgs e)
        //{
        //    pbPersonImage.Image = Resources.Male_512;
        //}
    }
}


